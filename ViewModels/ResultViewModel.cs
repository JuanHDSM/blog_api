namespace blog_api.ViewModels
{
    public class ResultViewModel<T>
    {

        public ResultViewModel(T data, List<string> errors)
        {
            Data = data;
            Errors = errors;
        }

        public ResultViewModel(T data)
        {
            Data = data;
        }
        public ResultViewModel(List<string> errors)
        {
            Errors = errors;
        }

        public ResultViewModel(string error)
        {
            Errors.Add(error);
        }
        
        public ResultViewModel(int total, int page, int pageSize, T data)
        {
            Total = total;
            Page = page;
            PageSize = pageSize;
            Data = data;
        }

        public T? Data { get; private set; }
        public List<string> Errors { get; private set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}