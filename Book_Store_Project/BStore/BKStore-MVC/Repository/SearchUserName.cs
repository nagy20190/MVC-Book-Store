using BKStore_MVC.Models.Context;
using BKStore_MVC.Repository.Interfaces;

namespace BKStore_MVC.Repository
{
    public class SearchUserName : ISearchUserName
    {
        BKstore_System context;
        public SearchUserName(BKstore_System bStore_Context) {
            context = bStore_Context;
        
        }
        public bool IsUserNameUnique(string userName)
        {
            var user = context.Users.FirstOrDefault(x => x.UserName == userName);
            return (user == null);
        }
    }
}
