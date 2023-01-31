using OkanDemir.Dto;

namespace OkanDemir.Business.Filters
{
    public class InvoiceTypeFilterModel : FilterModelBase
    {
        public string Term { get; set; }
        public int UserId { get; set; }

        public InvoiceTypeFilterModel(DataTableParameters dataTableParameters)
            : base(dataTableParameters)
        {
            if (dataTableParameters.UserId > 0)
                UserId = dataTableParameters.UserId;
            if (dataTableParameters.Search?.Value?.Length > 0)
                Term = dataTableParameters.Search.Value;
        }

        public InvoiceTypeFilterModel()
        {
        }
    }
    public static partial class FilterExtensions
    {
        public static IQueryable<InvoiceTypeDto> AddSearchFilters(this IQueryable<InvoiceTypeDto> input, InvoiceTypeFilterModel filter)
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
