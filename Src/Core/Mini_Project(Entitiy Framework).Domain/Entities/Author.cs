using Mini_Project_Entitiy_Framework_.Domain.Entities;
using Mini_Project_Entitiy_Framework_.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_Entitiy_Framework_.Domain.Entities
{
    public class Author: BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Surname { get; set; }
        public Gender Gender { get; set; }
        public List<Book> Books { get; set; } = new ();
    }
}
