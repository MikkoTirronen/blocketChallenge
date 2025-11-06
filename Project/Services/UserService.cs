using BlocketChallenge.Models;
using BlocketClallenge.Repositories;

namespace BlocketChallenge.Services;

public class UserService(IUserRepository repository) : IUserService
{
    public readonly IUserRepository _repository = repository;

    public IEnumerable<User> GetAllUsers() => _repository.GetAllUsers();

    public User? GetUserById(int id) => _repository.GetUserById(id);

    public void CreateUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
            throw new ArgumentException("Username is required.");
        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("Email is Required.");
        if (string.IsNullOrWhiteSpace(user.PasswordHash))
            throw new ArgumentException("Password is Required.");

        user.CreatedAt = DateTime.UtcNow;
        _repository.CreateUser(user);
    }

    public void UpdateUser(User user)
    {
        if (user.Id <= 0)
            throw new ArgumentException("Invalid user ID.");
        _repository.UpdateUser(user);
    }

    public void DeleteUser(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid user ID.");

        _repository.DeleteUser(id);
    }

public User? Authenticate(string username, string password)
{
    var user = _repository.GetUserByUsername(username);

    if (user == null)
        throw new Exception("User not found");

    if (string.IsNullOrWhiteSpace(password))
        throw new Exception("Password cannot be empty");

    if (string.IsNullOrWhiteSpace(user.PasswordHash))
        throw new Exception("Stored password hash is null");

    bool verified = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

    if (!verified)
        throw new Exception("Invalid password");

    return user;
}
}