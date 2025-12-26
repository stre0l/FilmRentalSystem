using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmRentalSystem.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required, MaxLength(50)]
        public string Category { get; set; }

        [Required, MaxLength(100)]
        public string Screenwriter { get; set; }

        [Required, MaxLength(100)]
        public string Director { get; set; }

        [Required, MaxLength(200)]
        public string ProductionCompany { get; set; }

        [Required, Range(1900, 2100)]
        public int ReleaseYear { get; set; }

        [Required]
        public decimal PurchaseCost { get; set; }

        public int SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; }

        public Movie()
        {
            Rentals = new List<Rental>();
        }
    }
}
