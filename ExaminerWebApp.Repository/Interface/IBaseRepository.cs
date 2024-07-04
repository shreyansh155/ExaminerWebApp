namespace ExaminerWebApp.Repository.Interface
{
   public interface IBaseRepository<T> 
    {
        public Task<T> Create(T _object);
        public void Update(T _object);
        public void Delete(int id);
        public T GetById(int id);
    }
}
