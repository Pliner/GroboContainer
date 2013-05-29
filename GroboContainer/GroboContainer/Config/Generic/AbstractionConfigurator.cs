using System.Linq;
using GroboContainer.Impl.Abstractions;

namespace GroboContainer.Config.Generic
{
    public class AbstractionConfigurator<T> : IAbstractionConfigurator<T>
    {
        private readonly AbstractionConfigurator worker;

        public AbstractionConfigurator(IAbstractionConfigurationCollection abstractionConfigurationCollection)
        {
            worker = new AbstractionConfigurator(typeof (T), abstractionConfigurationCollection);
        }

        #region IAbstractionConfigurator<T> Members

        public void UseInstances(params T[] instances)
        {
            object[] objects = instances.Cast<object>().ToArray();
            worker.UseInstances(objects);
        }


        public void Fail()
        {
            worker.Fail();
        }

        #endregion
    }
}