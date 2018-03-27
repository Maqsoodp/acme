# Acme Remote Flights

Requirement -
Create a Rest API which facilitates the below functionality:

List all flights:
Flight no, start time, end time, passenger capacity, departer city, arrival city
Search for bookings:
by passenger name, date, arrival city, departure city, flight number
Check availability :
by giving start and end Date. No of pax
Make booking:
by selecting flights and providing passenger details

# Solution Contains 
1. Rest api built using ASP NET Core 2.0 Web API
2. In memory Database (Can be configured to Local DB/ SQL Server)
3. Front end SPA built using ReactJS with Web pack.

# Run locally 
Clone the solution 
1. With InMemoryDatabase just Press F5
2. With Local Db 
		Create Local DB with name FlightsDb
		Run the Scripts present in Acme.RemoteFlights.Domain Project Script Folder
 		Press F5
# Azure Site
https://acmeremoteflightsapi20180327103924.azurewebsites.net/
