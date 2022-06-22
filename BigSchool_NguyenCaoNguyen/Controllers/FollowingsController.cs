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
    public class FollowingsController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Follow(Following followingDTO)
        {
            //nguoi theo doi chjinh la user dang nhap
            //nguoi duoc theo doi chinh la follow.folleweeID //duoc truyen tu scrips
            var userID = User.Identity.GetUserId();
            followingDTO.FollowerId = userID;

            BigSchoolContext context = new BigSchoolContext();

            Following find = context.Followings.FirstOrDefault(p => p.FollowerId == followingDTO.FollowerId && p.FollowerId == followingDTO.FolloweeId);
            if (find == null)
            
                context.Followings.Add(followingDTO);
            else
                context.Followings.Remove(find);
            context.SaveChanges();
            return Ok();
        }
    }
}
