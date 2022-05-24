using Term7MovieApi.Entities;

namespace Term7MovieApi.Repositories.Interfaces
{
    public interface IActorRepository
    {
        IEnumerable<Actor> GetAllActor();
        Actor GetActorById(int id);
        int CreateActor(Actor actor);
        int CreateActor(Actor[] actors);
        int UpdateActor(Actor actor);
        int DeleteActor(Actor actor);
    }
}
