using AppApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AppApi.Services
{
    public class API
    {

        #region Variable
        private HttpClient Client { get; set; }
        public Uri URL { get; set; }

        public Pagination studentPage;
        public School GetSchool;

        NetworkAccess current = Connectivity.NetworkAccess;

        private const string BASE_URL = "http://{IP}:{PORT}/api/school";

        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public API()
        {
            Client = new HttpClient();
            GetSchool = new School();
            URL = UrlApi();

            if (current == NetworkAccess.Internet)
            {
                PaginationClassroomAsync();
            }
        }


        /// <summary>
        /// AuthorizationApi
        /// </summary>
        /// <param name="authorization"></param>
        public void ApiAuthorizationAsync(string authorization = "")
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization);
        }


        /// <summary>
        /// Pagination Classroom
        /// </summary>
        public async void PaginationClassroomAsync()
        {
            Pagination classroomPages = await GetPaginationAsync("classroom/nb_element");
            GetSchool.nb_enreg = classroomPages.nb_enreg;
            GetSchool.nb_pages = classroomPages.nb_pages;
        }


        /// <summary>
        /// Pagination Student
        /// </summary>
        public async void PaginationStudentAsync(int classroomId)
        {
            studentPage = await GetPaginationAsync($"student/classroomid={classroomId}/nb_element");

        }


        /// <summary>
        /// Url Api
        /// </summary>
        /// <param name="route">Route Url</param>
        /// <returns>Uri Url</returns>
        public Uri UrlApi(string route = null)
        {
            if(route != "" || route == null)
                return new Uri($"{BASE_URL}/{route}");
            else
                return new Uri($"{BASE_URL}");
        }


        #region FunctionApi


        /// <summary>
        /// GET the pagination
        /// </summary>
        /// <param name="route">route url</param>
        /// <returns>Class Pagination</returns>
        public async Task<Pagination> GetPaginationAsync(string route)
        {
            List<Pagination> page = new List<Pagination>();
            try
            {
                if (current == NetworkAccess.Internet)
                {
                    var content = await Client.GetStringAsync(UrlApi(route));
                    page = JsonConvert.DeserializeObject<List<Pagination>>(content);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("ERROR", "Pas de connection !", "OK");
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("ERROR", e.Message, "OK");
            }

            return page[0];
        }


        /// <summary>
        /// GET data table Classroom
        /// </summary>
        /// <param name="pageId">Page number</param>
        /// <param name="route">Route url</param>
        /// <returns>List Class Classroom</returns>
        public async Task<List<Classroom>> GetClassroomAsync(int pageId, string route = null)
        {
            try
            {
                if (current == NetworkAccess.Internet)
                {
                    var content = await Client.GetStringAsync(UrlApi($"{route}/page={pageId}"));
                    GetSchool.ListClassroom = JsonConvert.DeserializeObject<List<Classroom>>(content);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("ERROR", "Pas de connection !", "OK");
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("ERROR", e.Message, "OK");
            }
            return GetSchool.ListClassroom;

        }


        /// <summary>
        /// GET data  table Student
        /// </summary>
        /// <param name="pageId">Page Number</param>
        /// <param name="route">Route url</param>
        /// <returns>List Class Student</returns>
        public async Task<List<Student>> GetStudentAsync(int pageId, string route = null)
        {
            List<Student> list = new List<Student>();
            try
            {
                if (current == NetworkAccess.Internet)
                {
                    var content = await Client.GetStringAsync(UrlApi($"{route}/page={pageId}"));
                    list = JsonConvert.DeserializeObject<List<Student>>(content);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("ERROR", "Pas de connection !", "OK");
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("ERROR", e.Message, "OK");
            }
            return list;

        }


        /// <summary>
        /// GET data table Classroom with ClassroomId = id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Classroom GetIdAsync(int id)
        {
            foreach(var classroom in GetSchool.ListClassroom)
            {
                if(classroom.ClassroomId == id)
                {
                    return classroom;
                }
            }

            return new Classroom();
        }


        /// <summary>
        /// GET data table Student with Student = studentId & ClassroomId = classroomId
        /// </summary>
        /// <param name="classroomId">Id de classroom</param>
        /// <param name="studentId">Id de Student</param>
        /// <returns>class Student</returns>
        public async Task<Student> GetIdAsync(int classroomId, int studentId)
        {
            List<Student> list = new List<Student>();
            try
            {
                if (current == NetworkAccess.Internet)
                {
                    var content = await Client.GetStringAsync(UrlApi($"classroom={classroomId}/student={studentId}"));
                    list = JsonConvert.DeserializeObject<List<Student>>(content);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("ERROR", "Pas de connection !", "OK");
                }

            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("ERROR", e.Message, "OK");
            }
            return list[0];
        }


        /// <summary>
        /// Create a new row in the Classroom Table
        /// </summary>
        /// <param name="classroom">Class Classroom</param>
        /// <returns></returns>
        public async Task PostAsync(Classroom classroom, bool displayResponse = true)
        {
            try
            {
                if (current == NetworkAccess.Internet)
                {
                    string json = JsonConvert.SerializeObject(classroom);
                    HttpContent content = new StringContent(json);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await Client.PostAsync(UrlApi(null), content);
                    if (response.IsSuccessStatusCode && displayResponse)
                    {
                        await App.Current.MainPage.DisplayAlert("Successfuly", "Data successfully saved.", "OK");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("ERROR", "Pas de connection !", "OK");
                }

            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("ERROR", e.Message, "OK");
            }
        }


        /// <summary>
        /// Create a new row in the Student Table
        /// </summary>
        /// <param name="student">Class Student</param>
        /// <returns></returns>
        public async Task PostAsync(Student student, bool displayResponse = true)
        {
            try
            {
                if (current == NetworkAccess.Internet)
                {
                    string json = JsonConvert.SerializeObject(student);
                    HttpContent content = new StringContent(json);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await Client.PostAsync(UrlApi($"classroom={student.ClassroomId}"), content);
                    if (response.IsSuccessStatusCode && displayResponse)
                    {
                        await App.Current.MainPage.DisplayAlert("Successfuly", "Data successfully saved.", "OK");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("ERROR", "Pas de connection !", "OK");
                }

            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("ERROR", e.Message, "OK");
            }
        }


        /// <summary>
        /// Updating a row in the Table Classroom
        /// </summary>
        /// <param name="classroom">Class Classroom</param>
        /// <returns></returns>
        public async Task PutAsync(Classroom classroom, bool displayResponse = true)
        {
            try
            {
                if (current == NetworkAccess.Internet)
                {
                    string json = JsonConvert.SerializeObject(classroom);
                    HttpContent content = new StringContent(json);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await Client.PutAsync(UrlApi($"classroom={classroom.ClassroomId}"), content);
                    if (response.IsSuccessStatusCode && displayResponse)
                    {
                        await App.Current.MainPage.DisplayAlert("Successfuly", "Data successfully saved.", "OK");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("ERROR", "Pas de connection !", "OK");
                }

            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("ERROR", e.Message, "OK");
            }
        }


        /// <summary>
        /// Updating a row in the Table Student
        /// </summary>
        /// <param name="student">Class Student</param>
        /// <returns></returns>
        public async Task PutAsync(Student student, bool displayResponse = true)
        {
            try
            {
                if (current == NetworkAccess.Internet)
                {
                    string json = JsonConvert.SerializeObject(student);
                    HttpContent content = new StringContent(json);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await Client.PutAsync(UrlApi($"classroom={student.ClassroomId}/student={student.StudentId}"), content);
                    if (response.IsSuccessStatusCode && displayResponse)
                    {
                        await App.Current.MainPage.DisplayAlert("Successfuly", "Data successfully saved.", "OK");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("ERROR", "Pas de connection !", "OK");
                }

            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("ERROR", e.Message, "OK");
            }
        }


        /// <summary>
        /// Delete a row in the Classroom Table
        /// </summary>
        /// <param name="classroom">class Classroom</param>
        /// <returns></returns>
        public async Task DeleteAsync(Classroom classroom, bool displayResponse = true)
        {
            try
            {
                if (current == NetworkAccess.Internet)
                {
                    var response = await Client.DeleteAsync(UrlApi($"classroom={classroom.ClassroomId}"));
                    if (response.IsSuccessStatusCode && displayResponse)
                    {
                        await App.Current.MainPage.DisplayAlert("Successfuly", "Data successfully saved.", "OK");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("ERROR", "Pas de connection !", "OK");
                }

            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("ERROR", e.Message, "OK");
            }
        }


        /// <summary>
        /// Delete a row in the Student Table
        /// </summary>
        /// <param name="student">class Student</param>
        /// <returns></returns>
        public async Task DeleteAsync(Student student, bool displayResponse = true)
        {
            try
            {
                if (current == NetworkAccess.Internet)
                {
                    var response = await Client.DeleteAsync(UrlApi($"classroom={student.ClassroomId}/student={student.StudentId}"));
                    if (response.IsSuccessStatusCode && displayResponse)
                    {
                        await App.Current.MainPage.DisplayAlert("Successfuly", "Data successfully saved.", "OK");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("ERROR", "Pas de connection !", "OK");
                }

            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("ERROR", e.Message, "OK");
            }
        }


        #endregion
    }
}
