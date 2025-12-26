using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmRentalSystem.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(500)]
        public string LegalAddress { get; set; }

        [Required, MaxLength(200)]
        public string Bank { get; set; }

        [Required, MaxLength(50)]
        public string AccountNumber { get; set; }

        [Required, MaxLength(20)]
        public string INN { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

        public Supplier()
        {
            Movies = new List<Movie>();
        }
    }
}