# ExpenseManager
Web API for an expense manager : 

- GET// /api/Users
Get the list of users.

- GET// /api/Expenses
Get the list of expenses

- GET// /api/Expenses/1
Get the expense with id=1

- GET// /api/Expenses/byUser?userid=1&sort=amount_desc
Get the list of expenses for userId=1 sorted by descending amount

- POST// /api/Expenses
Post an expense, with a json body as following :
{
    "UserId": 1,
    "Currency": "dollar américain",
    "CreationDate": "2021-04-13T15:00:00",
    "Type": "Restaurant",
    "Amount": 10,
    "Comment": "This is a comment"
}

- PUT// /api/Expenses/1
Update expense with id=1 :
{
    "Id":1,
    "UserId": 1,
    "Currency": "dollar américain",
    "CreationDate": "2021-04-13T15:00:00",
    "Type": "Hotel",
    "Amount": 10,
    "Comment": "This is a comment"
}

- DELETE// /api/Expenses/1
Delete expense with id=1


