using Microsoft.AspNetCore.Http;

namespace UserDirectory.WebApi.Services;

public interface IDataSourceContext
{
    string GetCurrentDataSource();
}

public class DataSourceContext : IDataSourceContext
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DataSourceContext(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentDataSource()
    {
        var headerValue = _httpContextAccessor.HttpContext?.Request.Headers["X-Data-Source"].ToString();
        if (!string.IsNullOrWhiteSpace(headerValue))
            return headerValue;
        return _configuration["DataSource"] ?? "MSSMS";
    }
}
