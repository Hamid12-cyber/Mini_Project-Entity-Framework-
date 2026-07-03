using Mini_Project_Entitiy_Framework_.Domain.Entities;
using Mini_Project_Entitiy_Framework_.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_Entitiy_Framework_.Domain.Entities
{
    public class ReservedItem: BaseEntity
    {
        public string FinCode { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public Status Status { get; set; }
    }
}
