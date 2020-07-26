using System;

namespace WpfWrapperGenerator
{
    public static class TargetTypeClassifier
    {
        public static (string TypeName, string whereClause, string templateparam) Classify(Type type) =>
            IsClosedType(type)? 
                (type.CSharpName(),"",""): 
                ("TChild", "where TChild:" + type.CSharpName(), "TChild, ");

        private static bool IsClosedType(Type type) =>
            type.IsSealed || type == typeof(System.Object);
    }
}