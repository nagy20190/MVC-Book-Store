using AutoMapper;
using BKStore_MVC.Models;
using BKStore_MVC.Repository;
using BKStore_MVC.Repository.Interfaces;
using BKStore_MVC.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using static System.Reflection.Metadata.BlobBuilder;
using System.Net;

namespace BKStore_MVC.Controllers
{
    public class BookController : Controller
    {
        IBookRepository bookRepository;
        ICategoryRepository categoryRepository;
        private readonly IGovernorateRepository governorateRepository;
        IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        IShippingMethodRepository shippingMethodRepository;
        IBookRatingRepository bookRatingRepository;
        public BookController(IBookRepository _bookRepository, IWebHostEnvironment webHostEnvironment,
            IGovernorateRepository governorateRepository,IShippingMethodRepository shippingMethodRepository,
            IMapper mapper,IBookRatingRepository bookRatingRepository,
            ICategoryRepository _categoryRepository)
        {
            this.shippingMethodRepository = shippingMethodRepository;
            bookRepository = _bookRepository;
            categoryRepository = _categoryRepository;
            _mapper = mapper;
            this.governorateRepository = governorateRepository;
            _webHostEnvironment = webHostEnvironment;
            this.bookRatingRepository = bookRatingRepository;
        }
        public IActionResult Index(int? page, string sortOrder)
        {
            if (shippingMethodRepository.GetAll == null)
            {
                ShippingMethod shipping = new ShippingMethod()
                {
                    Name = "Cash on delivery",
                    PaymentFees = 60
                };
                shippingMethodRepository.Add(shipping);
                shippingMethodRepository.Save();
            }
            int pageSize = 12; // Number of items per page
            int pageNumber = (page ?? 1); // Default to page 1 if no page is specified

            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            var books = from b in bookRepository.GetAll()
                        select b;

            switch (sortOrder)
            {
                case "date_desc":
                    books = books.OrderByDescending(b => b.Publishdate);
                    break;
                case "Price":
                    books = books.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    books = books.OrderByDescending(b => b.Price);
                    break;
                default:
                    books = books.OrderBy(b => b.Publishdate);
                    break;
            }

            BookCategVM bookCategVM = new BookCategVM();
            bookCategVM.categories = categoryRepository.GetAll();
            bookCategVM.books = books.ToPagedList(pageNumber, pageSize);

            return View("Index", bookCategVM);
        }
        public IActionResult Details(int Bookid)
        {
            Book book = bookRepository.GetByID(Bookid);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            Category category = categoryRepository.GetByID(book.CategoryID);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            var bookVM = _mapper.Map<BookWithAuthorWithPuplisherWithCategVM>(book);
            bookVM.BookDiscount = book.discount;
            _mapper.Map(category, bookVM); // Map category properties to the view model
            var ratings = bookRatingRepository.GetByBookID(Bookid);
            ViewData["AvgRating"] = ratings.Any() ? Math.Round(ratings.Average(b => b.Rating), 1) : 0;

            return View("Details", bookVM);
        }
        [HttpGet]
        public IActionResult New()
        {
            ViewData["Categories"] = categoryRepository.GetAll();

            return View("New");
        } // Add New Book
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllToAdmin()
        {
            return View("GetAllToAdmin", bookRepository.GetAll());
        }
        [HttpGet]
        public IActionResult SearchByName(string name, int? bookId)
        {
            if (bookId.HasValue)
            {
                // Handle the case where a specific book is selected
                return RedirectToAction("Details", new { id = bookId.Value });
            }

            if (!string.IsNullOrEmpty(name))
            {
                var categories = categoryRepository.GetAll();
                var books = bookRepository.GetByNameList(name);

                var bookCategVM = new BookCategVM
                {
                    categories = categories,
                    books = books.ToPagedList(1, 10), // Assuming you want the first page with 10 items per page
                    SearchName = name
                };

                ViewBag.SearchTerm = name; // Pass the search term to the view

                return View("Index", bookCategVM);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult SearchBooks(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var books = bookRepository.GetByNameList(name);
                return Json(books.Select(b => new { b.BookID, b.Title }));
            }
            return Json(new List<object>());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveNew(Book bookFromRequest, IFormFile ImagePath)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ImagePath != null)
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImagePath.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImagePath.CopyToAsync(fileStream);
                        }
                        bookFromRequest.ImagePath = uniqueFileName;
                    }

