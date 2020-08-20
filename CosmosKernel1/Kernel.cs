using Cosmos.Core;
using Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using System;
using System.Drawing;
using Sys = Cosmos.System;

namespace CosmosKernel1
{
    public class Kernel : Sys.Kernel
    {
        uint screenWidth = 640;
        uint screenHeight = 480;
        public static DoubleBufferedVMWareSVGAII vMWareSVGAII;
        Bitmap bitmap;

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
        public static bool Pressed;

        protected override void BeforeRun()
        {
            CosmosVFS cosmosVFS = new CosmosVFS();
            VFSManager.RegisterVFS(cosmosVFS);

            bitmap = new Bitmap(@"0:\timg.bmp");

            vMWareSVGAII = new DoubleBufferedVMWareSVGAII();
            vMWareSVGAII.SetMode(screenWidth, screenHeight);
            MouseManager.ScreenWidth = screenWidth;
            MouseManager.ScreenHeight = screenHeight;
            MouseManager.X = screenWidth / 2;
            MouseManager.Y = screenHeight / 2;
            logView = new LogView(300, 200, 10, 10);
            Clock = new Clock(200, 200, 400, 200);
            notepad = new Notepad(200, 100, 10, 300);
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
            vMWareSVGAII.DoubleBuffer_SetVRAM(bitmap.rawData);
            logView.text = $"Time: {DateTime.Now}\nInstall RAM: {CPU.GetAmountOfRAM()}MB, Video RAM: {vMWareSVGAII.Video_Memory.Size}Bytes";
            logView.Update();
            Clock.Update();
            notepad.Update();
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
