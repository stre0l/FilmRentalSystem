using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmRentalSystem.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal RentalFee { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PenaltyFee { get; set; }

        [Required]
        public bool IsReturned { get; set; }

        public int MovieId { get; set; }
        public int CinemaId { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Cinema Cinema { get; set; }

        [NotMapped]
        public bool IsOverdue => DateTime.Now > EndDate && !IsReturned;

        [NotMapped]
        public decimal TotalAmount => RentalFee + PenaltyFee;
    }
}