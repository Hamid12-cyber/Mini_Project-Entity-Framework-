using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_Entitiy_Framework_.Applications.İnterfaces.Repositories
{
    internal interface IAuthorRepository: IGenericRepository<Author>
    {
        List<Author> GetAllWithBooks();
        Author? GetByIdWithBooks(int id);
    }
}
