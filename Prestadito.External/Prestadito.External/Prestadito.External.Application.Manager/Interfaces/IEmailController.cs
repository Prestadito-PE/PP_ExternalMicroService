using Microsoft.AspNetCore.Http;
using Prestadito.External.Application.Dto.Email;

namespace Prestadito.External.Application.Manager.Interfaces
{
    public interface IEmailController
    {
        ValueTask<IResult> SendEmail(EmailRequest request);
    }
}