using Caliburn.Micro;
using ToolboxUI.EventModels;
using ToolboxUI.Library.Api;
using ToolboxUI.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxUI.ViewModels
{
    public class SideMenuViewModel : Screen
    {
        private IAPIHelper _apiHelper;
        private IEventAggregator _events;
        private ILoggedInUserModel _user;

        public SideMenuViewModel(IAPIHelper apiHelper, IEventAggregator events, ILoggedInUserModel user)
        {
            _apiHelper = apiHelper;
            _events = events;
            _user = user;
        }

        public bool CanViewDataArchitecture
        {
            get
            {
                bool output = false;

                if (_user.CanViewDataArchitecture == 1)
                    output = true;

                return output;
            }
        }

        public bool CanViewEmployeeManagement
        {
            get
            {
                bool output = false;

                if (_user.CanViewEmployeeManagement == 1)
                    output = true;

                return output;
            }
        }

        public bool CanViewTermsAndDefinitions
        {
            get
            {
                bool output = false;

                if (_user.CanViewTermsAndDefinitions == 1)
                    output = true;

                return output;
            }
        }

        public bool CanViewTicketTracker
        {
            get
            {
                bool output = false;

                if (_user.CanViewTicketTracker == 1)
                    output = true;

                return output;
            }
        }
    }
}
