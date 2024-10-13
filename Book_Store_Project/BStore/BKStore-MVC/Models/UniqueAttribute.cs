using BKStore_MVC.Models.Context;
using BKStore_MVC.Repository.Interfaces;
using BKStore_MVC.Repository;
using System.ComponentModel.DataAnnotations;

namespace BKStore_MVC.Models
{
    public class UniqueAttribute : ValidationAttribute
    {

        ISearchUserName Search;
        public UniqueAttribute()
        {
            Search = new SearchUserName(new BKstore_System());
        }


        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return null;
            string name = value.ToString() ?? "";
            if (Search.IsUserNameUnique(name))
            {

                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(name);
            }
        }
    }
}
