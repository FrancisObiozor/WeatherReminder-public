using Microsoft.Extensions.DependencyInjection;

namespace WeatherReminder.Models.BackgroundModel
{
    public interface IScopedFactory<T>
    {
        T Create();
    }

    public class ScopedFactory<T> : IScopedFactory<T>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ScopedFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public T Create()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                return scope.ServiceProvider.GetRequiredService<T>();
            }
        }
    }

}
