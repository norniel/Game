using System;
using System.Collections.Generic;
using Engine.BridgeObjects;
using Engine.Interfaces;
using Engine.Tools;

namespace Engine
{
    public interface IDrawer
    {
        Func<int, int, List<string>> GetAction { get; set; }

        void Clear();

        void DrawHero( Point position, double angle, List<Point> pointList, bool isHorizontal);

        void DrawObject( uint id, long x, long y, int height);

        void DrawSurface(uint p1, uint p2);

        void DrawMenu(int x, int y, IEnumerable<ClientAction> actions);

        void DrawContainer(IEnumerable<MenuItems> objects);

        void DrawHeroProperties(IEnumerable<KeyValuePair<string, int>> objects);

        void DrawActing(bool showActing);

        void DrawDayNight(double lightness, List<BurningProps> lightObjects);

        void DrawShaddow(Point innerPoint, Size innerSize);

        void DrawTime(GameDateTime gameDateTime);

        void DrawHaltScreen(Dictionary<string, uint> knowledges, Action<Dictionary<string, uint>> newKnowledges);

        void SetPaused(bool isPaused);

        void ShowKnowledges(bool isKnowledgesShown, Dictionary<string, uint> knowledges);

        void DrawKnowledges();
    }
}
