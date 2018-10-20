using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

using GroboContainer.Impl.ClassCreation;

namespace GroboContainer.New
{
    public class Implementation : IImplementation
    {
        public Implementation(Type implementationType)
        {
            ObjectType = implementationType;
        }

        private IClassFactory ChooseFactory(ICreationContext creationContext, Type[] parameterTypes)
        {
            if (parameterTypes.Length == 0)
            {
                if (noArgumentsFactory == null)
                    lock (configurationLock)
                        if (noArgumentsFactory == null)
                            noArgumentsFactory = creationContext.BuildFactory(ObjectType, Type.EmptyTypes);
                return noArgumentsFactory;
            }

            return creationContext.BuildFactory(ObjectType, parameterTypes);
        }

        private readonly object configurationLock = new object();
        private volatile IClassFactory noArgumentsFactory;

        #region IImplementation Members

        public Type ObjectType { get; }

        public IClassFactory GetFactory(Type[] parameterTypes, ICreationContext creationContext)
        {
            IClassFactory classFactory = ChooseFactory(creationContext, parameterTypes);
            return classFactory;
        }

        #endregion
    }
}