                    if (bookFromRequest.Publishdate == null)
                        bookFromRequest.Publishdate = DateTime.Now;

                    bookRepository.Add(bookFromRequest);
                    bookRepository.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                }
            }

            ViewData["Categories"] = categoryRepository.GetAll();
            return View("New", bookFromRequest);
        }
        [Authorize]
        [HttpPost]
        public IActionResult RateBook([FromBody] RatingModel ratingModel)
        {
            if (ratingModel == null || ratingModel.BookId <= 0 || ratingModel.Rating <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }
            if (User.Identity.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                bookRepository.RateBook(ratingModel.BookId, ratingModel.Rating, userId);
            }
            else
            {
                bookRepository.RateBook(ratingModel.BookId, ratingModel.Rating, null);
            }
            bookRepository.Save();

            var book = bookRepository.GetByID(ratingModel.BookId);
            return Ok(new { success = true, averageRating = book.AverageRating });
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var bookModel = bookRepository.GetByID(id);

            var bookVM = _mapper.Map<BookWithAuthorWithPuplisherWithCategVM>(bookModel);
            bookVM.categories = categoryRepository.GetAll();

            return View("Edit", bookVM);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveEdit(int id, BookWithAuthorWithPuplisherWithCategVM bookFromRequest)
        {
            if (ModelState.IsValid)
            {
                //try
                //{
                var bookFromDB = bookRepository.GetByID(id);
                if (bookFromDB == null)
                {
                    return NotFound();
                }

                if (bookFromRequest.ImagePath != null)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + bookFromRequest.ImagePath.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await bookFromRequest.ImagePath.CopyToAsync(fileStream);
                    }
                    bookFromDB.ImagePath = uniqueFileName;
                    bookFromRequest.BookImagePath = uniqueFileName; // Update the ViewModel
                }
                else
                {
                    bookFromRequest.BookImagePath = bookFromDB.ImagePath; // Retain the existing image path
                }

                // Map the properties from bookFromRequest to bookFromDB, excluding ImagePath
                _mapper.Map(bookFromRequest, bookFromDB);
                bookRepository.Update(bookFromDB);
                bookRepository.Save();
                return RedirectToAction("GetAllToAdmin");
                //}
                //catch (Exception ex)
                //{
                //    string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                //    ModelState.AddModelError(string.Empty, errorMessage);
                //}
            }

            bookFromRequest.categories = categoryRepository.GetAll();
            return View("Edit", bookFromRequest);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            // Fetch the book to delete
            Book bookFromDB = bookRepository.GetByID(id);

            // If the book does not exist, return a NotFound view or error
            if (bookFromDB == null)
            {
                return NotFound();
            }
            bookRepository.Delete(id);
            bookRepository.Save();

            return RedirectToAction("GetAllToAdmin"); // Display confirmation before deletion
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Fetch the book to delete
            Book bookFromDB = bookRepository.GetByID(id);

            // Check if the book exists
            if (bookFromDB != null)
            {
                // Delete the book from the database
                bookRepository.Delete(id);

                // Save changes to the database
                bookRepository.Save();
            }

            // Redirect to Index after deletion
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult AddToCart(int bookId, int Quantity)
        {
            ViewData["PaymentFees"] = shippingMethodRepository.GetByID(1).PaymentFees;
            var cookie = Request.Cookies["Cart"];
            List<BookCartItem> cartItems;

            if (cookie != null)
            {
                // Deserialize the existing cookie value
                cartItems = JsonConvert.DeserializeObject<List<BookCartItem>>(cookie) ?? new List<BookCartItem>();

            }
            else
            {
                // Initialize a new list if the cookie does not exist
                cartItems = new List<BookCartItem>();
            }
            Book book = bookRepository.GetByID(bookId);
            // Add the new item to the list
            if (cartItems.Any(item => item.BookId == bookId))
            {
                BookCartItem bookCart = cartItems.Where(item => item.BookId == bookId).FirstOrDefault();
                if (bookCart.StockQuantity >= Quantity + bookCart.Quantity)
                {
                    bookCart.Quantity = Quantity + bookCart.Quantity;
                }
                else
                {
                    bookCart.Quantity = bookCart.StockQuantity;
                }
            }
            else
            {
                cartItems.Add(new BookCartItem
                {
                    BookId = bookId,
                    Quantity = Quantity,
                    ImagePath = book.ImagePath,
                    Title = book.Title,
                    Price = book.Price,
                    StockQuantity = bookRepository.GetByID(bookId).StockQuantity
                });
            }

            // Serialize the updated list
            string serializedCartItems = JsonConvert.SerializeObject(cartItems);

            // Create or update the cookie
            Response.Cookies.Append("Cart", serializedCartItems, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7) // Set the cookie to expire in 7 days
            });
            return RedirectToAction(nameof(ShowCart));
            //return RedirectToAction(nameof(ShowCart));
        }
        public IActionResult ShowCart()
        {
            ViewData["PaymentFees"] = shippingMethodRepository.GetByID(1).PaymentFees;

            // Retrieve the existing cookie
            var cookie = Request.Cookies["Cart"];
            List<BookCartItem> cartItems;

            if (cookie != null)
            {
                // Deserialize the existing cookie value
                cartItems = JsonConvert.DeserializeObject<List<BookCartItem>>(cookie);
            }
            else
            {
                // Initialize an empty list if the cookie does not exist
                cartItems = new List<BookCartItem>();
            }


            // Pass the ViewModel to the view
            return View("Cart", cartItems);
        }
        public IActionResult RemoveFromCart(int bookId)
        {
            // Retrieve the existing cookie
            var cookie = Request.Cookies["Cart"];
            List<BookCartItem> cartItems;

            if (cookie != null)
            {
                // Deserialize the existing cookie value
                cartItems = JsonConvert.DeserializeObject<List<BookCartItem>>(cookie);
            }
            else
            {
                // Initialize an empty list if the cookie does not exist
                cartItems = new List<BookCartItem>();
            }

            // Find the item to remove
            var itemToRemove = cartItems.Find(item => item.BookId == bookId);
            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
            }

            // Serialize the updated list
            string serializedCartItems = JsonConvert.SerializeObject(cartItems);

            // Create or update the cookie
            Response.Cookies.Append("Cart", serializedCartItems, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7) // Set the cookie to expire in 7 days
            });

            // Calculate new totals
            var newSubtotal = cartItems.Sum(item => item.Price * item.Quantity);
            var newTotal = newSubtotal + shippingMethodRepository.GetByID(1).PaymentFees; // Assuming a fixed shipping cost of 50 EGP

            // Return the new totals as JSON
            return Json(new { newSubtotal, newTotal });
        }
        public IActionResult GetBooksCategory(int ID)
        {
            BookCategVM bookCategVM = new BookCategVM();
            bookCategVM.categories = categoryRepository.GetAll();
            var books = bookRepository.GetBooksByCatgyId(ID);
            bookCategVM.books = books.ToPagedList(pageNumber: 1, pageSize: 10); // Adjust pageNumber and pageSize as needed
            return View("Index", bookCategVM);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DetailedBookForAdmin(int ID)
        {
            return View("DetailedBookForAdmin", bookRepository.GetByID(ID));
        }
        private static List<BookCartItem> cart = new List<BookCartItem>();
        [HttpPost]
        public IActionResult UpdateQuantity(int bookId, int quantity)
        {
            var item = cart.FirstOrDefault(i => i.BookId == bookId);
            if (item == null)
            {
                return Json(new { success = false, message = "Item not found in cart." });
            }

            item.Quantity = quantity;

            var newSubtotal = item.Price * item.Quantity;
            var newCartSubtotal = cart.Sum(i => i.Price * i.Quantity);
            var newCartTotal = newCartSubtotal + shippingMethodRepository.GetByID(1).PaymentFees; // Assuming a fixed shipping cost

            return Json(new
            {
                success = true,
                newSubtotal = newSubtotal,
                newCartSubtotal = newCartSubtotal,
                newCartTotal = newCartTotal
            });
        }
        [HttpPost]
        public IActionResult AddToWishlist(int bookId)
        {
            try
            {
                var cookie = Request.Cookies["Wishlist"];
                List<int> Books;

                if (cookie != null)
                {
                    // Deserialize the existing cookie value
                    Books = JsonConvert.DeserializeObject<List<int>>(cookie);
                }
                else
                {
                    // Initialize a new list if the cookie does not exist
                    Books = new List<int>();
                }

                // Check if the book exists
                Book book = bookRepository.GetByID(bookId);
                if (book == null)
                {
                    return NotFound("Book not found");
                }

                // Add the new item to the list if it doesn't already exist
                if (!Books.Contains(bookId))
                {
                    Books.Add(bookId);
                }

                // Serialize the updated list
                string serializedCartItems = JsonConvert.SerializeObject(Books);

                // Create or update the cookie
                Response.Cookies.Append("Wishlist", serializedCartItems, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(7) // Set the cookie to expire in 7 days
                });

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult RemoveFromWishlist(int bookId)
        {
            var cookie = Request.Cookies["Wishlist"];
            List<int> Books;

            if (cookie != null)
            {
                // Deserialize the existing cookie value
                Books = JsonConvert.DeserializeObject<List<int>>(cookie);
            }
            else
            {
                // Initialize a new list if the cookie does not exist
                Books = new List<int>();
            }

            // Remove the item from the list
            if (Books.Contains(bookId))
            {
                Books.Remove(bookId);
            }
            List<Book> books = new List<Book>();
            foreach (var item in Books)
            {
                Book book99 = bookRepository.GetByID(item);
                books.Add(book99);
            }

            // Serialize the updated list
            string serializedCartItems = JsonConvert.SerializeObject(Books);

            // Create or update the cookie
            Response.Cookies.Append("Wishlist", serializedCartItems, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7) // Set the cookie to expire in 7 days
            });

            return RedirectToAction("ShowWishlist");
        }
        public IActionResult ShowWishlist()
        {
            var cookie = Request.Cookies["Wishlist"];
            List<int> Books;

            if (cookie != null)
            {
                // Deserialize the existing cookie value
                Books = JsonConvert.DeserializeObject<List<int>>(cookie);
            }
            else
            {
                // Initialize a new list if the cookie does not exist
                Books = new List<int>();
            }

            // Remove the item from the list
            List<Book> books = new List<Book>();
            if (Books != null)
            {
                foreach (var item in Books ?? new List<int>())
                {
                    Book book99 = bookRepository?.GetByID(item);
                    if (book99 != null)
                    {
                        books.Add(book99);
                    }
                }
                return View("Wishlist", books);
            }

            return View("Wishlist", books);
        }

    }
}
#region MyImportantTests
//public IActionResult Index(int? page)
//{
//    int pageSize = 10; // Number of items per page
//    int pageNumber = (page ?? 1); // Default to page 1 if no page is specified

