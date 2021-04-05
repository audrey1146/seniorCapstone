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


        public UserTable ()
		{
            this.UID = string.Empty;
            this.UserName = string.Empty;
            this.Password = string.Empty;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Email = string.Empty;
        }


		public UserTable (ref UserTable userEntry)
		{
            this.UID = userEntry.UID;
            this.UserName = userEntry.UserName;
            this.Password = userEntry.Password;
            this.FirstName = userEntry.FirstName;
            this.LastName = userEntry.LastName;
            this.Email = userEntry.Email;
		}
    }
}
