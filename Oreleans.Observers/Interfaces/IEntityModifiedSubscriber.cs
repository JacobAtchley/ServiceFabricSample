using System.Threading.Tasks;

namespace Oreleans.Observers.Interfaces
{
    public interface IEntityModifiedSubscriber
    {
        Task InitClientAsync();

        void Unsubscribe();
    }
}
