**Readme:**

# BuilderFactory

**Builder Factory** is a powerful library for creating complex objects using the Builder pattern in C#.

## Installation
To use BuilderFactory, add it to your C# project. You can also use NuGet:
```bash
dotnet add package BuilderFactory
```

## Usage
```csharp
// Create an instance of a class using BuilderFactory
var person = Instance.From<Person>()
    .SetProperty(p => p.Name = "John")
    .SetProperty(p => p.Age = 30)
    .Build();

// with contractor
var book = Instance.From<Book>(name, id).Build();
//or
var book = Instance.From(() => new Book(name, id)).Build();
```

## Features
- Fluent and readable object creation
- Easy property setting and constructor management
- Compatible with the Builder pattern in C#
