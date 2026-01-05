using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Presentation.Factory
{
    public class BookFactory
    {
        public IPaper Build(string json)
        {
            //dynamic is a generic data type like var, however it allows me to inspect the properties of an anonymous object.
            dynamic builtObject = JsonConvert.DeserializeObject<dynamic>(json);
            if (builtObject != null)
            {
                if (builtObject.Volume != null)
                {
                    Journal j = JsonConvert.DeserializeObject<Journal>(json);
                    return j;
                }
                else
                {
                    Book b = JsonConvert.DeserializeObject<Book>(json);
                    return b;
                }
            }

            return null;
        }

        //This entry (method) will know what to build, what to do to save, and where to save it.
        public void BuildAndSave(
            string json, 
            IBooksRepository bookRepository,
            JournalsRepository journalsRepository)
        {
            IPaper paper = this.Build(json);
            if(paper.GetType() == typeof(Book))
            {
                //Call the BooksRepository.
                bookRepository.Add((Book)paper);
;           }
            else
            {
                //Call the JournalsRepository.
                journalsRepository.Add((Journal)paper);
            }
        }
    }
}
