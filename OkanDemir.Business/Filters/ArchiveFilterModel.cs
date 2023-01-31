using OkanDemir.Dto;

namespace OkanDemir.Business.Filters
{
    public class ArchiveFilterModel : FilterModelBase
    {
        public string Term { get; set; }
        public int UserId { get; set; }

        public ArchiveFilterModel(DataTableParameters dataTableParameters)
            : base(dataTableParameters)
        {
            if (dataTableParameters.UserId > 0)
                UserId = dataTableParameters.UserId;
            if (dataTableParameters.Search?.Value?.Length > 0)
                Term = dataTableParameters.Search.Value;
        }

        public ArchiveFilterModel()
        {
        }
    }
    public static partial class FilterExtensions
    {
        public static IQueryable<ArchiveDto> AddSearchFilters(this IQueryable<ArchiveDto> input, ArchiveFilterModel filter)
        {
            input = input.Where(x => x.UserId == filter.UserId);

            if (filter != null)
            {
                if (filter.Term?.Length > 0)
                {
                    input = input.Where(x => x.Domain.Contains(filter.Term) || x.Fullname.Contains(filter.Term));
                }
            }

            return input;
        }
    }
}
