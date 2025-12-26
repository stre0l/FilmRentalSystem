using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmRentalSystem.Models
{
    public class Cinema
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(500)]
        public string Address { get; set; }

        [Required, MaxLength(20)]
        public string Phone { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int SeatCount { get; set; }

        [Required, MaxLength(100)]
        public string Director { get; set; }

        [Required, MaxLength(100)]
        public string Owner { get; set; }

        [Required, MaxLength(200)]
        public string Bank { get; set; }

        [Required, MaxLength(50)]
        public string AccountNumber { get; set; }

        [Required, MaxLength(20)]
        public string INN { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; }

        public Cinema()
        {
            Rentals = new List<Rental>();
        }
    }
}