using DAL.Rows;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class ProfilePictureRepo : Repo<ProfilePicture>
    {
        public static ProfilePictureRepo Instance { get; private set; } = new ProfilePictureRepo();
        ProfilePictureRepo()
        {
            TableName = "PROFILEPICTURE";
            PKColsName = new string[] { "ID" };
        }
    }
}
