using MediatR;

namespace SiaAdmin.Application.Features.Commands.User.CreateBlockUser
{
    public class CreateBlockUserRequest : IRequest<CreateBlockUserResponse>
    {
        public string Note { get; set; }
        public string Data { get; set; }
        public int Active { get; set; } = 1;
        public int RecType { get; set; } = 2;
        public DateTime TimeStamp { get; set; }=DateTime.UtcNow;
    }
}
