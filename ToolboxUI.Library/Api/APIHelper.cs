﻿using ToolboxUI.Library.Models;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxUI.Library.Api
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient _apiClient { get; set; }
        private ILoggedInUserModel _loggedInUser;


        public APIHelper(ILoggedInUserModel loggedInUser)
        {
            InitializeClient();
            _loggedInUser = loggedInUser;
        }

        public HttpClient ApiClient
        {
            get
            {
                Console.WriteLine("Entering ToolboxUI.Library.Api.APIHelper.ApiClient");

                return _apiClient;
            }
        }


        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(api);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            Console.WriteLine("Entering ToolboxUI.Library.Api.ApiHelper.Authenticate");

            var data = new FormUrlEncodedContent(new[]
            {

                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)

            });

            using (HttpResponseMessage response = await _apiClient.PostAsync("/token", data))
            {

                Console.WriteLine("ApiHelper.Authenticate: Received HttpResponseMessage");

                if (response.IsSuccessStatusCode)
                {

                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;

                }
                else
                {

                    Console.WriteLine("ApiHelper.Authenticate: Exception Identified");

                    throw new Exception(response.ReasonPhrase);

                }

            }

        }


        public async Task GetLoggedInUserInfo(string token)
        {
            // For every request that we make, we are adding the authorization token to validate the request.
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

            using (HttpResponseMessage response = await _apiClient.GetAsync("/api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();

                    _loggedInUser.CreatedDate = result.CreatedDate;
                    _loggedInUser.EmailAddress = result.EmailAddress;
                    _loggedInUser.FirstName = result.FirstName;
                    _loggedInUser.Id = result.Id;
                    _loggedInUser.LastModified = result.LastModified;
                    _loggedInUser.LastName = result.LastName;
                    _loggedInUser.Token = token;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task GetLoggedInUserPermissions(string token)
        {
            // For every request that we make, we are adding the authorization token to validate the request.
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

            using (HttpResponseMessage response = await _apiClient.GetAsync("/api/UserPermission"))
            {
                if (response.IsSuccessStatusCode)
                {
                    //TODO: Need to modify to read permissions correctly.
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();

                    _loggedInUser.CreatedDate = result.CreatedDate;
                    _loggedInUser.EmailAddress = result.EmailAddress;
                    _loggedInUser.FirstName = result.FirstName;
                    _loggedInUser.Id = result.Id;
                    _loggedInUser.LastModified = result.LastModified;
                    _loggedInUser.LastName = result.LastName;
                    _loggedInUser.Token = token;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
