using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebStore.Clients.Base;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    public class EmployeeClient : BaseClient, IEmployeeService
    {
        public EmployeeClient(IConfiguration conf):base(conf, "api/employees"){ }
        public void AddNew(EmployeeView model) => Post<EmployeeView>(_serviceAddress, model);

        public void Commit() { }

        public void Delete(int id) => Delete($"{_serviceAddress}/{id}");

        public IEnumerable<EmployeeView> GetAll() => Get<List<EmployeeView>>(_serviceAddress);

        public EmployeeView GetById(int id) => Get<EmployeeView>($"{_serviceAddress}/{id}");

        public EmployeeView Update(int id, EmployeeView model) 
        { 
            var response = Put<EmployeeView>($"{_serviceAddress}/{id}", model);
            return response.Content.ReadAsAsync<EmployeeView>().Result;
        }  
    }
}
