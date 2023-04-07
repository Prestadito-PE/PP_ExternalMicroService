using FluentValidation;
using Prestadito.External.Application.Dto.Email;
using Prestadito.External.Application.Manager.Interfaces;
using Prestadito.External.Infrastructure.Data.Constants;

namespace Prestadito.External.API.Endpoints
{
    public static class EmailEndpoints
    {
        readonly static string collection = "email";

        public static WebApplication UseEmailEndpoints(this WebApplication app, string basePath)
        {
            string path = $"{basePath}/{collection}";

            app.MapPost(path + "/send",
               async (IValidator<EmailRequest> validator, EmailRequest request, IEmailController controller) =>
               {
                   var validationResult = await validator.ValidateAsync(request);
                   if (!validationResult.IsValid)
                   {
                       return Results.ValidationProblem(validationResult.ToDictionary());
                   }
                   return await controller.SendEmail(request);
               });

            return app;
        }
    }
}
