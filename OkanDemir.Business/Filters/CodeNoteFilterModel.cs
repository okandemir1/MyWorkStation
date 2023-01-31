using OkanDemir.Dto;

namespace OkanDemir.Business.Filters
{
    public class CodeNoteFilterModel : FilterModelBase
    {
        public string Term { get; set; }
        public int UserId { get; set; }

        public CodeNoteFilterModel(DataTableParameters dataTableParameters)
            : base(dataTableParameters)
        {
            if (dataTableParameters.Search?.Value?.Length > 0)
                Term = dataTableParameters.Search.Value;
            if (dataTableParameters.UserId > 0)
                UserId = dataTableParameters.UserId;
        }

        public CodeNoteFilterModel()
        {
        }
    }
    public static partial class FilterExtensions
    {
        public static IQueryable<CodeNoteDto> AddSearchFilters(this IQueryable<CodeNoteDto> input, CodeNoteFilterModel filter)
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
