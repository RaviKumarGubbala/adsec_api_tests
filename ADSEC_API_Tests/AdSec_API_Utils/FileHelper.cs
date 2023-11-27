using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSec_API_Utils
{
    public static class FileHelper
    {
        public static string GetAdsFileNamewithCurrentTime()
        {
            DateTime date = DateTime.Now;
            var filename = date.ToString("yyyyMMdd_HH.mm.ss.fff") + "_APITest" + ".ads";
            var filePath = Path.GetTempPath() + "AdSecAPI\\Test files\\" + filename;

            FileInfo file = new FileInfo(filePath);
            file.Directory.Create();

            return filePath;
        }
    }
}
