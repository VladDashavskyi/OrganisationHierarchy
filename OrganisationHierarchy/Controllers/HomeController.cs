using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganisationHierarchy.Dal.Context;
using OrganisationHierarchy.Interfaces;
using OrganisationHierarchy.Models;
using OrganisationHierarchy.Services;
using Newtonsoft;
using System.Diagnostics;
using Newtonsoft.Json;

namespace OrganisationHierarchy.Controllers
{
    public class HomeController : Controller
    {
        
        ApplicationContext db;
        private readonly ILogger<HomeController> _logger;
        private readonly IOrganisationServices _organisationServices;
        public HomeController(ApplicationContext context, ILogger<HomeController> logger, IOrganisationServices organisationServices)
        {
            _logger = logger;
            db = context;
            _organisationServices = organisationServices;
        }

        public async Task<IActionResult> Index()
        {            
            return View();
        }

        public async Task<IActionResult> EmployeeList()
        {
            var r = await _organisationServices.GetEmployees();
            return View(r);
        }
        [HttpGet]
        public IActionResult ListEmployee()
        {
            var r = _organisationServices.GetEmployees().GetAwaiter().GetResult();
            return View(r);
        }

        public async Task<IActionResult> TreeView()
        {
            var nodes = await _organisationServices.GetDataForTree();
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            var r = await _organisationServices.GetEmployees();
            return View(r.Where(w => w.Id == id).FirstOrDefault());
        }

        public async Task<IActionResult> AddUser()
        {
            var r = await _organisationServices.GetDataForNewUser();
            return View(r);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(EmployeeDataModel employeeDataModel)
        {
            await _organisationServices.UpdateEmployee(employeeDataModel);
            return RedirectToAction("ListEmployee");
        }        
        
        [HttpPost]
        public async Task<IActionResult> AddNewUser(EmployeeDataModel employeeDataModel)
        {
            await _organisationServices.AddEmployee(employeeDataModel);
            return RedirectToAction("ListEmployee");
        }

        [HttpPost]
        public async Task<IActionResult> SaveTree(TreeDataModelView treeDataModelView)
        {
            await _organisationServices.UpdateTree(treeDataModelView);

            var nodes = await _organisationServices.GetDataForTree();
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            return View();
        }
    }
}