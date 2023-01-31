using OkanDemir.Dto;

namespace OkanDemir.Business.Filters
{
    public class ArchiveCategoryFilterModel : FilterModelBase
    {
        public string Term { get; set; }
        public int UserId { get; set; }

        public ArchiveCategoryFilterModel(DataTableParameters dataTableParameters)
            : base(dataTableParameters)
        {
            if (dataTableParameters.Search?.Value?.Length > 0)
                Term = dataTableParameters.Search.Value;
            if (dataTableParameters.UserId > 0)
                UserId = dataTableParameters.UserId;
        }

        public ArchiveCategoryFilterModel()
        {
        }
    }
    public static partial class FilterExtensions
    {
        public static IQueryable<ArchiveCategoryDto> AddSearchFilters(this IQueryable<ArchiveCategoryDto> input, ArchiveCategoryFilterModel filter)
        {
            input = input.Where(x => x.UserId == filter.UserId);

            if (filter != null)
            {
                if (filter.Term?.Length > 0)
                {
                    input = input.Where(x => x.Name.Contains(filter.Term) 
                    || x.Summary.Contains(filter.Term));
                }
            }

            return input;
        }
    }
}
