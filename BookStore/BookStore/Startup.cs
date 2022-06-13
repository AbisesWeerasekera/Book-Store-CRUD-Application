using AutoMapper;
using BookStore.Data;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
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
            //declare the dbcontext class
            //services.AddDbContext<BookStoreContext>(options =>options.UseSqlServer("Server=.;Database=BookStoreAPI;Integrated Security = true"));
            //Use app.setting file to store the connection string as a best pratice. we can't write the connection string as hard coded
            services.AddDbContext<BookStoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BookStoreDB")));

            //After installation Identity framework we have to tell it to the application, AddIdentity<TUserclass,TRoleclass>
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<BookStoreContext>().AddDefaultTokenProviders();

            //We are going to tell the application that we are going to use Token for this JWT,added authentication
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            //add the JWT bearer
            .AddJwtBearer(option => {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))

                };
             });
                       
            //services.AddControllers();
            services.AddControllers().AddNewtonsoftJson();
            //register Autotmapper
            services.AddAutoMapper(typeof(Startup));

            //Register the Books Repository service
            // A new instance of the service will be created everytime it is requested
            services.AddTransient<IBookRepository, BookRepository>();

            //Register the Account Repository service which is created for signup and login
            services.AddTransient<IAccountRepository, AccountRepository>();

            //Define the CORS - We use whenever u are trying to access resource from a domain that is different from your current application domain. Then you will get this error.
            //Cross origin resource sharing - The request is blocked because of Invalid or missing response headers
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
