using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
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
