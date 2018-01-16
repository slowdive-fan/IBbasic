using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IBbasic
{
    /// <summary>
    /// Define an API for loading and saving a text file. Reference this interface
    /// in the common code, and implement this interface in the app projects for
    /// iOS, Android and WinPhone. Remember to use the 
    ///     [assembly: Dependency (typeof (SaveAndLoad_IMPLEMENTATION_CLASSNAME))]
    /// attribute on each of the implementations.
    /// </summary>
    public interface ISaveAndLoad
    {
        void SaveText(string filename, string text);
        string LoadText(string filename);
        string GetModuleFileString(string modFilename);
        string GetModuleAssetFileString(string modFolder, string assetFilename);
        string GetSettingsString();
        string GetDataAssetFileString(string assetFilename);
        string GetSaveFileString(string modName, string filename);
        List<string> GetFiles(string path, string assetPath, string endsWith);
        void SaveSettings(Settings toggleSettings);
        void SaveCharacter(string pathAndFilename, Player pc);
        void SaveModule(string modFolder, string modFilename);
        void SaveSaveGame(string modName, string filename, SaveGame save);

        void SaveBitmap(string filename, SKBitmap bmp);
        SKBitmap LoadBitmap(string filename);

        List<string> GetAllModuleFiles();        

        bool FileExists(string filename);
    }
}
