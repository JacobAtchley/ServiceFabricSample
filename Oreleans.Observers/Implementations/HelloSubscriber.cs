using App.Core.Models;
using Grains.Interfaces.Grains;
using Grains.Interfaces.Observers;
using Oreleans.Observers.Abstractions;

namespace Oreleans.Observers.Implementations
{
    public class OrleansSubscriber : AbstractOrleansSubscriber<IMyFirstGrain, IHelloObserver>
    {
        /// <inheritdoc />
        public OrleansSubscriber(IHelloObserver observer, OrleansClientConnectionOptions options) : base(observer, options) { }
    }
}