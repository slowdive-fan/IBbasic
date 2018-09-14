using System.Linq;
using Foundation;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Raventhal.iOS;
using SkiaSharp;
using System.Reflection;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.IO.Compression;
using Google.Analytics;
using IBbasic;
using Plugin.SimpleAudioPlayer;

[assembly: Dependency(typeof(SaveAndLoad_iOS))]
namespace Raventhal.iOS
{
    public class SaveAndLoad_iOS : ISaveAndLoad
    {
        public string TrackingId = "UA-60615839-14";
        public ITracker Tracker;
        const string AllowTrackingKey = "AllowTracking";
        int numOfTrackerEventHitsInThisSession = 0;
        public ISimpleAudioPlayer soundPlayer;
        public ISimpleAudioPlayer areaMusicPlayer;
        public ISimpleAudioPlayer areaAmbientSoundsPlayer;

        #region Instantition...
        private static SaveAndLoad_iOS thisRef;
        public SaveAndLoad_iOS()
        {
            // no code req'd
        }

        public static SaveAndLoad_iOS GetGASInstance()
        {
            if (thisRef == null)
                // it's ok, we can call this constructor
                thisRef = new SaveAndLoad_iOS();
            return thisRef;
        }
        #endregion

        public string GetVersion()
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString();
        }

        public void RateApp()
        {
            string urlStore = "https://itunes.apple.com/us/app/the-raventhal-ibbasic-rpg/id1434796255?action=write-review";
            Device.OpenUri(new Uri(urlStore));
        }

        public bool AllowReadWriteExternal()
        {
            return true;
        }

        public void CreateUserFolders()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var directoryname = Path.Combine(documents, "saves");
            Directory.CreateDirectory(directoryname);
        }

        public void CreateBackUpModuleFolder(string modFilename)
        {
            //not used
        }

        public void ZipModule(string modFilename)
        {
            //not used
        }

        public void UnZipModule(string modFilename)
        {
            //not used
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
        public void SaveImage(string fullPath, SKBitmap bmp)
        {
            //not used
        }

        public string LoadStringFromUserFolder(string fullPath)
        {
            string text = "";
            //check in app module folderr first
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets" + ConvertFullPath(fullPath, "."));
            if (stream != null)
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            }

            //check in user module folder next
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
            /*foreach (var res in assembly.GetManifestResourceNames())
            {
                 //System.Diagnostics.Debug.WriteLine(res);
            }*/
            Stream stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets" + ConvertFullPath(fullPath, "."));
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS." + filename);
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
            Stream stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets" + ConvertFullPath(assetFolderpath, "."));
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS." + filename);
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
            string modFilenameNoExtension = modFilename.Replace(".mod", "");
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.modules." + modFilenameNoExtension + "." + modFilename);
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
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.modules." + mdl.moduleName + ".graphics." + filename);            
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.modules." + mdl.moduleName + ".graphics." + filename + ".png");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS." + filename);
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS." + filename + ".png");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS." + filename + ".jpg");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.graphics." + filename);
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.graphics." + filename + ".png");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.graphics." + filename + ".jpg");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.tiles." + filename);
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.tiles." + filename + ".png");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.tiles." + filename + ".jpg");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.ui." + filename);
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.ui." + filename + ".png");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.ui." + filename + ".jpg");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.Assets.graphics.ui_missingtexture.png");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("Raventhal.iOS.ui_missingtexture.png");
            }
            SKManagedStream skStream = new SKManagedStream(stream);

            return SKBitmap.Decode(skStream);
        }

        public List<string> GetAllFilesWithExtensionFromUserFolder(string folderpath, string extension)
        {
            List<string> list = new List<string>();

            //FROM ASSETS
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            //DEBUGGING RESOURCE PATH
            //foreach (var res in assembly.GetManifestResourceNames())
            //{
            //    int x3 = 0;
            //}

            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.Contains(ConvertFullPath(folderpath, "."))) && (res.EndsWith(extension)))
                {
                    string[] split = res.Split('.');
                    list.Add(split[split.Length - 2]);
                }
            }

            //FROM USER FOLDER
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (Directory.Exists(documents + ConvertFullPath(folderpath, "/")))
            {
                string[] files = Directory.GetFiles(documents + ConvertFullPath(folderpath, "/"), "*" + extension, SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    list.Add(Path.GetFileNameWithoutExtension(file));
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
            //module folder in app 
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.Contains("Raventhal.iOS.Assets" + ConvertFullPath(userFolderpath, "."))) && (res.EndsWith(extension)))
                {
                    string[] split = res.Split('.');
                    list.Add(split[split.Length - 2]);
                }
            }

            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.Contains("Raventhal.iOS.Assets" + ConvertFullPath(assetFolderpath, "."))) && (res.EndsWith(extension)))
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

            if (!userOnly)
            {
                //search in assets
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

        public void TrackAppEvent(string Category, string EventAction, string EventLabel)
        {
            try
            {
                if (numOfTrackerEventHitsInThisSession > 300)
                {
                    Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateScreenView().Build());
                    Gai.SharedInstance.Dispatch(); // Manually dispatch the event immediately
                    numOfTrackerEventHitsInThisSession = 0;
                }
                else
                {
                    numOfTrackerEventHitsInThisSession++;
                }
                Gai.SharedInstance.DefaultTracker.Send(DictionaryBuilder.CreateEvent("iOS_" + Category, "iOS_" + EventAction, "iOS_" + EventLabel, null).Build());
                Gai.SharedInstance.Dispatch(); // Manually dispatch the event immediately
            }
            catch
            {

            }
        }
        public void InitializeNativeGAS()
        {
            try
            {
                var optionsDict = NSDictionary.FromObjectAndKey(new NSString("YES"), new NSString(AllowTrackingKey));
                NSUserDefaults.StandardUserDefaults.RegisterDefaults(optionsDict);

                Gai.SharedInstance.OptOut = !NSUserDefaults.StandardUserDefaults.BoolForKey(AllowTrackingKey);

                Gai.SharedInstance.DispatchInterval = 10;
                Gai.SharedInstance.TrackUncaughtExceptions = true;

                Tracker = Gai.SharedInstance.GetTracker("TestApp", TrackingId);
            }
            catch
            {

            }
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
}