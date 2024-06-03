using Microsoft.AspNetCore.Mvc;
using MvcCoreTutorial.Models;
using MvcCoreTutorial.Models.Domain;

namespace MvcCoreTutorial.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly DatabaseContext _ctx;
        public EmployeeController(DatabaseContext ctx)
        {
            _ctx = ctx;
        }
        public IActionResult AddEmployees()
        {
            var employees = new List<Employee> { 
            new Employee{Name="John",Email="john@xyz.com"},
            new Employee{Name="Mary",Email="mary@xyz.com"},
            new Employee{Name="Max",Email="max@xyz.com"},
            new Employee{Name="jack",Email="jack@xyz.com"},
            new Employee{Name="emp5",Email="emp5@xyz.com"},
            new Employee{Name="emp6",Email="emp6@xyz.com"},
            new Employee{Name="emp7",Email="emp7@xyz.com"},
            new Employee{Name="emp8",Email="emp8@xyz.com"},
            new Employee{Name="emp9",Email="emp9@xyz.com"},
            new Employee{Name="emp10",Email="emp10@xyz.com"},
            new Employee{Name="emp11",Email="emp11@xyz.com"},
            new Employee{Name="Jacob",Email="jacob@gmail.com"},
            };
            _ctx.AddRange(employees);
            try
            {
                _ctx.SaveChanges();
            return Content("done");
            }
            catch(Exception ex)
            {
                return Content("failed");

            }
        }

        public IActionResult Employees(string term = "",string orderBy="",int currentPage=1)
        {
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();
            var empData = new EmployeeViewModel();
            // we are toggling order cases
            empData.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "name_desc" : "";
            empData.EmailSortOrder = orderBy == "email" ? "email_desc" : "email";

            var employees = (from emp in _ctx.Employee
                             where term == "" || emp.Name.ToLower().StartsWith(term)
                             select new Employee
                             {
                                 Id = emp.Id,
                                 Name = emp.Name,
                                 Email = emp.Email
                             }
                            );
            switch (orderBy)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(a => a.Name);
                    break;
                case "email_desc":
                    employees = employees.OrderByDescending(a => a.Email);
                    break;
                case "email":
                    employees = employees.OrderBy(a => a.Email);
                    break;
                default:
                    employees = employees.OrderBy(a => a.Name);
                    break;
            }
            int totalRecords = employees.Count();
            int pageSize = 5;
            int totalPages = (int) Math.Ceiling(totalRecords / (double)pageSize);
            employees = employees.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            empData.Employees = employees;
            empData.CurrentPage = currentPage;
            empData.TotalPages = totalPages;
            empData.Term = term;
            empData.PageSize = pageSize;
            empData.OrderBy = orderBy;
            return View(empData);
        }

    }
}
