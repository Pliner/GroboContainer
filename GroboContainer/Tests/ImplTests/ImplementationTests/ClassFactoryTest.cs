using System;
using System.Collections.Generic;
using GroboContainer.Impl;
using GroboContainer.Impl.ClassCreation;
using GroboContainer.Impl.Injection;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests.ImplTests.ImplementationTests
{
    public class ClassFactoryTest : TestBase
    {
        #region Setup/Teardown

        public override void SetUp()
        {
            base.SetUp();
            internalContainer = GetMock<IInternalContainer>();
            injectionContext = GetMock<IInjectionContext>();
            injectionContext.Expect(c => c.InternalContainer).Return(internalContainer).Repeat.Any();
        }

        #endregion

        private IInternalContainer internalContainer;
        private IInjectionContext injectionContext;

        [Test]
        public void TestCrash()
        {
            var args = new object[1];
            Type constructedType = typeof (int);
            var classFactory = new ClassFactory(delegate { throw new MockException(); }, constructedType);

            injectionContext.Expect(c => c.BeginConstruct(constructedType));
            injectionContext.Expect(c => c.Crash());
            injectionContext.Expect(c => c.EndConstruct(constructedType));
            RunMethodWithException<MockException>(() =>
                                                  classFactory.Create(injectionContext, args));
        }

        [Test]
        public void TestWork()
        {
            var args = new object[1];
            Type constructedType = typeof (int);
            var q = new Queue<int>();
            var classFactory =
                new ClassFactory(delegate(IInternalContainer aContainer, IInjectionContext aContext, object[] arg3)
                                     {
                                         Assert.AreSame(internalContainer, aContainer);
                                         Assert.AreSame(injectionContext, aContext);
                                         Assert.AreSame(args, arg3);
                                         return q.Dequeue();
                                     }, constructedType);

            injectionContext.Expect(c => c.BeginConstruct(constructedType));
            injectionContext.Expect(c => c.EndConstruct(constructedType));
            q.Enqueue(100);
            Assert.AreEqual(100, classFactory.Create(injectionContext, args));


            injectionContext.Expect(c => c.BeginConstruct(constructedType));
            injectionContext.Expect(c => c.EndConstruct(constructedType));
            q.Enqueue(200);
            Assert.AreEqual(200, classFactory.Create(injectionContext, args));
        }
    }
}