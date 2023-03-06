using TypeRunnerBE.Models;

namespace TypeRunnerBE.Services.Data
{
    using SourceDataResult = DataOpResult<Source>;

    public class EfcSourcesService : ISourcesService
    {
        private readonly TypeRunnerContext _context;

        public EfcSourcesService(TypeRunnerContext context)
        {
            _context = context;
        }

        public SourceDataResult Create(Source source)
        {
            var existing = _context.Sources.FirstOrDefault(a => a.Name == source.Name);
            if (existing != null)
                SourceDataResult.Err(DataOpError.SourceAlreadyExists);
            _context.Sources.Add(source);
            _context.SaveChanges();
            return SourceDataResult.Ok(source);
        }

        public SourceDataResult DeleteById(int id)
        {
            var source = _context.Sources.Find(id);
            if (source == null)
                return SourceDataResult.Err(DataOpError.SourceNotExist);
            _context.Sources.Remove(source);
            _context.SaveChanges();
            return SourceDataResult.Ok();
        }

        public Source? GetById(int id)
        {
            return _context.Sources.Find(id);
        }

        public SourceDataResult Update(Source source)
        {
            var existing = _context.Sources.Find(source.Id);
            if (existing == null)
                return SourceDataResult.Err(DataOpError.SourceNotExist);
            existing.Name = source.Name;
            _context.SaveChanges();
            return SourceDataResult.Ok(existing);
        }
    }
}
