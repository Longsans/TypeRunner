using TypeRunnerBE.Models;

namespace TypeRunnerBE.Services.Data
{
    using PassageDataResult = DataOpResult<Passage>;
    using PassageListDataResult = DataOpResult<IList<Passage>>;

    public class EfcPassagesService : IPassagesService
    {
        private readonly TypeRunnerContext _context;
        private readonly ISourcesService _sourcesService;

        public EfcPassagesService(TypeRunnerContext context, ISourcesService sourcesService)
        {
            _context = context;
            _sourcesService = sourcesService;
        }

        public PassageDataResult Create(Passage passage)
        {
            var existing = _context.Passages.Find(passage.Id);
            if (existing != null)
                return PassageDataResult.Err(DataOpError.PassageAlreadyExists);
            _context.Passages.Add(passage);
            _context.SaveChanges();
            return PassageDataResult.Ok(passage);
        }

        public PassageDataResult DeleteById(int id)
        {
            var passage = _context.Passages.Find(id);
            if (passage == null)
                return PassageDataResult.Err(DataOpError.IdNotExist);
            _context.Passages.Remove(passage);
            _context.SaveChanges();
            return PassageDataResult.Ok();
        }

        public PassageListDataResult GetBySourceId(int sourceId)
        {
            var source = _sourcesService.GetById(sourceId);
            if (source == null)
                return PassageListDataResult.Err(DataOpError.SourceNotExist);
            var passages = _context.Passages.Where(q => q.SourceId == sourceId).ToList();
            return PassageListDataResult.Ok(passages);
        }

        public Passage? GetRandom()
        {
            var passages = _context.Passages.ToArray();
            if (passages.Length < 1)
                return null;
            var rnd = new Random();
            return passages[rnd.Next(0, passages.Length)];
        }

        public bool HasPassage()
        {
            return _context.Passages.Any();
        }

        public PassageDataResult Update(Passage passage)
        {
            var existing = _context.Passages.Find(passage.Id);
            if (existing == null)
                return PassageDataResult.Err(DataOpError.IdNotExist);

            var source = _sourcesService.GetById(passage.SourceId);
            if (source == null)
                return PassageDataResult.Err(DataOpError.SourceNotExist);

            existing.Text = passage.Text;
            existing.SourceId = passage.SourceId;
            _context.SaveChanges();
            return PassageDataResult.Ok(existing);
        }
    }
}
