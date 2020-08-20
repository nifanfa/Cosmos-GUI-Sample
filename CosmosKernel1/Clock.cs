using System;
using System.Drawing;

namespace CosmosKernel1
{
    class Clock : App
    {
        public Clock(uint width, uint height, uint x = 0, uint y = 0) : base(width, height, x, y)
        {
            name = "Clock";
        }

        public override void _Update()
        {
            drawHand(Kernel.vMWareSVGAII, (uint)Color.Black.ToArgb(), (int)(x + width / 2), (int)(y + height / 2), DateTime.Now.Hour, 40);
            drawHand(Kernel.vMWareSVGAII, (uint)Color.Black.ToArgb(), (int)(x + width / 2), (int)(y + height / 2), DateTime.Now.Minute, 60);
            drawHand(Kernel.vMWareSVGAII, (uint)Color.Red.ToArgb(), (int)(x + width / 2), (int)(y + height / 2), DateTime.Now.Second, 80);
        }

        void drawHand(DoubleBufferedVMWareSVGAII vMWareSVGAII, uint color, int xStart, int yStart, int angle, int radius)
        {
            int[] sine = new int[16] { 0, 27, 54, 79, 104, 128, 150, 171, 190, 201, 221, 233, 243, 250, 254, 255 };
            int xEnd, yEnd, quadrant, x_flip, y_flip;

            quadrant = angle / 15;

            switch (quadrant)
            {
                case 0: x_flip = 1; y_flip = -1; break;
                case 1: angle = Math.Abs(angle - 30); x_flip = y_flip = 1; break;
                case 2: angle = angle - 30; x_flip = -1; y_flip = 1; break;
                case 3: angle = Math.Abs(angle - 60); x_flip = y_flip = -1; break;
                default: x_flip = y_flip = 1; break;
            }

            xEnd = xStart;
            yEnd = yStart;

            if (angle > sine.Length) return;

            xEnd += (x_flip * ((sine[angle] * radius) >> 8));
            yEnd += (y_flip * ((sine[15 - angle] * radius) >> 8));

            vMWareSVGAII.DoubleBuffer_DrawLine(color, xStart, yStart, xEnd, yEnd);
        }
    }
}
