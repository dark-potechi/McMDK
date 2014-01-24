using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace McMDK.Utils
{
    /// <summary>
    /// System.IOのアレ
    /// </summary>
    public class FileController
    {
        /// <summary>
        /// ディレクトリを作成します。
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(String path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// ファイル、もしくはフォルダーを削除します。
        /// </summary>
        /// <param name="path"></param>
        public static void Delete(String path)
        {
            if (path.EndsWith("/") || path.EndsWith("\\"))
            {
                return;
            }
            System.Diagnostics.Debug.WriteLine(path);
            if (File.Exists(path))
            {
                DeleteFile(path);
            }
            else if (Directory.Exists(path))
            {
                DeleteDirectory(path);
            }
        }

        /// <summary>
        /// ディレクトリを削除します。
        /// </summary>
        /// <param name="path">パス</param>
        /// <exception cref="UnauthorizedAccessException"></exception>
        private static void DeleteDirectory(String path)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                foreach (FileInfo info in dirInfo.GetFiles())
                {
                    if ((info.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        info.Attributes = FileAttributes.Normal;
                    }
                }

                foreach (DirectoryInfo info in dirInfo.GetDirectories())
                {
                    DeleteDirectory(info.FullName);
                }

                if ((dirInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    dirInfo.Attributes = FileAttributes.Directory;
                }

                dirInfo.Delete(true);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ファイルを削除します。
        /// </summary>
        /// <param name="path">削除するファイル</param>
        private static void DeleteFile(String path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// ファイル、フォルダーが存在するかを返します。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Boolean Exists(String path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            else if (File.Exists(path))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// ファイルもしくはフォルダーをコピーします。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public static void Coly(String source, String dest)
        {
            //ファイルの場合
            if (File.Exists(source))
            {
                if (!Directory.Exists(Path.GetDirectoryName(dest)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(dest));
                }
                File.Copy(source, dest);
                return;
            }

            //フォルダーの場合
            if (Directory.Exists(source))
            {
                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                }
                dest += "\\";
                String[] files = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);
                foreach (String file in files)
                {
                    if (!Directory.Exists(Path.GetDirectoryName(file)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(file));
                    }
                    File.Copy(file, dest + file, true);
                }
                return;
            }
        }

        /// <summary>
        /// ファイル名を変更します。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public static void Rename(String path, String name)
        {
            File.Move(path, name);
        }

        public static string[] LoadDirectory(string path)
        {
            return FileController.LoadDirectory(path, false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] LoadDirectory(string path, bool isdir)
        {
            if (Directory.Exists(path))
            {
                if(isdir)
                {
                    return Directory.GetDirectories(path);
                }
                return Directory.GetFiles(path);
            }
            else
            {
                throw new DirectoryNotFoundException(path);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string LoadFile(string path)
        {
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path);
                string t = sr.ReadToEnd();
                sr.Dispose();
                return t;
            }
            else
            {
                throw new FileNotFoundException(path);
            }
        }

        public static string GetMD5(string path)
        {
            if(File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] hash = md5.ComputeHash(fs);
                md5.Clear();
                fs.Close();

                return BitConverter.ToString(hash).ToLower().Replace("-", "");
            }
            else
            {
                throw new FileNotFoundException(path);
            }
        }

        public static string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }
    }
}
