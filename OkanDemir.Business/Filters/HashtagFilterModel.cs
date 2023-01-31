using OkanDemir.Dto;

namespace OkanDemir.Business.Filters
{
    public class HashtagFilterModel : FilterModelBase
    {
        public string Term { get; set; }
        public int UserId { get; set; }

        public HashtagFilterModel(DataTableParameters dataTableParameters)
            : base(dataTableParameters)
        {
            if (dataTableParameters.UserId > 0)
                UserId = dataTableParameters.UserId;
            if (dataTableParameters.Search?.Value?.Length > 0)
                Term = dataTableParameters.Search.Value;
        }

        public HashtagFilterModel()
        {
        }
    }
    public static partial class FilterExtensions
    {
        public static IQueryable<HashtagDto> AddSearchFilters(this IQueryable<HashtagDto> input, HashtagFilterModel filter)
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
