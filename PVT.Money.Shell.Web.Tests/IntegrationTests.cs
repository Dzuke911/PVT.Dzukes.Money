using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace PVT.Money.Shell.Web.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public void ResponseFromAbout()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:50462/Authorization/About");
                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
                string html = responseMessage.Content.ReadAsStringAsync().Result;
            }
        }

        [Test]
        public void ResponseFromSignIn()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:50462/Authorization/SignIn");
                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
                string html = responseMessage.Content.ReadAsStringAsync().Result;
            }
        }

        [Test]
        public void ResponseFromRegistration()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:50462/Authorization/Registration");
                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
                string html = responseMessage.Content.ReadAsStringAsync().Result;
            }
        }

        [Test]
        public void ResponseFromSignOut()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:50462/Main/SignOut");
                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
                string html = responseMessage.Content.ReadAsStringAsync().Result;
            }
        }

        [Test]
        public void ResponseFromSignInWithDataOK()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Authorization/SignInFormAction");
                Dictionary<string,string> data= new Dictionary<string, string>();
                data.Add("Login","Dzuke911");
                data.Add("Password","1");
                requestMessage.Content = new FormUrlEncodedContent(data);

                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
            }
        }

        [Test]
        public void ResponseFromSignInWithDataFail()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Authorization/SignInFormAction");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Login", "Dzuke911");
                data.Add("Password", "2");
                requestMessage.Content = new FormUrlEncodedContent(data);

                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.Unauthorized, responseMessage.StatusCode);
            }
        }

        [Test]
        public void ResponseFromMainIndexK()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Authorization/SignInFormAction");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Login", "Dzuke911");
                data.Add("Password", "1");
                requestMessage.Content = new FormUrlEncodedContent(data);

                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);

                requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:50462/Main/Index");
                responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
                string html = responseMessage.Content.ReadAsStringAsync().Result;
            }
        }

        [Test]
        public void ResponseFromMainHistory()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Authorization/SignInFormAction");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Login", "Dzuke911");
                data.Add("Password", "1");
                requestMessage.Content = new FormUrlEncodedContent(data);

                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);

                requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:50462/Main/History");
                responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
                string html = responseMessage.Content.ReadAsStringAsync().Result;
            }
        }

        [Test]
        public void ResponseFromPersonalData()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Authorization/SignInFormAction");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Login", "Dzuke911");
                data.Add("Password", "1");
                requestMessage.Content = new FormUrlEncodedContent(data);

                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);

                requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:50462/Main/PersonalData");
                responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
                string html = responseMessage.Content.ReadAsStringAsync().Result;
            }
        }

        [Test]
        public void ResponseFromUsersManagement()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Authorization/SignInFormAction");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Login", "Dzuke911");
                data.Add("Password", "1");
                requestMessage.Content = new FormUrlEncodedContent(data);

                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);

                requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:50462/Main/UsersManagement");
                responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
                string html = responseMessage.Content.ReadAsStringAsync().Result;
            }
        }

        [Test]
        public void ResponseFromRolesManagement()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Authorization/SignInFormAction");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Login", "Dzuke911");
                data.Add("Password", "1");
                requestMessage.Content = new FormUrlEncodedContent(data);

                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);

                requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:50462/Main/RolesManagement");
                responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
                string html = responseMessage.Content.ReadAsStringAsync().Result;
            }
        }

        [Test]
        public void ResponseFromRoleNameExistsOK()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Authorization/SignInFormAction");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Login", "Dzuke911");
                data.Add("Password", "1");
                requestMessage.Content = new FormUrlEncodedContent(data);

                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);

                requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Main/RoleNameExists");
                data = new Dictionary<string, string>();
                data.Add("NewRole", "InexistentRole");
                requestMessage.Content = new FormUrlEncodedContent(data);

                responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
                bool result = JsonConvert.DeserializeObject<bool>(responseMessage.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(true, result);
            }
        }

        [Test]
        public void ResponseFromRoleNameExistsFail()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Authorization/SignInFormAction");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Login", "Dzuke911");
                data.Add("Password", "1");
                requestMessage.Content = new FormUrlEncodedContent(data);

                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);

                requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Main/RoleNameExists");
                data = new Dictionary<string, string>();
                data.Add("NewRole", "Admin");
                requestMessage.Content = new FormUrlEncodedContent(data);

                responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
                bool result = JsonConvert.DeserializeObject<bool>(responseMessage.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(false, result);
            }
        }

        [Test]
        public void IntegrationJsonTest()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Authorization/SignInFormAction");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Login", "Dzuke911");
                data.Add("Password", "1");
                requestMessage.Content = new FormUrlEncodedContent(data);

                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);

                requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Main/IntegrationJsonTest");
                data = new Dictionary<string, string>();
                data.Add("Login", "Tratata");
                data.Add("Password", "Trololo");

                requestMessage.Content = new FormUrlEncodedContent(data);

                responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);

                JsonTestData jsonResult = JsonConvert.DeserializeObject<JsonTestData>(responseMessage.Content.ReadAsStringAsync().Result);
                Assert.AreEqual("Tratata",jsonResult.Login );
                Assert.AreEqual("Trololo",jsonResult.Password);
            }
        }

        [Test]
        public void ResponseFromLoginExistsFail()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Authorization/LoginExists");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Login", "Dzuke911");
                requestMessage.Content = new FormUrlEncodedContent(data);

                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
                bool result = JsonConvert.DeserializeObject<bool>(responseMessage.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(false, result);
            }
        }

        [Test]
        public void ResponseFromLoginExistsOK()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Authorization/LoginExists");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Login", "InexistentUser");
                requestMessage.Content = new FormUrlEncodedContent(data);

                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
                bool result = JsonConvert.DeserializeObject<bool>(responseMessage.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(true, result);
            }
        }

        [Test]
        public void ResponseFromEmailExistsFail()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Authorization/EmailExists");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Email", "Dzuke911@gmail.com");
                requestMessage.Content = new FormUrlEncodedContent(data);

                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
                bool result = JsonConvert.DeserializeObject<bool>(responseMessage.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(false, result);
            }
        }

        [Test]
        public void ResponseFromEmailExistsOK()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:50462/Authorization/EmailExists");
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Email", "InexistentEmail@gmail.com");
                requestMessage.Content = new FormUrlEncodedContent(data);

                HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
                Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
                bool result = JsonConvert.DeserializeObject<bool>(responseMessage.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(true, result);
            }
        }
    }
}
