using System;
using System.Collections.Generic;

namespace Game.Engine
{
    public interface IDrawer
    {
        Func<int, int, List<string>> GetAction { get; set; }

        void Clear();

        void DrawHero( Point position, double angle, List<WeakReference> pointList);

        void DrawObject( uint id, long x, long y);

        void DrawSurface(uint p1, uint p2);

        void DrawMenu(int x, int y, IEnumerable<string> actions);

        void DrawContainer(IEnumerable<string> objects );
    }
}
