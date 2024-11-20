using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MonthlyRevenueAPI.Core.Models;
using MonthlyRevenueAPI.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonthlyRevenueAPI.Core.Services;

public class MonthlyRevenueService(IConfiguration configuration, ILogger<MonthlyRevenueService> logger) : IMonthlyRevenueService
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException(nameof(configuration));

    public async Task<IEnumerable<MonthlyRevenue>> GetMonthlyRevenueAsync(MonthlyRevenueQuery query)
    {
        using var connection = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@DataYearMonth", query.DataYearMonth);
        parameters.Add("@CompanyCode", query.CompanyCode);
        parameters.Add("@Industry", query.Industry);

        try
        {
            var results = await connection.QueryAsync<MonthlyRevenue>(
                "sp_GetMonthlyRevenue",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return results;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting monthly revenue data");
            throw;
        }
    }

    public async Task<bool> InsertMonthlyRevenueAsync(MonthlyRevenue revenue)
    {
        using var connection = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();

        parameters.Add("@ReportDate", revenue.ReportDate);
        parameters.Add("@DataYearMonth", revenue.DataYearMonth);
        parameters.Add("@CompanyCode", revenue.CompanyCode);
        parameters.Add("@CompanyName", revenue.CompanyName);
        parameters.Add("@Industry", revenue.Industry);
        parameters.Add("@CurrentRevenue", revenue.CurrentRevenue);
        parameters.Add("@PreviousRevenue", revenue.PreviousRevenue);
        parameters.Add("@LastYearRevenue", revenue.LastYearRevenue);
        parameters.Add("@MoMChange", revenue.MoMChange);
        parameters.Add("@YoYChange", revenue.YoYChange);
        parameters.Add("@CurrentYTDRevenue", revenue.CurrentYTDRevenue);
        parameters.Add("@LastYearYTDRevenue", revenue.LastYearYTDRevenue);
        parameters.Add("@YTDChange", revenue.YTDChange);
        parameters.Add("@Notes", revenue.Notes);

        try
        {
            await connection.ExecuteAsync(
                "sp_InsertMonthlyRevenue",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inserting monthly revenue data");
            throw;
        }
    }
}
