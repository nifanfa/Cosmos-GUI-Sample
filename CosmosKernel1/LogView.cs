using nifanfa.CosmosDrawString;
using System.Drawing;

namespace CosmosKernel1
{
    class LogView : App
    {
        int textEachLine;
        public string text = string.Empty;

        public LogView(uint width, uint height, uint x = 0, uint y = 0) : base(width, height, x, y)
        {
            //ASC16 = 16*8
            textEachLine = (int)width / 8;
            name = "LogView";
        }

        public override void _Update()
        {
            Kernel.vMWareSVGAII.DoubleBuffer_DrawFillRectangle(x, y, width, height, (uint)Color.Black.ToArgb());

            string s = string.Empty;
            int i = 0;
            foreach (char c in text)
            {
                s += c;
                i++;
                if (i + 1 == textEachLine || c == '\n')
                {
                    if (c != '\n')
                    {
                        s += "\n";
                    }
                    i = 0;
                }
            }

            Kernel.vMWareSVGAII._DrawACSIIString(s, (uint)Color.White.ToArgb(), x, y);
        }
    }
}
