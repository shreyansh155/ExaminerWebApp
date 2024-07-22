namespace ExaminerWebApp.Repository.Interface
{
    public interface IBaseRepository<T>
    {
        public Task<T> Create(T _object);
     
        public Task<bool> Update(T _object);
        
        public Task<bool> Delete(int id);
        
        public Task<T> GetById(int id);
    }
}