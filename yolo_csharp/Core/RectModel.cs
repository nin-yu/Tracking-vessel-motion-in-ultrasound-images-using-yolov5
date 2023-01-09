using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yolo_csharp.Core
{
    public class RectModel
    {

        private int top;
        private int bottom;
        private int left;
        private int right;

        public int Top { get => top; set => top = value; }
        public int Bottom { get => bottom; set => bottom = value; }
        public int Left { get => left; set => left = value; }
        public int Right { get => right; set => right = value; }
    }
}
