using Mini_Project_Entitiy_Framework_.Domain.Entities;

namespace Mini_Project_Entitiy_Framework_.Persistence.JsonServices
{
    public class AuthorJsonRepository : JsonRepository<Author>
    {
        public AuthorJsonRepository()
            : base(@"Src\Infrastructure\Mini_Project(Entitiy Framework).Persistence\DataJson\Authors.json") { }
    }
}
