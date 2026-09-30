using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public class UserServices
    {
        private readonly Db _db;
        private readonly ILogger<UserServices> _logger;

        public UserServices(Db db, ILogger<UserServices> logger)
        {
            _db = db;
            _logger = logger;
        }

        public List<User> GetUser(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return _db.Data.Values.ToList();
            }

            if (_db.Data.TryGetValue(id, out var user) && user != null)
            {
                return new List<User> { user };
            }

            return new List<User>();
        }

        public User CreateUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            try
            {
                if (string.IsNullOrWhiteSpace(user.Id))
                {
                    user.Id = Guid.NewGuid().ToString();
                }

                _db.Data[user.Id] = user;
                _logger.LogInformation("User created with id {UserId}", user.Id);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                throw;
            }
        }

        public bool UpdateUser(string id, User updated)
        {
            if (string.IsNullOrWhiteSpace(id) || updated == null) return false;
            try
            {
                if (!_db.Data.ContainsKey(id)) return false;

                updated.Id = id;
                _db.Data[id] = updated;
                _logger.LogInformation("User updated {UserId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", id);
                throw;
            }
        }

        public bool DeleteUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            try
            {
                var removed = _db.Data.Remove(id);
                if (removed) _logger.LogInformation("User deleted {UserId}", id);
                return removed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", id);
                throw;
            }
        }
    }
}
