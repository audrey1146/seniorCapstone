using SQLite;
using SQLiteNetExtensions.Attributes;

namespace seniorCapstone.Tables
{
    [Table ("UserTable")]
    public class UserTable
    {
        [PrimaryKey, AutoIncrement]
        [Column ("UID")]
        public string UID { get; set; }

        [Unique]
        [Column ("UserName")]
        public string UserName { get; set; }

        [Column ("Password")]
        public string Password { get; set; }

        [Column ("FirstName")]
        public string FirstName { get; set; }

        [Column ("LastName")]
        public string LastName { get; set; }

        [Unique]
        [Column ("Email")]
        public string Email { get; set; }
    }
}
