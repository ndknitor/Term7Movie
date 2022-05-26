using Term7MovieApi.Entities;
using Term7MovieApi.Repositories.Interfaces;

namespace Term7MovieApi.Repositories.Implement
{
    public class ActorRepository : IActorRepository
    {
        private AppDbContext _context;
        public ActorRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Actor> GetAllActor(int movieId)
        {
            IEnumerable<Actor> actors = new List<Actor>();
            return actors;
        }

        public Actor GetActorById(int id)
        {
            Actor actor = null;
            return actor;
        }

        public int CreateActor(Actor actor)
        {
            int count = 0;
            return count;
        }

        public int CreateActor(Actor[] actors)
        {
            int count = 0;
            return count;
        }

        public int UpdateActor(Actor actor)
        {
            int count = 0;
            return count;
        }

        public int DeleteActor(Actor actor)
        {
            int count = 0;
            return count;
        }
    }
}
