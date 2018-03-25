﻿using System;
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

namespace WebApplication1
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
            services.AddDbContext<FlightsDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("FlightsContext")));
            //services.AddScoped<>

            services.AddSingleton(CreateMapper());

            services.AddScoped<IFlightService, FlightService>();
            services.AddScoped<IBookingsService, BookingsService>();

            services.AddScoped<FlightsDbContext>();
            services.AddTransient<DbInit>();
            services.AddScoped<IFlightsRepository, FlightsRepository>();
            services.AddScoped<IFlightBookingsRepository, FlightBookingsRepository>();
            services.AddScoped<IPassengerRepository, PassengerRepository>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DbInit dbInit, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            // env.EnvironmentName = EnvironmentName.Production;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseExceptionHandler("/error");
            //}
            app.UseMvc();
            app.UseExceptionHandler(
                options =>
                {
                    options.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "text/html";
                        var ex = context.Features.Get<IExceptionHandlerFeature>();
                        if (ex != null)
                        {
                            var err = $"{ex.Error.Message}";
                            await context.Response.WriteAsync(err).ConfigureAwait(false);
                        }
                    });
                }
            );
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