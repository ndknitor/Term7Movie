using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ILanguageRepository
    {
        IEnumerable<Language> GetAllLanguage();
    }
}
