namespace Cinema.Models
{
    public class UserProfile
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Balance { get; set; }
        public bool IsAdmin { get; set; }
    }
}
