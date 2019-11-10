using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        public string ErrorString { get; set; }
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            //return Content("Привет изконтроллера!");
            return View(_employeeService.GetAll());
        }

        public IActionResult Details(int id)
        {
            return View(_employeeService.GetById(id));
        }
        [Authorize(Roles ="Administrator")]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return View(new EmployeeView());
            }
            var emp = _employeeService.GetById(id.Value);
            if (emp is null)
                return NotFound();
            return View(emp);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(EmployeeView model)
        {
            var Age = (DateTime.Now.Year - model.DateOfBirth.Year);
            if (Age < 18 | Age > 100)
                ModelState.AddModelError("DateOfBirth", $"Возраст должен быть больше 18 лет и меньшк 100 лет {Age}"); ;
            if (!ModelState.IsValid)
                return View(model);
            if (model.Id > 0)
            {
                var dbItem = _employeeService.GetById(model.Id);
                if (dbItem is null)
                    return NotFound();
                dbItem.FirstName = model.FirstName;
                dbItem.SurName = model.SurName;
                dbItem.Patronymic = model.Patronymic;
                dbItem.DateOfBirth = model.DateOfBirth;
                dbItem.Post = model.Post;
            }
            else
            {
                _employeeService.AddNew(model);
            }
            _employeeService.Commit();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            _employeeService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}