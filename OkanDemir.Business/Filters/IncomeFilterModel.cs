using OkanDemir.Dto;

namespace OkanDemir.Business.Filters
{
    public class IncomeFilterModel : FilterModelBase
    {
        public string Term { get; set; }
        public int UserId { get; set; }

        public IncomeFilterModel(DataTableParameters dataTableParameters)
            : base(dataTableParameters)
        {
            if (dataTableParameters.UserId > 0)
                UserId = dataTableParameters.UserId;
            if (dataTableParameters.Search?.Value?.Length > 0)
                Term = dataTableParameters.Search.Value;
        }

        public IncomeFilterModel()
        {
        }
    }
    public static partial class FilterExtensions
    {
        public static IQueryable<IncomeDto> AddSearchFilters(this IQueryable<IncomeDto> input, IncomeFilterModel filter)
        {
            input = input.Where(x => x.UserId == filter.UserId);
            return input;
        }
    }
}
