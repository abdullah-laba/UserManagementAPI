using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public class UserServices
    {
        private readonly Db _db;

        public UserServices(Db db)
        {
            _db = db;
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
            if (string.IsNullOrWhiteSpace(user.Id))
            {
                user.Id = Guid.NewGuid().ToString();
            }

            _db.Data[user.Id] = user;
            return user;
        }

        public bool UpdateUser(string id, User updated)
        {
            if (string.IsNullOrWhiteSpace(id) || updated == null) return false;
            if (!_db.Data.ContainsKey(id)) return false;

            updated.Id = id;
            _db.Data[id] = updated;
            return true;
        }

        public bool DeleteUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            return _db.Data.Remove(id);
        }
    }
}
