using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;

namespace HolaAPI
{
    public class Logger
    {
        public static void Write(string message)
        {
            string path = ConfigurationManager.AppSettings["logpath"];

            using (StreamWriter stream = File.AppendText(path))
            {
                stream.Write(DateTime.Now + " - ");
                stream.WriteLine(message);
            }
        }

    }

}