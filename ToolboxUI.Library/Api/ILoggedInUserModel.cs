using System;

namespace ToolboxUI.Library.Api
{
    public interface ILoggedInUserModel
    {
        int CanEditDataArchitecture { get; set; }
        int CanEditEmployeeManagement { get; set; }
        int CanEditTermsAndDefinitions { get; set; }
        int CanEditTicketTracker { get; set; }
        int CanViewDataArchitecture { get; set; }
        int CanViewEmployeeManagement { get; set; }
        int CanViewTermsAndDefinitions { get; set; }
        int CanViewTicketTracker { get; set; }
        DateTime CreatedDate { get; set; }
        string EmailAddress { get; set; }
        string FirstName { get; set; }
        string Id { get; set; }
        DateTime LastModified { get; set; }
        string LastName { get; set; }
        string Token { get; set; }
    }
}