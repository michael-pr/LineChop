using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace owaitlist.Models
{
    public class FileBase : IFileBase
    {
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        } 
    }
}