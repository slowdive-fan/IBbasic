using System.Linq;
using Foundation;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using IBbasic.iOS;
using SkiaSharp;
using System.Reflection;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.IO.Compression;

[assembly: Dependency(typeof(SaveAndLoad_iOS))]
namespace IBbasic.iOS
{
    public class SaveAndLoad_iOS : ISaveAndLoad
    {
        public void CreateUserFolders()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var directoryname = Path.Combine(documents, "modules");
            Directory.CreateDirectory(directoryname);
            directoryname = Path.Combine(documents, "saves");
            Directory.CreateDirectory(directoryname);
            directoryname = Path.Combine(documents, "module_backups");
            Directory.CreateDirectory(directoryname);
        }

        public void CreateBackUpModuleFolder(string modFilename)
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var directoryname = Path.Combine(documents, "module_backups");
            try
            {
                string incrementFolderName = "";
                for (int i = 0; i < 999; i++) // add an incremental save option (uses directoryName plus number for folder name)
                {
                    if (!Directory.Exists(directoryname + "/" + modFilename + "(" + i.ToString() + ")"))
                    {
                        incrementFolderName = modFilename + "(" + i.ToString() + ")";
                        DirectoryInfo diSource = new DirectoryInfo(documents + "/modules/" + modFilename);
                        DirectoryInfo diTarget = new DirectoryInfo(documents + "/module_backups/" + modFilename + "(" + i.ToString() + ")");

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
            catch (Exception ex)
            {

            }
        }

        public void ZipModule(string modFilename)
        {
            try
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryname = Path.Combine(documents, "modules");
                var path = Path.Combine(directoryname, modFilename);
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
                //if module folder already exists then copy to back-up folder and delete
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryname = Path.Combine(documents, "modules");
                var path = Path.Combine(directoryname, modFilename);
                if (Directory.Exists(path))
                {
                    CreateBackUpModuleFolder(modFilename);
                    Directory.Delete(path, true);
                }
                ZipFile.ExtractToDirectory(path + ".zip", path);
            }
            catch (Exception ex)
            {

            }
        }

        public void SaveText(string fullPath, string text)
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string convertedFullPath = documents + ConvertFullPath(fullPath, "/");
            string dir = Path.GetDirectoryName(convertedFullPath);
            try
            {
                Directory.CreateDirectory(dir);
                using (StreamWriter sw = File.CreateText(convertedFullPath))
                {
                    sw.Write(text);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public string LoadStringFromUserFolder(string fullPath)
        {
            string text = "";
            //check in module folder first
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string convertedFullPath = documents + ConvertFullPath(fullPath, "/");
            if (File.Exists(convertedFullPath))
            {
                try
                {
                    text = File.ReadAllText(convertedFullPath);
                }
                catch (Exception ex)
                {

                }
                return text;
            }
            return text;
        }
        public string LoadStringFromAssetFolder(string fullPath)
        {
            string text = "";
            //string filename = Path.GetFileName("C:" + fullPath);
            int pos = fullPath.LastIndexOf("\\") + 1;
            string filename = fullPath.Substring(pos, fullPath.Length - pos);
            //check in Assests folder last
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                 //System.Diagnostics.Debug.WriteLine(res);
            }
            Stream stream = assembly.GetManifestResourceStream("IBbasic.iOS.Assets" + ConvertFullPath(fullPath, "."));
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.iOS." + filename);
            }
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            return text;
        }
        public string LoadStringFromEitherFolder(string assetFolderpath, string userFolderpath)
        {
            string text = "";
            int pos = userFolderpath.LastIndexOf("\\") + 1;
            string filename = userFolderpath.Substring(pos, userFolderpath.Length - pos);
            //check in module folder first
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string convertedFullPath = documents + ConvertFullPath(userFolderpath, "/");
            if (File.Exists(convertedFullPath))
            {
                try
                {
                    text = File.ReadAllText(convertedFullPath);
                }
                catch (Exception ex)
                {

                }
                return text;
            }
            //check in Assests folder last
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("IBbasic.iOS.Assets" + ConvertFullPath(assetFolderpath, "."));
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.iOS." + filename);
            }
            if (stream != null)
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
            }
            return text;
        }
        
        public string ConvertFullPath(string fullPath, string replaceWith)
        {
            string convertedFullPath = "";
            convertedFullPath = fullPath.Replace("\\", replaceWith);
            return convertedFullPath;
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
                Stream stream = assembly.GetManifestResourceStream("IBbasic.iOS.NewModule.mod");
                using (var reader = new System.IO.StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            else
            {
                string modFolder = Path.GetFileNameWithoutExtension(modFilename);
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string convertedFullPath = documents + "/modules/" + modFolder + "/" + modFilename;
                if (File.Exists(convertedFullPath))
                {
                    string text = "";
                    try
                    {
                        text = File.ReadAllText(convertedFullPath);
                    }
                    catch (Exception ex)
                    {

                    }
                    return text;
                }
                               
                //try asset module            
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("IBbasic.iOS." + modFilename);
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
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var modulesDir = Path.Combine(documents, "modules");
            var modFolder = Path.Combine(modulesDir, mdl.moduleName);
            var modGraphicsFolder = Path.Combine(modFolder, "graphics");
            var filePath = Path.Combine(modGraphicsFolder, filename);

            if (File.Exists(filePath))
            {
                SKBitmap bm = SKBitmap.Decode(filePath);
                if (bm != null)
                {
                    return bm;
                }
            }
            else if (File.Exists(filePath + ".png"))
            {
                SKBitmap bm = SKBitmap.Decode(filePath + ".png");
                if (bm != null)
                {
                    return bm;
                }
            }
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("IBbasic.iOS." + filename);
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.iOS." + filename + ".png");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.iOS." + filename + ".jpg");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.iOS.Assets.graphics." + filename);
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.iOS.Assets.graphics." + filename + ".png");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.iOS.Assets.graphics." + filename + ".jpg");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.iOS.Assets.tiles." + filename);
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.iOS.Assets.tiles." + filename + ".png");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.iOS.Assets.tiles." + filename + ".jpg");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.iOS.Assets.ui." + filename);
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.iOS.Assets.ui." + filename + ".png");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.iOS.Assets.ui." + filename + ".jpg");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.iOS.Assets.graphics.ui_missingtexture.png");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.iOS.ui_missingtexture.png");
            }
            SKManagedStream skStream = new SKManagedStream(stream);

            //Stream fileStream = File.OpenRead("btn_small_on.png");
            return SKBitmap.Decode(skStream);

            //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //StorageFile sampleFile = await storageFolder.GetFileAsync(filename);
            //SKBitmap text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            //return text;


        }

        public List<string> GetAllFilesWithExtensionFromUserFolder(string folderpath, string extension)
        {
            List<string> list = new List<string>();

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

            //FROM USER FOLDER
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (Directory.Exists(documents + ConvertFullPath(userFolderpath, "/")))
            {
                string[] files = Directory.GetFiles(documents + ConvertFullPath(userFolderpath, "/"), "*" + extension, SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    list.Add(Path.GetFileNameWithoutExtension(file));
                }
            }

            //FROM ASSETS
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            
            //DEBUGGING RESOURCE PATH
            //foreach (var res in assembly.GetManifestResourceNames())
            //{
            //    int x3 = 0;
            //}

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
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string[] files = Directory.GetFiles(documents + "/modules", "*.mod", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (Path.GetFileName(file) != "NewModule.mod")
                {
                    list.Add(Path.GetFileName(file));
                }
            }

            return list;
        }

        //Android.Media.MediaPlayer playerAreaMusic;
        public void CreateAreaMusicPlayer()
        {
            //playerAreaMusic = new Android.Media.MediaPlayer();
            //playerAreaMusic.Looping = true;
            //playerAreaMusic.SetVolume(0.5f, 0.5f);
        }
        public void LoadAreaMusicFile(string fileName)
        {
            //playerAreaMusic.Reset();
            //AssetFileDescriptor afd = Android.App.Application.Context.Assets.OpenFd(fileName);
            //playerAreaMusic.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
        }
        public void PlayAreaMusic()
        {
            /*
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
            */
        }
        public void StopAreaMusic()
        {
            //playerAreaMusic.Pause();
            //playerAreaMusic.SeekTo(0);
        }
        public void PauseAreaMusic()
        {
            //playerAreaMusic.Pause();
        }
    }
}