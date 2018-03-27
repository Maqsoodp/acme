using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Acme.RemoteFlights.Application;
using Acme.RemoteFlights.Application.Services;
using Acme.RemoteFlights.Domain;
using Acme.RemoteFlights.Domain.Contracts;
using Acme.RemoteFlights.Domain.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace Acme.RemoteFlights.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //services.AddDbContext<FlightsDbContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("FlightsContext")));

            services.AddDbContext<FlightsDbContext>(options => options.UseInMemoryDatabase("FlightsDb"));

            services.AddSingleton(CreateMapper());

            services.AddScoped<IFlightService, FlightService>();
            services.AddScoped<IBookingsService, BookingsService>();

            services.AddScoped<FlightsDbContext>();
            services.AddTransient<DbInit>();
            services.AddScoped<IFlightsRepository, FlightsRepository>();
            services.AddScoped<IFlightBookingsRepository, FlightBookingsRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("flight", new Info { Title = "Acme remote flight Rest API", Version = "v1" });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DbInit dbInit, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/flight/swagger.json", "Acme remote flight Rest API");
            });

            app.UseMvc();
            app.UseStaticFiles();
            dbInit.Load();
        }

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DomainMappingProfile>();
                cfg.AddProfile<ApplicationMappingProfile>();
            });
            return config.CreateMapper();
        }
    }
}
