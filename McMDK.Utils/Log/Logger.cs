using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK.Utils;

namespace McMDK.Utils.Log
{
    public class Logger
    {
        private string name = "";
        private string file = "";

        public Logger(string log)
        {
            this.name = log;
            if (!FileController.Exists(Define.LogDirectory))
            {
                FileController.CreateDirectory(Define.LogDirectory);
            }
            this.file = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
        }

        public static Logger GetLogger(string log)
        {
            return new Logger(log);
        }

        public void Fine(object msg)
        {
            this.Fine(msg, null);
        }

        public void Fine(object msg, Exception e)
        {
            this.Write(Level.FINE, msg, e);
        }

        public void Info(object msg)
        {
            this.Info(msg, null);
        }

        public void Info(object msg, Exception e)
        {
            this.Write(Level.INFO, msg, e);
        }

        public void Warning(object msg)
        {
            this.Warning(msg, null);
        }

        public void Warning(object msg, Exception e)
        {
            this.Write(Level.WARNING, msg, e);
        }

        public void Error(object msg)
        {
            this.Error(msg, null);
        }

        public void Error(object msg, Exception e)
        {
            this.Write(Level.ERROR, msg, e);
        }

        public void Severe(object msg)
        {
            this.Severe(msg, null);
        }

        public void Severe(object msg, Exception e)
        {
            this.Write(Level.SEVERE, msg, e);
        }

        
        public void Debug(object msg)
        {
            this.Debug(msg, null);
        }

        public void Debug(object msg, Exception e)
        {
            this.Write(Level.DEBUG, msg, e);
        }

        public void Write(Level level, object msg, Exception e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss "));
            sb.Append("[" + this.name + "]");
            switch(level)
            {
                case Level.FINE:
                    sb.Append("[FINE ]");
                    break;

                case Level.INFO:
                    sb.Append("[INFO ]");
                    break;

                case Level.WARNING:
                    sb.Append("[WARN ]");
                    break;

                case Level.ERROR:
                    sb.Append("[ERROR]");
                    break;

                case Level.SEVERE:
                    sb.Append("[FATAL]");
                    break;

                case Level.DEBUG:
                    sb.Append("[DEBUG]");
                    break;

                default:
                    break;
            }
            sb.Append(msg.ToString().Replace("\n", "").Replace("\r", ""));
            if(e != null)
            {
                sb.Append("\r\n" + e.ToString());
            }
            Console.WriteLine(sb.ToString());

            StreamWriter sw = new StreamWriter(Define.LogDirectory + "\\" + this.file + ".log", true);
            sw.AutoFlush = true;
            sw.WriteLine(sb.ToString());
            sw.Close();
            sw.Dispose();
        }

        public enum Level
        {
            /// <summary>
            /// FINE Level
            /// </summary>
            FINE,

            /// <summary>
            /// INFO Level
            /// </summary>
            INFO,

            /// <summary>
            /// WARN Level
            /// </summary>
            WARNING,

            /// <summary>
            /// ERROR Level
            /// </summary>
            ERROR,

            /// <summary>
            /// FATAL Level
            /// </summary>
            SEVERE,

            /// <summary>
            /// DEBUG Level
            /// </summary>
            DEBUG
        }
    }
}
