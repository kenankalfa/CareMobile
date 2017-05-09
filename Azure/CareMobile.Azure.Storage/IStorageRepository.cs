using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CareMobile.Azure.Storage
{
    public interface IStorageRepository
    {
        string UploadFile(string fileName, Stream stream);
        void DeleteFile(string fileName);
        byte[] GetByteArray(string fileName);
    }
}
