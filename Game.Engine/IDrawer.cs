using System;
using System.Collections.Generic;
using Game.Engine.BridgeObjects;
using Game.Engine.Interfaces;

namespace Game.Engine
{
    public interface IDrawer
    {
        Func<int, int, List<string>> GetAction { get; set; }

        void Clear();

        void DrawHero( Point position, double angle, List<Point> pointList, bool isHorizontal);

        void DrawObject( uint id, long x, long y);

        void DrawSurface(uint p1, uint p2);

        void DrawMenu(int x, int y, IEnumerable<ClientAction> actions);

        void DrawContainer(IEnumerable<KeyValuePair<string, Func<IEnumerable<ClientAction>>>> objects);

        void DrawHeroProperties(IEnumerable<KeyValuePair<string, int>> objects);

        void DrawActing(bool showActing);

        void DrawDayNight(double lightness, GameDateTime gameDateTime, List<BurningProps> lightObjects);
    }
}
