namespace NutsStore.Models
{
    public class UserInfo
    {
        public UserInfo(string username, string firstname, string lastname, string phoneNumber, string address, string creationDate, string modificationDate, string email, string password, int age, bool? gender, bool isAdmin)
        {
            Username = username;
            Firstname = firstname;
            Lastname = lastname;
            PhoneNumber = phoneNumber;
            Address = address;
            CreationDate = creationDate;
            ModificationDate = modificationDate;
            Email = email;
            Password = password;
            Age = age;
            Gender = gender;
            IsAdmin = isAdmin;
        }
        public int Id { get; set; }

        private string _username;
        public string Username { get { return _username?.ToLower(); } set { _username = value != null ? value.ToLower() : null; } }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CreationDate { get; set; }
        public string ModificationDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public bool? Gender { get; set; }
        public bool IsAdmin { get; set; }
    }
}
