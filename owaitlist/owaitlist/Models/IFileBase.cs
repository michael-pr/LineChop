using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace owaitlist.Models
{
    public interface IFileBase
    {
        byte[] ReadAllBytes(string path);
    }
}
