using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User? GetById(Guid id);
        User Create(User user);
        bool Update(Guid id, User updatedUser);
        bool Delete(Guid id);
        bool EmailExists(string email, Guid? excludeUserId = null);
    }
}