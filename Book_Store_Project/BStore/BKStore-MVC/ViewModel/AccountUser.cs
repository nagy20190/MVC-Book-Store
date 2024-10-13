namespace BKStore_MVC.ViewModel
{
    public class AccountUser
    {
        public string? ImagePath { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public OrderDetailVM? OrderDetail { get; set; }

    }
}
