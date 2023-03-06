using TypeRunnerBE.Models;

namespace TypeRunnerBE.Services.Data
{
    using PassageDataResult = DataOpResult<Passage>;
    using PassageListDataResult = DataOpResult<IList<Passage>>;

    public interface IPassagesService
    {
        Passage? GetRandom();
        PassageListDataResult GetBySourceId(int sourceId);
        bool HasPassage();
        PassageDataResult Create(Passage passage);
        PassageDataResult Update(Passage passage);
        PassageDataResult DeleteById(int id);
    }
}
