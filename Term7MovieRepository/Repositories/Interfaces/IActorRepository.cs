using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IActorRepository
    {
        IEnumerable<Actor> GetAllActor(int movieId);
        Actor GetActorById(int id);
        int CreateActor(Actor actor);
        int CreateActor(Actor[] actors);
        int UpdateActor(Actor actor);
        int DeleteActor(Actor actor);
    }
}
