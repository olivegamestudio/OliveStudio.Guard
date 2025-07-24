# OliveStudio.Guard

A C# library providing guard clauses for validating method arguments and preconditions. Guard clauses help you fail fast by validating inputs at the beginning of methods, making your code more robust and easier to debug.

## Installation

```bash
# Package manager
Install-Package OliveStudio.Guard

# .NET CLI
dotnet add package OliveStudio.Guard
```

## Available Guard Methods

### `ThrowIfNull<T>(T item)`

Throws an `InvalidOperationException` if the specified item is null.

```csharp
public void ProcessUser(User user)
{
    Guard.ThrowIfNull(user); // Throws if user is null
    
    // Safe to use user here
    Console.WriteLine($"Processing user: {user.Name}");
}
```

### `ThrowIfNotNull<T>(T item)`

Throws an `InvalidOperationException` if the specified item is not null.

```csharp
public void InitializeConnection(IDbConnection connection)
{
    Guard.ThrowIfNotNull(connection); // Throws if connection already exists
    
    // Safe to initialize new connection
    connection = CreateNewConnection();
}
```

### `ThrowIfIntegerNotInRange(int item, int start, int end)`

Throws an `InvalidOperationException` if the specified integer is not within the specified range (inclusive).

```csharp
public void SetAge(int age)
{
    Guard.ThrowIfIntegerNotInRange(age, 0, 150); // Throws if age < 0 or age > 150
    
    // Safe to use age
    this.Age = age;
}

public void SetPercentage(int percentage)
{
    Guard.ThrowIfIntegerNotInRange(percentage, 0, 100); // Valid percentage range
    
    this.Percentage = percentage;
}
```

### `ThrowIfStringNullOrEmpty(string value)`

Throws an `InvalidOperationException` if the specified string is null or empty.

```csharp
public void SetUserName(string userName)
{
    Guard.ThrowIfStringNullOrEmpty(userName); // Throws if null or empty
    
    // Safe to use userName
    this.UserName = userName;
}
```

## Usage Examples

### Constructor Validation

```csharp
public class User
{
    public string Name { get; }
    public string Email { get; }
    public int Age { get; }

    public User(string name, string email, int age)
    {
        Name = Guard.ThrowIfStringNullOrEmpty(name);
        Email = Guard.ThrowIfStringNullOrEmpty(email);
        Age = Guard.ThrowIfIntegerNotInRange(age, 0, 120);
    }
}
```

### Method Parameter Validation

```csharp
public class FileProcessor
{
    public void ProcessFile(string filePath, byte[] content)
    {
        Guard.ThrowIfStringNullOrEmpty(filePath);
        Guard.ThrowIfNull(content);
        
        // Process the file safely
        File.WriteAllBytes(filePath, content);
    }
    
    public void SetPriority(int priority)
    {
        Guard.ThrowIfIntegerNotInRange(priority, 1, 10);
        
        this.Priority = priority;
    }
}
```

### Service Class Example

```csharp
public class UserService
{
    private readonly IUserRepository _repository;
    
    public UserService(IUserRepository repository)
    {
        _repository = Guard.ThrowIfNull(repository);
    }
    
    public async Task<User> CreateUserAsync(string name, string email, int age)
    {
        Guard.ThrowIfStringNullOrEmpty(name);
        Guard.ThrowIfStringNullOrEmpty(email);
        Guard.ThrowIfIntegerNotInRange(age, 13, 100); // Minimum age requirement
        
        var user = new User(name, email, age);
        return await _repository.SaveAsync(user);
    }
    
    public void UpdateUserStatus(User user, string status)
    {
        Guard.ThrowIfNull(user);
        Guard.ThrowIfStringNullOrEmpty(status);
        
        user.Status = status;
    }
}
```

### API Controller Example

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    
    public UsersController(UserService userService)
    {
        _userService = Guard.ThrowIfNull(userService);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            Guard.ThrowIfNull(request);
            
            var user = await _userService.CreateUserAsync(
                request.Name, 
                request.Email, 
                request.Age);
                
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
```

## Chaining Guards

Since all guard methods return the original value, you can chain operations:

```csharp
public void ProcessData(string data)
{
    var processedData = Guard.ThrowIfStringNullOrEmpty(data)
        .Trim()
        .ToUpperInvariant();
    
    // Use processedData safely
}

public User CreateUser(User template)
{
    return Guard.ThrowIfNull(template) with 
    { 
        Id = Guid.NewGuid(),
        CreatedAt = DateTime.UtcNow 
    };
}
```

## Best Practices

### 1. Use at Method Entry Points
Place guard clauses at the very beginning of methods to fail fast:

```csharp
public void ProcessOrder(Order order, int quantity)
{
    Guard.ThrowIfNull(order);
    Guard.ThrowIfIntegerNotInRange(quantity, 1, 1000);
    
    // Method logic here
}
```

### 2. Validate Constructor Parameters
Always validate constructor parameters to ensure object invariants:

```csharp
public class Product
{
    public Product(string name, decimal price, int stock)
    {
        Name = Guard.ThrowIfStringNullOrEmpty(name);
        Price = Guard.ThrowIfIntegerNotInRange((int)(price * 100), 1, int.MaxValue) / 100m;
        Stock = Guard.ThrowIfIntegerNotInRange(stock, 0, int.MaxValue);
    }
}
```

### 3. Combine with Logging
```csharp
public void ProcessUser(User user)
{
    try
    {
        Guard.ThrowIfNull(user);
        // Process user
    }
    catch (InvalidOperationException ex)
    {
        _logger.LogError("Invalid user provided: {Error}", ex.Message);
        throw;
    }
}
```
