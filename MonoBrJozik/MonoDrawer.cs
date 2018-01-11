using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Engine;
using Engine.BridgeObjects;
using Engine.Interfaces;
using Point = Engine.Point;
namespace MonoBrJozik
{
    class MonoDrawer : IDrawer
    {
        private readonly SpriteBatch _spriteBatch;
        private Texture2D _texture;
        public Func<int, int, List<string>> GetAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public MonoDrawer(SpriteBatch spriteBatch, Texture2D texture)
        {
            _spriteBatch = spriteBatch;
            _texture = texture;
        }

        public void Clear()
        {}

        public void DrawActing(bool showActing)
        {
            //throw new NotImplementedException();
        }

        public void DrawContainer(IEnumerable<MenuItems> objects)
        {
            //throw new NotImplementedException();
        }

        public void DrawDayNight(double lightness, GameDateTime gameDateTime, List<BurningProps> lightObjects)
        {
            //throw new NotImplementedException();
        }

        public void DrawHero(Point position, double angle, List<Point> pointList, bool isHorizontal)
        {
            //throw new NotImplementedException();
        }

        public void DrawHeroProperties(IEnumerable<KeyValuePair<string, int>> objects)
        {
            //throw new NotImplementedException();
        }

        public void DrawMenu(int x, int y, IEnumerable<ClientAction> actions)
        {
            //throw new NotImplementedException();
        }

        public void DrawObject(uint id, long x, long y)
        {
            _spriteBatch.Draw(_texture, new Vector2(x, y), Color.White);
        }

        public void DrawShaddow(Point innerPoint, Size innerSize)
        {
            //throw new NotImplementedException();
        }

        public void DrawSurface(uint p1, uint p2)
        {
            //throw new NotImplementedException();
        }
    }
}
