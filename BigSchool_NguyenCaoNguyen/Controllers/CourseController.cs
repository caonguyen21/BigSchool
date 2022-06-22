using BigSchool_NguyenCaoNguyen.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool_NguyenCaoNguyen.Controllers
{
    public class CourseController : Controller
    {
        // GET: Courses
        public ActionResult Create()
        {
            BigSchoolContext context = new BigSchoolContext();
            Course course = new Course();
            course.ListCategory = context.Categories.ToList();
            return View(course);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course e)
        {
            BigSchoolContext context = new BigSchoolContext();
            ModelState.Remove("LectureId");
            if (!ModelState.IsValid)
            {
                e.ListCategory = context.Categories.ToList();
                return View("Create", e);
            }
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            e.LecturerId = user.Id;
            context.Courses.Add(e);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();
            var userID = User.Identity.GetUserId();
            //lay danh sach khoa hoc ma userlogin da tham du
            var listattend = context.Attendances.Where(p => p.Attendee == userID).ToList();
            var courses2 = new List<Course>();
            //tim chi tiet khoa hoc tu listattend 
            foreach(Attendance temp in listattend)
            {
                Course objcourse = temp.Course;
                objcourse.Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objcourse.LecturerId).Name;
                courses2.Add(objcourse);
            }
            return View(courses2);
        }

        public ActionResult Mine()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var course = context.Courses.Where(c => c.LecturerId == user.Id && c.IsCanceled !=true).ToList();
            foreach (Course i in course)
            {
                i.Name = user.Name;
            }
            return View(course);
        }

        [Authorize]
        public ActionResult Edit (int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            var userID = User.Identity.GetUserId();
            var course = context.Courses.FirstOrDefault(c => c.LecturerId == userID && c.Id == id);
            if (course == null)
                return HttpNotFound("Khong tim thay khoa hoc");
            course.ListCategory = context.Categories.ToList();//lay danh sach khoa hc
            return View("Create", course);    
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            var userID = User.Identity.GetUserId();
            var findcourse = context.Courses.FirstOrDefault(p => p.Id == id);
            findcourse.IsCanceled = true;
            context.SaveChanges();
            return RedirectToAction("Mine");
        }

        public ActionResult Following()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser loginUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listFollowings = context.Followings.Where(p => p.FollowerId == loginUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Following temp in listFollowings)
            {
                //lay ra tat ca cac khoa hoc duoc theo doi
                var listCourse = context.Courses.Where(p => p.LecturerId == temp.FolloweeId).ToList();
                if(listCourse.Count > 0)
                {
                    //tim name cua gv
                    string Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(listCourse[0].LecturerId).Name;
                    foreach (Course i in listCourse)
                        i.Name = Name;
                    //add vao course
                    courses.AddRange(listCourse);
                }
            }
            return View(courses);
        }
    }
}