//    BookCategVM bookCategVM = new BookCategVM();
//    bookCategVM.categories = categoryRepository.GetAll();
//    bookCategVM.books = bookRepository.GetAll().ToPagedList(pageNumber, pageSize);

//    return View("Index", bookCategVM);
//} // Show All Books

//public IActionResult SearchByName(string name)
//{
//        if (name != null)
//        {
//            var categories = categoryRepository.GetAll();
//            var books = bookRepository.GetByName(name);

//            var bookCategVM = new BookCategVM
//            {
//                categories = categories,
//                books = _mapper.Map<List<Book>>(books),
//                SearchName = name
//            };

//            return View("Index", bookCategVM);
//        }
//        return RedirectToAction(nameof(Index));
//}

//public IActionResult SaveNew(Book bookFromRequest)
//{
//    if (ModelState.IsValid)
//    {
//        try
//        {
//            //save
//            if (bookFromRequest.Publishdate==null)
//            bookFromRequest.Publishdate= DateTime.Now;
//            bookRepository.Add(bookFromRequest);
//            bookRepository.Save();
//            return RedirectToAction("Index");
//        }
//        catch (Exception ex)
//        {
//            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
//        }
//    }
//    ViewData["CategoryName"] = categoryRepository.GetAll();
//    return RedirectToAction("New", bookFromRequest);
//} // Save Data
//public IActionResult BuyNow(int bookId, int Quantity)
//{
//    Book book = bookRepository.GetByID(bookId);
//    BookCartItem cartItem = new BookCartItem()
//    {
//        BookId=bookId,
//        Quantity = Quantity,
//        ImagePath=book.ImagePath,
//        Title = book.Title,
//        Price = book.Price

