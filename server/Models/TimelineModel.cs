using System.Collections.Generic;

namespace WebApplication.Models
{
    public class TimelineModel {
        public IEnumerable<TimelineItemModel> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int OverallLength { get; set; }
    }
}