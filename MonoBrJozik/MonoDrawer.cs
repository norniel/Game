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
        private readonly Dictionary<uint, Texture2D> _textures;
        public Func<int, int, List<string>> GetAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        private int _drawCount = 0;
        string _actingString = string.Empty;
        readonly Texture2D _heroTexture;
        private readonly int _dcenter;

        public MonoDrawer(SpriteBatch spriteBatch, Dictionary<uint, Texture2D> textures, Texture2D heroTexture)
        {
            _spriteBatch = spriteBatch;
            _textures = textures;
            _heroTexture = heroTexture;
        }

        public void Clear()
        {}

        public void DrawActing(bool showActing)
        {            
            if (showActing)
            {
                _drawCount = (_drawCount + 1) % 20;
                if (_drawCount == 0)
                {
                    _actingString = _actingString.Length == 9 ? "Acting." : _actingString.Length == 8 ? "Acting..." : "Acting..";
                }

               // _spriteBatch.DrawString()
            }
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
            _spriteBatch.Draw(_heroTexture, new Vector2(position.X - _dcenter, position.Y - _dcenter), Color.White);


            /*
            _canvas.Children.Add(_visibleWay);
            _visWayCollection.Clear();

            System.Windows.Point point = new System.Windows.Point(position.X, position.Y);
            _visWayCollection.Add(point);
            foreach (var wayPoint in wayList)
            {
                point = new System.Windows.Point(wayPoint.X, wayPoint.Y);
                _visWayCollection.Add(point);
            }
            if (isHorizontal)
            {
                _canvas.Children.Add(_horizontalAppearance);
                Canvas.SetLeft(_horizontalAppearance, position.X - _dcenter);
                Canvas.SetTop(_horizontalAppearance, position.Y - _dcenter);
            }
            else
            {
                _canvas.Children.Add(_appearance);
                Canvas.SetLeft(_appearance, position.X - _dcenter);
                Canvas.SetTop(_appearance, position.Y - _dcenter);
                _t.Angle = angle;
            }*/
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

            var origId = (id / 0x1000) * 0x1000;
            if (origId == 0x00018000 || origId == 0x10018000)
            {
                if (!_textures.TryGetValue(origId, out texture))
                    return; 

                DrawRotatedImage(texture, x, y, id % 0x1000);
                return;
            }
            
            if (_textures.TryGetValue(id, out texture))
            {

                _spriteBatch.Draw(texture, new Vector2(x, y), Color.White);
                return;
            }
        }

        public void DrawShaddow(Point innerPoint, Size innerSize)
        {
            //throw new NotImplementedException();
        }

        public void DrawSurface(uint p1, uint p2)
        {
            //throw new NotImplementedException();
        }

        private void DrawRotatedImage(Texture2D texture, long x, long y, uint angle)
        {
            var angleInRads = (float)(((float)angle - 90) / 180f * Math.PI);
            _spriteBatch.Draw(texture, new Vector2(x + texture.Width / 2, y + texture.Height / 2), null, Color.White, angleInRads, new Vector2(texture.Width / 2, texture.Height / 2), Vector2.One, SpriteEffects.None, 0);
        }

        private void DrawHeroVisualWay(List<Point> pointList)
        {
        }
    }
}
