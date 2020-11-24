using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementNetCoreMVC.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private AppDbContext Context { get; }
        public SQLEmployeeRepository(AppDbContext context)
        {
            Context = context;
        }


        public Employee Add(Employee employee)
        {
            Context.Employees.Add(employee);
            Context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            var employee = Context.Employees.Find(id);
            if (employee != null)
            {
                Context.Employees.Remove(employee);
                Context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return Context.Employees;
        }

        public Employee GetEmployee(int? id)
        {
            return Context.Employees.Find(id);
        }

        public Employee Update(Employee employeeChanges)
        {
           var employee = Context.Employees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            Context.SaveChanges();
            return employeeChanges;
        }
    }
}
