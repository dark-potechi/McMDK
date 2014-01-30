using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK.Data;
using McMDK.Utils;
using McMDK.ViewModels;

namespace McMDK.MCP
{
    public class Recompile
    {
        private Project project;
        private ProgressWindowViewModel viewModel;

        public Recompile(Project project)
        {
            this.project = project;
            this.viewModel = null;
        }

        public void SetProgressWindow(ProgressWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        // ================================================================================================================
        // ====== MINECRAFT FORGE RECOMPILE ===============================================================================
        // ================================================================================================================
        #region MinecraftForge

        public void RecompileForge()
        {
            string work = Define.ProjectDirectory + "\\" + this.project.Name + "\\";

            Process proc = new Process();
            proc.StartInfo.FileName = work + "\\runtime\\bin\\python\\python_mcp.exe";
            proc.StartInfo.Arguments = work + "\\runtime\\recompile.py";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.EnableRaisingEvents = true;
            proc.OutputDataReceived += DataReceived;
            proc.ErrorDataReceived += DataReceived;
            proc.Exited += RecompileEnd;
            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
        }

        #endregion

        // ================================================================================================================
        // ====== FORGE GRADLE RECOMPILE ==================================================================================
        // ================================================================================================================
        #region ForgeGradle
        #endregion


        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
            Define.GetLogger().Info(e.Data);
        }

        private void RecompileEnd(object sender, EventArgs e)
        {

        }
    }
}
