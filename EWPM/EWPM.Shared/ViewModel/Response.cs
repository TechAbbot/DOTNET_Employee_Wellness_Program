namespace EWPM.Shared.ViewModel
{
    //common response model for all over project
    public class Response
    {
        public string Message { get; set; }
        public object Data { get; set; }
        public int StatusCode { get; set; }
    }
}
