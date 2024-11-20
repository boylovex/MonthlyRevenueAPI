USE tempdb;
GO

-- 建立資料表
CREATE TABLE [dbo].[MonthlyRevenue](
	[ReportDate] char(7) NOT NULL, 
	[DataYearMonth] nvarchar(5) NOT NULL, 
	[CompanyCode] nvarchar(4) NOT NULL, 
	[CompanyName] nvarchar(100) NOT NULL, 
	[Industry] nvarchar(50) NOT NULL, 
	[CurrentRevenue] decimal(18, 0) NOT NULL, 
	[PreviousRevenue] decimal(18, 0) NOT NULL, 
	[LastYearRevenue] decimal(18, 0) NOT NULL, 
	[MoMChange] decimal(18, 6), 
	[YoYChange] decimal(18, 6), 
	[CurrentYTDRevenue] decimal(18, 0) NOT NULL, 
	[LastYearYTDRevenue] decimal(18, 0) NOT NULL, 
	[YTDChange] decimal(18, 6), 
	[Notes] nvarchar(max),
    CONSTRAINT [PK_MonthlyRevenue] PRIMARY KEY CLUSTERED 
    (
        [DataYearMonth] ASC,
        [CompanyCode] ASC
    )
);
GO

-- 建立預存程序：查詢
CREATE PROCEDURE [dbo].[sp_GetMonthlyRevenue]
    @DataYearMonth NVARCHAR(5) = NULL,
    @CompanyCode NVARCHAR(4) = NULL,
    @Industry NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM [dbo].[MonthlyRevenue]
    WHERE (@DataYearMonth IS NULL OR DataYearMonth = @DataYearMonth)
        AND (@CompanyCode IS NULL OR CompanyCode = @CompanyCode)
        AND (@Industry IS NULL OR Industry = @Industry);
END;
GO

-- 建立預存程序：新增
CREATE PROCEDURE [dbo].[sp_InsertMonthlyRevenue]
    @ReportDate CHAR(7),
    @DataYearMonth NVARCHAR(5),
    @CompanyCode NVARCHAR(4),
    @CompanyName NVARCHAR(100),
    @Industry NVARCHAR(50),
    @CurrentRevenue DECIMAL(18,0),
    @PreviousRevenue DECIMAL(18,0),
    @LastYearRevenue DECIMAL(18,0),
    @MoMChange DECIMAL(18,6),
    @YoYChange DECIMAL(18,6),
    @CurrentYTDRevenue DECIMAL(18,0),
    @LastYearYTDRevenue DECIMAL(18,0),
    @YTDChange DECIMAL(18,6),
    @Notes NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- 檢查是否已存在相同的記錄
    IF EXISTS (
        SELECT 1 
        FROM [dbo].[MonthlyRevenue] 
        WHERE DataYearMonth = @DataYearMonth 
        AND CompanyCode = @CompanyCode
    )
    BEGIN
        UPDATE [dbo].[MonthlyRevenue]
        SET 
            ReportDate = @ReportDate,
            CompanyName = @CompanyName,
            Industry = @Industry,
            CurrentRevenue = @CurrentRevenue,
            PreviousRevenue = @PreviousRevenue,
            LastYearRevenue = @LastYearRevenue,
            MoMChange = @MoMChange,
            YoYChange = @YoYChange,
            CurrentYTDRevenue = @CurrentYTDRevenue,
            LastYearYTDRevenue = @LastYearYTDRevenue,
            YTDChange = @YTDChange,
            Notes = @Notes
        WHERE DataYearMonth = @DataYearMonth 
        AND CompanyCode = @CompanyCode;
    END
    ELSE
    BEGIN
        INSERT INTO [dbo].[MonthlyRevenue]
        (
            ReportDate,
            DataYearMonth,
            CompanyCode,
            CompanyName,
            Industry,
            CurrentRevenue,
            PreviousRevenue,
            LastYearRevenue,
            MoMChange,
            YoYChange,
            CurrentYTDRevenue,
            LastYearYTDRevenue,
            YTDChange,
            Notes
        )
        VALUES
        (
            @ReportDate,
            @DataYearMonth,
            @CompanyCode,
            @CompanyName,
            @Industry,
            @CurrentRevenue,
            @PreviousRevenue,
            @LastYearRevenue,
            @MoMChange,
            @YoYChange,
            @CurrentYTDRevenue,
            @LastYearYTDRevenue,
            @YTDChange,
            @Notes
        );
    END
END;
GO

-- 建立索引
CREATE NONCLUSTERED INDEX [IX_MonthlyRevenue_CompanyCode]
ON [dbo].[MonthlyRevenue] (CompanyCode)
INCLUDE (
    CompanyName,
    CurrentRevenue,
    MoMChange,
    YoYChange,
    CurrentYTDRevenue
);

CREATE NONCLUSTERED INDEX [IX_MonthlyRevenue_Industry]
ON [dbo].[MonthlyRevenue] (Industry)
INCLUDE (
    CompanyCode,
    CompanyName,
    CurrentRevenue,
    YoYChange,
    CurrentYTDRevenue
);

CREATE NONCLUSTERED INDEX [IX_MonthlyRevenue_ReportDate]
ON [dbo].[MonthlyRevenue] (ReportDate)
INCLUDE (
    DataYearMonth,
    CompanyCode,
    CurrentRevenue,
    YoYChange
);

CREATE NONCLUSTERED INDEX [IX_MonthlyRevenue_CurrentRevenue]
ON [dbo].[MonthlyRevenue] (CurrentRevenue DESC)
INCLUDE (
    DataYearMonth,
    CompanyCode,
    CompanyName,
    Industry
);

CREATE NONCLUSTERED INDEX [IX_MonthlyRevenue_Industry_DataYearMonth]
ON [dbo].[MonthlyRevenue] (Industry, DataYearMonth)
INCLUDE (
    CompanyCode,
    CompanyName,
    CurrentRevenue,
    YoYChange
);
GO