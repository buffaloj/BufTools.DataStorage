# DataStorage

This solution puts Entities, Views, SPROCs, and DB Functions "at your fingertips"; just add an Entity, Func, or Sproc and use it. No setup, registration, or other work.

Add an entity:
```cs
[Entity]
public class Person
{
	public int Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
}
```

Then use it:
```cs
var lastName = uow.Get<Person>().Where(p => p.LastName == "Doe").FirstOrDefault();
```

# Packages

This solution is made up of three packages to limit the required dependencies when using this your own solution.

- BufTools.DataAnnotations.Schema - Provides Attributes to mark data classes/methods as an Entity, View, Procedure, or Function.  

- BufTools.DataStorage - Provides abstractions CRUD operation, access Views, Functions, and SPROCS

- BufTools.DataStorage.EntityFramework - An implementation of DataStorage using EntityFrameworkCore including IServiceExtensions to simplify with initial setup.

# Getting Started
There are two one-time setup steps to start using this package:

1. Create your own IDataContext implementation

```cs
public class TestDataContext : AbstractDataContext
{
	public TestDataContext()
	{
		IncludeWithClassAttribute<EntityAttribute>(GetType().Assembly);
		IncludeWithClassAttribute<ViewAttribute>(GetType().Assembly);
		Include(typeof(Funcs));
	}
}
```
* derive from AbstractDataContext unless you have a special need not to. This gives you reflection support to register all data classes with a specified attribute in one call and never touch it again.

2. Register your DataContext, a UnitOfWork with an IServiceCollection, and init the database you want to use:

```cs
services.AddSingleton<IDataContext, DataContext>();
services.AddScopedUnitOfWork<DataContext>(options => options.UseSqlServer(connectionString));
```

# Usage

## Access a DB Table

To access a DB table, add a class that represents the table, mark it with [Entity] and use it:

```cs
[Entity]
public class Person
{
	public int Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
}
```

```cs
var lastName = uow.Get<Person>().Where(p => p.LastName == "Doe").FirstOrDefault();
```

- DO use [Attribute]s from System.ComponentModel.DataAnnotations.Schema and this package as needed
  - for ex, [Key], [CompositeKey], [ForeignKey(...)], [Column(...)], [JsonIgnore]


## Access a DB View

The same rules that apply to an Entity also apply to a View.  To access a View, add a class that represents the View, mark it with [View] and use it:

```cs
[View]
public class PersonView
{
	public int Id { get; set; }
	public string Name { get; set; }
}
```

```cs
var name = uow.Get<PersonView>().Where(p => p.Name == "John Q Public").FirstOrDefault();
```

## Scalar Functions
To use a Scalar Function that lives in the database, you first define an empty static function in C# with the same signature as the DB function and mark it with [Function(dbFuncName)].  That function can then be used from within a LINQ query.

Define the function:
```cs
public static partial class Funcs
{
	[Function("number_of_cars_owned", "dbo")]
	public static int NumberOfCarsOwned(int personId)
		=> throw new NotSupportedException();
}
```

And use it:
```cs
var people = uow.Get<Person>()
                .Where(p => Funcs.NumberOfCarsOwned(p.Id) == 2)
                .ToList();
```

## Table Functions
To use a Table Function that lives in the database, you first define an empty static function in C# with the same signature as the DB function and mark it with [Function(dbFuncName)].  That function can then be called from an instance of IUnitOfWork.

Define the function:
```cs
public static partial class Funcs
{
	[Function("owners_of_vehicle", "dbo")]
	public static IQueryable<Owner> OwnersOfVehicle(string vin)
		=> throw new NotSupportedException();
}
```

And use it:
```cs
var owners = uow.Sproc<Owner>()
				.GetOwnersOfVehicle("12345678901234567")
				.ToList();
```

## Execute a SPROC
To keep running SPROCs simple, IUnitOfWork provices a Sproc() method that returns a IProcedure instance to run a sproc.  IProcedure provides builder method to easily build up the signature of the SPROC.
* A convenient for to write the C# side of the SPROCs is to use an extension method for IProcedure.

Define the SPROC:
```cs
public static partial class Procs
{
	public static IQueryable<Owner> GetOwnersOfVehicle(this IProcedure<Owner> proc,
															string vin)
	{
		return proc.WithParam("@Vin", vin)
				   .Run("[dbo].[get_owners]");
	}
}
```

And use it:
```cs
var owners = uow.Sproc<Owner>()
                .GetOwnersOfVehicle("12345678901234567")
				.ToList();
```
