using FileCrypt.Cryptography;
using FileCrypt.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace FileCrypt.Extensions
{
    internal static class DependencyContainer
    {
        public static void Scoped(IServiceCollection services)
        {
            services.AddKeyedTransient<ICypher, Encrypt>("encrypt");
            services.AddKeyedTransient<ICypher, Decrypt>("decrypt");
            services.AddTransient<IConfiguration, ConfigurationFile>();
        }
    }
}
