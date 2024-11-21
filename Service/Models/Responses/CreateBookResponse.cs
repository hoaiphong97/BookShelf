using Infrastructure.Models.BaseModels;

namespace Service.Models.Responses
{
    public class CreateBookResponse : BaseResponse<CreateBook>
    {
        public CreateBookResponse() : base(new CreateBook()) { }
    }
    public class CreateBook
    {
        public Guid Id { get; set; }
    }
}
