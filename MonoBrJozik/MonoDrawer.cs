using System;
using System.Collections.Generic;
using System.Linq;
using Engine;
using Engine.BridgeObjects;
using Engine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBrJozik.Animation;
using MonoBrJozik.Controls;
using Point = Engine.Point;

namespace MonoBrJozik
{
    internal class MonoDrawer : IDrawer
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Dictionary<uint, Texture2D> _textures;
        private readonly Dictionary<uint, FixedObjectEntity> _animations;
        private readonly Dictionary<string, Texture2D> _heroPropTextures;

        public Func<int, int, List<string>> GetAction
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        private int _drawCount;
        private string _actingString = string.Empty;
        private readonly Texture2D _heroTexture;
        private readonly SpriteFont _font;
        private readonly MonoMenu _menu;
        private readonly MonoInventory _inventory;
        
        private readonly int _dcenter;
        private readonly GraphicsDevice _graphicsDevice;

        private readonly MonoSwitch _pauseSwitch;
        private readonly MonoSwitch _knowledgesSwitch;

        private readonly MonoKnowledges _monoKnowledges;
        private readonly MonoKnowledgesSimple _monoKnowledgesSimple;

        private readonly CharacterEntity _heroCharacterEntity;
        private readonly CharacterEntity _foxCharacterEntity;

        private int _tick;
        private CharacterEntity _hareCharacterEntity;

        public int MaxTextureWidth { get; private set; } = 0;
        public int MaxTextureHeight { get; private set; } = 0;

        public const int ScreenWidth = 564;
        public const int ScreenHeight = 394;
        public const int HealthBarHeight = 45;
        public const int InventoryWidth = 100;

