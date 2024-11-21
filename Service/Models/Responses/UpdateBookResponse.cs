using Infrastructure.Models.BaseModels;

namespace Service.Models.Responses
{
    public class UpdateBookResponse : BaseResponse<UpdateBook>
    {
        public UpdateBookResponse() : base(new UpdateBook()) { }
    }
    public class UpdateBook
    {
        public Guid Id { get; set; }
    }
}
