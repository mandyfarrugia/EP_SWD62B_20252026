using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json; //To allow us to read and write from a file.

namespace DataAccess.Repositories
{
    public class BooksFileRepository : IBooksRepository
    {
        private string filePath = "C:\\Users\\Mandy\\Documents\\Enterprise Programming\\mandyfarrugia\\EP_SWD62B_20252026\\EP_SWD62B_20252026\\Presentation\\wwwroot\\books.json";

        public void Add(Book book)
        {
            //Stringify the Book class - serialise the Book class/convert to JSON format/object.
            string json = JsonConvert.SerializeObject(book);
            File.AppendAllLines(this.filePath, new List<string>() { json });
        }

        public IQueryable<Book> Get()
        {
            string[] fileContent = File.ReadAllLines(this.filePath);
            List<Book> books = new List<Book>();
            foreach(string line in fileContent)
            {
                books.Add(JsonConvert.DeserializeObject<Book>(line));
            }

            return books.AsQueryable();
        }

        public Book Get(int id)
        {
            if (!File.Exists(filePath))
                return null;

            foreach (var line in File.ReadLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var book = JsonConvert.DeserializeObject<Book>(line);

                if (book != null && book.Id == id)
                    return book;
            }

            return null;
        }
    }
}
