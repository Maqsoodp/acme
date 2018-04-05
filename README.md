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

First clone the solution. 

1. With current settings having InMemoryDatabase just Press F5

2. With Local Db 

        a. Create Local DB with name "FlightsDb"
	
        b. Run the Db Scripts file (Sql.sql) present in the project Acme.RemoteFlights.Domain => DBScripts folder
	
        c. In Project Acme.RemoteFlights.Api => Startup.cs file 
	
            Un-comment line
            //services.AddDbContext<FlightsDbContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("FlightsContext")));
	    
        d. In Project Acme.RemoteFlights.Api => Startup.cs file 
	
            Comment the line
            services.AddDbContext<FlightsDbContext>(options => options.UseInMemoryDatabase("FlightsDb"));
        
		e. Press F5
        

