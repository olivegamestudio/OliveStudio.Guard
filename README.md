\# OliveStudio.Guard



A C# library providing guard clauses for validating method arguments and preconditions. Guard clauses help you fail fast by validating inputs at the beginning of methods, making your code more robust and easier to debug.



\## Installation



```bash

\# Package manager

Install-Package OliveStudio.Guard



\# .NET CLI

dotnet add package OliveStudio.Guard

```



\## Available Guard Methods



\### `ThrowIfNull<T>(T item)`



Throws an `InvalidOperationException` if the specified item is null.



```csharp

public void ProcessUser(User user)

{

&nbsp;   Guard.ThrowIfNull(user); // Throws if user is null

&nbsp;   

&nbsp;   // Safe to use user here

&nbsp;   Console.WriteLine($"Processing user: {user.Name}");

}

```



\### `ThrowIfNotNull<T>(T item)`



Throws an `InvalidOperationException` if the specified item is not null.



```csharp

public void InitializeConnection(IDbConnection connection)

{

&nbsp;   Guard.ThrowIfNotNull(connection); // Throws if connection already exists

&nbsp;   

&nbsp;   // Safe to initialize new connection

&nbsp;   connection = CreateNewConnection();

}

```



\### `ThrowIfIntegerNotInRange(int item, int start, int end)`



Throws an `InvalidOperationException` if the specified integer is not within the specified range (inclusive).



```csharp

public void SetAge(int age)

{

&nbsp;   Guard.ThrowIfIntegerNotInRange(age, 0, 150); // Throws if age < 0 or age > 150

&nbsp;   

&nbsp;   // Safe to use age

&nbsp;   this.Age = age;

}



public void SetPercentage(int percentage)

{

&nbsp;   Guard.ThrowIfIntegerNotInRange(percentage, 0, 100); // Valid percentage range

&nbsp;   

&nbsp;   this.Percentage = percentage;

}

```



\### `ThrowIfStringNullOrEmpty(string value)`



Throws an `InvalidOperationException` if the specified string is null or empty.



```csharp

public void SetUserName(string userName)

{

&nbsp;   Guard.ThrowIfStringNullOrEmpty(userName); // Throws if null or empty

&nbsp;   

&nbsp;   // Safe to use userName

&nbsp;   this.UserName = userName;

}

```



\## Usage Examples



\### Constructor Validation



```csharp

public class User

{

&nbsp;   public string Name { get; }

&nbsp;   public string Email { get; }

&nbsp;   public int Age { get; }



&nbsp;   public User(string name, string email, int age)

&nbsp;   {

&nbsp;       Name = Guard.ThrowIfStringNullOrEmpty(name);

&nbsp;       Email = Guard.ThrowIfStringNullOrEmpty(email);

&nbsp;       Age = Guard.ThrowIfIntegerNotInRange(age, 0, 120);

&nbsp;   }

}

```



\### Method Parameter Validation



```csharp

public class FileProcessor

{

&nbsp;   public void ProcessFile(string filePath, byte\[] content)

&nbsp;   {

&nbsp;       Guard.ThrowIfStringNullOrEmpty(filePath);

&nbsp;       Guard.ThrowIfNull(content);

&nbsp;       

&nbsp;       // Process the file safely

&nbsp;       File.WriteAllBytes(filePath, content);

&nbsp;   }

&nbsp;   

&nbsp;   public void SetPriority(int priority)

&nbsp;   {

&nbsp;       Guard.ThrowIfIntegerNotInRange(priority, 1, 10);

&nbsp;       

&nbsp;       this.Priority = priority;

&nbsp;   }

}

```



\### Service Class Example



