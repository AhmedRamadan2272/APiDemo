using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiDemo.Controllers
{
    public class EmployeeController : ApiController
    {
        EmployeeDBEntities emp = new EmployeeDBEntities();
        public   IEnumerable<Employee> Get ()
        {
            return emp.Employees.ToList() ; 
        }
        public  HttpResponseMessage  Get(int id )
        {
            var entity= emp.Employees.FirstOrDefault(e=>e.ID==id);
            if (entity!=null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, entity);
 
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Not Found");
            }
        }
        public HttpResponseMessage POST([FromBody]Employee EmpRequest)
        {
            try
            {
                emp.Employees.Add(EmpRequest);
                emp.SaveChanges();
                var message = Request.CreateResponse(HttpStatusCode.Created, EmpRequest);
                message.Headers.Location = new Uri(Request.RequestUri + "/" + EmpRequest.ID);
                return message;
            }
            catch (Exception ex) 
            {

             return   Request.CreateErrorResponse(HttpStatusCode.BadRequest ,ex);
            }
            
        }

        public HttpResponseMessage Delete (int id)
        {
            var entity = emp.Employees.FirstOrDefault(e => e.ID == id);
            
            if (entity==null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No Data to be deleted");

            }
            else
            {
                emp.Employees.Remove(entity);
                emp.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "The data had been deleted");

            }

        }
        public HttpResponseMessage Put (int id , [FromBody] Employee empRequest)
        {
            try
            {
                var entity = emp.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    entity.FirstName = empRequest.FirstName;
                    entity.LastName = empRequest.LastName;
                    entity.Salary = empRequest.Salary;
                    entity.Gender = empRequest.Gender;

                    emp.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "DataSaved" + id);


                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Not Found" + id);
                }
            }
            catch (Exception ex )
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }


        }

    }

}
