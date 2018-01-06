using Newtonsoft.Json;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IBbasic
{
	public partial class MainPage : ContentPage
	{
        GameView gv;
        SKPaint backFill = new SKPaint { Style = SKPaintStyle.Fill };
        SKBitmap bm = new SKBitmap(100, 100, true);
        public int counter = 0;
        public Stopwatch gameTimerStopwatch = new Stopwatch();
        public long previousTime = 0;
        public float fps = 0;
        public int reportFPScount = 0;
        public string mainDirectory;
        public string actionType = "";
        public int locX = 0;
        public int locY = 0;

        public MainPage()
        {
            InitializeComponent();

            mainDirectory = Directory.GetCurrentDirectory();

            gv = new GameView();

            SetUpBitmap();

            Device.StartTimer(TimeSpan.FromSeconds(1f / 30), () =>
            {
                gv.gameTimer_Tick(canvasView);                
                return true;
            });

            gameTimerStopwatch.Start();
            previousTime = gameTimerStopwatch.ElapsedMilliseconds;
        }

        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.Blue);

            gv.Render(canvas);
            /*
            counter++;

            long current = gameTimerStopwatch.ElapsedMilliseconds; //get the current total amount of ms since the game launched
            int elapsed = (int)(current - previousTime); //calculate the total ms elapsed since the last time through the game loop
            if (reportFPScount >= 10)
            {
                reportFPScount = 0;
                fps = 1000 / (current - previousTime);
            }
            reportFPScount++;
            previousTime = current; //remember the current time at the beginning of this tick call for the next time through the game loop to calculate elapsed time

            canvas.Clear(SKColors.CornflowerBlue);

            for (int z = 0; z < 5; z++)
            {
                for (int x = 0; x < 1000; x += 50)
                {
                    for (int y = 0; y < 1000; y += 50)
                    {
                        canvas.DrawBitmap(bm, x + counter + z, y + counter);
                    }
                }
            }

            if (counter > 200) { counter = 0; }

            // set up drawing tools
            using (var paint = new SKPaint())
            {
                paint.TextSize = 16.0f;
#if __ANDROID__
                paint.TextSize = 30.0f;
#endif
                paint.IsAntialias = true;
                paint.Color = new SKColor(255, 255, 255);
                paint.IsStroke = false;

                // draw the text
                canvas.DrawText("FPS:" + fps, 0.0f, 64.0f, paint);
                canvas.DrawText("Dir:" + mainDirectory, 0.0f, 128.0f, paint);
                canvas.DrawText("ActionType:" + actionType.ToString(), 0.0f, 192.0f, paint);
                canvas.DrawText("LocX:" + locX, 0.0f, 256.0f, paint);
                canvas.DrawText("LocY:" + locY, 0.0f, 320.0f, paint);
            }
            */
        }

        private void OnTouch(object sender, SKTouchEventArgs e)
        {
            gv.onMouseEvent(sender, e);
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    // start of a stroke
                    var p = new SKPath();
                    p.MoveTo(e.Location);
                    //temporaryPaths[e.Id] = p;
                    //string file = LoadTextAndroid("test3d.mod");
                    break;
                case SKTouchAction.Moved:
                    // the stroke, while pressed
                    actionType = e.ActionType.ToString();
                    locX = (int)e.Location.X;
                    locY = (int)e.Location.Y;
                    //if (e.InContact)
                    //temporaryPaths[e.Id].LineTo(e.Location);
                    break;
                case SKTouchAction.Released:
                    // end of a stroke
                    int x = 0;
                    //paths.Add(temporaryPaths[e.Id]);
                    //temporaryPaths.Remove(e.Id);
                    break;
                case SKTouchAction.Cancelled:
                    // we don't want that stroke
                    x = 0;
                    //temporaryPaths.Remove(e.Id);
                    break;
            }

            // we have handled these events
            e.Handled = true;

            // update the UI
            ((SKCanvasView)sender).InvalidateSurface();
        }

        public void SetUpBitmap()
        {

            //string filename = "TodoDatabase.db3";
            //string libraryPath = "";

#if __UWP__
            // UWP
            //Assembly assembly = GetType().GetTypeInfo().Assembly;
            //Stream stream = assembly.GetManifestResourceStream("IBbasicTest2.Graphics.btn_small_on.png");
            //SKManagedStream skStream = new SKManagedStream(stream);

            Stream fileStream = File.OpenRead("btn_small_on.png");
            bm = SKBitmap.Decode(fileStream);
            //libraryPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
#endif

#if __IOS__
            // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
            // (they don't want non-user-generated data in Documents)
            //string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
            //libraryPath = Path.Combine (documentsPath, "..", "Library");
#endif

#if __LANTERNA__
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }
            Stream stream = assembly.GetManifestResourceStream("Lanterna.Droid.Assets.ui.tgl_portrait_on.png");
            SKManagedStream skStream = new SKManagedStream(stream);
            bm = SKBitmap.Decode(skStream);

            //libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
