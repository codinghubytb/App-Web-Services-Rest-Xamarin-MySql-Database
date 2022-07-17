using APIWebApp.Models;
using APIWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebApp.Controllers
{
    [Route("api/school")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        #region Variable
        private readonly IConfiguration _configuration;
        private FunctionsDatabase functionsDatabase;
        private DataTable table;
        List<List<object>> listParameters;
        #endregion


        public StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
            functionsDatabase = new FunctionsDatabase(configuration);
            table = new DataTable();
            listParameters = new List<List<object>>();
        }


        [HttpGet("student/classroomid={classroomId}/nb_element")]
        public JsonResult PaginationStudent(int classroomId)
        {
            string query = @"SELECT count(student.classroomId) as nb_enreg, ceil(count(student.classroomId)/@id) as nb_pages
                            from student where ClassroomId = @classroomId";

            listParameters.Clear();
            listParameters = new List<List<object>>
            { 
                new List<object> { "@id", 10 },
                new List<object> { "@classroomId", classroomId }
            };

            table = functionsDatabase.Request_MySql(query, listParameters);

            return new JsonResult(table);
        }


        [HttpGet("classroom={classroomId}/student=all/page={page}")]
        public JsonResult Get(int classroomId, int page=1)
        {
            string query = @"select StudentId, StudentName, StudentAge, StudentChatty, StudentCreated, StudentModified, ClassroomId 
                            from student where classroomId=@classroomId
                            order by StudentId 
                            limit 10
                            offset @page";

            listParameters.Clear();
            listParameters = new List<List<object>> 
            { 
                new List<object> { "@classroomId", classroomId },
                new List<object> { "@page", (page-1) * 10},
            };

            table = functionsDatabase.Request_MySql(query, listParameters);

            return new JsonResult(table);
        }


        [HttpGet("classroom={classroomId}/student={studentId}")]
        public JsonResult GetId(int classroomId, int studentId)
        {
            string query = @"select StudentId, ClassroomId, StudentName, StudentAge, StudentCreated, StudentModified, StudentChatty 
                             from Student where ClassroomId=@ClassroomId and StudentId=@StudentId ";

            listParameters.Clear();
            listParameters = new List<List<object>>
            {
                new List<object> { "@ClassroomId", classroomId },
                new List<object> { "@StudentId", studentId },
            };

            table = functionsDatabase.Request_MySql(query, listParameters);

            return new JsonResult(table);

        }


        [HttpPost("classroom={classroomId}")]
        public JsonResult Post(Student student)
        {
            string query = @" insert into student (classroomId, studentName, studentAge, studentCreated, studentModified, studentChatty)
                           values (@ClassroomId, @StudentName, @StudentAge, @StudentCreated, @StudentModified, @StudentChatty) ";

            listParameters.Clear();
            listParameters = new List<List<object>>
            {
                new List<object> { "@ClassroomId", student.ClassroomId },
                new List<object> { "@StudentName", student.StudentName },
                new List<object> { "@StudentAge", student.StudentAge },
                new List<object> { "@StudentCreated", student.StudentCreated },
                new List<object> { "@StudentModified", student.StudentModified },
                new List<object> { "@StudentChatty", student.StudentChatty },
            };

            functionsDatabase.Request_MySql(query, listParameters);

            return new JsonResult("Added Successfully");
        }


        [HttpPut("classroom={classroomId}/student={studentId}")]
        public JsonResult Put(Student student)
        {
            string query = @" update Student set ClassroomId=@ClassroomId, StudentName= @StudentName, StudentAge= @StudentAge,
                              StudentCreated= @StudentCreated, StudentModified= @StudentModified, StudentChatty = @StudentChatty  
                              where StudentId=@StudentId ";

            listParameters.Clear();
            listParameters = new List<List<object>>
            {
                new List<object> { "@StudentId", student.StudentId },
                new List<object> { "@ClassroomId", student.ClassroomId },
                new List<object> { "@StudentName", student.StudentName },
                new List<object> { "@StudentAge", student.StudentAge },
                new List<object> { "@StudentCreated", student.StudentCreated },
                new List<object> { "@StudentModified", student.StudentModified },
                new List<object> { "@StudentChatty", student.StudentChatty },
            };

            functionsDatabase.Request_MySql(query, listParameters);

            return new JsonResult("Put Successfully");
        }


        [HttpDelete("classroom={classroomId}/student={studentId}")]
        public JsonResult Delete(int studentId)
        {
            string query = @" delete from Student where StudentId=@StudentId ";

            listParameters.Clear();
            listParameters = new List<List<object>> { new List<object> { "@StudentId", studentId } };

            functionsDatabase.Request_MySql(query, listParameters);

            return new JsonResult("Deleted Successfully");
        }
    }
}
