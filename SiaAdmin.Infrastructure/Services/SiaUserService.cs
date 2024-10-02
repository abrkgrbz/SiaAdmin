using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Interfaces.SiaUser;
using SiaAdmin.Infrastructure.Extensions;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace SiaAdmin.Infrastructure.Services
{
    public class SiaUserService : ISiaUserService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SiaUserService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadProfilePicture(IFormFile file, Guid InternalGUID)
        {
            long result = 0;
            var bytes = await file.GetByte();
            using (MagickImage image = new MagickImage(bytes))
            {
                int width = 512;
                int height = 512;

                if (image.Height != height || image.Width != width)
                {
                    decimal result_ratio = (decimal)height / (decimal)width;
                    decimal current_ratio = (decimal)image.Height / (decimal)image.Width;

                    Boolean preserve_width = false;
                    if (current_ratio > result_ratio)
                    {
                        preserve_width = true;
                    }
                    int new_width = 0;
                    int new_height = 0;
                    if (preserve_width)
                    {
                        new_width = width;
                        new_height = (int)Math.Round((decimal)(current_ratio * new_width));
                    }
                    else
                    {
                        new_height = height;
                        new_width = (int)Math.Round((decimal)(new_height / current_ratio));
                    }


                    String geomStr = width.ToString() + "x" + height.ToString();
                    String newGeomStr = new_width.ToString() + "x" + new_height.ToString();

                    MagickGeometry intermediate_geo = new MagickGeometry(newGeomStr);
                    MagickGeometry final_geo = new MagickGeometry(geomStr);

                    image.Resize(intermediate_geo);
                    image.Crop(final_geo);
                }

                image.Format = MagickFormat.Jpg;
                try
                {

                    string uploadPath = Path.Combine("C:\\Web\\siapanel.com", "a");
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);
                    uploadPath += "\\"+InternalGUID.ToString() + ".jpg";
                    image.Write(uploadPath); 
                    FileInfo fi = new FileInfo(uploadPath);
                    result = fi.Length;

                }
                catch (Exception)
                {
                    result = -1;
                }

            }
            return result.ToString();


        }

        public string GetUserProfilePicture(Guid InternalGUID)
        {
            string uploadPath = Path.Combine("C:\\Web\\siapanel.com", "a");
            uploadPath += "\\" + InternalGUID.ToString() + ".jpg";
            FileInfo fi = new FileInfo(uploadPath);
            if (fi.Exists)
            { 
                uploadPath = "/a/" + InternalGUID.ToString() + ".jpg";
                return uploadPath;
            }
            else
            { 
                string defaultPath = "\\profile-picture\\default\\avatar-404" + ".png"; 
                return defaultPath;
            }
        }


    }
}
