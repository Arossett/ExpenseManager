# ExpenseManager
Web API for an expense manager : 

GET// /api/Users <br/>
Get the list of users.

GET// /api/Expenses <br/>
Get the list of expenses

GET// /api/Expenses/1 <br/>
Get the expense with id=1

GET// /api/Expenses/byUser?userid=1&sort=amount_desc <br/>
Get the list of expenses for userId=1 sorted by descending amount

POST// /api/Expenses <br/>
Post an expense, with a json body as following : <br/>
{\
    "UserId": 1,\
    "Currency": "dollar américain",\
    "CreationDate": "2021-04-13T15:00:00",\
    "Type": "Restaurant",\
    "Amount": 10,\
    "Comment": "This is a comment"\
}

PUT// /api/Expenses/1 <br/>
Update expense with id=1 : <br/>
{\
    "Id":1,\
    "UserId": 1,\
    "Currency": "dollar américain",\
    "CreationDate": "2021-04-13T15:00:00",\
    "Type": "Hotel",\
    "Amount": 10,\
    "Comment": "This is a comment"\
}

DELETE// /api/Expenses/1<br/>
Delete expense with id=1


