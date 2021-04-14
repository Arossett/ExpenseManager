using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManager.CustomValidation
{
    public class CustomExpenseDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateTime = Convert.ToDateTime(value);
            var lessthan = DateTime.Now.AddMonths(-3);
            return (dateTime <= DateTime.Now && dateTime>=lessthan);
        }
    }

    public class MinValueAttribute : ValidationAttribute
    {
        private readonly int _minValue;

        public MinValueAttribute(int minValue)
        {
            _minValue = minValue;
        }

        public override bool IsValid(object value)
        {
            return (float)value > _minValue;
        }
    }
}
