using System;
using GroboContainer.Impl.Implementations;
using NMock2;

namespace Tests.TypesHelperTests
{
    public static class TypesHelperExpensions
    {
        public static void ExpectIsIgnoredImplementation(this ITypesHelper mock, Type type, bool result)
        {
            Expect.Once.On(mock)
                .Method("IsIgnoredImplementation")
                .With(type)
                .Will(Return.Value(result));
        }

        public static void ExpectIsIgnoredAbstraction(this ITypesHelper mock, Type type, bool result)
        {
            Expect.Once.On(mock)
                .Method("IsIgnoredAbstraction")
                .With(type)
                .Will(Return.Value(result));
        }

        public static void ExpectTryGetImplementation(this ITypesHelper mock, Type abstractionType, Type type,
                                                      Type result)
        {
            Expect.Once.On(mock)
                .Method("TryGetImplementation")
                .With(abstractionType, type)
                .Will(Return.Value(result));
        }
    }
}