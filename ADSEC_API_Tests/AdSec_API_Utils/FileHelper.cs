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
            return GetFileNamewithCurrentTime(".ads"); ;
        }

        public static string GetTemporaryLogFileName()
        {
            return GetFileNamewithCurrentTime(".log"); ;
        }


        private static string GetFileNamewithCurrentTime(string extension)
        {
            DateTime date = DateTime.Now;
            var filename = date.ToString("yyyyMMdd_HH.mm.ss.fff") + "_APITest" + extension;
            var filePath = Path.GetTempPath() + "AdSecAPI\\Test files\\" + filename;

            FileInfo file = new FileInfo(filePath);
            file.Directory.Create();

            return filePath;
        }
    }
}
