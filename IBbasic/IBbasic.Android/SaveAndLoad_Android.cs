using IBbasic.Droid;
using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveAndLoad_Android))]
namespace IBbasic.Droid
{
    public class SaveAndLoad_Android : ISaveAndLoad
    {
        public string ConvertFullPath(string fullPath, string replaceWith)
        {
            string convertedFullPath = "";
            convertedFullPath = fullPath.Replace("\\", replaceWith);
            return convertedFullPath;
        }

        public void CreateUserFolders()
        {
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            string convertedFullPath = sdCard.AbsolutePath + "/IBbasic/modules";
            string path = ConvertFullPath(convertedFullPath, "\\");
            Directory.CreateDirectory(path);
            convertedFullPath = sdCard.AbsolutePath + "/IBbasic/saves";
            path = ConvertFullPath(convertedFullPath, "\\");
            Directory.CreateDirectory(path);
            convertedFullPath = sdCard.AbsolutePath + "/IBbasic/module_backups";
            path = ConvertFullPath(convertedFullPath, "\\");
            Directory.CreateDirectory(path);
        }

        public void CreateBackUpModuleFolder(string modFilename)
        {
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            //string androidPath = sdCard.AbsolutePath + "/IBbasic/module_backups";
            string winPath = ConvertFullPath(sdCard.AbsolutePath + "/IBbasic/module_backups/", "\\");
            for (int i = 0; i < 999; i++) // add an incremental save option (uses directoryName plus number for folder name)
            {
                Java.IO.File testFile = new Java.IO.File(winPath + modFilename + "(" + i.ToString() + ")");
                if (!testFile.Exists())
                {
                    DirectoryInfo diSource = new DirectoryInfo(sdCard.AbsolutePath + "/IBbasic/modules/" + modFilename);
                    DirectoryInfo diTarget = new DirectoryInfo(sdCard.AbsolutePath + "/IBbasic/module_backups/" + modFilename + "(" + i.ToString() + ")");

                    Directory.CreateDirectory(diTarget.FullName);

                    // Copy each file into the new directory.
                    foreach (FileInfo fi in diSource.GetFiles())
                    {
                        fi.CopyTo(Path.Combine(diTarget.FullName, fi.Name), true);
                    }
                    break;
                }
            }
        }

        public void ZipModule(string modFilename)
        {
            try
            {
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                string convertedFullPath = sdCard.AbsolutePath + "/IBbasic/modules/" + modFilename;
                string path = ConvertFullPath(convertedFullPath, "\\");
                ZipFile.CreateFromDirectory(path, path + ".zip");
            }
            catch (Exception ex)
            {

            }
        }

        public void UnZipModule(string modFilename)
        {
            try
            {
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                string convertedFullPath = sdCard.AbsolutePath + "/IBbasic/modules/" + modFilename;
                string path = ConvertFullPath(convertedFullPath, "\\");
                ZipFile.ExtractToDirectory(path + ".zip", path);
            }
            catch (Exception ex)
            {

            }
        }

        public void SaveText(string fullPath, string text)
        {
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            string convertedFullPath = sdCard.AbsolutePath + "/IBbasic" + ConvertFullPath(fullPath, "/");
            string path = ConvertFullPath(fullPath, "\\");
            string dir = Path.GetDirectoryName(convertedFullPath);
            Directory.CreateDirectory(dir);
            using (StreamWriter sw = File.CreateText(convertedFullPath))
            {
                sw.Write(text);
            }
        }

