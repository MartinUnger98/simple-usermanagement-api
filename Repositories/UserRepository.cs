using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new();
        private readonly object _lock = new();

        public UserRepository()
        {
            _users.AddRange(new[]
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "alice.johnson@techhive.com",
                    Department = "HR",
                    IsActive = true,
                    CreatedAtUtc = DateTime.UtcNow
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Bob",
                    LastName = "Smith",
                    Email = "bob.smith@techhive.com",
                    Department = "IT",
                    IsActive = true,
                    CreatedAtUtc = DateTime.UtcNow
                }
            });
        }

        public IEnumerable<User> GetAll()
        {
            lock (_lock)
            {
                return _users.ToList(); // ✅ return copy
            }
        }

        public User? GetById(Guid id)
        {
            lock (_lock)
            {
                return _users.FirstOrDefault(u => u.Id == id);
            }
        }

        public User Create(User user)
        {
            lock (_lock)
            {
                _users.Add(user);
                return user;
            }
        }

        public bool Update(Guid id, User updatedUser)
        {
            lock (_lock)
            {
                var existingUser = _users.FirstOrDefault(u => u.Id == id);
                if (existingUser == null)
                    return false;

                existingUser.FirstName = updatedUser.FirstName;
                existingUser.LastName = updatedUser.LastName;
                existingUser.Email = updatedUser.Email;
                existingUser.Department = updatedUser.Department;
                existingUser.IsActive = updatedUser.IsActive;

                return true;
            }
        }

        public bool Delete(Guid id)
        {
            lock (_lock)
            {
                var user = _users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                    return false;

                _users.Remove(user);
                return true;
            }
        }

        public bool EmailExists(string email, Guid? excludeUserId = null)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            lock (_lock)
            {
                return _users.Any(u =>
                    u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                    (!excludeUserId.HasValue || u.Id != excludeUserId.Value));
            }
        }
    }
}