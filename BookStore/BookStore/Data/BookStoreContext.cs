using BookStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data
{
    //Before installation Identity Frame work
    /*
    public class BookStoreContext:DbContext
    */

    //After installation Identity core, "BookStoreContext" should inherit "IdentityDbContext" with <ApplicationUser> if there isnothing then don't want to mention it.
    public class BookStoreContext : IdentityDbContext<ApplicationUser>
    {
        //Iside the DbcontextOptions provide the name of out Context class(BookStore)
        //options is a meaningful name for DbContextOptions, those options should be passed to the base constructor too
        public BookStoreContext(DbContextOptions<BookStoreContext> options):base(options)
        {

        }

        //To addd a new class in this cintext class we have to use DbSet<>, There will be a new table which is named Books
        public DbSet<Books> Books { get; set; }


        //connection string declaring inside the Context class method 1 - not much recommended
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=BookStoreAPI;Integrated Security = true");
            base.OnConfiguring(optionsBuilder);
        }*/


    }
}
