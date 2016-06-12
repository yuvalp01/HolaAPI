using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Tracing;

namespace HolaAPI
{
    public class MyTraceWriter : ITraceWriter
    {
        public void Trace(System.Net.Http.HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            TraceRecord record = new TraceRecord(request, category, level);
            traceAction(record);
            string path = "C:\\logfiles\\logger.txt" ;

            //using (StreamWriter stream = File.AppendText(path))
            //{
            //    stream.Write(DateTime.Now + " - " );
            //    stream.WriteLine(request);
            //}

            //using (StreamWriter stream = new StreamWriter(path, true))
            //{
            //    stream.Write(DateTime.Now);
            //    stream.Write(level);
            //    //stream.WriteLine("line2");
            //    //stream.WriteLine("line3");
            //}

           // File.AppendAllText(path, record.Status + " - " + record.Message + "\r\n");
            File.AppendAllText(path, record.Status + " - " + record.Message + "\r\n");

        }
    }
}