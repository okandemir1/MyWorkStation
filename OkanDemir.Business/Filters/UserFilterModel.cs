using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OkanDemir.Dto;
using OkanDemir.Model;

namespace OkanDemir.Business.Filters
{
    public class UserFilterModel : FilterModelBase
    {
        public string Term { get; set; }

        public UserFilterModel(DataTableParameters dataTableParameters)
            : base(dataTableParameters)
        {
            if (dataTableParameters.Search?.Value?.Length > 0)
                Term = dataTableParameters.Search.Value;
        }

        public UserFilterModel()
        {
        }
    }
    public static partial class FilterExtensions
    {
        public static IQueryable<UserDto> AddSearchFilters(this IQueryable<UserDto> input, UserFilterModel filter)
        {
            if (filter != null)
            {
                if (filter.Term?.Length > 0)
                {
                    input = input.Where(x => x.Username.Contains(filter.Term) 
                    || x.Phone.Contains(filter.Term)
                    || x.Fullname.Contains(filter.Term));
                }
            }

            return input;
        }
    }
}
