using BigSchool_NguyenCaoNguyen.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;

namespace BigSchool_NguyenCaoNguyen.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BigSchoolContext context = new BigSchoolContext();
            var upcommingCourse = context.Courses.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();
            //lấy user login hiện tại 
            var userID = User.Identity.GetUserId();
            ViewBag.loginUser = userID;
            foreach (Course i in upcommingCourse)
            {
                //tìm Name của user từ lectureid
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LecturerId);
                i.Name = user.Name;
                //lấy ds tham gia khóa học 
                Attendance find = context.Attendances.FirstOrDefault(p => p.CourseId == i.Id && p.Attendee == userID);
                if (find == null)
                    i.isShowGoing = true;
                //kiem tra user da theo doi giang vien chua?
                Following findFollow = context.Followings.FirstOrDefault(p => p.FollowerId == userID && p.FolloweeId == i.LecturerId);
                if (findFollow == null)
                    i.isShowFollow = true;
            }
            return View(upcommingCourse);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}