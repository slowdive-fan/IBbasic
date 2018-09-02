using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Analytics;
using Android.Support.V4.App;
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
        public string TrackingId = "UA-60615839-12";
        private static GoogleAnalytics GAInstance;
        private static Tracker GATracker;
        public Context thisContext;
        int numOfTrackerEventHitsInThisSession = 0;

        #region Instantiation ...
        private static SaveAndLoad_Android thisRef;
        public SaveAndLoad_Android()
        {
            // no code req'd
        }

        public static SaveAndLoad_Android GetGASInstance()
        {
            if (thisRef == null)
                // it's ok, we can call this constructor
                thisRef = new SaveAndLoad_Android();
            return thisRef;
        }
        #endregion

        public void Initialize_NativeGAS(Context AppContext = null)
        {
            thisContext = AppContext;
            GAInstance = GoogleAnalytics.GetInstance(AppContext.ApplicationContext);
            GAInstance.SetLocalDispatchPeriod(10);

            GATracker = GAInstance.NewTracker(TrackingId);
            GATracker.EnableExceptionReporting(true);
            GATracker.EnableAdvertisingIdCollection(true);
        }

        public string ConvertFullPath(string fullPath, string replaceWith)
        {
            string convertedFullPath = "";
            convertedFullPath = fullPath.Replace("\\", replaceWith);
            return convertedFullPath;
        }

        public bool AllowReadWriteExternal()
        {
            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.M)
            {
                return true;
            }
            if (Android.App.Application.Context.CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted)
            {
                return true;
            }
            else
            {
                //string[] perms = {Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage};
                //ActivityCompat.RequestPermissions(MainActivity.ThisActivity, perms, 0);
                return false;
            }
        }

        public string GetVersion()
        {
            var context = global::Android.App.Application.Context;

            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            return info.VersionName;
        }

        public void CreateUserFolders()
        {
            /*
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
            */

            if (AllowReadWriteExternal())
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
            else
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string convertedFullPath = documents + "/IBbasic/saves";
                string path = ConvertFullPath(convertedFullPath, "\\");
                Directory.CreateDirectory(path);
            }
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
            string root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (AllowReadWriteExternal())
            {
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                root = sdCard.AbsolutePath;
            }
            //Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            string convertedFullPath = root + "/IBbasic" + ConvertFullPath(fullPath, "/");
            string path = ConvertFullPath(fullPath, "\\");
            string dir = Path.GetDirectoryName(convertedFullPath);
            Directory.CreateDirectory(dir);
            using (StreamWriter sw = File.CreateText(convertedFullPath))
            {
                sw.Write(text);
            }
        }
        public void SaveImage(string fullPath, SKBitmap bmp)
        {
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            string convertedFullPath = sdCard.AbsolutePath + "/IBbasic" + ConvertFullPath(fullPath, "/");
            string path = ConvertFullPath(fullPath, "\\");
            string dir = Path.GetDirectoryName(convertedFullPath);

            try
            {
                Directory.CreateDirectory(dir);
                // create an image and then get the PNG (or any other) encoded data
                using (var image = SKImage.FromBitmap(bmp))
                {
                    using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                    {
                        // save the data to a stream
                        using (var stream = File.OpenWrite(convertedFullPath))
                        {
                            data.SaveTo(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public string LoadStringFromUserFolder(string fullPath)
        {
            string text = "";
            //check in app module folderr first
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets" + ConvertFullPath(fullPath, "."));
            if (stream != null)
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    text = reader.ReadToEnd();                    
                }
                return text;
            }

            //check in user module folder next
            string root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (AllowReadWriteExternal())
            {
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                root = sdCard.AbsolutePath;
            }
            //Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            string filePath = root + "/IBbasic" + ConvertFullPath(fullPath, "/");
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
            string root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (AllowReadWriteExternal())
            {
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                root = sdCard.AbsolutePath;
            }
            //Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            string filePath = root + "/IBbasic" + ConvertFullPath(userFolderpath, "/");
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

                string root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (AllowReadWriteExternal())
                {
                    Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                    root = sdCard.AbsolutePath;
                }
                string filePath = root + "/IBbasic/modules/" + modFolder + "/" + modFilename;
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath);
                }
                //try from personal folder first
                /*var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
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
                }*/
                //try asset area            
                string modFilenameNoExtension = modFilename.Replace(".mod", "");
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.modules." + modFilenameNoExtension + "." + modFilename);
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
                if (AllowReadWriteExternal())
                {
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
                }
                //STOP here if already found bitmap
                if (bm != null)
                {
                    return bm;
                }
                //If not found then try in Asset folder
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.modules." + mdl.moduleName + ".graphics." + filename);
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.modules." + mdl.moduleName + ".graphics." + filename + ".png");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.Droid.Assets.graphics." + filename);
                }
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
            //FROM ASSETS
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.Contains(ConvertFullPath(folderpath, "."))) && (res.EndsWith(extension)))
                {
                    string[] split = res.Split('.');
                    list.Add(split[split.Length - 2]);
                }
            }

            //search in external folder
            string root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (AllowReadWriteExternal())
            {
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                root = sdCard.AbsolutePath;
            }
            //Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            Java.IO.File directory = new Java.IO.File(root + "/IBbasic" + ConvertFullPath(folderpath, "/"));
            //directory.Mkdirs();
            if (directory.Exists())
            {
                foreach (Java.IO.File f in directory.ListFiles())
                {
                    if (f.Name.EndsWith(extension))
                    {
                        string[] split = f.Name.Split('.');
                        list.Add(split[split.Length - 2]);
                    }
                }
            }
            return list;
        }
        public List<string> GetAllFilesWithExtensionFromAssetFolder(string folderpath, string extension) //NOT USED
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
            string root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (AllowReadWriteExternal())
            {
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                root = sdCard.AbsolutePath;
            }
            //Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            Java.IO.File directory = new Java.IO.File(root + "/IBbasic" + ConvertFullPath(userFolderpath, "/"));
            //directory.Mkdirs();
            if (directory.Exists())
            {
                foreach (Java.IO.File f in directory.ListFiles())
                {
                    if (f.Name.EndsWith(extension))
                    {
                        string[] split = f.Name.Split('.');
                        list.Add(split[split.Length - 2]);
                    }
                }
            }
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            //module folder in app 
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.Contains("IBbasic.Droid.Assets" + ConvertFullPath(userFolderpath, "."))) && (res.EndsWith(extension)))
                {
                    string[] split = res.Split('.');
                    list.Add(split[split.Length - 2]);
                }
            }
            //from main asset folder
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.Contains("IBbasic.Droid.Assets" + ConvertFullPath(assetFolderpath, "."))) && (res.EndsWith(extension)))
                {
                    string[] split = res.Split('.');
                    list.Add(split[split.Length - 2]);
                }
            }
            return list;
        }
        public List<string> GetAllModuleFiles(bool userOnly)
        {
            List<string> list = new List<string>();

            //search in assets
            if (!userOnly)
            {
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                foreach (var res in assembly.GetManifestResourceNames())
                {
                    if ((res.EndsWith(".mod")) && (!res.EndsWith("NewModule.mod")))
                    {
                        list.Add(res);
                    }
                }
            }

            //search in personal folder
            /*var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            Java.IO.File directory = new Java.IO.File(documentsPath + "/modules");
            if (directory.Exists())
            {
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
            }*/
            //search in external folder
            string root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (AllowReadWriteExternal())
            {
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                root = sdCard.AbsolutePath;
            }
            //Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            Java.IO.File directory = new Java.IO.File(root + "/IBbasic/modules");
            if (directory.Exists())
            {
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
            }

            return list;
        }

        public void TrackAppEvent(string Category, string EventAction, string EventLabel)
        {
            try
            {
                if (numOfTrackerEventHitsInThisSession > 300)
                {
                    GATracker.Send(new HitBuilders.EventBuilder().SetNewSession().Build());
                    numOfTrackerEventHitsInThisSession = 0;
                }
                else
                {
                    numOfTrackerEventHitsInThisSession++;
                }
                HitBuilders.EventBuilder builder = new HitBuilders.EventBuilder();
                builder.SetCategory("An_" + Category);
                builder.SetAction("An_" + EventAction);
                builder.SetLabel("An_" + EventLabel);
                GATracker.Send(builder.Build());
            }
            catch { }
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