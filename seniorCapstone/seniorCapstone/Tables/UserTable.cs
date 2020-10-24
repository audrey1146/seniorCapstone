using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace seniorCapstone.Tables
{
    [Table("UserTable")]
	class UserTable
	{
        [PrimaryKey, AutoIncrement]
        [Column("UID")]
        public int UID { get; set; }

        [Unique]
        [Column("UserName")]
        public string UserName { get; set; }

        [Column("FirstName")]
        public string FirstName { get; set; }

        [Column("LastName")]
        public string LastName { get; set; }

        [Unique]
        [Column("Email")]
        public string Email { get; set; }
    }
}
