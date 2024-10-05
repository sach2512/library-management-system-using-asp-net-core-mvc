using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Librarymanagement.Models
{
    public class BorrowViewModel
    {
        [Required(ErrorMessage = "Book ID is required.")]
        public int BookId { get; set; }

        // Optional: If you want this to be populated from the controller only and not bound from the form
        [BindNever]
        public string? BookTitle { get; set; }

        [Required(ErrorMessage = "Your name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string BorrowerName { get; set; }

        [Required(ErrorMessage = "Your email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string BorrowerEmail { get; set; }

        [Required(ErrorMessage = "Your Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }
    }
}
