using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Prestadito.External.Application.Dto.Email;
using Prestadito.External.Application.Manager.Controller;
using Prestadito.External.Application.Manager.Interfaces;
using Prestadito.External.Application.Manager.Validators;

namespace Prestadito.External.Application.Manager.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<EmailRequest>, EmailValidator>();

            return services;
        }

        public static IServiceCollection AddSExternalControllers(this IServiceCollection services)
        {
            services.AddScoped<IEmailController, EmailController>();
            return services;
        }
    }
}