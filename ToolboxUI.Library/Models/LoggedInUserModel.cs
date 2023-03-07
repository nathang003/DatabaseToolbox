using ToolboxUI.Library.Api;

using System;

namespace ToolboxUI.Library.Models
{
    public class LoggedInUserModel : ILoggedInUserModel
    {
        public string Token { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
        public int CanViewDataArchitecture { get; set; }
        public int CanEditDataArchitecture { get; set; }
        public int CanViewEmployeeManagement { get; set; }
        public int CanEditEmployeeManagement { get; set; }
        public int CanViewTermsAndDefinitions { get; set; }
        public int CanEditTermsAndDefinitions { get; set; }
        public int CanViewTicketTracker { get; set; }
        public int CanEditTicketTracker { get; set; }
    }
}
