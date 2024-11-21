namespace CoreInfrastructure.Request
{
    public class CreateBookDbRequest
    {
        public string Name { get; set; }
        public short? ReadingStatus { get; set; }
        public short? Status { get; set; }
    }
}
