using System;
using System.Diagnostics;
using GroboContainer.Core;
using GroboContainer.Impl;
using NUnit.Framework;

namespace Tests.FunctionalTests
{
    public class GetContainerTest : CoreTestBase
    {
        #region Setup/Teardown

        public override void SetUp()
        {
            base.SetUp();
            configuration = new ContainerConfiguration(new[] {GetType().Assembly});
            container = new Container(configuration);
        }


        public override void TearDown()
        {
            Debug.WriteLine(container.LastConstructionLog);
            base.TearDown();
        }

        #endregion

        private interface I1
        {
        }

        private class C1 : I1
        {
        }

        private class C2
        {
            public readonly IContainer container;

            public C2(IContainer container)
            {
                this.container = container;
            }
        }

        private class C3
        {
            public readonly Func<IContainer> getContainer;

            public C3(Func<IContainer> getContainer)
            {
                this.getContainer = getContainer;
            }
        }

        private IContainerConfiguration configuration;
        private IContainer container;


        [Test]
        public void TestContinerInConstructor()
        {
            var expected = container.Get<I1>();
            var c2 = container.Get<C2>();
            Assert.AreSame(expected, c2.container.Get<I1>());
        }

        [Test]
        public void TestGetFunc()
        {
            var expected = container.Get<I1>();
            var c3 = container.Get<C3>();
            Assert.AreSame(expected, c3.getContainer().Get<I1>());
        }

        [Test]
        public void TestGetImplementationTypes()
        {
            Type[] types = container.GetImplementationTypes(typeof (IContainer));
            CollectionAssert.AreEquivalent(new[] {typeof (Container)}, types);
        }

        [Test]
        public void TestShareInstances()
        {
            var expected = container.Get<I1>();
            var copy = container.Get<IContainer>();
            Assert.AreSame(expected, copy.Get<I1>());
        }
    }
}