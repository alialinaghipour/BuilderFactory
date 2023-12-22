namespace BuilderFactory;

public static class Instance
{
    public static Instance<T> From<T>() where T : new()
    {
        return new Instance<T>();
    }

    public static Instance<T> From<T>(params object[] parameters)
    {
        return new Instance<T>(parameters);
    }

    public static Instance<T> From<T>(Func<T> instanceCreator)
    {
        return new Instance<T>(instanceCreator);
    }
}

public class Instance<T>
{
    private readonly T _instance;

    public Instance()
    {
        _instance = CreateInstance();
    }

    public Instance(params object[] parameters)
    {
        ValidateConstructor(parameters);
        _instance = CreateInstance(parameters);
    }

    public Instance(Func<T> instanceCreator)
    {
        _instance = instanceCreator.Invoke();
    }

    public Instance<T> SetProperty(Action<T>? propertySetter)
    {
        propertySetter?.Invoke(_instance);
        return this;
    }

    public T Build()
    {
        return _instance;
    }

    private static T CreateInstance()
    {
        return Activator.CreateInstance<T>();
    }

    private static T CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            throw new InvalidOperationException($"Parameters cannot be null or empty");
        }

        var constructorInfo = typeof(T).GetConstructor(parameters.Select(p => p.GetType()).ToArray());
        if (constructorInfo == null)
        {
            throw new InvalidOperationException();
        }

        var constantExpressions = parameters.Select(Expression.Constant);
        var newExpression = Expression.New(constructorInfo, constantExpressions);
        var lambda = Expression.Lambda<Func<T>>(newExpression);
        return lambda.Compile().Invoke();
    }

    private static void ValidateConstructor(params object[] parameters)
    {
        var constructors = typeof(T).GetConstructors();

        if (constructors.Length == 0)
        {
            throw new InvalidOperationException($"Type {typeof(T).Name} does not have any constructor.");
        }

        if (parameters.Length == 0) return;
        
        var suitableConstructors = constructors.Where(ctor =>
            ctor.GetParameters().Length == parameters.Length &&
            ctor.GetParameters().Zip(parameters, (p, arg) => p.ParameterType.IsInstanceOfType(arg))
                .All(p => p));

        if (!suitableConstructors.Any())
        {
            throw new InvalidOperationException(
                $"No suitable constructors found for type {typeof(T).Name} with the given parameters.");
        }
    }
}