        public MonoDrawer(
            SpriteBatch spriteBatch, 
            GraphicsDevice graphicsDevice, 
            Dictionary<uint, Texture2D> textures, 
            Texture2D heroTexture, 
            Dictionary<string, Texture2D> heroPropTextures, 
            SpriteFont font, 
            MonoMenu menu, 
            MonoInventory inventory, 
            MonoSwitch pauseSwitch, 
            MonoSwitch knowledgesSwitch, 
            MonoKnowledges knowledges)
        {
            _spriteBatch = spriteBatch;
             var tmptextures = textures;
            _heroTexture = heroTexture;
            _graphicsDevice = graphicsDevice;
            _heroPropTextures = heroPropTextures;
            _font = font;
            _menu = menu;
            _inventory = inventory;

            var menuTexture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            var c = new Color[1];
            c[0] = Color.White;
            menuTexture.SetData(c);

            var foxTexture = tmptextures[0x00020000];
            tmptextures.Remove(0x00020000);
            var hareTexture = tmptextures[0x00019000];
            tmptextures.Remove(0x00019000);
            _heroCharacterEntity = new CharacterEntity(_heroTexture, 8);
            _foxCharacterEntity = new CharacterEntity(foxTexture, 4);
            _hareCharacterEntity = new CharacterEntity(hareTexture, 8);

            _textures = new Dictionary<uint, Texture2D>();
            _animations = new Dictionary<uint, FixedObjectEntity>();

            foreach (var tmptexture in tmptextures)
            {
                if (tmptexture.Value.Width >= Map.CellMeasure*2)
                {
                    _animations[tmptexture.Key] = new FixedObjectEntity(tmptexture.Value, tmptexture.Value.Width/Map.CellMeasure);
                }
                else
                {
                    _textures[tmptexture.Key] = tmptexture.Value;
                }
            }

            _pauseSwitch = pauseSwitch;
            _knowledgesSwitch = knowledgesSwitch;

            _monoKnowledges = knowledges;

            _monoKnowledgesSimple = new MonoKnowledgesSimple(_graphicsDevice, _font);
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

                _spriteBatch.DrawString(_font, _actingString, new Vector2(ScreenWidth/2f, ScreenHeight/2f), Color.White);
            }
        }

        public void DrawContainer(IEnumerable<MenuItems> objects)
        {
            _inventory.SetItems(objects.Select(it => 
            {
                _textures.TryGetValue(it.Id, out var texture);
                var infoList = it.GetClientActions().Select(act => new MonoItemInfo(null, null, act.Name, () => act.Do())).ToList();

                return new MonoInvItemInfo(null, texture, it.Name, null, infoList);
            }).ToList());
        }

        public void DrawDayNight(double lightness, List<BurningProps> lightObjects)
        {
            //You can probably turn this in to a re-useable method
            Byte transparencyAmount = (Byte)(255 * lightness); //0 transparent; 255 opaque
            Texture2D texture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] c = new Color[1];
            c[0] = Color.FromNonPremultiplied(255, 255, 255, transparencyAmount);
            texture.SetData(c);

            _spriteBatch.Draw(texture, new Rectangle(0, 0, 564, 394), Color.Black);
        }

        public void DrawHero(Point position, double angle, List<Point> pointList, bool isMoving, bool isHorizontal)
        {
 /*           _spriteBatch.Draw(_heroTexture, 
                new Vector2(position.X - _dcenter, position.Y - _dcenter), 
                null, 
                Color.White, 
                (float)(angle - Math.PI*0.25), 
                new Vector2(_heroTexture.Width / 2f, _heroTexture.Height / 2f), 
                Vector2.One, 
                SpriteEffects.None, 
                0);
*/
            _heroCharacterEntity.Draw(_spriteBatch, _tick, new Vector2(position.X, position.Y), angle, isMoving, isHorizontal);

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

        public void DrawMobileObject(uint id, Point position, double angle, bool isMoving)
        {
            var origId = id / 0x1000 * 0x1000;
            if (origId == 0x00018000 || origId == 0x10018000 )
            {
                Texture2D texture = null;
                if (!_textures.TryGetValue(origId, out texture))
                    return;

                DrawRotatedImage(texture, position.X, position.Y, id % 0x1000);
                return;
            }
            else if (origId == 0x00020000)
            {
                _foxCharacterEntity.Draw(_spriteBatch, _tick, new Vector2(position.X, position.Y), angle, isMoving, false);
            }
            else if (origId == 0x00019000)
            {
                _hareCharacterEntity.Draw(_spriteBatch, _tick, new Vector2(position.X, position.Y), angle, isMoving, false);
            }
        }

        private void DrawPath(Point position, List<Point> pointList)
        {
            var pointCount = pointList.Count + 1;

            if (pointCount <= 1)
                return;

            var primitiveList = new List<Point>{position}.Concat(pointList)
                .Select(p => new VertexPositionColor(new Vector3(p.X, p.Y, 0), Color.White))
                .ToArray();

            var lineListIndices = new short[pointCount * 2 - 2];

            // Populate the array with references to indices in the vertex buffer
            for (int i = 0; i < pointCount - 1; i++)
            {
                lineListIndices[i * 2] = (short) i;
                lineListIndices[i * 2 + 1] = (short) (i + 1);
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
            var i = 0;
            foreach (var heroProp in objects)
            {
                if (_heroPropTextures.TryGetValue(heroProp.Key, out var texture))
                {
                    if (heroProp.Key.Equals("health", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var health = heroProp.Value;
                        var healthKoef = (health+10)/10*10;
                        var t = _tick % (healthKoef <= 0 ? 10 : healthKoef) < 5 ? 1 : 0; 
                        _spriteBatch.Draw(texture, new Rectangle(2 + 70 * i + t, ScreenHeight + 2 + t, texture.Width - 2*t, texture.Height - 2*t ), Color.White);
                    }
                    else
                    _spriteBatch.Draw(texture, new Vector2(2 + 70*i, ScreenHeight + 2), Color.White);
                    
                }
                _spriteBatch.DrawString(_font, $"({heroProp.Value})", new Vector2(2 + 70 * i+ 33, ScreenHeight + 20), Color.Black);
                    i++;
            }
        }

        public void DrawMenu(int x, int y, IEnumerable<ClientAction> actions)
        {
            Texture2D texture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] c = new Color[1];
            c[0] = Color.LightBlue;
            texture.SetData(c);

            var infoList = actions.Select(act => new MonoItemInfo(null, null, act.Name,() => act.Do())).ToList();

            _menu.Show(infoList, x, y);
        }

        public void DrawObject(uint id, long x, long y, int height)
        {
            if (_animations.TryGetValue(id, out var fixedObjectEntity))
            {
                fixedObjectEntity.Draw(_spriteBatch, _tick, new Vector2(x, y));
                var size = fixedObjectEntity.GetSize(_tick);
                MaxTextureWidth = Math.Max(MaxTextureWidth, size.X/2);
                MaxTextureHeight = Math.Max(MaxTextureHeight, size.Y);
            }

            if (_textures.TryGetValue(id, out var texture))
            {
                y = y - texture.Height + Map.CellMeasure;
                x = texture.Width <= Map.CellMeasure ? x : x - (texture.Width - Map.CellMeasure) / 2;

                _spriteBatch.Draw(texture, new Vector2(x, y), Color.White);
                MaxTextureWidth = Math.Max(MaxTextureWidth, texture.Width/2);
                MaxTextureHeight = Math.Max(MaxTextureHeight, texture.Height);
            }
        }

        public void DrawShadow(Point innerPoint, Size innerSize)
        {
            //throw new NotImplementedException();
        }

        public void DrawSurface(uint p1, uint p2)
        {
               Texture2D texture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
               Color[] c = new Color[1];
               c[0] = new Color(23, 90, 0); 
               texture.SetData(c);//_screenTexture
            _spriteBatch.Draw(texture, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
        }

        public void DrawTime(GameDateTime gameDateTime)
        {
            var timeOfDay = $"{gameDateTime.Day}:{gameDateTime.Hour}:{gameDateTime.Minute}";
            var timeStrLength = _font.MeasureString(timeOfDay);
            _spriteBatch.DrawString(_font, timeOfDay, new Vector2(ScreenWidth - timeStrLength.X - 2, ScreenHeight + 30), Color.Black);
        }

        public void DrawHaltScreen(Dictionary<string, uint> knowledges, Action<Dictionary<string, uint>> newKnowledgesAction)
        {
            if (_monoKnowledges.IsVisible)
            {
                _monoKnowledges.Draw(_spriteBatch);
                return;
            }

            _monoKnowledges.Init(true, knowledges, newKnowledgesAction);
            _monoKnowledges.Draw(_spriteBatch);
        }

        public void SetPaused(bool isPaused)
        {
            _pauseSwitch.SetSwitched(isPaused);
        }

        public void ShowKnowledges(bool isKnowledgesShown, Dictionary<string, uint> knowledges)
        {
            _knowledgesSwitch.SetSwitched(isKnowledgesShown);
            _monoKnowledgesSimple.Init(isKnowledgesShown, knowledges);
        }

        public void DrawKnowledges()
        {
            _monoKnowledgesSimple.Draw(_spriteBatch);
        }

        public bool CheckPointInObject(uint id, Point destination, Point objectPoint)
        {
            if (_animations.TryGetValue(id, out var fixedObjectEntity))
            {
                return fixedObjectEntity.CheckPoint(_tick, destination, objectPoint);
            }
            
            if (!_textures.TryGetValue(id, out var texture))
                return false;

            var destInTexture = new Microsoft.Xna.Framework.Point(destination.X - (objectPoint.X + Map.CellMeasure/2 - texture.Width/2), 
                destination.Y - (objectPoint.Y - texture.Height + Map.CellMeasure));

            if (!texture.Bounds.Contains(destInTexture))
                return false;
            
            var buffer = new Color[1];
            texture.GetData(0, new Rectangle(destInTexture.X, destInTexture.Y, 1, 1), buffer, 0, 1);
            return buffer.Any(c => c != Color.Transparent);
        }

        private void DrawRotatedImage(Texture2D texture, long x, long y, uint angle)
        {
            var angleInRads = (float)(((float)angle - 90) / 180f * Math.PI);
            _spriteBatch.Draw(texture, new Vector2(x + texture.Width / 2, y + texture.Height / 2), null, Color.White, angleInRads, new Vector2(texture.Width / 2, texture.Height / 2), Vector2.One, SpriteEffects.None, 0);
        }
    }
}
