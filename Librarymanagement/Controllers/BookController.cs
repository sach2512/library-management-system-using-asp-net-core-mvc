using Librarymanagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Librarymanagement.Controllers
{
    public class BookController : Controller
    {
        private readonly LibrarayContext _context;

        public BookController(LibrarayContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var books = await _context.Books.Include(b => b.BorrowRecords)
                                                  .AsNoTracking()
                                                  .ToListAsync();
                return View(books);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while fetching the list of books.";
                return RedirectToAction("Error");
            }
        }

        public async Task<IActionResult> Details(int bookId)
        {
            if (bookId == 0) // Check if the BookId is valid
            {
                TempData["ErrorMessage"] = "Invalid book ID.";
                return RedirectToAction("Error");
            }

            try
            {
                var book = await _context.Books.FindAsync(bookId);

                if (book == null)
                {
                    TempData["ErrorMessage"] = "No book found with the given ID.";
                    return RedirectToAction("Error");
                }

                return View(book);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while fetching the book details.";
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        public IActionResult Error()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int BookId)
        {
            try
            {
                if (BookId <= 0) 
                {
                    TempData["ErrorMessage"] = "Invalid book ID.";
                    return RedirectToAction("Error");
                }

               
                var book = await _context.Books.FindAsync(BookId);

                if (book == null) 
                {
                    TempData["ErrorMessage"] = "No book found with that Id.";
                    return RedirectToAction("Error");
                }

                
                return View(book);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while trying to fetch the book.";
                return RedirectToAction("Error");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int BookId,Book book)
        {
            try
            {
                var existingBook = await _context.Books.FindAsync(BookId);
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.ISBN = book.ISBN;
                existingBook.PublishedDate = book.PublishedDate;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }catch(DbUpdateConcurrencyException ex)
            {
                if(book.BookId==0)
                {
                    TempData["ErrorMessage"] = " book was deleted";
                    return View("Error");
                }
                else
                {
                    TempData["ErrorMessage"] = "A concurrency error occurred during the update.";
                    return View("Error");
                }
            }

        }
        public async Task<IActionResult> Delete(int? BookId)
        {
            
            if (BookId == null)
            {
                return NotFound();
            }

           
            var book = await _context.Books.FindAsync(BookId);

           
            if (book == null)
            {
                return NotFound();
            }

            
            return View(book); 
        }
        public async Task<IActionResult> DeleteConfirmed(int BookId)
        {
            var book = await _context.Books.FindAsync(BookId);
            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }

    }
