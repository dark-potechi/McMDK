using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK.Utils
{
    public class Patcher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir">Patches Directory</param>
        /// <param name="work">Target Directory</param>
        public static void ApplyPatch(string dir, string work)
        {
            if(!FileController.Exists(dir))
            {
                return;
            }
            List<string> files = Directory.GetFiles(dir, "*.patch", SearchOption.AllDirectories).ToList<string>();
            foreach(string file in files)
            {
                StreamReader sr = new StreamReader(file);
                string line, diff = "";
                while((line = sr.ReadLine()) != null)
                {
                    if(line.StartsWith("+++"))
                    {
                        diff += line.Replace("+++ work\\", "+++ ") + Environment.NewLine;
                    }
                    else
                    {
                        diff += line + Environment.NewLine;
                    }
                }
                sr.Close();
                sr.Dispose();

                StreamWriter sw = new StreamWriter(work + "\\temp.patch");
                sw.WriteLine(diff);

                string args = "-p1 -u -i {0} -d {1}";

                Process proc = new Process();
                proc.StartInfo.FileName = work + "runtime\\bin\\applydiff.exe";
                proc.StartInfo.Arguments = String.Format(args, work + "\\temp.patch", work);
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.OutputDataReceived += DataReceived;
                proc.ErrorDataReceived += DataReceived;
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();

                proc.WaitForExit();
            }
        }

        private static void DataReceived(object sender, DataReceivedEventArgs e)
        {
            Define.GetLogger().Debug(e.Data);
        }
    }
}
