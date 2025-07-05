using Microsoft.AspNetCore.Mvc;

namespace UserDirectory.WebApi.Controllers;

[ApiController]
[Route("datasource")]
public class DataSourceController : ControllerBase
{
    private readonly Services.IDataSourceContext _dataSourceContext;
    public DataSourceController(Services.IDataSourceContext dataSourceContext)
    {
        _dataSourceContext = dataSourceContext;
    }

    [HttpGet]
    public ActionResult<string> GetDataSource()
    {
        var ds = _dataSourceContext.GetCurrentDataSource();
        return Ok(ds);
    }
}
