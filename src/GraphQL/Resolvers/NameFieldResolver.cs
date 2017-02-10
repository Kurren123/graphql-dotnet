using System;
using System.Collections.Generic;
using System.Reflection;
using GraphQL.Types;

namespace GraphQL.Resolvers
{
    internal class NameFieldResolver : IFieldResolver
    {
        private BindingFlags _flags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

        public object Resolve(ResolveFieldContext context)
        {
            var source = context.Source;

            if (source == null)
            {
                return null;
            }

            var sourceDict = source as IDictionary<string, object>;
            if (sourceDict != null)
            {
                try
                {
                    return sourceDict[context.FieldAst.Name];
                }
                catch (KeyNotFoundException)
                {
                    throw new InvalidOperationException($"Expected to find property {context.FieldAst.Name} on {context.Source.GetType().Name} but it does not exist.");
                }
            }

            var prop = source.GetType()
                .GetProperty(context.FieldAst.Name, _flags);

            if (prop == null)
            {
                throw new InvalidOperationException($"Expected to find property {context.FieldAst.Name} on {context.Source.GetType().Name} but it does not exist.");
            }

            return prop.GetValue(source, null);
        }
    }
}
