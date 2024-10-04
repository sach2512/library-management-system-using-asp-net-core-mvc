using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Librarymanagement.Models
{
    public class Book
    {
        [BindNever] 
        public int BookId {  get; set; }
        [Required(ErrorMessage ="book name is required")]
        [MinLength(3,ErrorMessage ="length,must be greate than 3")]
        [MaxLength(100,ErrorMessage ="length must be less than 100")]
        public string Title { get; set; }
        [Required(ErrorMessage ="Author Name is Required")]
        [StringLength(30,ErrorMessage ="author name vant be greater than 30")]
        public string Author { get; set; }
        [Required]
        [RegularExpression(@"^\d{3}-\d{10}$",ErrorMessage ="the pattern should match")]

        public string ISBN { get; set; }
        [Required(ErrorMessage ="date is required")]
        [DataType(DataType.Date)]
       [Display(Name="Published Date")]
        public DateTime PublishedDate { get; set; }
        [BindNever]
        [Display(Name =" Avaiablable")]

        public bool IsAvailable { get; set; } = true;
        [BindNever]
        public ICollection<BorrowRecord> BorrowRecords { get; set; }
    }
}
