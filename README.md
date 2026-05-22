# Intranet-Hub

Intranet-Hub is an ASP.NET Core MVC application providing centralized communication and operations tools.

## Features Added

* **Dolar Rate Synchronization:** Retrieves the Dolar (USD) rates periodically (08:00, 10:00, 12:00, 14:00, 16:00, 17:00) from the Brazilian Central Bank (BCB) API, parses the CSV output, and saves the purchase and sale rates to the local SQLite database.
* **Dashboard Update:** The dashboard ignores the Euro and fetches the latest USD purchase and sale rates directly from the local database.

## Running

* Run `dotnet run` to start the application. The database is initialized and seed data is created if it does not exist at startup.
