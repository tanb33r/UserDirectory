using Microsoft.AspNetCore.Http;

namespace UserDirectory.WebApi.Services;

public interface IDataSourceContext
{
    string GetCurrentDataSource();
    void SetGlobalDataSource(string dataSource);
}

public class DataSourceContext : IDataSourceContext
{
    private readonly IConfiguration _configuration;
    private static string? _globalDataSource = null;

    public DataSourceContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetCurrentDataSource()
    {
        _globalDataSource ??= _configuration["DataSource"] ?? "MSSMS";
        return _globalDataSource!;
    }

    public void SetGlobalDataSource(string dataSource)
    {
        _globalDataSource = dataSource;
    }
}
