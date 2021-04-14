
namespace ExpenseManager
{
    public static class Constants
    {
        public static class ErrorMessages
        {
            public const string CreationDate = "Creation Date must happen in the last 3 months.";
            public const string IncorrectExpense = "This expense has already been added.";
            public const string IncorrectCurrency = "This expense has already been added.";
            public const string IncorrectUser = "User doesn't exist in our database, please enter another user.";
            public const string IncorrectAmount = "The amount should be more than 0.";
            public const string ModelError = "Something went wrong with the json definition, please check your request body.";
        }

        public static class ErrorTitle
        {
            public const string IncorrectExpense = "Incorrect Expense.";
            public const string IncorrectCurrency = "Incorrect currency.";
            public const string IncorrectUser = "Incorrect User Id.";
            public const string ModelError = "Model error.";
        }
    }
}