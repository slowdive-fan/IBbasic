using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Analytics;
using Android.Support.V4.App;
using IBbasic.Droid;
using Newtonsoft.Json;
using Plugin.SimpleAudioPlayer;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
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
        public ISimpleAudioPlayer soundPlayer;
        public ISimpleAudioPlayer areaMusicPlayer;
        public ISimpleAudioPlayer areaAmbientSoundsPlayer;

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

        public void RateApp()
        {
            //string urlStore = "https://play.google.com/store/apps/details?id=com.iceblinkengine.ibb.ibbraventhal";
            //Device.OpenUri(new Uri(urlStore));
        }

        public bool DownloadFile(string url, string filename)
        {
            //string pathToNewFolder = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, folder);
            //Directory.CreateDirectory(pathToNewFolder);
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            string convertedFullPath = sdCard.AbsolutePath + "/IBbasic/modules/" + filename;
            string path = ConvertFullPath(convertedFullPath, "\\");
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(url, path);
                return true;
                //webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                //string pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName(url));
                //webClient.DownloadFileAsync(new Uri(url), path);
            }
            catch (Exception ex)
            {
                return false;
            }
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
                convertedFullPath = sdCard.AbsolutePath + "/IBbasic/user";
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
                //ZipFile.CreateFromDirectory(path, path + ".zip");
                IbbFile ibbfile = new IbbFile();
                ibbfile.WriteIbbFile(path);
                ibbfile = null;
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
                string convertedFullPath = sdCard.AbsolutePath + "/IBbasic/modules/" + modFilename + ".ibb";
                string path = ConvertFullPath(convertedFullPath, "\\");
                if (Directory.Exists(path))
                {
                    //CreateBackUpModuleFolder(modFilename);
                    Directory.Delete(path, true);
                }
                //ZipFile.ExtractToDirectory(path + ".zip", path, true);
                IbbFile ibbfile = new IbbFile();
                ibbfile.ReadIbbFile(path);
                ibbfile = null;
            }
            catch (Exception ex)
            {
                
            }
            try
            {
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                string convertedFullPath = sdCard.AbsolutePath + "/IBbasic/modules/" + modFilename + ".ibb";
                string fullpath = ConvertFullPath(convertedFullPath, "\\");
                Java.IO.File full = new Java.IO.File(fullpath);
                Android.Net.Uri path = Android.Net.Uri.FromFile(full);

                full.SetReadable(true, false);
                var email = new Intent(Android.Content.Intent.ActionSend);
                email.PutExtra(Android.Content.Intent.ExtraEmail,
                new string[] { "iceblinkengine@gmail.com" });
                email.PutExtra(Android.Content.Intent.ExtraSubject, "Submit Module");
                email.PutExtra(Android.Content.Intent.ExtraText,
                "I am submitting a module or updated module for inclusion in the IBbasic module downloads list.");
                email.PutExtra(Intent.ExtraStream, path);
                email.SetType("message/rfc822");
                Android.App.Application.Context.StartActivity(Intent.CreateChooser(email, "Send email..."));

            }
            catch (Exception ex)
            {

            }
        }

        public void SubmitModule(string modFilename)
        {
            try
            {
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                string convertedFullPath = sdCard.AbsolutePath + "/IBbasic/modules/" + modFilename + ".ibb";
                string fullpath = ConvertFullPath(convertedFullPath, "\\");
                Java.IO.File full = new Java.IO.File(fullpath);
                Android.Net.Uri path = Android.Net.Uri.FromFile(full);

                full.SetReadable(true, false);
                var email = new Intent(Android.Content.Intent.ActionSend);
                email.PutExtra(Android.Content.Intent.ExtraEmail,
                new string[] { "iceblinkengine@gmail.com" });
                email.PutExtra(Android.Content.Intent.ExtraSubject, "Submit Module");
                email.PutExtra(Android.Content.Intent.ExtraText,
                "I am submitting a module or updated module for inclusion in the IBbasic module downloads list.");
                email.PutExtra(Intent.ExtraStream, path);
                email.SetType("message/rfc822");
                Android.App.Application.Context.StartActivity(Intent.CreateChooser(email, "Send email..."));
                Android.Widget.Toast.MakeText(Android.App.Application.Context, "Send Email...Success...yeah, more modules!", Android.Widget.ToastLength.Short);
            }
            catch (Exception ex)
            {
                Android.Widget.Toast.MakeText(Android.App.Application.Context, "Failed to Send Email...", Android.Widget.ToastLength.Short);
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
                    else  if (File.Exists(storageFolder + "/user/" + filename + ".png"))
                    {
                        bm = SKBitmap.Decode(storageFolder + "/user/" + filename + ".png");
                    }
                    else if (File.Exists(storageFolder + "/user/" + filename + ".PNG"))
                    {
                        bm = SKBitmap.Decode(storageFolder + "/user/" + filename + ".PNG");
                    }
                    else if (File.Exists(storageFolder + "/user/" + filename))
                    {
                        bm = SKBitmap.Decode(storageFolder + "/user/" + filename);
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

        Stream GetStreamFromFile(GameView gv, string filename)
        {
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.modules." + gv.mod.moduleName + "." + filename);
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.modules." + gv.mod.moduleName + "." + filename + ".wav");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.modules." + gv.mod.moduleName + "." + filename + ".mp3");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.sounds." + filename);
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.sounds." + filename + ".wav");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.sounds." + filename + ".mp3");
            }
            return stream;
        }
        public void PlaySound(GameView gv, string filenameNoExtension)
        {
            if ((filenameNoExtension.Equals("none")) || (filenameNoExtension.Equals("")) || (!gv.mod.playSoundFx))
            {
                //play nothing
                return;
            }
            else
            {
                if (soundPlayer == null)
                {
                    soundPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                }
                try
                {
                    soundPlayer.Loop = false;
                    soundPlayer.Load(GetStreamFromFile(gv, filenameNoExtension));
                    soundPlayer.Play();
                }
                catch (Exception ex)
                {
                    if (gv.mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<yl>failed to play sound" + filenameNoExtension + "</yl><BR>");
                    }
                }
            }
        }
        public void PlayAreaMusic(GameView gv, string filenameNoExtension)
        {
            if ((filenameNoExtension.Equals("none")) || (filenameNoExtension.Equals("")) || (!gv.mod.playSoundFx))
            {
                //play nothing
                return;
            }
            else
            {
                if (areaMusicPlayer == null)
                {
                    areaMusicPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                }
                try
                {
                    areaMusicPlayer.Loop = true;
                    areaMusicPlayer.Load(GetStreamFromFile(gv, filenameNoExtension));
                    areaMusicPlayer.Play();
                }
                catch (Exception ex)
                {
                    if (gv.mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<yl>failed to play area music" + filenameNoExtension + "</yl><BR>");
                    }
                }
            }
        }
        public void PlayAreaAmbientSounds(GameView gv, string filenameNoExtension)
        {
            if ((filenameNoExtension.Equals("none")) || (filenameNoExtension.Equals("")) || (!gv.mod.playSoundFx))
            {
                //play nothing
                return;
            }
            else
            {
                if (areaAmbientSoundsPlayer == null)
                {
                    areaAmbientSoundsPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                }
                try
                {
                    areaAmbientSoundsPlayer.Loop = true;
                    areaAmbientSoundsPlayer.Load(GetStreamFromFile(gv, filenameNoExtension));
                    areaAmbientSoundsPlayer.Play();
                }
                catch (Exception ex)
                {
                    if (gv.mod.debugMode) //SD_20131102
                    {
                        gv.cc.addLogText("<yl>failed to play area music" + filenameNoExtension + "</yl><BR>");
                    }
                }
            }
        }
        public void RestartAreaMusicIfEnded(GameView gv)
        {
            //restart area music
            if (areaMusicPlayer == null)
            {
                areaMusicPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            }
            try
            {
                if ((!areaMusicPlayer.IsPlaying) && (gv.mod.playSoundFx))
                {
                    try
                    {
                        areaMusicPlayer.Play();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }

            //restart area ambient sounds
            if (areaAmbientSoundsPlayer == null)
            {
                areaAmbientSoundsPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            }
            try
            {
                if ((!areaAmbientSoundsPlayer.IsPlaying) && (gv.mod.playSoundFx))
                {
                    try
                    {
                        areaAmbientSoundsPlayer.Play();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void StopAreaMusic()
        {
            if (areaMusicPlayer == null)
            {
                areaMusicPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            }
            try
            {
                if (areaMusicPlayer.IsPlaying)
                {
                    areaMusicPlayer.Stop();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void PauseAreaMusic()
        {
            //playerAreaMusic.Pause();
        }
    }

    public class IbbFile
    {
        public byte[] fileBytes = new byte[0];
        public byte[] resourceBytes = new byte[0];
        public byte[] keyBytes = new byte[0];
        public byte[] resourceListBytes = new byte[0];
        public IbbHeader thisHeader = new IbbHeader();
        public List<IbbKeyStruct> KeyList = new List<IbbKeyStruct>();
        public List<IbbResourceStruct> ResourceList = new List<IbbResourceStruct>();

        public IbbFile()
        {
        }
        public void WriteIbbFile(string path)
        {
            fileBytes = new byte[0];
            resourceBytes = new byte[0];
            keyBytes = new byte[0];
            resourceListBytes = new byte[0];

            int numberOfEntries = 0;
            //setup resource list
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    //read all bytes and store in resourceFileBytes
                    byte[] newResArray = File.ReadAllBytes(file);
                    keyBytes = Combine(keyBytes, BitConverter.GetBytes(Path.GetFileName(file).Length));
                    keyBytes = Combine(keyBytes, Encoding.ASCII.GetBytes(Path.GetFileName(file)));
                    keyBytes = Combine(keyBytes, BitConverter.GetBytes(false));
                    resourceListBytes = Combine(resourceListBytes, BitConverter.GetBytes(resourceBytes.Length));
                    resourceListBytes = Combine(resourceListBytes, BitConverter.GetBytes(newResArray.Length));
                    resourceBytes = Combine(resourceBytes, newResArray);
                    numberOfEntries++;
                }
            }
            //go through module's graphics folder if exists
            var pathGraphics = Path.Combine(path, "graphics");
            if (Directory.Exists(pathGraphics))
            {
                string[] files = Directory.GetFiles(pathGraphics);
                foreach (string file in files)
                {
                    //read all bytes and store in resourceFileBytes
                    byte[] newResArray = File.ReadAllBytes(file);
                    keyBytes = Combine(keyBytes, BitConverter.GetBytes(Path.GetFileName(file).Length));
                    keyBytes = Combine(keyBytes, Encoding.ASCII.GetBytes(Path.GetFileName(file)));
                    keyBytes = Combine(keyBytes, BitConverter.GetBytes(true));
                    resourceListBytes = Combine(resourceListBytes, BitConverter.GetBytes(resourceBytes.Length));
                    resourceListBytes = Combine(resourceListBytes, BitConverter.GetBytes(newResArray.Length));
                    resourceBytes = Combine(resourceBytes, newResArray);
                    numberOfEntries++;
                }
            }
            //setup header info
            fileBytes = Combine(fileBytes, BitConverter.GetBytes(numberOfEntries));
            fileBytes = Combine(fileBytes, BitConverter.GetBytes(12));
            fileBytes = Combine(fileBytes, BitConverter.GetBytes(12 + keyBytes.Length));
            //combine all byte arrays
            fileBytes = Combine(fileBytes, keyBytes);
            fileBytes = Combine(fileBytes, resourceListBytes);
            fileBytes = Combine(fileBytes, resourceBytes);
            //write out all
            File.WriteAllBytes(path + ".ibb", fileBytes);
        }
        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }
        public void ReadIbbFile(string path)
        {
            fileBytes = new byte[0];
            resourceBytes = new byte[0];
            keyBytes = new byte[0];
            resourceListBytes = new byte[0];

            fileBytes = File.ReadAllBytes(path);
            ReadHeader();
            LoadKeyList();
            LoadResourceList();
            string modulesfolder = Path.GetDirectoryName(path);
            string modname = Path.GetFileNameWithoutExtension(path);
            //var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //var modulesfolder = Path.Combine(documents, "modules");
            var modfolder = Path.Combine(modulesfolder, modname);
            var graphicsfolder = Path.Combine(modfolder, "graphics");
            Directory.CreateDirectory(modfolder);
            Directory.CreateDirectory(graphicsfolder);
            //iterate through all files and add to modIBmini
            int startToResources = thisHeader.OffsetToResourceList + (thisHeader.EntryCount * 8);
            for (int i = 0; i < thisHeader.EntryCount; i++)
            {
                byte[] newArray = new byte[ResourceList[i].ResourceSize];
                Array.Copy(fileBytes, ResourceList[i].OffsetToResource + startToResources, newArray, 0, ResourceList[i].ResourceSize);
                try
                {
                    if (!KeyList[i].graphicsFolder)
                    {
                        var file = Path.Combine(modfolder, KeyList[i].filename);
                        File.WriteAllBytes(file, newArray);
                    }
                    else
                    {
                        var file = Path.Combine(graphicsfolder, KeyList[i].filename);
                        File.WriteAllBytes(file, newArray);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        public void ReadHeader()
        {
            int i = 0;
            thisHeader.EntryCount = readInt(fileBytes, i, 4); //32bits
            thisHeader.OffsetToKeyList = readInt(fileBytes, i += 4, 4); //32bits
            thisHeader.OffsetToResourceList = readInt(fileBytes, i += 4, 4); //32bits
        }
        public void LoadKeyList()
        {
            int offset = thisHeader.OffsetToKeyList - 1;
            KeyList.Clear();
            for (int i = 0; i < thisHeader.EntryCount; i++)
            {
                IbbKeyStruct newKeyStruct = new IbbKeyStruct();
                newKeyStruct.filenameLength = readInt(fileBytes, offset += 1, 4);
                newKeyStruct.filename = readString(fileBytes, offset += 4, newKeyStruct.filenameLength);
                newKeyStruct.graphicsFolder = readBool(fileBytes, offset += newKeyStruct.filenameLength, 1);
                KeyList.Add(newKeyStruct);
            }
        }
        public void LoadResourceList()
        {
            int startToResources = thisHeader.OffsetToResourceList;
            int offset = startToResources - 4;
            ResourceList.Clear();
            for (int i = 0; i < thisHeader.EntryCount; i++)
            {
                IbbResourceStruct newResStruct = new IbbResourceStruct();
                newResStruct.OffsetToResource = readInt(fileBytes, offset += 4, 4);
                newResStruct.ResourceSize = readInt(fileBytes, offset += 4, 4);
                ResourceList.Add(newResStruct);
            }
        }

        public string readString(byte[] array, int index, int length)
        {
            string val = "";
            for (int i = index; i < index + length; i++)
            {
                char c = Convert.ToChar(array[i]);
                if (c == '\0') { continue; }
                val += Convert.ToChar(array[i]).ToString();
            }
            return val;
        }
        public int readInt(byte[] array, int index, int length)
        {
            byte[] newArray = new byte[length];
            Array.Copy(array, index, newArray, 0, length);

            int i = BitConverter.ToInt32(newArray, 0);
            return i;
        }
        public ushort readShort(byte[] array, int index, int length)
        {
            byte[] newArray = new byte[length];
            Array.Copy(array, index, newArray, 0, length);
            ushort i = BitConverter.ToUInt16(newArray, 0);
            return i;
        }
        public bool readBool(byte[] array, int index, int length)
        {
            byte[] newArray = new byte[length];
            Array.Copy(array, index, newArray, 0, length);
            bool i = BitConverter.ToBoolean(newArray, 0);
            return i;
        }
    }

    public class IbbHeader
    {
        public int EntryCount = 0; //32bits
        public int OffsetToKeyList = 0; //32bits
        public int OffsetToResourceList = 0; //32bits

        public IbbHeader()
        {

        }
    }

    public class IbbKeyStruct
    {
        public int filenameLength = 0; //32bits
        public string filename = ""; //48 bytes
        public bool graphicsFolder = false; //1 bit

        public IbbKeyStruct()
        {

        }
    }

    public class IbbResourceStruct
    {
        public int OffsetToResource = 0;
        public int ResourceSize = 0;

        public IbbResourceStruct()
        {

        }
    }
}