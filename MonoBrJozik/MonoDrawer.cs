using System;
using System.Collections.Generic;
using System.Linq;
using Engine;
using Engine.BridgeObjects;
using Engine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoBrJozik.Controls;
using MoreLinq;
using Point = Engine.Point;

namespace MonoBrJozik
{
    internal class MonoDrawer : IDrawer
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Dictionary<uint, Texture2D> _textures;
        private readonly Dictionary<string, Texture2D> _heroPropTextures;

        public Func<int, int, List<string>> GetAction
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        private int _drawCount;
        private string _actingString = string.Empty;
        private readonly Texture2D _heroTexture;
        private readonly Texture2D _screenTexture;
        private readonly Texture2D _menuTexture;
        private readonly SpriteFont _font;
        private readonly MonoMenu _menu;
        private readonly MonoInventory _inventory;
        
        private readonly int _dcenter;
        private readonly GraphicsDevice _graphicsDevice;

        private readonly MonoSwich _pauseSwich;
        private readonly MonoSwich _knowledgesSwich;

        private readonly MonoKnowledges _monoKnowledges;

        private int _tick = 0;

        public const int SCREEN_WIDTH = 564;
        public const int SCREEN_HEIGHT = 394;
        public const int HEALTH_BAR_HEIGHT = 45;
        public const int INVENTORY_WIDTH = 100;

        public MonoDrawer(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Dictionary<uint, Texture2D> textures, Texture2D heroTexture,
            Texture2D screenTexture, Dictionary<string, Texture2D> heroPropTextures, SpriteFont font, Controls.MonoMenu menu, Controls.MonoInventory inventory, MonoSwich pauseSwich, MonoSwich knowledgesSwich)
        {
            _spriteBatch = spriteBatch;
            _textures = textures;
            _heroTexture = heroTexture;
            _screenTexture = screenTexture;
            _graphicsDevice = graphicsDevice;
            _heroPropTextures = heroPropTextures;
            _font = font;
            _menu = menu;
            _inventory = inventory;

            _menuTexture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] c = new Color[1];
            c[0] = Color.White;
            _menuTexture.SetData<Color>(c);

            _pauseSwich = pauseSwich;
            _knowledgesSwich = knowledgesSwich;

