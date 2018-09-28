using IBbasic.UWP;
using Newtonsoft.Json;
using Plugin.SimpleAudioPlayer;
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
        public ISimpleAudioPlayer soundPlayer;
        public ISimpleAudioPlayer areaMusicPlayer;
        public ISimpleAudioPlayer areaAmbientSoundsPlayer;

        public bool AllowReadWriteExternal()
        {
            return true;
        }

        public string GetVersion()
        {
            return typeof(App).GetTypeInfo().Assembly.GetName().Version.ToString();
        }

        public void RateApp()
        {

        }

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
        public void SaveImage(string fullPath, SKBitmap bmp)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            string convertedFullPath = storageFolder.Path + ConvertFullPath(fullPath, "\\");
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
            //check in assets module folder
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets" + ConvertFullPath(fullPath, "."));
            if (stream != null)
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                    return text;
                }
            }
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
            /*foreach (var res in assembly.GetManifestResourceNames())
            {
                if (res.EndsWith(".json"))
                {
                    int x3 = 0;
                }
            }*/
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
                if (File.Exists(storageFolder.Path + "\\modules\\" + mdl.moduleName + "\\graphics\\" + filename + ".png"))
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
                //STOP here if already found bitmap
                if (bm != null)
                {
                    return bm;
                }
                //If not found then try in Asset folder
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.modules." + mdl.moduleName + ".graphics." + filename);
                if (stream == null)
                {
                    stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.modules." + mdl.moduleName + ".graphics." + filename + ".png");
                }
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
                    stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.graphics." + filename);
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

            //FROM USER FOLDER
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            if (Directory.Exists(storageFolder.Path + ConvertFullPath(folderpath, "\\")))
            {
                string[] files = Directory.GetFiles(storageFolder.Path + ConvertFullPath(folderpath, "\\"), "*" + extension, SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    list.Add(Path.GetFileNameWithoutExtension(file));
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

            //FROM USER FOLDER
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            if (Directory.Exists(storageFolder.Path + ConvertFullPath(userFolderpath, "\\")))
            {
                string[] files = Directory.GetFiles(storageFolder.Path + ConvertFullPath(userFolderpath, "\\"), "*" + extension, SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    list.Add(Path.GetFileNameWithoutExtension(file));
                }
            }

            //FROM ASSETS
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.Contains("IBbasic.UWP.Assets" + ConvertFullPath(userFolderpath, "."))) && (res.EndsWith(extension)))
                {
                    string[] split = res.Split('.');
                    list.Add(split[split.Length - 2]);
                }
            }

            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.Contains("IBbasic.UWP.Assets" + ConvertFullPath(assetFolderpath, "."))) && (res.EndsWith(extension)))
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

        Stream GetStreamFromFile(GameView gv, string filename)
        {
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.modules." + gv.mod.moduleName + "." + filename);
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.modules." + gv.mod.moduleName + "." + filename + ".wav");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.modules." + gv.mod.moduleName + "." + filename + ".mp3");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.sounds." + filename);
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.sounds." + filename + ".wav");
            }
            if (stream == null)
            {
                stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.sounds." + filename + ".mp3");
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
}
