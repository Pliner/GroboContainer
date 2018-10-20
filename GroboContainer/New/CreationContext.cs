using System;
using System.Collections;
using System.Collections.Concurrent;

using GroboContainer.Core;
using GroboContainer.Impl.ClassCreation;

namespace GroboContainer.New
{
    public class CreationContext : ICreationContext
    {
        public CreationContext(IClassCreator classCreator, IConstructorSelector constructorSelector, IClassWrapperCreator classWrapperCreator)
        {
            this.classCreator = classCreator;
            this.constructorSelector = constructorSelector;
            this.classWrapperCreator = classWrapperCreator;

            valueFactory = ValueFactory;
        }

        #region ICreationContext Members

        public IClassFactory BuildFactory(Type implementationType, Type[] parameterTypes)
        {
            var classFactoryParameters = new ClassFactoryParameters(implementationType, parameterTypes);
            return factories.GetOrAdd(classFactoryParameters, valueFactory);
        }

        #endregion

        private IClassFactory ValueFactory(ClassFactoryParameters parameters)
        {
            var implementationType = parameters.implementationType;
            var parameterTypes = parameters.parameterTypes;
            return classCreator.BuildFactory(constructorSelector.GetConstructor(implementationType, parameterTypes), classWrapperCreator == null ? null : classWrapperCreator.Wrap(implementationType));
        }

        private readonly IClassCreator classCreator;
        private readonly IConstructorSelector constructorSelector;
        private readonly IClassWrapperCreator classWrapperCreator;
        private readonly ConcurrentDictionary<ClassFactoryParameters, IClassFactory> factories = new ConcurrentDictionary<ClassFactoryParameters, IClassFactory>();
        private readonly Func<ClassFactoryParameters, IClassFactory> valueFactory;

        private struct ClassFactoryParameters
        {
            public readonly Type implementationType;
            public readonly Type[] parameterTypes;

            public ClassFactoryParameters(Type implementationType, Type[] parameterTypes)
            {
                this.implementationType = implementationType;
                this.parameterTypes = parameterTypes;
            }

            private bool Equals(ClassFactoryParameters other)
            {
                return implementationType == other.implementationType
                       && StructuralComparisons.StructuralEqualityComparer.Equals(parameterTypes, other.parameterTypes);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }
                return obj is ClassFactoryParameters other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (implementationType.GetHashCode() * 397) ^ StructuralComparisons.StructuralEqualityComparer.GetHashCode(parameterTypes);
                }
            }
        }
    }
}