using BlocketChallenge.Models;

namespace BlocketClallenge.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User? GetUserById(int id);
    void CreateUser(User user);
    void Update(User user);
    void Delete(int id);
    User? GetUserByUsername(string username);
}