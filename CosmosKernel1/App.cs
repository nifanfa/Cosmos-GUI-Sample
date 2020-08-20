using Cosmos.System;
using nifanfa.CosmosDrawString;
using System.Drawing;

namespace CosmosKernel1
{
    class App
    {
        public readonly uint _width;
        public readonly uint _height;
        public readonly uint width;
        public readonly uint height;

        public uint _x;
        public uint _y;
        public uint x;
        public uint y;
        public string name;

        bool pressed;

        public App(uint width, uint height, uint x = 0, uint y = 0)
        {
            this._width = width;
            this._height = height;
            this._x = x;
            this._y = y;

            this.x = x + 2;
            this.y = y + 22;
            this.width = width - 4;
            this.height = height - 22 - 1;
        }

        public void Update()
        {
            if (Kernel.Pressed)
            {
                if (MouseManager.X > _x && MouseManager.X < _x + 22 && MouseManager.Y > _y && MouseManager.Y < _y + 22)
                {
                    this.pressed = true;
                }
            }
            else
            {
                this.pressed = false;
            }

            if (this.pressed)
            {
                this._x = MouseManager.X;
                this._y = MouseManager.Y;

                this.x = MouseManager.X + 2;
                this.y = MouseManager.Y + 22;
            }

            Kernel.vMWareSVGAII.DoubleBuffer_DrawFillRectangle(_x, _y, _width, _height, (uint)Color.FromArgb(200, 200, 200).ToArgb());
            Kernel.vMWareSVGAII.DoubleBuffer_DrawFillRectangle(_x + 1, _y + 1, _width - 2, 20, (uint)Color.FromArgb(0, 0, 135).ToArgb());
            Kernel.vMWareSVGAII._DrawACSIIString(name, (uint)Color.White.ToArgb(), _x + 2, _y + 2);
            //Kernel.vMWareSVGAII.DoubleBuffer_DrawFillRectangle(_x + 22, _y, 1, 22, (uint)Color.FromArgb(200, 200, 200).ToArgb());
            //Kernel.vMWareSVGAII.DoubleBuffer_DrawFillRectangle(x, y, 20, 20, (uint)Color.Gray.ToArgb());
            _Update();
        }

        public virtual void _Update()
        {
        }
    }
}
