using System.Collections;

namespace UserManagementAPI.Models
{
    public class Db
    {
        public Dictionary<string, User> Data { get; set; } = new Dictionary<string, User>(); 
    }
}
