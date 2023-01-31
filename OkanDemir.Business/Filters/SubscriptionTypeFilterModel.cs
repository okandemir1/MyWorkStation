using OkanDemir.Dto;

namespace OkanDemir.Business.Filters
{
    public class SubscriptionTypeFilterModel : FilterModelBase
    {
        public string Term { get; set; }
        public int UserId { get; set; }

        public SubscriptionTypeFilterModel(DataTableParameters dataTableParameters)
            : base(dataTableParameters)
        {
            if (dataTableParameters.UserId > 0)
                UserId = dataTableParameters.UserId;
            if (dataTableParameters.Search?.Value?.Length > 0)
                Term = dataTableParameters.Search.Value;
        }

        public SubscriptionTypeFilterModel()
        {
        }
    }
    public static partial class FilterExtensions
    {
        public static IQueryable<SubscriptionTypeDto> AddSearchFilters(this IQueryable<SubscriptionTypeDto> input, SubscriptionTypeFilterModel filter)
        {
            input = input.Where(x => x.UserId == filter.UserId);

            if (filter != null)
            {
                if (filter.Term?.Length > 0)
                {
                    input = input.Where(x => x.Title.Contains(filter.Term));
                }
            }

            return input;
        }
    }
}
