using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonthlyRevenueAPI.Core.Models;

public class MonthlyRevenue
{
    public string ReportDate { get; set; } = string.Empty;
    public string DataYearMonth { get; set; } = string.Empty;
    public string CompanyCode { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public decimal CurrentRevenue { get; set; }
    public decimal PreviousRevenue { get; set; }
    public decimal LastYearRevenue { get; set; }
    public decimal? MoMChange { get; set; }
    public decimal? YoYChange { get; set; }
    public decimal CurrentYTDRevenue { get; set; }
    public decimal LastYearYTDRevenue { get; set; }
    public decimal? YTDChange { get; set; }
    public string? Notes { get; set; }
}
