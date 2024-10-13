using AutoMapper;
using BKStore_MVC.Models;
using BKStore_MVC.Repository;
using BKStore_MVC.Repository.Interfaces;
using BKStore_MVC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKStore_MVC.Controllers
{
    [Authorize(Roles = "Admin")]

    public class CategoryController : Controller
    {
        ICategoryRepository categoryRepository;
        IBookRepository bookRepository;
        IMapper _mapper;
        public CategoryController(ICategoryRepository _categoryRepository, IMapper mapper, IBookRepository _bookRepository)
        {
            categoryRepository = _categoryRepository;
            bookRepository = _bookRepository;
            _mapper = mapper;
        }
        // DONE !
        public IActionResult Index()
        {
            return View("Index", categoryRepository.GetAll());
        }

        // DONE !
        public IActionResult Details(int id)
        {
            var categoryFromDB = categoryRepository.GetByID(id);
            if (categoryFromDB == null)
            {
                return NotFound("Category Not Found");
            }

            var books = bookRepository.GetBooksByCatgyId(categoryFromDB.CategoryID);
            if (books == null || !books.Any())
            {
                return NotFound("There are no books in this category");
            }

            var bookVM = _mapper.Map<BookWithCategoryVM>(categoryFromDB);
            bookVM.books = books;

            return View("Details", bookVM);
        }
                public IActionResult New()
        {
            return View("New");
        }

        // DONE !
        public IActionResult SaveNew(int id, Category categoryFromRequest)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Add(categoryFromRequest);
                categoryRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Edit", categoryFromRequest);
        }

        // DONE !
        public IActionResult Edit(int id)
        {
           Category categoryFromDB = categoryRepository.GetByID(id);
            if (categoryFromDB == null)
            {
                return NotFound("Category Not Found");
            }
            return View("Edit", categoryFromDB); 
        }

        // DONE !
        public IActionResult SaveEdit(int id, Category categoryFromRequest)
        {
            
                if (ModelState.IsValid)
                {
                    var categoryFromDB = categoryRepository.GetByID(id);
                    if (categoryFromDB == null)
                    {
                        return NotFound("Not Found");
                    }

                    // Map the properties from categoryFromRequest to categoryFromDB
                    _mapper.Map(categoryFromRequest, categoryFromDB);

                    // Save 
                    categoryRepository.Update(categoryFromDB);
                    categoryRepository.Save();

                    // Redirect
                    return RedirectToAction("Index");
                }

                return View("Edit", categoryFromRequest);
            
        }

        // DONE !
        public IActionResult Delete(int id)
        {
           Category categoryFromDB =  categoryRepository.GetByID(id);

            if (categoryFromDB == null)
            {
                return NotFound("Not Found");
            }
            return View("Delete", categoryFromDB);
        }

        // DONE !
        public IActionResult ConfirmDelete(int id)
        {
            Category categoryFromDB = categoryRepository.GetByID(id);
            if (categoryFromDB == null)
            {
                return NotFound("Not Found");
            }

            categoryRepository.Delete(id);
            categoryRepository.Save();
            return RedirectToAction("Index");
        }


    }
}
#region Test
//public IActionResult Details(int id)
//{
//   Category categoryFromDB = categoryRepository.GetByID(id);
//    if (categoryFromDB == null)
//    {
//        return NotFound("Category Not Found");
//    }
//    List<Book> Books = bookRepository.GetBooksByCatgyId(categoryFromDB.CategoryID);

//    if (Books == null)
//    {
//        return NotFound("There is no Books in this Category");
//    }

//    BookWithCategoryVM bookVM = new BookWithCategoryVM();

//    bookVM.CategoryId = categoryFromDB.CategoryID;
//    bookVM.CategoryName = categoryFromDB.Name;
//    bookVM.books = Books;

//    return View("Details", bookVM);
//}

// DONE !

#endregion