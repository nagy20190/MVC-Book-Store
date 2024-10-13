using BKStore_MVC.Models;
using BKStore_MVC.Models.Context;
using BKStore_MVC.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BKStore_MVC.Repository
{
    public class BookRepository : IBookRepository
    {
        BKstore_System context;
        public BookRepository(BKstore_System _context)
        {
            context = _context;
        }
        public void Add(Book book)
        {
            context.Add(book);
        }

        public void Delete(int ID)
        {
            Book book = GetByID(ID);
            context.Remove(book);
        }

        public List<Book> GetAll()
        {
            return context.Book.ToList();
        }
        public List<Book> GetBooksByCatgyId(int id)
        {
            return context.Book.Where(B => B.CategoryID == id).ToList();
        }
        public Book GetByID(int ID)
        {
            return context.Book.FirstOrDefault(c => c.BookID== ID);
        }
        public List<Book> GetByName(string name)
        {
            return context.Book.Where(b => b.Title.Contains(name)).ToList();
        }
        public void Save()
        {
            context.SaveChanges();
        }
        public void RateBook(int BookID, int rating, string? UserID)
        {
            var book = context.Book.Include(b => b.Ratings).FirstOrDefault(b => b.BookID == BookID);

            if (book != null)
            {
                BookRating userRating;

                if (UserID != null)
                {
                    userRating = book.Ratings?.FirstOrDefault(r => r.UserID == UserID);
                    if (userRating == null)
                    {
                        userRating = new BookRating { BookID = BookID, UserID = UserID, Rating = rating };
                        if (book.Ratings == null)
                        {
                            book.Ratings = new List<BookRating>();
                        }
                        book.Ratings.Add(userRating);
                    }
                    else
                    {
                        userRating.Rating = rating;
                    }
                }
                else
                {
                    // Handle guest rating
                    var guestRating = book.Ratings?.FirstOrDefault(r => r.UserID == null);
                    if (guestRating == null)
                    {
                        guestRating = new BookRating { BookID = BookID, Rating = rating };
                        if (book.Ratings == null)
                        {
                            book.Ratings = new List<BookRating>();
                        }
                        book.Ratings.Add(guestRating);
                    }
                    else
                    {
                        guestRating.Rating = rating;
                    }
                }

                book.AverageRating = book.Ratings.Average(r => r.Rating);
            }
        }
        public void Update(Book book)
        {
            context.Update(book);
        }
        public IEnumerable<Book> GetByNameList(string name)
        {
            return context.Book.Where(b => b.Title.ToLower().Contains(name.ToLower())).ToList();
        }
    }
}
