using System.ComponentModel.DataAnnotations;

namespace ExpenseManager.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Currency { get; set; }

    }
}