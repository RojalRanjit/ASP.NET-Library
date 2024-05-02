using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }    
        public string AuthorEmail { get; set; } 
        public string Password { get; set; }
        public int? BookCount { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
