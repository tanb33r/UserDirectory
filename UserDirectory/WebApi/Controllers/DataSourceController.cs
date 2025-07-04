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

    [HttpPost]
    public IActionResult SetDataSource([FromBody] string dataSource)
    {
        _dataSourceContext.SetGlobalDataSource(dataSource);
        return Ok();
    }

    [HttpGet]
    public ActionResult<string> GetDataSource()
    {
        var ds = _dataSourceContext.GetCurrentDataSource();
        return Ok(ds);
    }
}
