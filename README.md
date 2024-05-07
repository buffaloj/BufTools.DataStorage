# DataStorage

This solution provides an abstraction for a DataStore, an implementation of it using EntityFramework, and a 
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
var lastName = store.Get<Person>().Where(p => p.LastName == "Doe").FirstOrDefault();
```

# Packages

This solution is made up of three packages to limit the required dependencies when using this in your own solution.

- BufTools.DataStore - Provides a DataStore abstraction for CRUD operations, and accessing Views, Functions, and SPROCS

- BufTools.DataStore.Schema - Provides Attributes to mark data classes/methods as StoredData, StoredView, StoredProcedure, or StoredFunction.  

- BufTools.DataStore.EntityFramework - An implementation of UnitOfWork using EntityFrameworkCore.

# Getting Started
There are two one-time setup steps to start using this package:

1. Create your own AutoRegisterDbContext to auto register class types:
  * The DataStore package requires a DBContext. Using AutoRegisterDbContext is optional.
    
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

2. Register your DBContext and a DataStore for dependency injection:

```cs
builder.Services.AddDbContext<MyDbContext>(   
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("MySqlConnection")), 
    ServiceLifetime.Scoped,     
    ServiceLifetime.Scoped);
	
builder.Services.AddScoped<IStoreData, EntityFrameworkDataStore<MyDbContext>>();
```

# Usage

## Access a DB Table

To access a DB table, add a class that represents the table, mark it with [Entity] and use it:

```cs
[StoredData]
public class Person
{
    [Key]
	public int Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
}
```

```cs
var lastName = store.Get<Person>().Where(p => p.LastName == "Doe").FirstOrDefault();
```

- DO use [Attribute]s from System.ComponentModel.DataAnnotations.Schema and this package as needed
  - for ex, [Key], [CompositeKey], [ForeignKey(...)], [Column(...)], [JsonIgnore]


## Access a DB View

The same rules that apply to an Entity also apply to a View.  To access a View, add a class that represents the View, mark it with [View] and use it:

```cs
[StoredView]
public class PersonView
{
	public int Id { get; set; }
	public string Name { get; set; }
}
```
  * Note [Key] is not included for a View

```cs
var name = store.Get<PersonView>().Where(p => p.Name == "John Q Public").FirstOrDefault();
```

## Scalar Functions
To use a Scalar Function that lives in the database, first define an empty static function in C# with the same signature as the DB function and mark it with [Function(dbFuncName)].  That function can then be used from within a LINQ query.

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
var people = store.Get<Person>()
                .Where(p => Funcs.NumberOfCarsOwned(p.Id) == 2)
                .ToList();
```

## Table Functions
To use a Table Function that lives in the database, first define an empty static function in C# with the same signature as the DB function and mark it with [Function(dbFuncName)].  That function can then be called from an instance of IStoreData.

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
To keep running SPROCs simple, IStoreData provides a Sproc() method that returns an IRunStoredProcedures instance to run a sproc.  IRunStoredProcedures provides a builder method to easily build up the signature of the SPROC.
* A convenient way to write the C# side of the SPROCs is to use an extension method for IRunStoredProcedures.

Define the SPROC:
```cs
public static partial class Sprocs
{
	public static IQueryable<Owner> GetOwnersOfVehicle(this IRunStoredProcedures<Owner> sproc,
															string vin)
	{
		return sproc.WithParam("@Vin", vin)
                   .Run("[dbo].[get_owners]");
	}
}
```

And use it:
```cs
var owners = store.Sproc<Owner>()
                .GetOwnersOfVehicle("12345678901234567")
                .ToList();
```

# Running the Tests
The tests are mainly integration tests that require a SQL database to be running. Docker files are included that will spin up a SQL Server instance and populate it with required test data.

Run the tests locally:
1. Set docker-compose as the startup project
2. Run the project
  - this will build a cointainer that has a seeded SQL server instance
  - once the project stops running, the database is built, seeded, and ready to use.
3. Run the tests in the test runner as normal and they should all pass