//    };
//    List<BookCartItem> cartItems = new List<BookCartItem>();
//    cartItems.Add(item: cartItem);
//    ViewData["Governoratelst"] = governorateRepository.GetAll();
//    CustomerOrderVM customerOrderVM = new CustomerOrderVM
//    {
//        BookItems = cartItems,
//        TotalAmount = (decimal?)(book.Price * Quantity)
//    };
//    return View("AddCustomer", customerOrderVM);
//}

//[HttpPost]
//public IActionResult SaveEdit(int id, BookWithAuthorWithPuplisherWithCategVM bookFromRequest)
//{
//    if (ModelState.IsValid)
//    {
//        try
//        {
//            Book bookFromDB =
//                bookRepository.GetByID(id);

//            bookFromDB.Title = bookFromRequest.Title;
//            bookFromDB.AuthorName = bookFromRequest.AuthorName;
//            bookFromDB.StockQuantity = bookFromRequest.StockQuantity;
//            bookFromDB.Price = bookFromRequest.Price;
//            bookFromDB.PublisherName = bookFromRequest.PublisherName;
//            bookFromDB.Description = bookFromRequest.Description;
//            bookFromDB.ImagePath = bookFromRequest.BookImagePath;
//            bookFromDB.CategoryID = bookFromRequest.CategoryID;

