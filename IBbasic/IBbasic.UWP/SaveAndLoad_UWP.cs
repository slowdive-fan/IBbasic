﻿using IBbasic.UWP;
using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveAndLoad_UWP))]
namespace IBbasic.UWP
{    
    public class SaveAndLoad_UWP : ISaveAndLoad
    {
        #region ISaveAndLoad Text implementation
        public void SaveText(string filename, string text)
        {
            /*StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, text);*/
        }
        public void SaveSettings(Settings toggleSettings)
        {
            //try personal folder first
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = documentsPath + "\\settings.json";
            string json = JsonConvert.SerializeObject(toggleSettings, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(json.ToString());
            }
        }
        public void SaveCharacter(string modName, string filename, Player pc)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = documentsPath + "\\saves\\" + modName + "\\characters\\" + filename;
            string json = JsonConvert.SerializeObject(pc, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(json.ToString());
            }
        }
        public void SaveModuleAssetFile(string modFolder, string assetFilenameWithExtension, string json)
        {

        }
        public void SaveSaveGame(string modName, string filename, SaveGame save)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = documentsPath + "\\IBbasic\\saves\\" + modName + "\\" + filename;
            System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            file.Directory.Create(); // If the directory already exists, this method does nothing.
            string json = JsonConvert.SerializeObject(save, Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(json.ToString());
            }
        }

        public string LoadText(string filename)
        {
            return "";
            /*StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.GetFileAsync(filename);
            string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            return text;*/
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
            else
            {
                string modFolder = Path.GetFileNameWithoutExtension(modFilename);
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

                //try from personal folder first
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = documentsPath + "/modules/" + modFolder + "/" + modFilename;
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
            }
            return "";
        }
        public string GetModuleAssetFileString(string modFolder, string assetFilename)
        {
            //try asset area            
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.modules." + modFolder + "." + assetFilename);
            if (stream != null)
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            //try from personal folder first
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //string modFolder = Path.GetFileNameWithoutExtension(areaFilename);
            var filePath = documentsPath + "/modules/" + modFolder + "/" + assetFilename;
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else //try from external folder
            {
                /*Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                filePath = sdCard.AbsolutePath + "/IBbasic/modules/" + modFolder + "/" + areaFilename + ".are";
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath);
                }*/
            }
            return "";
        }
        public string GetDataAssetFileString(string assetFilename)
        {
            //try asset area            
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("IBbasic.UWP.Assets.data." + assetFilename);
            if (stream != null)
            {
                using (var reader = new System.IO.StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            return "";
        }
        public string GetSettingsString()
        {
            //try from personal folder first
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //string modFolder = Path.GetFileNameWithoutExtension(areaFilename);
            var filePath = documentsPath + "/settings.json";
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            /*else //try from external folder
            {
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                filePath = sdCard.AbsolutePath + "/IBbasic/settings.json";
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath);
                }
            }*/
            return "";
        }
        public string GetSaveFileString(string modName, string filename)
        {
            //try from personal folder first
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = documentsPath + "\\IBbasic\\saves\\" + modName + "\\" + filename;
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            /*else //try from external folder
            {
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                filePath = sdCard.AbsolutePath + "/IBbasic/saves/" + filename;
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath);
                }
            }*/
            return "";
        }
        #endregion

        #region ISaveAndLoad Bitmap implementation
        public void SaveBitmap(string filename, SKBitmap bmp)
        {
            //StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            //StorageFile sampleFile = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            //await FileIO.WriteTextAsync(sampleFile, bmp);
        }
        public SKBitmap LoadBitmap(string filename)
        {
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

            //Stream fileStream = File.OpenRead("btn_small_on.png");
            return SKBitmap.Decode(skStream);

            //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //StorageFile sampleFile = await storageFolder.GetFileAsync(filename);
            //SKBitmap text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            //return text;


        }
        #endregion

        public List<string> GetAllModuleFiles()
        {
            List<string> list = new List<string>();
            //search in assets
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if (res.EndsWith(".mod"))
                {
                    list.Add(res);
                }
            }

            return list;
        }
        public List<string> GetAllAreaFilenames(string modFolder)
        {
            List<string> list = new List<string>();
            //search in assets
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if (res.EndsWith(".mod"))
                {
                    list.Add(res);
                }
            }

            return list;
        }
        public List<string> GetAllConvoFilenames(string modFolder)
        {
            List<string> list = new List<string>();
            //search in assets
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if (res.EndsWith(".mod"))
                {
                    list.Add(res);
                }
            }

            return list;
        }
        public List<string> GetAllEncounterFilenames(string modFolder)
        {
            List<string> list = new List<string>();
            //search in assets
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if (res.EndsWith(".mod"))
                {
                    list.Add(res);
                }
            }

            return list;
        }
        public List<string> GetFiles(string path, string assetPath, string endsWith)
        {
            List<string> list = new List<string>();

            //search in assets
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.EndsWith(endsWith)) && (res.Contains(assetPath)))
                {
                    list.Add(GetFileNameFromResource(res));
                }
            }
            /*
            //search in personal folder
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            Java.IO.File directory = new Java.IO.File(documentsPath + "/" + path);
            directory.Mkdirs();
            foreach (Java.IO.File f in directory.ListFiles())
            {
                if (f.Name.EndsWith(endsWith))
                {
                    list.Add(f.Name);
                }
            }

            //search in external folder
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            directory = new Java.IO.File(sdCard.AbsolutePath + "/IBbasic/" + path);
            directory.Mkdirs();
            //check to see if Lanterna2 exists, if not copy it over
            foreach (Java.IO.File f in directory.ListFiles())
            {
                if (f.Name.EndsWith(endsWith))
                {
                    list.Add(f.Name);
                }
            }*/
            return list;
        }
        public List<string> GetGraphicsFiles(string modFolder, string endsWith)
        {
            List<string> list = new List<string>();

            //search in assets
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.EndsWith(endsWith)) && (res.Contains(".graphics.")))
                {
                    list.Add(GetFileNameFromResource(res));
                }
            }
            /*
            //search in personal folder
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            Java.IO.File directory = new Java.IO.File(documentsPath + "/" + path);
            directory.Mkdirs();
            foreach (Java.IO.File f in directory.ListFiles())
            {
                if (f.Name.EndsWith(endsWith))
                {
                    list.Add(f.Name);
                }
            }

            //search in external folder
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            directory = new Java.IO.File(sdCard.AbsolutePath + "/IBbasic/" + path);
            directory.Mkdirs();
            //check to see if Lanterna2 exists, if not copy it over
            foreach (Java.IO.File f in directory.ListFiles())
            {
                if (f.Name.EndsWith(endsWith))
                {
                    list.Add(f.Name);
                }
            }*/
            return list;
        }
        public List<string> GetTileFiles(string modFolder, string endsWith)
        {
            List<string> list = new List<string>();

            //search in assets
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.EndsWith(endsWith)) && (res.Contains(".tiles.")))
                {
                    list.Add(GetFileNameFromResource(res));
                }
            }
            /*
            //search in personal folder
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            Java.IO.File directory = new Java.IO.File(documentsPath + "/" + path);
            directory.Mkdirs();
            foreach (Java.IO.File f in directory.ListFiles())
            {
                if (f.Name.EndsWith(endsWith))
                {
                    list.Add(f.Name);
                }
            }

            //search in external folder
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            directory = new Java.IO.File(sdCard.AbsolutePath + "/IBbasic/" + path);
            directory.Mkdirs();
            //check to see if Lanterna2 exists, if not copy it over
            foreach (Java.IO.File f in directory.ListFiles())
            {
                if (f.Name.EndsWith(endsWith))
                {
                    list.Add(f.Name);
                }
            }*/
            return list;
        }
        public List<string> GetCharacterFiles(string modFolder, string endsWith)
        {
            List<string> list = new List<string>();

            //search in assets
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                if ((res.EndsWith(endsWith)) && (res.Contains(".saves." + modFolder + ".characters")))
                {
                    list.Add(GetFileNameFromResource(res));
                }
            }
            /*
            //search in personal folder
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            Java.IO.File directory = new Java.IO.File(documentsPath + "/" + path);
            directory.Mkdirs();
            foreach (Java.IO.File f in directory.ListFiles())
            {
                if (f.Name.EndsWith(endsWith))
                {
                    list.Add(f.Name);
                }
            }

            //search in external folder
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            directory = new Java.IO.File(sdCard.AbsolutePath + "/IBbasic/" + path);
            directory.Mkdirs();
            //check to see if Lanterna2 exists, if not copy it over
            foreach (Java.IO.File f in directory.ListFiles())
            {
                if (f.Name.EndsWith(endsWith))
                {
                    list.Add(f.Name);
                }
            }*/
            return list;
        }

        public bool FileExists(string filename)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            try
            {
                localFolder.GetFileAsync(filename).AsTask().Wait();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string GetFileNameFromResource(string res)
        {
            string filename = "";
            List<string> parts = res.Split('.').ToList();
            filename = parts[parts.Count - 2] + "." + parts[parts.Count - 1];
            return filename;
        }
    }
}
