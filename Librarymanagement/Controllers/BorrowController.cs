using Librarymanagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Librarymanagement.Controllers
{
    public class BorrowController : Controller
    {
        private readonly LibrarayContext _context;

        public BorrowController(LibrarayContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Borrow(int? BookId)
        {
            var book = await _context.Books.FindAsync(BookId);
            if (book == null)
            {
                TempData["Errormessage"] = "no book found with that Id";
                return View("Error");
            }
            else
            {
                TempData["Sucessfull"] = "book found successfull";
                TempData["BookId"] = book.BookId;
                TempData["BookTitle"] = book.Title;
                TempData["Author"] = book.Author;
                return View();
            }
        }

        public async Task<IActionResult> SubmitBorrow(BorrowViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var book = await _context.Books.FindAsync(model.BookId);
                if (book == null)
                {
                    TempData["ErrorMessage"] = " no book found with that Id";
                    return View("Error");
                }
                else
                {
                    BorrowRecord record = new BorrowRecord()
                    {
                        BookId = model.BookId,
                        BorrowerName = model.BorrowerName,
                        BorrowerEmail = model.BorrowerEmail,
                        Phone = model.Phone,
                        BorrowDate = DateTime.Now
                    };
                    book.IsAvailable = false;

                    _context.Add(record);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Successfully borrowed the book: {book.Title}.";
                    return RedirectToAction("Index", "Book");
                }
            }
        }

        public async Task<IActionResult> Return(int borrowRecordId)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Model is not valid";
                return View("Error");
            }

            // Fetch the borrow record
            var borrowrecord = await _context.BorrowRecords.FindAsync(borrowRecordId);

            // Check if the record was found
            if (borrowrecord == null)
            {
                TempData["Error"] = "Borrow record not found.";
                return View("Error");
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(ReturnViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var borrowRecord = await _context.BorrowRecords
                    .Include(br=>br.Book)
                    .FirstOrDefaultAsync(br => br.BorrowRecordId == model.BorrowRecordId);
                if (borrowRecord == null)
                {
                    TempData["ErrorMessage"] = $"No borrow record found with ID {model.BorrowRecordId} to return.";
                    return View("NotFound");
                }
                if (borrowRecord.ReturnDate != null)
                {
                    TempData["ErrorMessage"] = $"The borrow record for '{borrowRecord.Book.Title}' has already been returned.";
                    return View("AlreadyReturned");
                }

                borrowRecord.ReturnDate = DateTime.UtcNow;

                borrowRecord.Book.IsAvailable = true;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Successfully returned the book: {borrowRecord.Book.Title}.";
                return RedirectToAction("Index", "Books");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing the return action.";
                return View("Error");
            }
        }
    }
}
}