#endif

#if __IBBASIC__
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }
            Stream stream = assembly.GetManifestResourceStream("IBbasicTest2.Droid.Assets.ui.tgl_portrait_on.png");
            SKManagedStream skStream = new SKManagedStream(stream);
            bm = SKBitmap.Decode(skStream);

            //libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
#endif

            //string path = Path.Combine(libraryPath, filename);
            /*
            #if __IOS__
            var resourcePrefix = "WorkingWithFiles.iOS.";
            #endif
            #if __ANDROID__
                        var resourcePrefix = "WorkingWithFiles.Droid.";
            #endif
            #if WINDOWS_PHONE
            var resourcePrefix = "WorkingWithFiles.WinPhone.";
            #endif

            Debug.WriteLine("Using this resource prefix: " + resourcePrefix);
            // note that the prefix includes the trailing period '.' that is required
            var assembly = typeof(SharedPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(resourcePrefix + "btn_small.png");
            */
        }
        
        public ModuleInfo LoadModuleFileInfo(String filename)
        {
            ModuleInfo toReturn = null;
            try
            {
                //File sdCard = Environment.getExternalStorageDirectory();
                //File directory = new File(sdCard.getAbsolutePath() + "/LanternaEngine/modules");
                //File file = new File(directory, filename);
                //FileInputStream fIn = new FileInputStream(file);

                //BufferedReader r = new BufferedReader(new InputStreamReader(fIn));

                String s = "";
                String keyword = "";
                //moduleBitmapList.clear();
                //ImageData imd;

                for (int i = 0; i < 99999; i++)
                {
                    //s = r.readLine();

                    if (s == null)
                    {
                        break;
                    }
                    else if (s.Equals(""))
                    {
                        continue;
                    }
                    else if (s.Equals("MODULEINFO"))
                    {
                        keyword = "MODULEINFO";
                        continue;
                    }
                    else if (s.Equals("TITLEIMAGE"))
                    {
                        keyword = "TITLEIMAGE";
                        continue;
                    }
                    else if (s.Equals("MODULE"))
                    {
                        keyword = "MODULE";
                        break;
                    }

                    if (keyword.Equals("MODULEINFO"))
                    {
                        //toReturn = (ModuleInfo)JsonConvert.DeserializeObject(s, typeof(ModuleInfo));
                        //GsonBuilder gsonb = new GsonBuilder();
                        //Gson gson = gsonb.create();
                        //toReturn = gson.fromJson(s, new TypeToken<ModuleInfo>() { }.getType());
                    }
                    else if (keyword.Equals("TITLEIMAGE"))
                    {
                        //GsonBuilder gsonb = new GsonBuilder();
                        //Gson gson = gsonb.create();
                        //imd = gson.fromJson(s, new TypeToken<ImageData>() { }.getType());
                        //moduleBitmapList.put(imd.name, gv.bsc.ConvertImageDataToBitmap(imd));
                    }
                }

            }
            catch (Exception ex)
            {
                //gv.errorReport("Failed on loadModuleFileInfo:" + filename);
                //MessageBox.Show("Failed to open ModuleInfo for " + folderAndFilename + ": " + ex.ToString());
            }

            return toReturn;
        }
        
        public void SaveText(string filename, string text)
        {
            //iOS and Android
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            System.IO.File.WriteAllText(filePath, text);
        }
        public string LoadText(string filename)
        {
            //iOS and Android
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            return System.IO.File.ReadAllText(filePath);
        }
        
        /*public string LoadTextAndroid(string filename)
        {
            //Android
            var path = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/LanternaEngine/modules";
            var filePath = Path.Combine(path, filename);
            using (StreamReader sr = File.OpenText(filePath))
            {
                string s = "";
                for (int i = 0; i < 99999; i++)
                {
                    s = sr.ReadLine();
                    if (s == null)
                    {
                        break;
                    }
                }
            }
            using (StreamReader file = File.OpenText("\\default\\NewModule\\data\\" + filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                //toReturn = (ModuleInfo)serializer.Deserialize(file, typeof(ModuleInfo));
            }

            return System.IO.File.ReadAllText(filePath);
        }*/
        
        public void readWriteWin10()
        {
            //Win10
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            if (isoStore.FileExists("TestStore.txt"))
            {
                Console.WriteLine("The file already exists!");
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("TestStore.txt", FileMode.Open, isoStore))
                {
                    using (StreamReader reader = new StreamReader(isoStream))
                    {
                        Console.WriteLine("Reading contents:");
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }
            }
            else
            {
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("TestStore.txt", FileMode.CreateNew, isoStore))
                {
                    using (StreamWriter writer = new StreamWriter(isoStream))
                    {
                        writer.WriteLine("Hello Isolated Storage");
                        Console.WriteLine("You have written to the file.");
                    }
                }
            }
        }
    }
}
