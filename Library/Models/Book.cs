using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }
        public string Title { get; set; }
        //public string? Genre { get; set; }
        public string Language { get; set; }
        public string Image { get; set; }
        public int? Likes { get; set; }
        public int? Comments { get; set; }
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        [ForeignKey("Genre")]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
