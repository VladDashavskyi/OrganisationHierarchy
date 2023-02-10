using System.ComponentModel.DataAnnotations;

namespace OrganisationHierarchy.Models
{
    public class EmployeeDataModel
    {
        
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public string Position { get; set; }
        public int PositionId { get; set; }
        public int? LeaderId { get; set; }
        public List<Item> Positions { get; set; }
        public List<Item> Leaders { get; set; }

    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
