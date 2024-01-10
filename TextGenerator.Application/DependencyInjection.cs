using Microsoft.Extensions.DependencyInjection;
using TextGenerator.Application.Interfaces;
using TextGenerator.Application.Services;

namespace TextGenerator.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTextGenerator(this IServiceCollection services)
        {
            services.AddScoped<IFileReader, TextFileReader>();
            services.AddScoped<ITemplateFiller, TemplateFiller>();
            services.AddScoped<ITemplateAccessor, TemplateAccessor>();
            services.AddScoped<ITextGeneratorManager, TextGeneratorManager>();

            return services;
        }
    }
}
