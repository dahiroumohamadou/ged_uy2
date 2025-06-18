using GED_APP.Models;

namespace GED_APP.Repository.Interfaces
{
    public interface IUser : IDisposable
    {
        ICollection<User> GetAll();
        User GetById(int id);
        User GetByEmail(string email);
        string HashPassword(string password);
        int Add(User user);
        int Update(User user);
        int Delete(int id);
        User AuthenticateUser(User user);

       
    }
}
