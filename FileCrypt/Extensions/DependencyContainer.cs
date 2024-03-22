using FileCrypt.Cryptography;
using FileCrypt.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace FileCrypt.Extensions
{
    internal static class DependencyContainer
    {
        public static void Transient(IServiceCollection services)
        {
            services.AddKeyedTransient<ICypher, Encrypt>("encrypt");
            services.AddKeyedTransient<ICypher, Decrypt>("decrypt");
            services.AddTransient<IConfiguration, ConfigurationFile>();
            services.AddTransient<ICommands, CommandHandler>();
            services.AddTransient<IGenerate, Generate>();
            services.AddTransient<IOperationResultMessage, OperationResultMessage>();
        }
    }
}
