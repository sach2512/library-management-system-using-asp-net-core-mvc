using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Librarymanagement.Models
{
    public class BorrowRecord
    {
        [Key]
        public int BorrowRecordId { get; set; }
        [Required]
        public int BookId { get; set; } //FK
        [Required(ErrorMessage ="BorrowerName is required ")]
       
        public string BorrowerName { get; set; }
        [Required(ErrorMessage ="Email id is required")]
        [EmailAddress(ErrorMessage ="please eneter a valid email address")]
        public string BorrowerEmail { get; set; }
        [Required(ErrorMessage ="Phone number is Required")]
        [Phone(ErrorMessage ="Please enter a valid number")]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime? ReturnDate { get; set; }
        [BindNever]
        public Book Book { get; set; }
    }
}
