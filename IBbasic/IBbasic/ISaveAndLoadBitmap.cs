﻿using SkiaSharp;
using System.Threading.Tasks;

namespace IBbasic
{
    /// <summary>
    /// Define an API for loading and saving a bitmap file. Reference this interface
    /// in the common code, and implement this interface in the app projects for
    /// iOS, Android and WinPhone. Remember to use the 
    ///     [assembly: Dependency (typeof (SaveAndLoad_IMPLEMENTATION_CLASSNAME))]
    /// attribute on each of the implementations.
    /// </summary>
    public interface ISaveAndLoadBitmap
    {
        void SaveBitmap(string filename, SKBitmap bmp);
        SKBitmap LoadBitmap(string filename);
        bool FileExists(string filename);
    }
}
