using System.Reflection;

namespace BuilderFactory;

public static class Instance
{
    public static Instance<T> From<T>(bool isDefaultValuesSet = true) 
    {
        return new Instance<T>(isDefaultValuesSet);
    }
}

public class Instance<T>
{
    private readonly T _instance;
    private bool _isDefaultValuesSet;

    public Instance(bool isDefaultValuesSet)
    {
        _isDefaultValuesSet = isDefaultValuesSet;
        _instance = CreateInstanceUsingAlternativeMethod();
        
        if (_isDefaultValuesSet)
        {
            SetDefaultPropertyValues();
        }
    }

    private void SetDefaultPropertyValues()
    {
        foreach (var property in typeof(T).GetProperties())
        {
            if (property.CanWrite)
            {
                object value = RandomValueGenerator.GetRandomValue(property.PropertyType);
                property.SetValue(_instance, value);
            }
        }
    }

    public Instance(params object[] parameters)
    {
        ValidateConstructor(parameters);
        _instance = CreateInstance(parameters);
    }

    public Instance(bool isDefaultValuesSet, params object[] parameters)
    {
        _isDefaultValuesSet = isDefaultValuesSet;
        ValidateConstructor(parameters);
        _instance = CreateInstance(parameters);
    }

    public Instance(Func<T> instanceCreator, bool isDefaultValuesSet)
    {
        _isDefaultValuesSet = isDefaultValuesSet;
        _instance = instanceCreator.Invoke();
    }

    public Instance<T> SetProperty(Action<T>? propertySetter)
    {
        propertySetter?.Invoke(_instance);
        return this;
    }

    public Instance<T> WithSetDefaultValues(bool value = true)
    {
        _isDefaultValuesSet = value;
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
    
    private T CreateInstanceUsingAlternativeMethod()
    {
        ConstructorInfo[] constructors = typeof(T).GetConstructors();

        foreach (ConstructorInfo constructor in constructors)
        {
            ParameterInfo[] parameters = constructor.GetParameters();

            object[] parameterValues = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameterValues[i] = RandomValueGenerator.GetRandomValue(parameters[i].ParameterType);
            }

            try
            {
                return (T)constructor.Invoke(parameterValues);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating instance: {ex.Message}");
            }
        }

        return default(T)!;
    }

}