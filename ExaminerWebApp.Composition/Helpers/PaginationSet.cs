namespace ExaminerWebApp.Composition.Helpers
{
    public class PaginationSet<T>
    {
        public PaginationSet()
        {
            Take = 10;
            PageSize = 10;
            Page = 1;
        }
        public int Skip { get; set; }
        public int Take { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<GridSort> Sort { get; set; }
        public GridFilters Filter { get; set; }
        public IEnumerable<T> Items { get; set; }
    }

    public class GridSort
    {
        public string Field { get; set; } //field name
        public string Dir { get; set; } //direction of sorting: asc or desc 
    }

    public class GridFilters
    {
        public List<GridFilter> Filters { get; set; }
        public string Logic { get; set; }
    }

    public class GridFilter
    {
        public string Operator { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
    }
}