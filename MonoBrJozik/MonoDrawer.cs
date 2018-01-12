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
        private Dictionary<uint, Texture2D> _textures;
        public Func<int, int, List<string>> GetAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public MonoDrawer(SpriteBatch spriteBatch, Dictionary<uint, Texture2D> textures)
        {
            _spriteBatch = spriteBatch;
            _textures = textures;
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
            Texture2D texture = null;
            if (_textures.TryGetValue(id, out texture))
            {
                _spriteBatch.Draw(texture, new Vector2(x, y), Color.White);
                return;
            }
            /*
            if (id == 0x00002000)
            {
                // _canvas.Children
                Rectangle rec = new Rectangle() { Fill = Brushes.DarkBlue, Stroke = Brushes.DarkBlue, Height = 20, Width = 20 };
                _canvas.Children.Add(rec);
                Canvas.SetLeft(rec, x);
                Canvas.SetTop(rec, y);

            }
            else if (id == 0x00002100)
            {
                // _canvas.Children
                Rectangle rec = new Rectangle() { Fill = Brushes.Blue, Stroke = Brushes.Blue, Height = 20, Width = 20 };
                _canvas.Children.Add(rec);
                Canvas.SetLeft(rec, x);
                Canvas.SetTop(rec, y);
            }*/
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
