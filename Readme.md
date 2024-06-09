# Test Transaction Api
.net core 8 test Http REST API

### Prerequisites
dotnet core 8


### Settings
SQL server **connection string** ("TransactionsSqlConnString") in **appsettings.json**


### Database preparation (SQL Server)
Database and tables script is included in **migrations.sql** file


### Http Api Quick docs
Postman collection 2.1 included in git repo
`postman_collection.json`


1. Bulk upsert by uploading CSV file

`POST http://localhost:5263/transactions/upload`
`Content-Type: text/csv`

`<binary csv data in body link the MOCK_DATA.csv included>`

2. Get transactions paginated
`GET http://localhost:5263/transactions?page=1&count=10`


3. Get transaction by id
`GET http://localhost:5263/transactions/<guid>`


3. Upsert transaction by id
`POST http://localhost:5263/transactions/upload`
`Content-Type: application/json`

    `
    {
    "id": "2214c3f4-028d-447c-a6ba-7b8358cbb08f",
    "applicationName": "Overhold123",
    "email": "tdeya2@joomla.org",
    "filename": "LuctusRutrumNulla.pdf",
    "url": "https://microsoft.com",
    "inception": "2017-04-04",
    "amount": 934.60,
    "currency": "USD",
    "allocation": 30.98
    }
    `


4. Delete transaction by id
`DELETE http://localhost:5263/transactions/<guid>`
