using HotChocolate.Types.Descriptors;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ConferencePlanner.GraphQL.Extensions
{
    public class PreserveParentAsAttribute : ObjectFieldDescriptorAttribute
    {
        public PreserveParentAsAttribute(string name, [CallerLineNumber] int order = 0)
        {
            Order = order;
            Name = name;
        }

        public string Name { get; }

        protected override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
        {
            descriptor.Use(next => async ctx =>
            {
                await next(ctx);
                ctx.SetScopedState(Name, ctx.Result);
            });
        }
    }
}