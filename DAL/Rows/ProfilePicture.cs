using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    [Serializable]
    public class ProfilePicture : Row
    {
        public string ID { get; set; }
        public byte[] Image { get; set; }
        public int Width { get; set; }
        public string Type { get; set; }

        // The same cause IO stream know when to truncate and when to add
        public override string Add(UnitOfWork uow = null)
        {
            return ProfilePictureRepo.Instance.Add(this);
        }

        public override string Save(UnitOfWork uow = null)
        {
            return ProfilePictureRepo.Instance.Save(this);
        }
    }
}
