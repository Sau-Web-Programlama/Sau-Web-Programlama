namespace SporSalonu2.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Member";

        // EKSİK OLAN BU SATIRI EKLE:
        public string Phone { get; set; }
    }
}