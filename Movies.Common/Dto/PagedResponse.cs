namespace Movies.Common.Dto
{
    public class PagedResponse<T> : ResponseDto<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
}