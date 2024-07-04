using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;

namespace ExaminerWebApp.Repository.Implementation
{
    public class ExaminerRepository : IExaminerRepository
    {
        private readonly ApplicationDbContext _context;
        public ExaminerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Examiner> Create(Examiner model)
        {
            await _context.Examiners.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public void Delete(int id)
        {
            var applicant = _context.Examiners.First(x => x.Id == id);
            applicant.IsDeleted = true;
            _context.Examiners.Update(applicant);
            _context.SaveChanges();
        }

        public IQueryable<Examiner> GetAll()
        {
            try
            {
                // return _context.Examiners.AsQueryable();
                return _context.Examiners.Where(x => x.IsDeleted != true).OrderBy(x => x.Id).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Examiner GetById(int id)
        {
            return _context.Examiners.First(x => x.Id == id);
        }

        public void Update(Examiner model)
        {
            //var examiner = _context.Examiners.First(x => x.Id == model.Id);

            //examiner.FirstName = model.FirstName;
            //examiner.LastName = model.LastName;
            //examiner.Email = model.Email;
            //examiner.Phone = model.Phone;
            //examiner.DateOfBirth = model.DateOfBirth;
            //examiner.FilePath = model.FilePath ?? examiner.FilePath;
            //examiner.ExaminerId = model.ExaminerId;

            _context.Examiners.Update(model);
            _context.SaveChanges();
        }
        public bool CheckEmail(string email)
        {
            return _context.Examiners.Where(x => x.Email == email && x.IsDeleted != true).Any();
        }
    }
}
