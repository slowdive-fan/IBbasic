using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Analytics;
using Android.Support.V4.App;
using LanternaExile.Droid;
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
using IBbasic;
using Plugin.SimpleAudioPlayer;

[assembly: Dependency(typeof(SaveAndLoad_Android))]
namespace LanternaExile.Droid
{
    public class SaveAndLoad_Android : ISaveAndLoad
    {
        public string TrackingId = "UA-60615839-17";
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

        public string ConvertFullPath(string fullPath, string replaceWith)
        {
            string convertedFullPath = "";
            convertedFullPath = fullPath.Replace("\\", replaceWith);
            return convertedFullPath;
        }

        public bool AllowReadWriteExternal()
        {
            return true;            
        }

        public void CreateUserFolders()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string convertedFullPath = documents + "/LanternaExile/saves";
            string path = ConvertFullPath(convertedFullPath, "\\");
            Directory.CreateDirectory(path);
        }

        public void CreateBackUpModuleFolder(string modFilename)
        {
            //not needed
        }

        public void ZipModule(string modFilename)
        {
            //not needed
        }

        public void UnZipModule(string modFilename)
        {
            //not needed
        }

        public void SaveText(string fullPath, string text)
        {
            string root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string convertedFullPath = root + "/LanternaExile" + ConvertFullPath(fullPath, "/");
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
            //not needed
        }

        public string LoadStringFromUserFolder(string fullPath)
        {
            string text = "";
            //check in app module folder first
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets" + ConvertFullPath(fullPath, "."));
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
            string filePath = root + "/LanternaExile" + ConvertFullPath(fullPath, "/");
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
            /*foreach (var res in assembly.GetManifestResourceNames())
            {
                if (res.EndsWith(".json"))
                {
                    int x3 = 0;
                }
            }*/
            Stream stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets" + ConvertFullPath(fullPath, "."));
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
            string filePath = root + "/LanternaExile" + ConvertFullPath(userFolderpath, "/");
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            //check in Assests folder last
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets" + ConvertFullPath(assetFolderpath, "."));
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
            string modFolder = Path.GetFileNameWithoutExtension(modFilename);
            string root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = root + "/LanternaExile/modules/" + modFolder + "/" + modFilename;
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            //try asset area            
            string modFilenameNoExtension = modFilename.Replace(".mod", "");
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if (res.EndsWith(".mod"))
                {
                    int x3 = 0;
                }
            }
            Stream stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.modules." + modFilenameNoExtension + "." + modFilename);
            if (stream != null)
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }            
            return "";
        }

        public SKBitmap LoadBitmap(string filename, IBbasic.Module mdl)
        {
            SKBitmap bm = null;
            try
            {
                //string storageFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                /*if (AllowReadWriteExternal())
                {
                    Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                    string storageFolder = sdCard.AbsolutePath + "/Raventhal";
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
                }*/
                //STOP here if already found bitmap
                /*if (bm != null)
                {
                    return bm;
                }*/
                //If not found then try in Asset folder
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.modules." + mdl.moduleName + ".graphics." + filename);
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.modules." + mdl.moduleName + ".graphics." + filename + ".png");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.graphics." + filename);
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.graphics." + filename + ".png");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.graphics." + filename + ".jpg");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.tiles." + filename);
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.tiles." + filename + ".png");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.tiles." + filename + ".jpg");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.ui." + filename);
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.ui." + filename + ".png");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.ui." + filename + ".jpg");
                }
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.graphics.ui_missingtexture.png");
                }
                SKManagedStream skStream = new SKManagedStream(stream);
                return SKBitmap.Decode(skStream);
            }
            catch (Exception ex)
            {
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.graphics.ui_missingtexture.png");
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
            Java.IO.File directory = new Java.IO.File(root + "/LanternaExile" + ConvertFullPath(folderpath, "/"));
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
            Java.IO.File directory = new Java.IO.File(root + "/LanternaExile" + ConvertFullPath(userFolderpath, "/"));
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
                if ((res.Contains("LanternaExile.Droid.Assets" + ConvertFullPath(userFolderpath, "."))) && (res.EndsWith(extension)))
                {
                    string[] split = res.Split('.');
                    list.Add(split[split.Length - 2]);
                }
            }
            //from main asset folder
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.Contains("LanternaExile.Droid.Assets" + ConvertFullPath(assetFolderpath, "."))) && (res.EndsWith(extension)))
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
            
            //search in external folder
            string root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Java.IO.File directory = new Java.IO.File(root + "/LanternaExile/modules");
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
            var stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.modules." + gv.mod.moduleName + "." + filename);
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.modules." + gv.mod.moduleName + "." + filename + ".wav");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.modules." + gv.mod.moduleName + "." + filename + ".mp3");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.sounds." + filename);
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.sounds." + filename + ".wav");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("LanternaExile.Droid.Assets.sounds." + filename + ".mp3");
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
                    soundPlayer = CrossSimpleAudioPlayer.Current;
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
            if ((filenameNoExtension.Equals("none")) || (filenameNoExtension.Equals("")) || (gv.mod.playSoundFx))
            {
                //play nothing
                return;
            }
            else
            {
                if (areaMusicPlayer == null)
                {
                    areaMusicPlayer = CrossSimpleAudioPlayer.Current;
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
            if ((filenameNoExtension.Equals("none")) || (filenameNoExtension.Equals("")) || (gv.mod.playSoundFx))
            {
                //play nothing
                return;
            }
            else
            {
                if (areaAmbientSoundsPlayer == null)
                {
                    areaAmbientSoundsPlayer = CrossSimpleAudioPlayer.Current;
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