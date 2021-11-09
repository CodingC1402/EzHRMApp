using DAL.Others;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Rows
{
    public class ProfilePicture : Row
    {
        public string ID { get; set; }
        public byte[] Image { get; set; }
        public int Width { get; set; }
        public string Type { get; set; }

        public override bool Save(UnitOfWork uow = null)
        {
            if (uow == null)
            {
                using (var uowNew = new UnitOfWork())
                {
                    ProfilePictureRepo.Instance.Update(new object[] { ID }, this, uowNew);
                    return uowNew.Complete();
                }
            }

            return ProfilePictureRepo.Instance.Update(new object[] { ID }, this, uow);
        }
    }
}
