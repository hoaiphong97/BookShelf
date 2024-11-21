namespace Service.Models.Requests
{
    public class UpdateBookRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public short ReadingStatus { get; set; }
        public short Status { get; set; }
    }
}
