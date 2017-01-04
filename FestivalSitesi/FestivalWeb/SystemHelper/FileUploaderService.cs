using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace FestivalWeb.SystemHelper
{
    public class FileUploaderService
    {
        public string AddPicture(HttpPostedFileBase picture, string uploadername, HttpContextBase ctx)
        {

            string newName = Path.GetFileNameWithoutExtension(picture.FileName) + "-" + Guid.NewGuid().ToString().Substring(0, 6) + Path.GetExtension(picture.FileName);

            Image orjRes = Image.FromStream(picture.InputStream);
            Bitmap buyukRes = new Bitmap(orjRes);
            buyukRes.Save(ctx.Server.MapPath("/UploadedFiles/" + uploadername + "/" + newName));
            return "/UploadedFiles/" + uploadername + "/" + newName;
        }
    }
}