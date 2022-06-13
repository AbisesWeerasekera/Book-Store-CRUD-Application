using AutoMapper;
using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Repository
{
    public class BookRepository: IBookRepository
    {
        private readonly BookStoreContext _context;
        private readonly IMapper _mapper;

        //We need to read the data from database. So we need DbContext. We use dependency injection in constructor
        public BookRepository(BookStoreContext context,IMapper mapper)
        {
            //get the instance of our DbContext class
            _context = context;
            _mapper = mapper;
        }

        //return the all the books in the database. That is why return type is a List
        public async Task<List<BookModel>> GetAllBooksAsync() {
            //Because Async function we should use "await" and "ToListAsync() instead of ToList()"
            //convert the List typed data of Book table to Book Model List
            //_context.Books ----- here Books is the table name wehre we get the data
            
            /*if we use without Automapper
             var records =await _context.Books.Select(x => new BookModel()
             {
                 Id = x.Id,
                 Title = x.Title,
                 Description = x.Description
             }).ToListAsync();

             return records;*/

            //If we use the Automapper
            var records = await _context.Books.ToListAsync();//getting data in here
            return _mapper.Map<List<BookModel>>(records);//mapping and returning data here
        }

        //get one item
        public async Task<BookModel> GetBooksByIdAsync(int bookId)
        {
            /* //Without using AutoMapper
            var records = await _context.Books.Where(x=>x.Id==bookId).Select(x => new BookModel()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            }).FirstOrDefaultAsync();
            //FirstOrDefaultAsync() method --- if book not exists, It will get null value and there will not be an error
            //FirstAsync() method -- get an error if there is not a  relevant book for that id

            return records;*/

            //With using Automapper
            var book = await _context.Books.FindAsync(bookId);
            return _mapper.Map<BookModel>(book);
        }
        //Add a new item
        public async Task<int> AddBookAsync(BookModel bookModel)
        {//The Books table only understand the typed "Book", It does not understand the typed "BookModel"
            //So we have to convert the data from "BookModel" to the "Books"
            var book = new Books()
            {
                Title=bookModel.Title,
                Description=bookModel.Description
                //No need to pass the Id here, It will be automaticalyy generated
            };

            //Now we are going to tell to the DbContext class that we are going to add a new record to the "Books" Talbe
            _context.Books.Add(book);
            //To hit the database
            //Once you save the change the database, The the Id of the new record will be generated and it will get assinged to this "book" object automatically
            await _context.SaveChangesAsync();

            //Return the newly created book.Id
            return book.Id;
        }


        //Update One item,we not return anything here
        //we need two things here--> fetch the book based on that id, all the properties that we need to update for that book
      
        public async Task UpdateBookAsync(int bookId,BookModel bookModel)
        {
            /*THIS WILL HIT MULTIPLES TIMES WITH DATABSE
            //hitting the database at this line number 79
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                book.Title = bookModel.Title;
                book.Description = bookModel.Description;
            }

            //again hittin the databse at 87 line-->hitting unnecessary multiple times with databse can cause performance issues
            await _context.SaveChangesAsync();
            */

            //THIS WILL SOLVE IT BY ONE DATABASE HITTING
            //Creating the object at 93 line
            var book = new Books()
            {
                Id=bookId,
                Title = bookModel.Title,
                Description = bookModel.Description,
            };

            //Assign the created object to the context at this 101 line
            _context.Books.Update(book);
            //Making all the changes in the database at this line number 104,Save all the changes made in this context to the database
            await _context.SaveChangesAsync();
        }


        //Partially update an item by using PATCH
        public async Task UpdateBookPatchAsync(int bookId, JsonPatchDocument bookModel)
        {
            var book = _context.Books.FindAsync(bookId);
            if (book != null)
            {
                bookModel.ApplyTo(book);
                await _context.SaveChangesAsync();
            }
        }

        //Delete item from the database
        public async Task DeleteBookAsync(int bookId)
        {
            /*
            // 1st method if we don't have a primary key to identify delete the record
            var book =_context.Books.Where(x => x.Title == "").FirstOrDefault();//hittin the database
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();//hitting the database

            return bookId;
            */

            //2nd method if we have the primary key as Id
            var book = new Books() { Id = bookId };
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}
