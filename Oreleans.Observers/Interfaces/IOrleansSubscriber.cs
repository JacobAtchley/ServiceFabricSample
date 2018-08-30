using System.Threading.Tasks;

namespace Oreleans.Observers.Interfaces
{
    public interface IOrleansSubscriber
    {
        Task InitClientAsync();

        void Unsubscribe();
    }
}