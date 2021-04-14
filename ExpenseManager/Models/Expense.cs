using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManager.Models
{
    public class Expense
    {
        public enum ExpenseType
        {
            Restaurant, Hotel, Misc
        }

        public int Id { get; set; }
        [ForeignKey("User")]
        [Required]
        public int UserID { get; set; }
        public UserDTO User { get; set; }
        [CustomValidation.CustomExpenseDateAttribute(ErrorMessage = Constants.ErrorMessages.CreationDate)]
        public DateTime CreationDate { get; set; }
        [Required]
        public ExpenseType Type { get; set; }
        [CustomValidation.MinValue(0, ErrorMessage = Constants.ErrorMessages.IncorrectAmount)]
        [Required]
        public float Amount { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public string Currency { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}