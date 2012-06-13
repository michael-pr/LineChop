using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using owaitlist.Models;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Web.Caching;

namespace owaitlist.Controllers
{
    public class ImageController : Controller
   { 
        OwlEntities db = new OwlEntities();

        IFileBase file; 
        const int cacheForMins = 60; 
 
        public ImageController() : this(null) { } 
        public ImageController(IFileBase fileBase) 
        { 
            this.file = fileBase ?? new FileBase(); 
        } 
 
        byte[] GetBytesFromCache(string path) 
        { 
            object fromCache = HttpRuntime.Cache.Get(path); 
            if (fromCache == null) return null; 
 
            try 
            { 
                return fromCache as byte[]; 
            } 
            catch 
            { 
                return null; 
            } 
        }

        public ActionResult GetImage(int id, int width = 1200, int height = 300)
        {
            var restaurant = db.Restaurants.Find(id);
            string mappedPath;
            if (restaurant != null)
                mappedPath = Server.MapPath("~/" + restaurant.BannerImageUri);
            else
                mappedPath = Server.MapPath("~/Content/img/noimage.jpg");
            return File(getImageStream(mappedPath, ImageFormat.Jpeg, width, height).ToArray(), "content-type/jpeg");
        }

        public ActionResult GetMainBanner()
        {
            string path = Server.MapPath("~/Content/img/main-bg.png");
            return File(getImageStream(path, ImageFormat.Png, 1024, 300).ToArray(), "content-type/jpeg");
        }

        private MemoryStream getImageStream(string path, ImageFormat format, int width = 1024, int height = 300)
        {
            byte[] imageBytes = GetBytesFromCache(path);
            if (imageBytes == null)
            {
                imageBytes = file.ReadAllBytes(path);

                //cache image src 
                HttpRuntime.Cache.Insert(
                    path,
                    imageBytes,
                    null, //or: new CacheDependency(mappedPath), 
                    Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(cacheForMins));
                Response.Cache.SetCacheability(HttpCacheability.Public);
                Response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
                Response.AddFileDependency(path);
                Response.Cache.SetLastModifiedFromFileDependencies();
            }
            MemoryStream original = new MemoryStream();
            original.Write(imageBytes, 0, imageBytes.Length);
            using (var srcImage = Image.FromStream(original))
            using (var newImage = new Bitmap(width, height))
            using (var graphics = Graphics.FromImage(newImage))
            using (var stream = new MemoryStream())
            {
                if (width != 1024 || height != 300)
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(srcImage, new Rectangle(0, 0, width, height));
                    newImage.Save(stream, format);
                }
                else
                    srcImage.Save(stream, format);
                return stream;
            }
        }
    }
}
