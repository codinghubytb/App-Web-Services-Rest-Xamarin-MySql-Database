using APIWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySql.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using APIWebApp.Services;

namespace APIWebApp.Controllers
{
    [Route("api/school")]
    [ApiController]

    public class ClassroomController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private FunctionsDatabase functionsDatabase;
        private DataTable table;
        List<List<object>> listParameters;


        public ClassroomController(IConfiguration configuration)
        {
            _configuration = configuration;
            functionsDatabase = new FunctionsDatabase(configuration);
            table = new DataTable();
            listParameters = new List<List<object>>();
        }


        [HttpGet("classroom/nb_element")]
        public JsonResult PaginationClassroom()
        {
            string query = @"SELECT count(classroom.classroomId) as nb_enreg, ceil(count(classroom.classroomId)/@id) as nb_pages
                            from classroom ";

            listParameters.Clear();

            listParameters = new List<List<object>> { new List<object> { "@id", 10 } };

            table = functionsDatabase.Request_MySql(query, listParameters);

            return new JsonResult(table);
        }


        [HttpGet("classroom/page={page}")]
        public JsonResult Get(int page = 1)
        {
            string query = @"select classroomId, classroomName, classroomCreated, classroomModified, classroomNbPerson 
                            from classroom order by classroomId limit 10 offset @page";


            listParameters.Clear();

            listParameters = new List<List<object>> { new List<object> { "@page", (page-1) * 10 } };

            table = functionsDatabase.Request_MySql(query, listParameters);

            return new JsonResult(table);
        }


        [HttpGet("classroom={id}")]
        public JsonResult GetId(int id)
        {
            string query = @"select ClassroomId, ClassroomName, ClassroomCreated, ClassroomModified, ClassroomNbPerson 
                            from Classroom where ClassroomId=@ClassroomId";

            listParameters.Clear();

            listParameters = new List<List<object>> { new List<object> { "@ClassroomId", id} };

            table = functionsDatabase.Request_MySql(query, listParameters);

            return new JsonResult(table);
        }


        [HttpPost]
        public JsonResult Post(Classroom classroom)
        {
            string query = @"insert into Classroom (classroomName, classroomCreated, classroomModified, classroomNbPerson) values 
                            (@classroomName, @classroomCreated, @classroomModified, @classroomNbPerson)";

            listParameters.Clear();

            listParameters = new List<List<object>> 
            {
                new List<object> { "@classroomName", classroom.ClassroomName },
                new List<object> { "@classroomCreated", classroom.ClassroomCreated },
                new List<object> { "@classroomModified", classroom.ClassroomModified },
                new List<object> { "@classroomNbPerson", classroom.ClassroomNbPerson }
            };

            functionsDatabase.Request_MySql(query, listParameters);

            return new JsonResult("Post Succefully");
        }


        [HttpPut("classroom={classroom}")]
        public JsonResult Put(Classroom classroom)
        {
            string query = @"
                           update Classroom
                           set ClassroomName= @ClassroomName, ClassroomCreated= @ClassroomCreated, 
                           ClassroomModified= @ClassroomModified, ClassroomNbPerson = (select count(classroomId) from student where classroomId  = @ClassroomId)
                           where ClassroomId=@ClassroomId";

            listParameters.Clear();

            listParameters = new List<List<object>>
            {
                new List<object> { "@ClassroomId", classroom.ClassroomId },
                new List<object> { "@ClassroomName", classroom.ClassroomName },
                new List<object> { "@ClassroomCreated", classroom.ClassroomCreated },
                new List<object> { "@ClassroomModified", classroom.ClassroomModified },
                new List<object> { "@ClassroomNbPerson", classroom.ClassroomNbPerson }
            };

            functionsDatabase.Request_MySql(query, listParameters);

            return new JsonResult("Put Succefully");
        }


        [HttpDelete("classroom={id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from Classroom where ClassroomId=@ClassroomId";

            listParameters.Clear();

            listParameters = new List<List<object>>{ new List<object> { "@ClassroomId", id } };

            functionsDatabase.Request_MySql(query, listParameters);

            return new JsonResult("Delete Succefully");
        }
    }

}