            _monoKnowledges = new MonoKnowledges(graphicsDevice, _font);
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
                    _actingString = _actingString.Length == 9 ? "Acting."
                        : _actingString.Length == 8 ? "Acting..." : "Acting..";
                }

                _spriteBatch.DrawString(_font, _actingString, new Vector2(SCREEN_WIDTH/2, SCREEN_HEIGHT/2), Color.White);
            }
        }

        public void DrawContainer(IEnumerable<MenuItems> objects)
        {
            _inventory.SetItems(objects.Select(it => 
            {
                Texture2D texture = null;
                _textures.TryGetValue(it.Id, out texture);
                var infoList = it.GetClientActions().Select(act => new MonoItemInfo(null, null, act.Name, () => act.Do())).ToList();

                return new MonoInvItemInfo(null, texture, it.Name, null, infoList);
            }).ToList());
        }

        public void DrawDayNight(double lightness, List<BurningProps> lightObjects)
        {
            //You can probably turn this in to a re-useable method
            Byte transparency_amount = (Byte)(255 * lightness); //0 transparent; 255 opaque
            Texture2D texture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] c = new Color[1];
            c[0] = Color.FromNonPremultiplied(255, 255, 255, transparency_amount);
            texture.SetData<Color>(c);

            _spriteBatch.Draw(texture, new Rectangle(0, 0, 564, 394), Color.Black);
        }

        public void DrawHero(Point position, double angle, List<Point> pointList, bool isHorizontal)
        {
            _spriteBatch.Draw(_heroTexture, new Vector2(position.X - _dcenter, position.Y - _dcenter), null, Color.White, (float)(angle - (Math.PI*0.25)), new Vector2(_heroTexture.Width / 2, _heroTexture.Height / 2), Vector2.One, SpriteEffects.None, 0);

            DrawPath(position, pointList);
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

        private void DrawPath(Point position, List<Point> pointList)
        {
            var pointCount = pointList.Count + 1;

            if (pointCount <= 1)
                return;

            var primitiveList = position.Concat(pointList)
                .Select(p => new VertexPositionColor(new Vector3(p.X, p.Y, 0), Color.White))
                .ToArray();

            var lineListIndices = new short[(pointCount * 2) - 2];

            // Populate the array with references to indices in the vertex buffer
            for (int i = 0; i < pointCount - 1; i++)
            {
                lineListIndices[i * 2] = (short) (i);
                lineListIndices[(i * 2) + 1] = (short) (i + 1);
            }

            _graphicsDevice.DrawUserIndexedPrimitives(
                PrimitiveType.LineList,
                primitiveList,
                0, // vertex buffer offset to add to each element of the index buffer
                pointCount, // number of vertices in pointList
                lineListIndices, // the index buffer
                0, // first index element to read
                pointCount - 1 // number of primitives to draw
            );
        }

        public void DrawHeroProperties(IEnumerable<KeyValuePair<string, int>> objects)
        {
            _tick++;
            Texture2D texture;
            var i = 0;
            foreach (var heroProp in objects)
            {
                if (_heroPropTextures.TryGetValue(heroProp.Key, out texture))
                {
                    if (heroProp.Key.Equals("health", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var health = heroProp.Value;
                        var t = _tick % ((int)((health+10)/10)*10) < 5 ? 1 : 0; 
                        _spriteBatch.Draw(texture, new Rectangle(2 + 70 * i + t, SCREEN_HEIGHT + 2 + t, texture.Width - 2*t, texture.Height - 2*t ), Color.White);
                    }
                    else
                    _spriteBatch.Draw(texture, new Vector2(2 + 70*i, SCREEN_HEIGHT + 2), Color.White);
                    
                }
                _spriteBatch.DrawString(_font, $"({heroProp.Value})", new Vector2(2 + 70 * i+ 33, SCREEN_HEIGHT + 20), Color.Black);
                    i++;
            }
        }

        public void DrawMenu(int x, int y, IEnumerable<ClientAction> actions)
        {
            Texture2D texture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] c = new Color[1];
            c[0] = Color.LightBlue;
            texture.SetData<Color>(c);

            var infoList = actions.Select(act => new MonoItemInfo(null, null, act.Name,() => act.Do())).ToList();

            _menu.Show(infoList, x, y);
        }

        public void DrawObject(uint id, long x, long y, int height)
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
                if (height > 1)
                    y = y - Engine.Map.CellMeasure*(height - 1);

                _spriteBatch.Draw(texture, new Vector2(x, y), Color.White);
            }
        }

        public void DrawShaddow(Point innerPoint, Size innerSize)
        {
            //throw new NotImplementedException();
        }

        public void DrawSurface(uint p1, uint p2)
        {
               Texture2D texture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
               Color[] c = new Color[1];
               c[0] = new Color(23, 90, 0); 
               texture.SetData<Color>(c);//_screenTexture
            _spriteBatch.Draw(texture, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);
        }

        public void DrawTime(GameDateTime gameDateTime)
        {
            var timeOfDay = $"{gameDateTime.Day}:{gameDateTime.Hour}:{gameDateTime.Minute}";
            var timeStrLength = _font.MeasureString(timeOfDay);
            _spriteBatch.DrawString(_font, timeOfDay, new Vector2(SCREEN_WIDTH - timeStrLength.X - 2, SCREEN_HEIGHT + 30), Color.Black);
        }

        public void DrawHaltScreen(Dictionary<string, uint> knowledges, Action<Dictionary<string, uint>> newKnowledgesAction)
        {
            if (_monoKnowledges.IsHaltShown)
            {
                _monoKnowledges.Draw(_spriteBatch);
                return;
            }

            _monoKnowledges.Init(true, knowledges, true, newKnowledgesAction);
            _monoKnowledges.Draw(_spriteBatch);
        }

        public void SetPaused(bool isPaused)
        {
            _pauseSwich.SetSwitched(isPaused);
        }

        public void ShowKnowledges(bool isKnowledgesShown, Dictionary<string, uint> knowledges)
        {
            _knowledgesSwich.SetSwitched(isKnowledgesShown);
            _monoKnowledges.Init(isKnowledgesShown, knowledges);
        }

        public void DrawKnowledges()
        {
            _monoKnowledges.Draw(_spriteBatch);
        }

        private void DrawRotatedImage(Texture2D texture, long x, long y, uint angle)
        {
            var angleInRads = (float)(((float)angle - 90) / 180f * Math.PI);
            _spriteBatch.Draw(texture, new Vector2(x + texture.Width / 2, y + texture.Height / 2), null, Color.White, angleInRads, new Vector2(texture.Width / 2, texture.Height / 2), Vector2.One, SpriteEffects.None, 0);
        }

        public bool MouseLClick(MouseState mouseState)
        {
            return _monoKnowledges.MouseLClick(mouseState);
        }
    }
}
