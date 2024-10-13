namespace BKStore_MVC.ViewModel
{
    public class DetaledUserInfoVM
    {
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsLocked { get; set; }
        public string? Role { get; set; }
        public int? CustomerID { get; set; }
        public int? OrderID { get; set; }
        public string? ImagePath { get; set; }

    }
}
