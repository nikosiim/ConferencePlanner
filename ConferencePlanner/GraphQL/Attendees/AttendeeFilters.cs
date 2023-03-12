using ConferencePlanner.Data.Entities;
using HotChocolate.Data.Filters;

namespace ConferencePlanner.GraphQL.Attendees
{
    public class AttendeeFilterType : FilterInputType<Attendee>
    {
        protected override void Configure(IFilterInputTypeDescriptor<Attendee> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(f => f.UserName).Type<UserNameOperationFilterInput>();
        }
    }

    public class UserNameOperationFilterInput : StringOperationFilterInputType
    {
        protected override void Configure(IFilterInputTypeDescriptor descriptor)
        {
            descriptor.Operation(DefaultFilterOperations.Equals).Type<StringType>();
            descriptor.Operation(DefaultFilterOperations.NotEquals).Type<StringType>();
        }
    }
}