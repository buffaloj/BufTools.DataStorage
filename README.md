# DataStorage

This solution provides an abstraction for a UnitOfWork, an implementation of it using EntityFramework, and a 
DBContext that uses reflection to register data classes automatically.  The Schema package also provides 
attributes to decorate data classes so the DBContext can find them.

The purpose of this solution is to simplify accessing a database using EntityFramework. The basic approach is:

1. Add initialization/class registration code once up front.
2. Add a data class, decorate it with an [Entity], [View], or [Function] attribute and use it. That's it.


Add an entity:
```cs
[Entity]
public class Person
{
    [Key]
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

- BufTools.EntityFramework.AutoTypeRegistration - Adds methods to DBContext to auto register class types with EF.

- BufTools.DataAnnotations.Schema - Provides Attributes to mark data classes/methods as an Entity, View, Procedure, or Function.  

- BufTools.Abstraction.UnitOfWork - Provides a UOW abstraction for CRUD operations, and accessing Views, Functions, and SPROCS

- BufTools.UnitOfWork.EntityFramework - An implementation of UnitOfWork using EntityFrameworkCore.


# Getting Started
There are two one-time setup steps to start using this package:

1. Create your own AutoRegisterDbContext to auto register class types:
  * The UnitOfWork package requires a DBContext. Using AutoRegisterDbContext is optional.
    
```cs
public class MyDbContext : AutoRegisterDbContext
{
	public MyDbContext(DbContextOptions options) : base(options)
	{
		RegisterEntities().WithAttribute<EntityAttribute>(GetType().Assembly);
		RegisterViews().WithAttribute<ViewAttribute>(GetType().Assembly);
		RegisterFunctions().WithAttribute<FunctionAttribute>(GetType().Assembly);
	}
}
```

2. Register your DBContext and a UnitOfWork for dependency injection:

```cs
builder.Services.AddDbContext<MyDbContext>(   
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("MySqlConnection")), 
    ServiceLifetime.Scoped,     
    ServiceLifetime.Scoped);
	
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<MyDbContext>>();
```

# Usage

## Access a DB Table

To access a DB table, add a class that represents the table, mark it with [Entity] and use it:

```cs
[Entity]
public class Person
{
    [Key]
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
  * Note [Key] is not included for a View

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
var results = _target.TableFunc(() => Funcs.OwnersOfVehicle("12345678901234567"))
                     .ToList();
```

## Execute a SPROC
To keep running SPROCs simple, IUnitOfWork provides a Sproc() method that returns a IProcedure instance to run a sproc.  IProcedure provides builder method to easily build up the signature of the SPROC.
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

# Running the Tests
The tests are mainly integration tests that require a SQL database to be running.  To ease setting this up and witness the tests passing, Docker files are included that will spin up a SQL Server instance and populate it with test data that the tests require.

To run the tests locally:
1. Set docker-compose as the startup project
2. Run the project
  - this will build a cointainer that has a seeded SQL server instance
  - once the project stops running, the database is built, seeded, and ready to use.
3. Run the tests in the test runner as normal and they should all pass
