using IBbasic.UWP;
using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveAndLoad_UWP))]
namespace IBbasic.UWP
{    
    public class SaveAndLoad_UWP : ISaveAndLoad
    {
        public string ConvertFullPath(string fullPath, string replaceWith)
        {
            string convertedFullPath = "";
            convertedFullPath = fullPath.Replace("\\", replaceWith);
            return convertedFullPath;
        }
        public string ConvertFullPath(string fullPath, string toReplace, string replaceWith)
        {
            string convertedFullPath = "";
            convertedFullPath = fullPath.Replace(toReplace, replaceWith);
            return convertedFullPath;
        }

        public void CreateUserFolders()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            string dir = storageFolder.Path + "\\modules";
            Directory.CreateDirectory(dir);
            dir = storageFolder.Path + "\\saves";
            Directory.CreateDirectory(dir);
            dir = storageFolder.Path + "\\module_backups";
            Directory.CreateDirectory(dir);
        }

        public void CreateBackUpModuleFolder(string modFilename)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            string dir = storageFolder.Path + "\\module_backups";
            //string folderName = gv.mod.moduleName;
            string incrementFolderName = "";
            for (int i = 0; i < 999; i++) // add an incremental save option (uses directoryName plus number for folder name)
            {
                if (!Directory.Exists(dir + "\\" + modFilename + "(" + i.ToString() + ")"))
                {
                    incrementFolderName = modFilename + "(" + i.ToString() + ")";
                    DirectoryInfo diSource = new DirectoryInfo(storageFolder.Path + "\\modules\\" + modFilename);
                    DirectoryInfo diTarget = new DirectoryInfo(storageFolder.Path + "\\module_backups\\" + modFilename + "(" + i.ToString() + ")");

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
            
        }

        public void UnZipModule(string modFilename)
        {

        }

        public void SaveText(string fullPath, string text)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            string convertedFullPath = storageFolder.Path + ConvertFullPath(fullPath, "\\");
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
            //check in module folder first
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            string convertedFullPath = storageFolder.Path + ConvertFullPath(fullPath, "\\");
            if (File.Exists(convertedFullPath))
            {
                text = File.ReadAllText(convertedFullPath);
                return text;
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
            Stream stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets" + ConvertFullPath(fullPath, "."));
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
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            string convertedFullPath = storageFolder.Path + ConvertFullPath(userFolderpath, "\\");
            if (File.Exists(convertedFullPath))
            {
                text = File.ReadAllText(convertedFullPath);
                return text;
            }
            //check in Assests folder last
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets" + ConvertFullPath(assetFolderpath, "."));
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
                Stream stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.NewModule.mod");
                using (var reader = new System.IO.StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            else
            {
                string modFolder = Path.GetFileNameWithoutExtension(modFilename);
                //try from personal folder first
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                var filePath = storageFolder.Path + "\\modules\\" + modFolder + "\\" + modFilename;
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath);
                }
                else //try from external folder
                {
                    /*Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                    filePath = sdCard.AbsolutePath + "/IBbasic/modules/" + modFolder + "/" + modFilename;
                    if (File.Exists(filePath))
                    {
                        return File.ReadAllText(filePath);
                    }*/
                }
                //try asset area            
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.modules." + modFolder + "." + modFilename);
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
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\tiles\\" + filename + ".png"))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\tiles\\" + filename + ".png");
                }
                else if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\tiles\\" + filename + ".PNG"))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\tiles\\" + filename + ".PNG");
                }
                else if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\tiles\\" + filename))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\tiles\\" + filename);
                }
                else if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\graphics\\" + filename + ".png"))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\graphics\\" + filename + ".png");
                }
                else if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\graphics\\" + filename + ".PNG"))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\graphics\\" + filename + ".PNG");
                }
                else if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\graphics\\" + filename + ".jpg"))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\graphics\\" + filename + ".jpg");
                }
                else if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\graphics\\" + filename))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\graphics\\" + filename);
                }
                else if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\ui\\" + filename + ".png"))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\ui\\" + filename + ".png");
                }
                else if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\ui\\" + filename + ".PNG"))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\ui\\" + filename + ".PNG");
                }
                else if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\ui\\" + filename))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\ui\\" + filename);
                }
                else if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\pctokens\\" + filename + ".png"))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\pctokens\\" + filename + ".png");
                }
                else if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\pctokens\\" + filename + ".PNG"))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\pctokens\\" + filename + ".PNG");
                }
                else if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\pctokens\\" + filename))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\pctokens\\" + filename);
                }
                else if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\portraits\\" + filename + ".png"))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\portraits\\" + filename + ".png");
                }
                else if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\portraits\\" + filename + ".PNG"))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\portraits\\" + filename + ".PNG");
                }
                else if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\portraits\\" + filename))
                {
                    bm = SKBitmap.Decode(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\portraits\\" + filename);
                }
                //STOP here if already found bitmap
                if (bm != null)
                {
                    return bm;
                }
                //If not found then try in Asset folder
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.graphics." + filename);
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.graphics." + filename + ".png");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.graphics." + filename + ".jpg");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.tiles." + filename);
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.tiles." + filename + ".png");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.tiles." + filename + ".jpg");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.ui." + filename);
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.ui." + filename + ".png");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.ui." + filename + ".jpg");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.graphics.ui_missingtexture.png");
                }
                SKManagedStream skStream = new SKManagedStream(stream);
                return SKBitmap.Decode(skStream);
            }
            catch (Exception ex)
            {
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.graphics.ui_missingtexture.png");
                SKManagedStream skStream = new SKManagedStream(stream);
                return SKBitmap.Decode(skStream);
            }
        }

        public List<string> GetAllFilesWithExtensionFromUserFolder(string folderpath, string extension)
        {
            List<string> list = new List<string>();
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            string[] files = Directory.GetFiles(storageFolder.Path + ConvertFullPath(folderpath, "\\"), "*" + extension, SearchOption.AllDirectories);
            foreach (string file in files)
            {
                list.Add(Path.GetFileNameWithoutExtension(file));
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
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            if (Directory.Exists(storageFolder.Path + ConvertFullPath(userFolderpath, "\\")))
            {
                string[] files = Directory.GetFiles(storageFolder.Path + ConvertFullPath(userFolderpath, "\\"), "*" + extension, SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    list.Add(Path.GetFileNameWithoutExtension(file));
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
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            string[] files = Directory.GetFiles(storageFolder.Path + "\\modules", "*.mod", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (Path.GetFileName(file) != "NewModule.mod")
                {
                    list.Add(Path.GetFileName(file));
                }
            }

            return list;
        }

        public void TrackAppEvent(string Category, string EventAction, string EventLabel)
        {

        }

        MediaPlayer playerAreaMusic;
        public void CreateAreaMusicPlayer()
        {
            playerAreaMusic = new MediaPlayer();
            playerAreaMusic.IsLoopingEnabled = true;
            playerAreaMusic.AutoPlay = false;
            playerAreaMusic.Volume = 0.5;
        }
        public void LoadAreaMusicFile(string fullPath)
        {
            string filename = Path.GetFileName(fullPath);
            if (filename != "none")
            {
                //check in module folder first
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                string convertedFullPath = storageFolder.Path + ConvertFullPath(fullPath, "\\");
                string convertedFullPathUri = ConvertFullPath(fullPath, "/");
                if (File.Exists(convertedFullPath))
                {
                    playerAreaMusic.Source = MediaSource.CreateFromUri(new Uri("ms-appdata:///local" + convertedFullPathUri));
                }
                else if (File.Exists(convertedFullPath + ".mp3"))
                {
                    playerAreaMusic.Source = MediaSource.CreateFromUri(new Uri("ms-appdata:///local" + convertedFullPathUri + ".mp3"));
                }
            }
            if (playerAreaMusic != null)
            {
                //playerAreaMusic.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/" + fileName));
                //playerAreaMusic.MediaEnded += OnPlaybackEnded;
            }
        }
        public void PlayAreaMusic()
        {
            if (playerAreaMusic == null || playerAreaMusic.Source == null)
            {
                return;
            }

            if (playerAreaMusic.PlaybackSession.PlaybackState == MediaPlaybackState.Playing)
            {
                playerAreaMusic.Pause();
                playerAreaMusic.PlaybackSession.Position = TimeSpan.FromSeconds(0);
                playerAreaMusic.Play();
            }
            else
            {
                playerAreaMusic.Play();
            }
        }
        public void StopAreaMusic()
        {
            playerAreaMusic.Pause();
            playerAreaMusic.PlaybackSession.Position = TimeSpan.FromSeconds(0);
        }
        public void PauseAreaMusic()
        {
            playerAreaMusic.Pause();
        }
    }
}