```csharp

public class UserService

{

&nbsp;   private readonly IUserRepository \_repository;

&nbsp;   

&nbsp;   public UserService(IUserRepository repository)

&nbsp;   {

&nbsp;       \_repository = Guard.ThrowIfNull(repository);

&nbsp;   }

&nbsp;   

&nbsp;   public async Task<User> CreateUserAsync(string name, string email, int age)

&nbsp;   {

&nbsp;       Guard.ThrowIfStringNullOrEmpty(name);

&nbsp;       Guard.ThrowIfStringNullOrEmpty(email);

&nbsp;       Guard.ThrowIfIntegerNotInRange(age, 13, 100); // Minimum age requirement

&nbsp;       

&nbsp;       var user = new User(name, email, age);

&nbsp;       return await \_repository.SaveAsync(user);

&nbsp;   }

&nbsp;   

&nbsp;   public void UpdateUserStatus(User user, string status)

&nbsp;   {

&nbsp;       Guard.ThrowIfNull(user);

&nbsp;       Guard.ThrowIfStringNullOrEmpty(status);

&nbsp;       

&nbsp;       user.Status = status;

&nbsp;   }

}

```



\### API Controller Example



```csharp

\[ApiController]

\[Route("api/\[controller]")]

public class UsersController : ControllerBase

{

&nbsp;   private readonly UserService \_userService;

&nbsp;   

&nbsp;   public UsersController(UserService userService)

&nbsp;   {

&nbsp;       \_userService = Guard.ThrowIfNull(userService);

&nbsp;   }

&nbsp;   

&nbsp;   \[HttpPost]

&nbsp;   public async Task<IActionResult> CreateUser(\[FromBody] CreateUserRequest request)

&nbsp;   {

&nbsp;       try

&nbsp;       {

&nbsp;           Guard.ThrowIfNull(request);

&nbsp;           

&nbsp;           var user = await \_userService.CreateUserAsync(

&nbsp;               request.Name, 

&nbsp;               request.Email, 

&nbsp;               request.Age);

&nbsp;               

&nbsp;           return Ok(user);

&nbsp;       }

&nbsp;       catch (InvalidOperationException ex)

&nbsp;       {

&nbsp;           return BadRequest(ex.Message);

&nbsp;       }

&nbsp;   }

}

```



\## Chaining Guards



Since all guard methods return the original value, you can chain operations:



```csharp

public void ProcessData(string data)

{

&nbsp;   var processedData = Guard.ThrowIfStringNullOrEmpty(data)

&nbsp;       .Trim()

&nbsp;       .ToUpperInvariant();

&nbsp;   

&nbsp;   // Use processedData safely

}



public User CreateUser(User template)

{

&nbsp;   return Guard.ThrowIfNull(template) with 

&nbsp;   { 

&nbsp;       Id = Guid.NewGuid(),

&nbsp;       CreatedAt = DateTime.UtcNow 

&nbsp;   };

}

```



\## Best Practices



\### 1. Use at Method Entry Points

Place guard clauses at the very beginning of methods to fail fast:



```csharp

public void ProcessOrder(Order order, int quantity)

{

&nbsp;   Guard.ThrowIfNull(order);

&nbsp;   Guard.ThrowIfIntegerNotInRange(quantity, 1, 1000);

&nbsp;   

&nbsp;   // Method logic here

}

```



\### 2. Validate Constructor Parameters

Always validate constructor parameters to ensure object invariants:



```csharp

public class Product

{

&nbsp;   public Product(string name, decimal price, int stock)

&nbsp;   {

&nbsp;       Name = Guard.ThrowIfStringNullOrEmpty(name);

&nbsp;       Price = Guard.ThrowIfIntegerNotInRange((int)(price \* 100), 1, int.MaxValue) / 100m;

&nbsp;       Stock = Guard.ThrowIfIntegerNotInRange(stock, 0, int.MaxValue);

&nbsp;   }

}

```



\### 3. Combine with Logging

```csharp

public void ProcessUser(User user)

{

&nbsp;   try

&nbsp;   {

&nbsp;       Guard.ThrowIfNull(user);

&nbsp;       // Process user

&nbsp;   }

&nbsp;   catch (InvalidOperationException ex)

&nbsp;   {

&nbsp;       \_logger.LogError("Invalid user provided: {Error}", ex.Message);

&nbsp;       throw;

&nbsp;   }

}

```



