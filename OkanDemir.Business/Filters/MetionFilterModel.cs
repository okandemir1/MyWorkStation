using OkanDemir.Dto;

namespace OkanDemir.Business.Filters
{
    public class MetionFilterModel : FilterModelBase
    {
        public string Term { get; set; }
        public int UserId { get; set; }

        public MetionFilterModel(DataTableParameters dataTableParameters)
            : base(dataTableParameters)
        {
            if (dataTableParameters.UserId > 0)
                UserId = dataTableParameters.UserId;
            if (dataTableParameters.Search?.Value?.Length > 0)
                Term = dataTableParameters.Search.Value;
        }

        public MetionFilterModel()
        {
        }
    }
    public static partial class FilterExtensions
    {
        public static IQueryable<MetionDto> AddSearchFilters(this IQueryable<MetionDto> input, MetionFilterModel filter)
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
