using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementNetCoreMVC.Controllers
{
    public class DepartmentsController : Controller
    {
        public string List()
        {
            return "list";
        }
        public string Details()
        {
            return "Details";
        }
    }
}