using MonthlyRevenueAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonthlyRevenueAPI.Core.Services.Interfaces;

public interface IMonthlyRevenueService
{
    Task<IEnumerable<MonthlyRevenue>> GetMonthlyRevenueAsync(MonthlyRevenueQuery query);
    Task<bool> InsertMonthlyRevenueAsync(MonthlyRevenue revenue);
}
