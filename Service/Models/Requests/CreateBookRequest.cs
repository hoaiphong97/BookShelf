namespace Service.Models.Requests
{
    public class CreateBookRequest
    {
        public string Name { get; set; }
        public short ReadingStatus { get; set; }
        public short Status { get; set; }
    }
}
