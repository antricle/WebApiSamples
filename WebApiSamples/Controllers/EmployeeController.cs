using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiSamples.Models;

namespace WebApiSamples.Controllers
{
    public class EmployeeController : ApiController
    {

        public string Get()
        {
            return "value";
        }

        public HttpResponseMessage Get(int id)
        {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var entity = context.Employees.FirstOrDefault(e => e.Id == id);

                    if(entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    } else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id = " + id.ToString() + " not found");
                    }
                }

        }

        // Create / Post Sample
        // Pass in the request to Fiddler like this: 
        // {"Name": "John", "Phone": "911"}
        // Set this in the Fiddler post Content-Type: application/json
        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    context.Employees.Add(employee);
                    context.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.Id.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);         
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {

                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var entity = context.Employees.FirstOrDefault(e => e.Id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "ID not found");
                    }
                    else
                    {
                        context.Employees.Remove(entity);
                        context.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody]Employee emp)
        {
            try
            {

                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var entity = context.Employees.FirstOrDefault(e => e.Id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "ID not found");
                    }
                    else
                    {
                        entity.Name = emp.Name;
                        entity.Phone = emp.Phone;

                        context.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }

}
