using System;
using System.ComponentModel.DataAnnotations;

namespace LibApp.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Author name is required.")]
        public string? AuthorName { get; set; }

        [Required(ErrorMessage = "Genre is required.")]
        public Genre Genre { get; set; }

        [Required(ErrorMessage = "Please select an appropriate genre from the list.")]
        public byte? GenreId { get; set; }

        public DateTime DateAdded { get; set; }

        [Required(ErrorMessage = "Release Date is required.")]
        public DateTime? ReleaseDate { get; set; }

        [Required]
        [Range(1,20, ErrorMessage = "Number in stock must be between 1 and 20.")]
        public int? NumberInStock { get; set; }

        public int NumberAvailable { get; set; }
    }

}
