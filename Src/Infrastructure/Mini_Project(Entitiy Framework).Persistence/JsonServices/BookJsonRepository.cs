using Mini_Project_Entitiy_Framework_.Domain.Entities;

namespace Mini_Project_Entitiy_Framework_.Persistence.JsonServices
{
    public class BookJsonRepository : JsonRepository<Book>
    {
        public BookJsonRepository()
            : base(@"Src\Infrastructure\Mini_Project(Entitiy Framework).Persistence\DataJson\Books.json") { }
    }
}
