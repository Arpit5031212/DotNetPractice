﻿using BookStoreAPI.Data;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context;

        public BookRepository(BookStoreContext context)
        {
            _context = context;
        }
        public async Task<List<BookModel>> GetAllBookAsync()
        {
            var records = await _context.Books.Select(x => new BookModel()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            }).ToListAsync();

            return records;
        }

        public async Task<BookModel> GetBookByIdAsync(int bookId)
        {
            // FindAsync() will work only with primary key for other columns we have to use where clause
            var records = await _context.Books.Where(x => x.Id == bookId).Select(x => new BookModel()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            }).FirstOrDefaultAsync();

            return records;
        }

        public async Task<int> AddBookAsync(BookModel bookModel)
        {
            var book = new Books()
            {
                Title = bookModel.Title,
                Description = bookModel.Description,
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book.Id;
           
        }

        public async Task UpdateBookAsync(int bookId, BookModel bookModel)
        {
            // this code uses two database calls to update an item in database
            //var book = await _context.Books.FindAsync(bookId);
            //if(book != null)
            //{
            //    book.Title = bookModel.Title;
            //    book.Description = bookModel.Description;

            //    await _context.SaveChangesAsync();
            //}

            // this code uses only one database call to update an item in database
            var book = new Books()
            {
                Id = bookId,
                Title = bookModel.Title,
                Description = bookModel.Description,
            };

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateBookPatchAsync(int bookId, JsonPatchDocument bookModel)
        {
            var book = await _context.Books.FindAsync(bookId);
            if(book != null)
            {
                bookModel.ApplyTo(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBookAsync(int bookId)
        {

            var book = new Books() { Id = bookId };

            _context.Books.Remove(book);

            await _context.SaveChangesAsync();

        }
    }
}
