using OkanDemir.Dto;

namespace OkanDemir.Business.Filters
{
    public class InvoiceFilterModel : FilterModelBase
    {
        public string Term { get; set; }
        public int UserId { get; set; }

        public InvoiceFilterModel(DataTableParameters dataTableParameters)
            : base(dataTableParameters)
        {
            if (dataTableParameters.UserId > 0)
                UserId = dataTableParameters.UserId;
            if (dataTableParameters.Search?.Value?.Length > 0)
                Term = dataTableParameters.Search.Value;
        }

        public InvoiceFilterModel()
        {
        }
    }
    public static partial class FilterExtensions
    {
        public static IQueryable<InvoiceDto> AddSearchFilters(this IQueryable<InvoiceDto> input, InvoiceFilterModel filter)
        {
            input = input.Where(x => x.UserId == filter.UserId);

            if (filter != null)
            {
                if (filter.Term?.Length > 0)
                {
                    input = input.Where(x => x.InvoiceTypeName.Contains(filter.Term));
                }
            }

            return input;
        }
    }
}
