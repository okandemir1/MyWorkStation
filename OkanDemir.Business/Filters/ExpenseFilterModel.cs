using OkanDemir.Dto;

namespace OkanDemir.Business.Filters
{
    public class ExpenseFilterModel : FilterModelBase
    {
        public string Term { get; set; }
        public int UserId { get; set; }

        public ExpenseFilterModel(DataTableParameters dataTableParameters)
            : base(dataTableParameters)
        {
            if (dataTableParameters.UserId > 0)
                UserId = dataTableParameters.UserId;
            if (dataTableParameters.Search?.Value?.Length > 0)
                Term = dataTableParameters.Search.Value;
        }

        public ExpenseFilterModel()
        {
        }
    }
    public static partial class FilterExtensions
    {
        public static IQueryable<ExpenseDto> AddSearchFilters(this IQueryable<ExpenseDto> input, ExpenseFilterModel filter)
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
