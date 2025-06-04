using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.Services.NewsArticles;
using FUNewsSystem.Services.Services.SystemAccounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace FUNewsSystem.API.Controllers
{
    public class SystemAccountODataController : ODataController
    {
        private readonly ISystemAccountService _systemAccountService;

        public SystemAccountODataController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            var result = _systemAccountService.GetAll();
            return Ok(result);
        }

        [EnableQuery]
        public SingleResult<SystemAccount> Get([FromODataUri] short key)
        {
            var result = _systemAccountService.GetAll().Where(b => b.AccountId == key);
            return SingleResult.Create(result);
        }
    }
}