using Term7MovieApi.Entities;

namespace Term7MovieApi.Repositories.Interfaces
{
    public interface ITheaterRepository
    {
        IEnumerable<Theater> GetAllTheater();
        Theater GetTheaterById(int id);
        int CreateTheater(Theater theater);
        int UpdateTheater(Theater theater);
        int DeleteTheater(int id);
    }
}
