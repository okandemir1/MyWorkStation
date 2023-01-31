using OkanDemir.Dto;

namespace OkanDemir.Business.Filters
{
    public class CodeCategoryFilterModel : FilterModelBase
    {
        public string Term { get; set; }
        public int UserId { get; set; }

        public CodeCategoryFilterModel(DataTableParameters dataTableParameters)
            : base(dataTableParameters)
        {
            if (dataTableParameters.Search?.Value?.Length > 0)
                Term = dataTableParameters.Search.Value;
            if (dataTableParameters.UserId > 0)
                UserId = dataTableParameters.UserId;
        }

        public CodeCategoryFilterModel()
        {
        }
    }
    public static partial class FilterExtensions
    {
        public static IQueryable<CodeCategoryDto> AddSearchFilters(this IQueryable<CodeCategoryDto> input, CodeCategoryFilterModel filter)
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