//            bookRepository.Save();
//            return RedirectToAction("Index");
//        }
//        catch (Exception ex)
//        {
//            string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
//            ModelState.AddModelError(string.Empty, errorMessage);
//        }
//    }

//    bookFromRequest.categories = categoryRepository.GetAll();
//    return View("Edit", bookFromRequest);
//}

// Delete
//public IActionResult Edit(int id)
//{
//    Book bookModel = bookRepository.GetByID(id);

//    BookWithAuthorWithPuplisherWithCategVM bookVM =
//        new BookWithAuthorWithPuplisherWithCategVM();

//    bookVM.BookID = id;
//    bookVM.Title = bookModel.Title;
//    bookVM.AuthorName = bookModel.AuthorName;
//    bookVM.StockQuantity = bookModel.StockQuantity;
//    bookVM.Price = bookModel.Price;
//    bookVM.BookImagePath = bookModel.ImagePath;
//    bookVM.categories = categoryRepository.GetAll();
//    bookVM.PublisherName = bookModel.PublisherName;
//    bookVM.Description = bookModel.Description;
//    bookVM.CategoryID = bookModel.CategoryID;
//    bookVM.categories = categoryRepository.GetAll();

//    return View("Edit", bookVM);
//}

//public IActionResult RemoveFromCart(int bookId)
//{
//    // Retrieve the existing cookie
//    var cookie = Request.Cookies["Cart"];
//    List<BookCartItem> cartItems;

