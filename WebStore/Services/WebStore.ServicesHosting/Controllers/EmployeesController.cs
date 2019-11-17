using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.ServicesHosting.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase, IEmployeeService
    {
        private readonly IEmployeeService employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        [HttpPost]
        public void AddNew([FromBody]EmployeeView model)=>employeeService.AddNew(model);
        
        [HttpPut("{id}")]
        public EmployeeView Update(int id, [FromBody] EmployeeView model) => employeeService.Update(id, model);
        [NonAction]
        public void Commit() => employeeService.Commit();
        [HttpDelete("{id}")]
        public void Delete(int id) => employeeService.Delete(id);
        [HttpGet]
        public IEnumerable<EmployeeView> GetAll() => employeeService.GetAll();
        [HttpGet("{id}")]
        public EmployeeView GetById(int id) => employeeService.GetById(id);
    }
}