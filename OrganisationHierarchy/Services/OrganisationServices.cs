using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using OrganisationHierarchy.Dal.Entities;
using OrganisationHierarchy.Dal.Interfaces;
using OrganisationHierarchy.Interfaces;
using OrganisationHierarchy.Models;
using System.Text.RegularExpressions;

namespace OrganisationHierarchy.Services
{
    public class OrganisationServices : IOrganisationServices
    {
        private readonly IApplicationContext _appContext;
        public OrganisationServices(IApplicationContext appContext)
        {
            _appContext = appContext;
        }
        public async Task<List<TreeDataModelView>> GetDataForTree()
        {
            var result = _appContext.Employee
                .Include(i => i.User)
                .Include(i => i.Position)
                .Select(s => new TreeDataModelView
                {
                    Id = s.Id.ToString(),
                    Parent = s.LeaderId == null ? "#" : s.LeaderId.ToString(),
                    Text = s.User.FirstName + " " + s.User.LastName + " (" + s.Position.DisplayName + ")"

                }).ToList();
            return result;
        }

        public async Task<List<EmployeeDataModel>> GetEmployees()
        {

            var positions = _appContext.Position.Select(s => new Item
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            var leaders = _appContext.Employee
                .Include(i => i.Position)
                .Include(i => i.User)
                .Select(s => new Item
                {
                    Id = s.Id,
                    Name = s.User.FirstName + " " + s.User.LastName + " " + " (" + s.Position.DisplayName + ")"
                }).ToList();


            var result = _appContext.Employee
                .Include(i => i.User)
                .Include(i => i.Position)
                .Include(i => i.User.IdentityUser)
                .Select(s => new EmployeeDataModel
                {
                    Id = s.Id,
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                    Email = s.User.IdentityUser.Email,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Salary = s.Salary,
                    Position = s.Position.Name,
                    PositionId = s.Position.Id,
                    Positions = positions,
                    LeaderId = s.LeaderId,
                    Leaders = leaders

                }).ToList();
            return result;
        }

        public async Task UpdateEmployee(EmployeeDataModel employeeDataModel)
        {
            try
            {
                var employee = await _appContext.Employee
                .Include(i => i.User)
                .Include(i => i.Position)
                .Include(i => i.User.IdentityUser)
                .FirstOrDefaultAsync(w => w.Id == employeeDataModel.Id);

                if (employee == null)
                {
                    return;
                }

                employee.User.FirstName = employeeDataModel.FirstName;
                employee.User.LastName = employeeDataModel.LastName;

                if (employeeDataModel.StartDate != null)
                {
                    employee.StartDate = employeeDataModel.StartDate;
                }
                employee.EndDate = employeeDataModel.EndDate;

                if (employeeDataModel.PositionId != null)
                {
                    employee.PositionId = employeeDataModel.PositionId;
                }
                if (employeeDataModel.Salary != null)
                {
                    employee.Salary = employeeDataModel.Salary;
                }
                if (employeeDataModel.LeaderId != null && employeeDataModel.LeaderId != employeeDataModel.Id)
                {
                    employee.LeaderId = employeeDataModel.LeaderId;
                }
                if (employeeDataModel.Email != null)
                {
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(employeeDataModel.Email);
                    if (match.Success)
                    {
                        employee.User.IdentityUser.Email = employeeDataModel.Email.Trim();
                    }
                }

                await _appContext.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }
        }

        public async Task<EmployeeDataModel> GetDataForNewUser()
        {
            var positions = _appContext.Position.Select(s => new
            Item
            { Id = s.Id, Name = s.Name }).ToList();

            var leaders = _appContext.Employee
                .Include(i => i.Position)
                .Include(i => i.User)
                .Select(s => new Item
                {
                    Id = s.Id,
                    Name = s.User.FirstName + " " + s.User.LastName + " " + " (" + s.Position.DisplayName + ")"
                }).ToList();

            var result = _appContext.Employee
                .Include(i => i.User)
                .Include(i => i.Position)

                .Select(s => new EmployeeDataModel
                {
                    LeaderId = 1,
                    PositionId = 1,
                    Positions = positions,
                    Leaders = leaders
                }).FirstOrDefault();

            return result;
        }

        public async Task AddEmployee(EmployeeDataModel employeeDataModel)
        {
            try
            {
                if (employeeDataModel.Email != null)
                {
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(employeeDataModel.Email);
                    if (!match.Success)
                    {
                        return;
                    }
                }

                var user = new User
                {
                    FirstName = employeeDataModel.FirstName,
                    LastName = employeeDataModel.LastName,
                    MiddleName = employeeDataModel.LastName                

                };

                var newUser = _appContext.User.Add(user);
                await _appContext.SaveChangesAsync();

                var identityUser = new IdentityUser
                {
                    Id = newUser.Entity.Id,
                    Email = employeeDataModel.Email,
                    PasswordHash = Guid.NewGuid().ToString(),
                    PasswordSalt = Guid.NewGuid().ToString()
                };
                _appContext.IdentityUser.Add(identityUser);

                var employee = new Employee
                {
                    Id = newUser.Entity.Id,
                    PositionId = employeeDataModel.PositionId,
                    StartDate = employeeDataModel.StartDate,
                    LeaderId = employeeDataModel.LeaderId,
                    EndDate = employeeDataModel.EndDate,
                    Salary = employeeDataModel.Salary,
                };
                _appContext.Employee.Add(employee);

                await _appContext.SaveChangesAsync();            
            }
            catch (Exception e)
            {

            }
        }
        public async Task UpdateTree(TreeDataModelView treeDataModelView)
        {
            try
            {
                if (!string.IsNullOrEmpty(treeDataModelView.Id) && !string.IsNullOrEmpty(treeDataModelView.Parent))
                {
                    var employee = _appContext.Employee
                     .Where(w => w.Id == int.Parse(treeDataModelView.Id)).FirstOrDefault();

                    if (employee != null)
                    {
                        employee.LeaderId = int.Parse(treeDataModelView.Parent);
                        await _appContext.SaveChangesAsync();

                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }

}
