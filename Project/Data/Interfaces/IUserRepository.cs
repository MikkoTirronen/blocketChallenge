using BlocketChallenge.Project.Domain.Models;

namespace BlocketClallenge.Project.Data.Interfaces;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User? GetUserById(int id);
    void CreateUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int id);
    User? GetUserByUsername(string username);
    User? GetUserByEmail(string email);
}