using Cosmos.Core;
using Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using Sys = Cosmos.System;

namespace CosmosKernel1
{
    public class Kernel : Sys.Kernel
    {
        public static uint screenWidth = 640;
        public static uint screenHeight = 480;
        public static DoubleBufferedVMWareSVGAII vMWareSVGAII;
        Bitmap bitmap;
        public static Bitmap programlogo;
        Bitmap bootBitmap;

        int[] cursor = new int[]
            {
                1,0,0,0,0,0,0,0,0,0,0,0,
                1,1,0,0,0,0,0,0,0,0,0,0,
                1,2,1,0,0,0,0,0,0,0,0,0,
                1,2,2,1,0,0,0,0,0,0,0,0,
                1,2,2,2,1,0,0,0,0,0,0,0,
                1,2,2,2,2,1,0,0,0,0,0,0,
                1,2,2,2,2,2,1,0,0,0,0,0,
                1,2,2,2,2,2,2,1,0,0,0,0,
                1,2,2,2,2,2,2,2,1,0,0,0,
                1,2,2,2,2,2,2,2,2,1,0,0,
                1,2,2,2,2,2,2,2,2,2,1,0,
                1,2,2,2,2,2,2,2,2,2,2,1,
                1,2,2,2,2,2,2,1,1,1,1,1,
                1,2,2,2,1,2,2,1,0,0,0,0,
                1,2,2,1,0,1,2,2,1,0,0,0,
                1,2,1,0,0,1,2,2,1,0,0,0,
                1,1,0,0,0,0,1,2,2,1,0,0,
                0,0,0,0,0,0,1,2,2,1,0,0,
                0,0,0,0,0,0,0,1,1,0,0,0
            };

        LogView logView;
        Clock Clock;
        Notepad notepad;
        Dock dock;
        public static bool Pressed;

        public static List<App> apps = new List<App>();

        public static Color avgCol;

        protected override void BeforeRun()
        {
            CosmosVFS cosmosVFS = new CosmosVFS();
            VFSManager.RegisterVFS(cosmosVFS);

            bootBitmap = new Bitmap(@"0:\boot.bmp");

            vMWareSVGAII = new DoubleBufferedVMWareSVGAII();
            vMWareSVGAII.SetMode(screenWidth, screenHeight);

            vMWareSVGAII.DoubleBuffer_DrawImage(bootBitmap, 640 / 4, 0);
            vMWareSVGAII.DoubleBuffer_Update();

            bitmap = new Bitmap(@"0:\timg.bmp");
            programlogo = new Bitmap(@"0:\program.bmp");

            uint r = 0;
            uint g = 0;
            uint b = 0;
            for (uint i = 0; i < bitmap.rawData.Length; i++)
            {
                Color color = Color.FromArgb(bitmap.rawData[i]);
                r += color.R;
                g += color.G;
                b += color.B;
            }
            avgCol = Color.FromArgb((int)(r / bitmap.rawData.Length), (int)(g / bitmap.rawData.Length), (int)(b / bitmap.rawData.Length));

            MouseManager.ScreenWidth = screenWidth;
            MouseManager.ScreenHeight = screenHeight;
            MouseManager.X = screenWidth / 2;
            MouseManager.Y = screenHeight / 2;

            logView = new LogView(300, 200, 10, 30);
            Clock = new Clock(200, 200, 400, 200);
            notepad = new Notepad(200, 100, 10, 300);
            dock = new Dock();

            apps.Add(logView);
            apps.Add(Clock);
            apps.Add(notepad);
        }

        protected override void Run()
        {
            switch (MouseManager.MouseState)
            {
                case MouseState.Left:
                    Pressed = true;
                    break;
                case MouseState.None:
                    Pressed = false;
                    break;
            }

            vMWareSVGAII.DoubleBuffer_Clear((uint)Color.Black.ToArgb());
            vMWareSVGAII.DoubleBuffer_SetVRAM(bitmap.rawData, (int)vMWareSVGAII.FrameSize);
            logView.text = $"Time: {DateTime.Now}\nInstall RAM: {CPU.GetAmountOfRAM()}MB, Video RAM: {vMWareSVGAII.Video_Memory.Size}Bytes";
            foreach (App app in apps)
                app.Update();

            dock.Update();

            DrawCursor(vMWareSVGAII, MouseManager.X, MouseManager.Y);

            vMWareSVGAII.DoubleBuffer_Update();
        }

        public void DrawCursor(DoubleBufferedVMWareSVGAII vMWareSVGAII, uint x, uint y)
        {
            for (uint h = 0; h < 19; h++)
            {
                for (uint w = 0; w < 12; w++)
                {
                    if (cursor[h * 12 + w] == 1)
                    {
                        vMWareSVGAII.DoubleBuffer_SetPixel(w + x, h + y, (uint)Color.Black.ToArgb());
                    }
                    if (cursor[h * 12 + w] == 2)
                    {
                        vMWareSVGAII.DoubleBuffer_SetPixel(w + x, h + y, (uint)Color.White.ToArgb());
                    }
                }
            }
        }
    }
}
