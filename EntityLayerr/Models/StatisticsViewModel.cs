using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayerr.Models
{
    public class StatisticsViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<PostStatisticsViewModel> MonthlyStats { get; set; } = new();

    }
}
