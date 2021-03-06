﻿using WebStore.Interfaces.Services;
using WebStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly List<EmployeeView> _employeeViews;
        public EmployeeService()
        {
            _employeeViews = new List<EmployeeView> {
            new EmployeeView {
                Id=1,
                FirstName="Иван",
                SurName="Петров",
                Patronymic="Вячеславович",
                DateOfBirth=DateTime.Parse("10.12.1956"),
                Post="Директор"
            },
            new EmployeeView
            {
                Id = 2,
                FirstName = "Александр",
                SurName = "Иванов",
                Patronymic = "Петрович",
                DateOfBirth = DateTime.Parse("11.02.1959"),
                Post = "Уборщик"
            },
            new EmployeeView
            {
                Id = 3,
                FirstName = "Виктория",
                SurName = "Сидоркина",
                Patronymic = "Альбертовна",
                DateOfBirth = DateTime.Parse("08.03.1989"),
                Post = "Секретарша"
            }
            };
        }
        public void AddNew(EmployeeView model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));
            model.Id = _employeeViews.Max(e => e.Id) + 1;
            _employeeViews.Add(model);
        }

        public void Commit()
        {
            
        }

        public void Delete(int id)
        {
            var emp = GetById(id);
            if (emp is null)
                return;
            _employeeViews.Remove(emp);
        }

        public IEnumerable<EmployeeView> GetAll()
        {
            return _employeeViews;
        }

        public EmployeeView GetById(int id)
        {
            return _employeeViews.FirstOrDefault(e => e.Id == id);
        }

        public EmployeeView Update(int id, EmployeeView model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));
            var item = GetById(id);
            if (item is null)
                throw new InvalidOperationException($"Сотрудник с id={id} не найден.");
            item.DateOfBirth = model.DateOfBirth;
            item.FirstName = model.FirstName;
            item.Patronymic = model.Patronymic;
            item.Post = model.Post;
            item.SurName = model.SurName;
            
            return item;
        }
    }
}
