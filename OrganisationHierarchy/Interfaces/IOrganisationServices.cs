using OrganisationHierarchy.Models;

namespace OrganisationHierarchy.Interfaces
{
    public interface IOrganisationServices
    {
        Task<List<TreeDataModelView>> GetDataForTree();
        Task<List<EmployeeDataModel>> GetEmployees();
        Task UpdateEmployee(EmployeeDataModel employeeDataModel);
        Task<EmployeeDataModel> GetDataForNewUser();
        Task UpdateTree(TreeDataModelView treeDataModelView);
        Task AddEmployee(EmployeeDataModel employeeDataModel);
    }
}
