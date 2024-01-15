namespace BuilderFactory.Tests;

public class BuilderFactoryTests
{
    [Fact]
    public void Create_InstanceWithDefaultConstructor_ShouldCreateInstance()
    {
        var factoryBuilder = Instance.From<Person>();

        var result = factoryBuilder.Build();

        result.Should().NotBeNull();
        result.Should().BeOfType<Person>();
        result.Name.Should().NotBeNullOrEmpty();
        result.Name.Should().Be(result.Name);
    }

    [Theory, AutoData]
    public void CanBuildInstanceWithDefaultConstructor(string name, int age)
    {
        var factoryBuilder = Instance.From<Person>()
            .SetProperty(p => p.Name = name)
            .SetProperty(p => p.Age = age);

        var instance = factoryBuilder.Build();

        instance.Should().NotBeNull();
        instance.Name.Should().Be(name);
        instance.Age.Should().Be(age);
    }

    [Theory, AutoData]
    public void Create_WithParameters_ShouldCreateInstance(int id, string name)
    {
        var factoryBuilder = Instance.From<Book>()
            .SetProperty(_ => _.Name = name)
            .SetProperty(_ => _.Id = id);

        var result = factoryBuilder.Build();

        result.Should().NotBeNull();
        result.Should().BeOfType<Book>();
        result.Name.Should().Be(name);
        result.Id.Should().Be(id);
    }
}

internal class Person
{
    public string? Name { get; set; }
    public int Age { get; set; }
}

internal class Book
{
    public Book(string name, int id)
    {
        Name = name;
        Id = id;
    }

    public string? Name { get; set; }
    public int Id { get; set; }
}