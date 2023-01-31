using OkanDemir.Dto;

namespace OkanDemir.Business.Filters
{
    public class IncomeTypeFilterModel : FilterModelBase
    {
        public string Term { get; set; }
        public int UserId { get; set; }

        public IncomeTypeFilterModel(DataTableParameters dataTableParameters)
            : base(dataTableParameters)
        {
            if (dataTableParameters.UserId > 0)
                UserId = dataTableParameters.UserId;
            if (dataTableParameters.Search?.Value?.Length > 0)
                Term = dataTableParameters.Search.Value;
        }

        public IncomeTypeFilterModel()
        {
        }
    }
    public static partial class FilterExtensions
    {
        public static IQueryable<IncomeTypeDto> AddSearchFilters(this IQueryable<IncomeTypeDto> input, IncomeTypeFilterModel filter)
        {
            input = input.Where(x => x.UserId == filter.UserId);

            if (filter != null)
            {
                if (filter.Term?.Length > 0)
                {
                    input = input.Where(x => x.Name.Contains(filter.Term));
                }
            }

            return input;
        }
    }
}
