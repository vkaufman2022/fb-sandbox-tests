using fbsandboxtests.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Configuration;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace fb_sandbox_tests.StepDefinitions
{
    [Binding]
    public sealed class UserStepDefs
    {
        private static readonly IConfiguration _config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false).Build();
        private static string _baseUrl = _config.GetSection("baseUrl").Value;
        private static string _createUserUrl = _baseUrl + _config.GetSection("createSingleUserUrl").Value;
        private List<UserDetails> _expectedUsersDetails = new List<UserDetails>();
        private List<UserDetails> _actualUsersDetails = new List<UserDetails>();
        private static string _baerer = _config.GetSection("bearer").Value;
        private HttpResponseMessage? _actualResponse =new HttpResponseMessage();



        [Given(@"new users details")]
        public void GivenNewUserDetails(Table table)
        {
            foreach(var row in table.Rows)
            {
                var details = row.CreateInstance<UserDetails>();
                _expectedUsersDetails.Add(details);
            }
            Console.WriteLine("Creating list with users details:");
            _expectedUsersDetails?.ForEach(user => Console.WriteLine(user));
        }

        [When(@"(.*) users successfully created")]
        [Then(@"(.*) users successfully created")]

        public void WhenUserSuccessfullyCreated(int numberOfUsers)
        {
            Console.WriteLine($"Sending create requests to {_createUserUrl} \nwith following details:");
            if (_expectedUsersDetails != null)
            {
                foreach (var user in _expectedUsersDetails)
                {
                    _actualResponse = REST.POST(_createUserUrl, _baerer, user);
                    var content = _actualResponse.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(content);
                    Assert.True(_actualResponse.IsSuccessStatusCode, $"Failed to add user {content}");
                    _actualUsersDetails.Add(JsonConvert.DeserializeObject<UserDetails>(content));
                    Console.WriteLine(user);
                }
            }
            else
                Assert.Fail($"Invalid user details provided in test {_expectedUsersDetails}");
            Assert.AreEqual(numberOfUsers, _actualUsersDetails.Count, "Some users failed to create");
            Assert.AreEqual(_expectedUsersDetails?.Count, _actualUsersDetails.Count, "Actual number of users doen't match");
        }

        [Then(@"the following response was got")]
        public void ThenTheFollowingResponseGot(Table table)
        {
            Console.WriteLine($"Expected result is:");
            List<UserDetails> expectedUsersDetails = new List<UserDetails>();
            foreach(var row in table.Rows)
            {
                var usersDetails = row.CreateInstance<UserDetails>();
                expectedUsersDetails.Add(usersDetails);
                Console.WriteLine(usersDetails);
            }
            Console.WriteLine("Actual result is:");
            _actualUsersDetails.ForEach((user) => Console.WriteLine(user));
            Assert.AreEqual(expectedUsersDetails.Count, _actualUsersDetails.Count, "Some requests to create user failed");
            for (var i=0;i< expectedUsersDetails.Count;i++)
            {
                Assert.AreEqual(expectedUsersDetails[i].Name, _actualUsersDetails?[i].Name, "Actual user name doesn't match");
                Assert.AreEqual(expectedUsersDetails[i].Balance, _actualUsersDetails?[i].Balance, "Actual user balance doesn't match");
            }
        }

        [When(@"unauthorized operator with email (.*) creates user")]
        public void WhenUnauthorizedOperatorWithEmailVkGmail_ComCreatesUser(string email)
        {
            Console.WriteLine($"Sending create requests to {_createUserUrl} \nwith following details:");
            Assert.True(_expectedUsersDetails != null, "Expected users list is empty");
            foreach (var user in _expectedUsersDetails)
            {
                _actualResponse = REST.POST(_createUserUrl, email, user);
                Console.WriteLine(_actualResponse);
            }
        }

        [Then(@"the following error got : status code=(.*), token=(.*), msg=(.*)")]
        public void ThenTheFollowingErrorGot(int expectedStatusCode, string expectedToken, string expectedMessage)
        {
            var actualErrorMsg = JsonConvert.DeserializeObject<RequestedDataResponse>(_actualResponse?.Content.ReadAsStringAsync().Result);
            Console.WriteLine($"Expected error is:{expectedStatusCode}|{expectedMessage}|{expectedToken}");
            Console.WriteLine($"Actual error is:{actualErrorMsg}");
            Assert.AreEqual(expectedStatusCode, actualErrorMsg.StatusCode, "Actual error status code doesn't match");
            Assert.AreEqual(expectedMessage, actualErrorMsg.Msg, "Actual error message doesn't match");
            Assert.AreEqual(expectedToken, actualErrorMsg.TokenReceived, "Actual token doesn't match");
        }

        [When(@"user failed to create")]
        public void FailedCreateUser()
        {
            Console.WriteLine($"Sending create requests to {_createUserUrl} \nwith following details:");
            Assert.True(_expectedUsersDetails != null, "Expected users list is empty");
            foreach (var user in _expectedUsersDetails)
            {
                Console.WriteLine(user);
                _actualResponse = REST.POST(_createUserUrl, _baerer, user);
                var content = _actualResponse.Content.ReadAsStringAsync().Result;
                Console.WriteLine(_actualResponse);
                Assert.True(!_actualResponse.IsSuccessStatusCode, $"Success to add user {content}"); 
            }
        }

        [Then(@"the following error got - status code=(.*), msg=(.*)")]
        public void FollowingErrorGot(int expectedStatusCode, string expectedMessage)
        {
            var actualErrorMsg = JsonConvert.DeserializeObject<ErrorMessage>(_actualResponse.Content.ReadAsStringAsync().Result);
            Console.WriteLine($"Expected error is:{expectedStatusCode}|{expectedMessage}");
            Console.WriteLine($"Actual error is:{actualErrorMsg}");
            Assert.AreEqual(expectedStatusCode, actualErrorMsg.StatusCode, "Actual error status code doesn't match");
            Assert.AreEqual(expectedMessage, actualErrorMsg.Message, "Actual error message doesn't match");
        }

        [When(@"provided (.*) valid users details")]
        public void WhenProvidedValidUsersDetails(int numberOfUsers)
        {
            for(var i=0; i < numberOfUsers; i++)
            {
                var details = new UserDetails()
                {
                    Name = "user" + new Random().Next(),
                    Balance= new Random().Next().ToString()
                };
                _expectedUsersDetails.Add(details);
            }
            Console.WriteLine("Creating list with users details:");
            _expectedUsersDetails?.ForEach(user => Console.WriteLine(user));
        }

        [When(@"update the user as follows")]
        public void WhenUpdateTheUserAsFollows(Table table)
        {
            var updatedUserDetails = new List<UserDetails>();
            foreach(var row in table.Rows)
            {
                var userDetails = row.CreateInstance<UserDetails>();
                updatedUserDetails.Add(userDetails);
            }
            Assert.AreEqual(updatedUserDetails.Count, _actualUsersDetails.Count, "Actual update list doesn't match to exiting users list");
            var actualUsersDetails = new List<UserDetails>();
            for (var i=0; i < updatedUserDetails.Count; i++)
            {
                Console.WriteLine($"Actual user details: {_actualUsersDetails[i]}");
                Console.WriteLine($"Updated user details: {updatedUserDetails[i]}");
                var url = _createUserUrl + $"/{_actualUsersDetails[i].Id}";
                Console.WriteLine($"Sending update request to: {url}");          
                _actualResponse = REST.PATCH(url, _baerer, updatedUserDetails[i]); 
                var content = _actualResponse.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);
                Assert.True(_actualResponse.IsSuccessStatusCode, $"Failed to update user {content}");
                actualUsersDetails.Add(JsonConvert.DeserializeObject<UserDetails>(content));  
            }
            _actualUsersDetails = actualUsersDetails;

        }

        [When(@"unauthorized operator with email (.*) updates user")]
        public void WhenUnauthorizedOperatorWithEmailVkGmail_ComUpdatesUser(string email,Table table)
        {
            var updatedUserDetails = new List<UserDetails>();
            foreach (var row in table.Rows)
            {
                var userDetails = row.CreateInstance<UserDetails>();
                updatedUserDetails.Add(userDetails);
            }
            Assert.AreEqual(updatedUserDetails.Count, _actualUsersDetails.Count, "Actual update list doesn't match to exiting users list");
            for (var i = 0; i < updatedUserDetails.Count; i++)
            {
                Console.WriteLine($"Actual user details: {_actualUsersDetails[i]}");
                Console.WriteLine($"Updated user details: {updatedUserDetails[i]}");
                var url = _createUserUrl + $"/{_actualUsersDetails[i].Id}";
                Console.WriteLine($"Sending update request to: {url}");
                _actualResponse = REST.PATCH(url, email, updatedUserDetails[i]);
            }
        }




    }
}