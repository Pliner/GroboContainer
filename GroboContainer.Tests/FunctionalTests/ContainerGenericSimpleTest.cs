using NUnit.Framework;

namespace GroboContainer.Tests.FunctionalTests
{
    public class ContainerGenericSimpleTest : ContainerTestBase
    {
        private interface IGenericInterface<T>
        {
        }

        private interface IGenericInterface2<T>
        {
        }

        [Test]
        public void TestGenericClass()
        {
            var genericInterface = container.Get<IGenericInterface2<int>>();
            Assert.That(genericInterface, Is.InstanceOf<ImplementationGenericClass<int>>());
        }

        [Test]
        public void TestGetGenericNonAbstract()
        {
            Assert.That(container.Get<GBase<int>>(), Is.InstanceOf<GBase<int>>());
            Assert.That(container.Get<GImpl<int>>(), Is.InstanceOf<GImpl<int>>());
        }

        [Test]
        public void TestSimpleGeneric()
        {
            var genericInterface = container.Get<IGenericInterface<int>>();
            Assert.That(genericInterface, Is.InstanceOf<ImplementationClass>());
        }

        private class ImplementationClass : IGenericInterface<int>
        {
        }

        private class ImplementationGenericClass<T> : IGenericInterface2<T>
        {
        }

        private class GBase<T>
        {
        }

        private class GImpl<T> : GBase<T>
        {
        }
    }
}