//    if (cookie != null)
//    {
//        // Deserialize the existing cookie value
//        cartItems = JsonConvert.DeserializeObject<List<BookCartItem>>(cookie);
//    }
//    else
//    {
//        // Initialize an empty list if the cookie does not exist
//        cartItems = new List<BookCartItem>();
//    }

//    // Find the item to remove
//    var itemToRemove = cartItems.Find(item => item.BookId == bookId);
//    if (itemToRemove != null)
//    {
//        cartItems.Remove(itemToRemove);
//    }

//    // Serialize the updated list
//    string serializedCartItems = JsonConvert.SerializeObject(cartItems);

//    // Create or update the cookie
//    Response.Cookies.Append("Cart", serializedCartItems, new CookieOptions
//    {
//        Expires = DateTimeOffset.Now.AddDays(7) // Set the cookie to expire in 7 days
//    });

//    return RedirectToAction("ShowCart");
//}

//public IActionResult Details(int Bookid)
//{
//    Book book = bookRepository.GetByID(Bookid);
//    if (book == null)
//    {
//        return NotFound("Book not found.");
//    }

//    Category category = categoryRepository.GetByID(book.CategoryID);

//    BookWithAuthorWithPuplisherWithCategVM bookVM =
//        new BookWithAuthorWithPuplisherWithCategVM();

//    // Pass Book Props to Book View Model Class
//    bookVM.BookID = book.BookID;
//    bookVM.BookImagePath = book.ImagePath;
//    bookVM.Title = book.Title;
//    bookVM.Price = book.Price;
//    bookVM.StockQuantity = book.StockQuantity;
//    bookVM.Description = book.Description;
//    bookVM.CategoryID = category.CategoryID;
//    bookVM.CategoryName = category.Name;

//    return View("Details", bookVM);
//} // Show Book by id

//public IActionResult Cart(int Quantity)
//{

//var bookID = Request.Cookies["BookID"].ToList();
//var cartitem = new List<BookCartItem>();
//foreach (var item in bookID) {
//    Book book = bookRepository.GetByID(item);
//    BookCartItem Cart = new BookCartItem
//    {
//        BookId = book.BookID,
//        Title = book.Title ?? "",
//        Price = book.Price,
//        Quantity = Quantity // Example quantity
//    };
//    cartitem.Add(Cart);
//}

//return View("Cart", cartitem);

//    var cartItems = new List<BookCartItem>
//{
//    new BookCartItem
//    {
//        BookId = book.BookID,
//        Title = book.Title??"",
//        Price = book.Price,
//        Quantity = Quantity // Example quantity
//    }
//};

//    string DeliveryIDValue = UserID;
//    CookieOptions options = new CookieOptions
//    {
//        Expires = DateTime.Now.AddDays(1)
//    };
//    Response.Cookies.Append("Did", DeliveryIDValue, options);


//    var bookId = Request.Cookies["BookID"];

//    // Your logic to get the book details using the bookId
//    var book = bookRepository.GetByID(int.Parse(bookId ?? "0"));

//    // Create a list of BookCartItem objects
//    var cartItems = new List<BookCartItem>
//{
//    new BookCartItem
//    {
//        BookId = book.BookID,
//        Title = book.Title??"",
//        Price = book.Price,
//        Quantity = 1 // Example quantity
//    }
//};

//}

// Set the cookie with the book ID
//CookieOptions options = new CookieOptions
//{
//    Expires = DateTime.Now.AddDays(7) // Set the cookie to expire in 7 days
//};
//Response.Cookies.Append("BookID", bookId.ToString(), options);

//Book book = bookRepository.GetByID(bookId);

//BookCartItem cart =
//    new BookCartItem
//    {
//        BookId = book.BookID,
//        Title = book.Title ?? "",
//        Price = book.Price,
//        Quantity = Quantity // Example quantity
//    };
//var cartItems =new List<BookCartItem>();
//cartItems.Add(cart);
//return View("Cart", cartItems);
////return RedirectToAction("Cart", Quantity);
#endregion