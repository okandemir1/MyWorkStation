using OkanDemir.Dto;

namespace OkanDemir.Business.Filters
{
    public class SubscriptionFilterModel : FilterModelBase
    {
        public string Term { get; set; }
        public int UserId { get; set; }

        public SubscriptionFilterModel(DataTableParameters dataTableParameters)
            : base(dataTableParameters)
        {
            if (dataTableParameters.UserId > 0)
                UserId = dataTableParameters.UserId;
            if (dataTableParameters.Search?.Value?.Length > 0)
                Term = dataTableParameters.Search.Value;
        }

        public SubscriptionFilterModel()
        {
        }
    }
    public static partial class FilterExtensions
    {
        public static IQueryable<SubscriptionDto> AddSearchFilters(this IQueryable<SubscriptionDto> input, SubscriptionFilterModel filter)
        {
            input = input.Where(x => x.UserId == filter.UserId);

            if (filter != null)
            {
                if (filter.Term?.Length > 0)
                {
                    input = input.Where(x => x.SubscriptionTypeName.Contains(filter.Term));
                }
            }

            return input;
        }
    }
}