        public string LoadStringFromUserFolder(string fullPath)
        {
            string text = "";
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            string filePath = sdCard.AbsolutePath + "/IBbasic" + ConvertFullPath(fullPath, "/");
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return text;
        }
        public string LoadStringFromAssetFolder(string fullPath)
        {
            string text = "";
            //check in Assests folder last
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if (res.EndsWith(".json"))
                {
                    int x3 = 0;
                }
            }
            Stream stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets" + ConvertFullPath(fullPath, "."));
            if (stream != null)
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
            }
            return text;
        }
        public string LoadStringFromEitherFolder(string assetFolderpath, string userFolderpath)
        {
            string text = "";
            //check in module folder first
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            string filePath = sdCard.AbsolutePath + "/IBbasic" + ConvertFullPath(userFolderpath, "/");
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            //check in Assests folder last
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            //foreach (var res in assembly.GetManifestResourceNames())
            //{
            //    System.Diagnostics.Debug.WriteLine("found resource: " + res);
            //}
            Stream stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets" + ConvertFullPath(assetFolderpath, "."));
            if (stream != null)
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
            }
            return text;
        }

        public string GetModuleFileString(string modFilename)
        {
            //asset module
            if (modFilename.StartsWith("IBbasic."))
            {
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream(modFilename);
                using (var reader = new System.IO.StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            else if (modFilename.Equals("NewModule.mod"))
            {
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.NewModule.mod");
                using (var reader = new System.IO.StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            else
            {
                string modFolder = Path.GetFileNameWithoutExtension(modFilename);
                //try from personal folder first
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = documentsPath + "/IBbasic/modules/" + modFolder + "/" + modFilename;
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath);
                }
                else //try from external folder
                {
                    Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                    filePath = sdCard.AbsolutePath + "/IBbasic/modules/" + modFolder + "/" + modFilename;
                    if (File.Exists(filePath))
                    {
                        return File.ReadAllText(filePath);
                    }
                }
                //try asset area            
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.modules." + modFolder + "." + modFilename);
                if (stream != null)
                {
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            return "";
        }

        public SKBitmap LoadBitmap(string filename, Module mdl)
        {
            SKBitmap bm = null;
            try
            {
                //string storageFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                string storageFolder = sdCard.AbsolutePath + "/IBbasic";
                if (File.Exists(storageFolder + "/modules/" + mdl.moduleName + "/graphics/" + filename + ".png"))
                {
                    bm = SKBitmap.Decode(storageFolder + "/modules/" + mdl.moduleName + "/graphics/" + filename + ".png");
                }
                else if (File.Exists(storageFolder + "/modules/" + mdl.moduleName + "/graphics/" + filename + ".PNG"))
                {
                    bm = SKBitmap.Decode(storageFolder + "/modules/" + mdl.moduleName + "/graphics/" + filename + ".PNG");
                }
                else if (File.Exists(storageFolder + "/modules/" + mdl.moduleName + "/graphics/" + filename + ".jpg"))
                {
                    bm = SKBitmap.Decode(storageFolder + "/modules/" + mdl.moduleName + "/graphics/" + filename + ".jpg");
                }
                else if (File.Exists(storageFolder + "/modules/" + mdl.moduleName + "/graphics/" + filename))
                {
                    bm = SKBitmap.Decode(storageFolder + "/modules/" + mdl.moduleName + "/graphics/" + filename);
                }                
                //STOP here if already found bitmap
                if (bm != null)
                {
                    return bm;
                }
                //If not found then try in Asset folder
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.graphics." + filename);
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.graphics." + filename + ".png");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.graphics." + filename + ".jpg");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.tiles." + filename);
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.tiles." + filename + ".png");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.tiles." + filename + ".jpg");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.ui." + filename);
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.ui." + filename + ".png");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.ui." + filename + ".jpg");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.graphics.ui_missingtexture.png");
                }
                SKManagedStream skStream = new SKManagedStream(stream);
                return SKBitmap.Decode(skStream);
            }
            catch (Exception ex)
            {
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.graphics.ui_missingtexture.png");
                SKManagedStream skStream = new SKManagedStream(stream);
                return SKBitmap.Decode(skStream);
            }
        }

        public List<string> GetAllFilesWithExtensionFromUserFolder(string folderpath, string extension)
        {
            List<string> list = new List<string>();
            //search in external folder
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            Java.IO.File directory = new Java.IO.File(sdCard.AbsolutePath + "/IBbasic" + ConvertFullPath(folderpath, "/"));
            directory.Mkdirs();
            //check to see if Lanterna2 exists, if not copy it over
            foreach (Java.IO.File f in directory.ListFiles())
            {
                if (f.Name.EndsWith(extension))
                {
                    string[] split = f.Name.Split('.');
                    list.Add(split[split.Length - 2]);
                }
            }
            return list;
        }
        public List<string> GetAllFilesWithExtensionFromAssetFolder(string folderpath, string extension)
        {
            List<string> list = new List<string>();
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.Contains(ConvertFullPath(folderpath, "."))) && (res.EndsWith(extension)))
                {
                    string[] split = res.Split('.');
                    list.Add(split[split.Length - 2]);
                }
            }
            return list;
        }
        public List<string> GetAllFilesWithExtensionFromBothFolders(string assetFolderpath, string userFolderpath, string extension)
        {
            List<string> list = new List<string>();
            //search in external folder
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            Java.IO.File directory = new Java.IO.File(sdCard.AbsolutePath + "/IBbasic" + ConvertFullPath(userFolderpath, "/"));
            directory.Mkdirs();
            //check to see if Lanterna2 exists, if not copy it over
            foreach (Java.IO.File f in directory.ListFiles())
            {
                if (f.Name.EndsWith(extension))
                {
                    string[] split = f.Name.Split('.');
                    list.Add(split[split.Length - 2]);
                }
            }
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.Contains(ConvertFullPath(assetFolderpath, "."))) && (res.EndsWith(extension)))
                {
                    string[] split = res.Split('.');
                    list.Add(split[split.Length - 2]);
                }
            }
            return list;
        }
        public List<string> GetAllModuleFiles()
        {
            List<string> list = new List<string>();

            //search in assets
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.EndsWith(".mod")) && (!res.EndsWith("NewModule.mod")))
                {
                    list.Add(res);
                }
            }

            //search in personal folder
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            Java.IO.File directory = new Java.IO.File(documentsPath + "/modules");
            directory.Mkdirs();
            foreach (Java.IO.File d in directory.ListFiles())
            {
                if (d.IsDirectory)
                {
                    Java.IO.File modDirectory = new Java.IO.File(directory.Path + "/" + d.Name);
                    foreach (Java.IO.File f in modDirectory.ListFiles())
                    {
                        try
                        {
                            if (f.Name.EndsWith(".mod"))
                            {
                                list.Add(f.Name);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

            }
            //search in external folder
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            directory = new Java.IO.File(sdCard.AbsolutePath + "/IBbasic/modules");
            directory.Mkdirs();
            //check to see if Lanterna2 exists, if not copy it over
            foreach (Java.IO.File d in directory.ListFiles())
            {
                if (d.IsDirectory)
                {
                    Java.IO.File modDirectory = new Java.IO.File(directory.Path + "/" + d.Name);
                    foreach (Java.IO.File f in modDirectory.ListFiles())
                    {
                        try
                        {
                            if (f.Name.EndsWith(".mod"))
                            {
                                list.Add(f.Name);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return list;
        }

        Android.Media.MediaPlayer playerAreaMusic;
        public void CreateAreaMusicPlayer()
        {
            playerAreaMusic = new Android.Media.MediaPlayer();
            playerAreaMusic.Looping = true;
            playerAreaMusic.SetVolume(0.5f, 0.5f);
        }
        public void LoadAreaMusicFile(string fullPath)
        {
            playerAreaMusic.Reset();
            string filename = Path.GetFileNameWithoutExtension(fullPath);
            if (filename != "none")
            {
                //check in module folder first
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                //string filePath = sdCard.AbsolutePath + "/IBx" + ConvertFullPath(fullPath, "/");
                if (File.Exists(sdCard.AbsolutePath + "/IBbasic" + ConvertFullPath(fullPath, "/")))
                {
                    playerAreaMusic.SetDataSource(sdCard.AbsolutePath + "/IBbasic" + ConvertFullPath(fullPath, "/"));
                }
                else if (File.Exists(sdCard.AbsolutePath + "/IBbasic" + ConvertFullPath(fullPath, "/") + ".mp3"))
                {
                    playerAreaMusic.SetDataSource(sdCard.AbsolutePath + "/IBbasic" + ConvertFullPath(fullPath, "/") + ".mp3");
                }
            }
        }
        public void PlayAreaMusic()
        {
            if (playerAreaMusic == null)
            {
                return;
            }
            if (playerAreaMusic.IsPlaying)
            {
                playerAreaMusic.Pause();
                playerAreaMusic.SeekTo(0);
            }
            playerAreaMusic.Start();
        }
        public void StopAreaMusic()
        {
            playerAreaMusic.Pause();
            playerAreaMusic.SeekTo(0);
        }
        public void PauseAreaMusic()
        {
            playerAreaMusic.Pause();
        }
    }
}