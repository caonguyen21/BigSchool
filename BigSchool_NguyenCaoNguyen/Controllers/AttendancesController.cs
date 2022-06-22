using BigSchool_NguyenCaoNguyen.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BigSchool_NguyenCaoNguyen.Controllers
{
    public class AttendancesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attend (Course courseId)
        {
            var userID = User.Identity.GetUserId();
            if (userID == null)
                return BadRequest("Pleses login first!");
            BigSchoolContext context = new BigSchoolContext();
            var attendance = new Attendance()
            {
                CourseId = courseId.Id,
                Attendee = userID
            };
            context.Attendances.Add(attendance);
            context.SaveChanges();
            return Ok();
        }
    }
}
