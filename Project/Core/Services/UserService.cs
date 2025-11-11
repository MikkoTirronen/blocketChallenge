using BlocketChallenge.Project.Core.Interfaces;
using BlocketChallenge.Project.Domain.Models;
using BlocketClallenge.Project.Data.Interfaces;

namespace BlocketChallenge.Services;

public class UserService(IUserRepository repository) : IUserService
{
    public readonly IUserRepository _repository = repository;

    public IEnumerable<User> GetAllUsers() => _repository.GetAllUsers();

    public User? GetUserById(int id) => _repository.GetUserById(id);
    public User? GetUserByUsername(string username) => _repository.GetUserByUsername(username);

    public User? GetUserByEmail(string email) => _repository.GetUserByEmail(email);
    public void CreateUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
            throw new ArgumentException("Username is required.");
        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("Email is Required.");
        if (string.IsNullOrWhiteSpace(user.PasswordHash))
            throw new ArgumentException("Password is Required.");

        var existing = _repository.GetUserByUsername(user.Username);
        var existingEmail = _repository.GetUserByEmail(user.Email);
        
        if (existing != null)
            throw new InvalidOperationException("Username already exists.");

        if (existingEmail != null)
            throw new InvalidOperationException("Email already exists.");
            

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
    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        return null;

    var user = _repository.GetUserByUsername(username);
    if (user == null) return null;

    if (string.IsNullOrWhiteSpace(user.PasswordHash)) return null;

    bool verified = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    if (!verified) return null;

    return user;
}

}