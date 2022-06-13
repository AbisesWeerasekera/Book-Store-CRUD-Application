using BookStore.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Repository
{
    public interface IBookRepository
    {
        Task<int> AddBookAsync(BookModel bookModel);
        Task DeleteBookAsync(int bookId);
        Task<List<BookModel>> GetAllBooksAsync();
        Task<BookModel> GetBooksByIdAsync(int bookId);
        Task UpdateBookAsync(int bookId, BookModel bookModel);
        Task UpdateBookPatchAsync(int bookId, JsonPatchDocument bookModel);
    }
}
