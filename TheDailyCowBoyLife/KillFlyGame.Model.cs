using System;
using System.Drawing;
using System.Drawing.Text;

namespace KillFlyGame.Model
{
    public class FlyModel
    {
        public Point Center { get; set; }
        public int Size { get; set; }
        public bool IsGrowing { get; set; }
        public TimeSpan TimeLeft { get; set; }
        public int Score { get; set; }
        public int MaxSize = 50;
        public int MinSize = 1;
        public Font GameFont { get; set; }
    }
}
