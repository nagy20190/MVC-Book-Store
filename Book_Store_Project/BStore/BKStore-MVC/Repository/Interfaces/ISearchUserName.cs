namespace BKStore_MVC.Repository.Interfaces
{
    public interface ISearchUserName
    {
        public bool IsUserNameUnique(string userName);
    }
}
