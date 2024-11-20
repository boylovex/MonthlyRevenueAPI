using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonthlyRevenueAPI.Core.Models;

public class MonthlyRevenueQuery
{
    public string? DataYearMonth { get; set; }
    public string? CompanyCode { get; set; }
    public string? Industry { get; set; }
}