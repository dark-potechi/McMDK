using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using McMDK.Data;
using McMDK.Utils;
using McMDK.ViewModels;

using Ionic.Zip;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace McMDK.MCP
{
    public class Setup
    {
        private Project project;
        private ProgressWindowViewModel viewModel;

        private List<string> downloads;
        private List<string> files;

        public Setup(Project project)
        {
            this.project = project;
            this.viewModel = null;
            this.downloads = new List<string>();
        }

        public void SetProgressWindow(ProgressWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        // ================================================================================================================
        // ====== MODLOADER SETUP =========================================================================================
        // ================================================================================================================
        #region ModLoader

        /// <summary>
        /// ModLoaderを用いたMod作成環境を作成します。
        /// </summary>
        public void SetupModLoader()
        {
            throw new NotImplementedException("ModLoaderにはまだ対応していません。");
        }

        #endregion

        // ================================================================================================================
        // ====== MINECRAFT FORGE SETUP ===================================================================================
        // ================================================================================================================
        #region MinecraftForge

        /// <summary>
        /// Minecraft Forgeを用いたMod作成環境を作成します。
        /// </summary>
        public void SetupForge()
        {
            String work = Define.ProjectDirectory + "\\" + this.project.Name + "\\";

            this.downloads = new List<string>
            { 
                String.Format(Define.CoderPackUrl, this.project.MCPVersion),
                String.Format(Define.MinecraftForgeUrl, this.project.MCVersion, this.project.ForgeVersion)
            };
            this.files = new List<string>
            {
                work + "mcp.zip",
                work + "minecraftforge.zip"
            };
            if(int.Parse(this.project.MCVersion) < 150)
            {
                this.downloads.Add(String.Format(Define.MinecraftJarUrl, this.project.MCVersion.Replace(".", "_")));
                this.downloads.Add(String.Format(Define.MinecraftSrvJarUrl, this.project.MCVersion.Replace(".", "_")));
                this.downloads.Add(Define.MinecraftResUrl + "lwjgl.jar");
                this.downloads.Add(Define.MinecraftResUrl + "lwjgl_util.jar");
                this.downloads.Add(Define.MinecraftResUrl + "jinput.jar");
                this.downloads.Add(Define.MinecraftResUrl + "windows_natives.jar");
                this.downloads.Add(Define.MinecraftResUrl + "macosx_natives.jar");
                this.downloads.Add(Define.MinecraftResUrl + "linux_natives.jar");
                this.downloads.Add(Define.LibrariesUrl + "fernflower.jar");

                this.files.Add(work + "jars\\bin\\minecraft.jar");
                this.files.Add(work + "jars\\minecraft_server.jar");
                this.files.Add(work + "jars\\bin\\lwjgl.jar");
                this.files.Add(work + "jars\\bin\\lwjgl_util.jar");
                this.files.Add(work + "jars\\bin\\jinput.jar");
                this.files.Add(work + "jars\\bin\\natives\\windows_natives.jar");
                this.files.Add(work + "jars\\bin\\natives\\macosx_natives.jar");
                this.files.Add(work + "jars\\bin\\natives\\linux_natives.jar");
                this.files.Add(work + "runtime\\bin\\fernflower.jar");
            }
            this.downloads.Add(String.Format(Define.PatchesUrl, this.project.MCVersion));
            this.files.Add(work + "patch.zip");

            Define.GetLogger().Fine(this.downloads.Count + " files download.");

            this.Forge_Download();
        }

        private void Forge_Download()
        {
            if(this.downloads.Count >= 1)
            {
                if(this.viewModel != null)
                {
                    this.viewModel.SetIsImmideate(false);
                    this.viewModel.SetProgressValue(0);
                    this.viewModel.SetText("Downloading " + this.downloads[0]);
                }

                if (!FileController.Exists(FileController.GetDirectoryName(this.files[0])))
                {
                    FileController.CreateDirectory(FileController.GetDirectoryName(this.files[0]));
                }

                Uri uri = new Uri(this.downloads[0]);
                WebClient client = new WebClient();
                client.DownloadProgressChanged += DownloadProgressChanged;
                client.DownloadFileCompleted += Forge_DownloadFileCompleted;
                client.DownloadFileAsync(uri, this.files[0]);
            }
            else
            {
                this.Forge_Setup();
            }
        }

        private void Forge_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if(this.files[0].Contains(".zip") || this.files[0].Contains("natives"))
            {
                try
                {
                    ZipFile file = ZipFile.Read(this.files[0]);
                    foreach (ZipEntry entry in file)
                    {
                        if (!entry.FileName.Contains("META-INF"))
                        {
                            entry.Extract(FileController.GetDirectoryName(this.files[0]));
                        }
                    }
                } catch (Exception ex)
                {
                    Define.GetLogger().Error("Error occured in extracting zip file.", ex);
                }
            }
            this.downloads.RemoveAt(0);
            this.files.RemoveAt(0);

            this.Forge_Download();
        }

        private void Forge_Setup()
        {
            string work = Define.ProjectDirectory + "\\" + this.project.Name + "\\";
            Patcher.ApplyPatch(work + "\\patches", work);

            if(FileController.Exists(work + "\\forge\\fml\\fml.json"))
            {
                this.Forge_DownloadLibs();
            }

            //Setup
        }

        private void Forge_DownloadLibs()
        {
            JObject json = JObject.Parse(FileController.LoadFile(Define.ProjectDirectory + "\\" + this.project.Name + "\\forge\\fml\\fml.json"));
            JArray libs = (JArray)json["libraries"];

            List<string> list = new List<string>();
            foreach(JObject o in libs)
            {
                string name = (string)o["name"];
                string url = (string)o["url"];
                string[] children = (string[])((JArray)o["children"]).Values().Cast<string>();
                string[] natives = new string[3];
                natives[0] = (string)o["natives"]["linux"];
                natives[1] = (string)o["natives"]["windows"];
                natives[2] = (string)o["natives"]["osx"];
                string[] p = name.Split(':');
                string format = "{0}/{1}/{2}/{3}-{4}";

                string uri = String.Format(format, url, name.Replace(".", "/") + p[1], p[2], p[1], p[2]);
                list.Add(uri + ".jar");
                if(children != null)
                {
                    foreach(string child in children)
                    {
                        list.Add(uri + "-" + children + ".jar");
                    }
                }
                foreach(string native in natives)
                {
                    if(!String.IsNullOrEmpty(native))
                    {
                        list.Add(uri + "-" + native + ".jar");
                    }
                }
            }
        }

        #endregion

        // ================================================================================================================
        // ====== MINECRAFT FORGE GRADLE SETUP ============================================================================
        // ================================================================================================================
        #region ForgeGradle

        /// <summary>
        /// Minecraft Forge Gradleを用いたMod作成環境を作成します。
        /// </summary>
        public void SetupForgeGradle()
        {

        }

        #endregion

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (this.viewModel != null)
            {
                this.viewModel.SetProgressValue(e.ProgressPercentage);
            }
        }
    }
}
