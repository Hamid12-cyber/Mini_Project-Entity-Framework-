using Mini_Project_Entitiy_Framework_.Domain.Entities;

namespace Mini_Project_Entitiy_Framework_.Persistence.JsonServices
{
    public class ReservedItemJsonRepository : JsonRepository<ReservedItem>
    {
        public ReservedItemJsonRepository()
            : base(@"Src\Infrastructure\Mini_Project(Entitiy Framework).Persistence\DataJson\ReservedItems.json") { }
    }
}
