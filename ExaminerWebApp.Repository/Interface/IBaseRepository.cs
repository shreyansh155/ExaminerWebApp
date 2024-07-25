namespace ExaminerWebApp.Repository.Interface
{
    public interface IBaseRepository<T>
    {
        public Task<T> Create(T _object);
     
        public Task<T> Update(T _object);
        
        public Task<int> Delete(int id);
        
        public Task<T> GetById(int id);
    }
}