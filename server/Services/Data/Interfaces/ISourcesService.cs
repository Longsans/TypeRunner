using TypeRunnerBE.Models;

namespace TypeRunnerBE.Services.Data
{
    using SourceDataResult = DataOpResult<Source>;

    public interface ISourcesService
    {
        Source? GetById(int id);
        SourceDataResult Create(Source source);
        SourceDataResult Update(Source source);
        SourceDataResult DeleteById(int id);
    }
}
