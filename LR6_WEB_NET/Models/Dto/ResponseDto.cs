namespace LR6_WEB_NET.Models.Dto
{
    public class ResponseDto<T>
    {
        public string Description { get; set; } = string.Empty;
        public int StatusCode { get; set; } = 0;
        public int TotalRecords { get; set; } = 0;
        public List<T> Values { get; set; } = new List<T>();
    }
}
