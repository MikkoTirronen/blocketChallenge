using BlocketChallenge.Models;

namespace BlocketClallenge.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User? GetUserById(int id);
    void CreateUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int id);
    User? GetUserByUsername(string username);
}