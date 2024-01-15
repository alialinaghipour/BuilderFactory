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

// Automatic property filling based on class properties
var configuredPerson = Instance.From<Person>(isDefaultValuesSet: true)
// or
var configuredPerson = Instance.From<Person>()
```

## Features
- Fluent and readable object creation
- Automatic constructor and property management (introduced in version 1.0.3)
- Automatic property filling based on class properties (introduced in version 1.0.3)
- Compatible with the Builder pattern in C#
