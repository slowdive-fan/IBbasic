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
        string GetVersion();
        void RateApp();

        bool AllowReadWriteExternal();
        void CreateUserFolders();
        void CreateBackUpModuleFolder(string modFilename);
        void SaveText(string fullPath, string text);
        void SaveImage(string fullPath, SKBitmap bmp);
        void ZipModule(string modFilename);
        void UnZipModule(string modFilename);

        string LoadStringFromUserFolder(string fullPath);
        string LoadStringFromAssetFolder(string fullPath);
        string LoadStringFromEitherFolder(string assetFolderpath, string userFolderpath);
        
        string GetModuleFileString(string modFilename);

        SKBitmap LoadBitmap(string filename, Module mdl);

        List<string> GetAllFilesWithExtensionFromUserFolder(string folderpath, string extension);
        List<string> GetAllFilesWithExtensionFromAssetFolder(string folderpath, string extension);
        List<string> GetAllFilesWithExtensionFromBothFolders(string assetFolderpath, string userFolderpath, string extension);
        List<string> GetAllModuleFiles(bool userOnly);

        void TrackAppEvent(string Category, string EventAction, string EventLabel);

        void RestartAreaMusicIfEnded(GameView gv);
        void PlaySound(GameView gv, string fileName);
        void PlayAreaMusic(GameView gv, string fileName);
        void PlayAreaAmbientSounds(GameView gv, string fileName);
        void StopAreaMusic();
        void PauseAreaMusic();
    }
}
