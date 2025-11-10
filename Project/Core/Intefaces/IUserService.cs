

using BlocketChallenge.Project.Domain.Models;

namespace BlocketChallenge.Project.Core.Interfaces;
public interface IUserService
{
    IEnumerable<User> GetAllUsers();
    User? GetUserById(int id);
    void CreateUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int id);

    User? Authenticate(string username, string password);

}