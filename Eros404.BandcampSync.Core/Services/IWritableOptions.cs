using Microsoft.Extensions.Options;

namespace Eros404.BandcampSync.Core.Services
{
    public interface IWritableOptions<out T> : IOptionsSnapshot<T> where T : class, new()
    {
        void Update(Action<T> applyChanges);
    }
}
