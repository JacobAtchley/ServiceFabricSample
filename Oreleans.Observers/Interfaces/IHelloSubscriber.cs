using System.Threading.Tasks;

namespace Oreleans.Observers.Interfaces
{
    public interface IHelloSubscriber
    {
        Task InitClientAsync();

        void Unsubscribe();
    }
}