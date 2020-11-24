using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementNetCoreMVC.Models
{
    public static class  ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Asif",
                    Email = "asif@mail.com",
                    Department = Dept.IT

                },
                new Employee
                {
                    Id = 2,
                    Name = "David",
                    Email = "david@mail.com",
                    Department = Dept.HR

                }
                );
        }
    }
}
