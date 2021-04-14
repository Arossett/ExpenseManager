using System.Text.Json.Serialization;

namespace ExpenseManager.Models
{
    public class UserDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string FullName { get; set; }


    }
}

