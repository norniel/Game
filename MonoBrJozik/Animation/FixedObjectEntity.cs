using System;
using System.Linq;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Point = Microsoft.Xna.Framework.Point;

namespace MonoBrJozik.Animation
{
    class FixedObjectEntity
    {
        private readonly Texture2D _texture;
        private readonly int _frameCount;
        private readonly Animation _animation;
        private static readonly TimeSpan _t25 = TimeSpan.FromSeconds(.25);
        private readonly int _width = Map.CellMeasure;
        private readonly int _height = Map.CellMeasure;

        public FixedObjectEntity(Texture2D texture, int frameCount)
        {
            _texture = texture;
            _frameCount = _texture.Width/ Map.CellMeasure;

            _animation = new Animation();
            for (var i = 0; i < _frameCount; i++)
            {
                _animation.AddFrame(new Rectangle(_width * i, 0, _width, _texture.Height), _t25);
            }
        }

        public void Draw(SpriteBatch spriteBatch, int tick, Vector2 position)
        {
            spriteBatch.Draw(_texture, new Vector2(position.X, position.Y - _texture.Height + Map.CellMeasure), _animation.CurrentRectangle(tick), Color.White);
        }

        public Point GetSize(int tick)
        {
            return _animation.CurrentRectangle(tick)?.Size ?? new Point(0,0);
        }

        public bool CheckPoint(int tick, Engine.Point destination, Engine.Point objectPoint)
        {
            var currectRect = _animation.CurrentRectangle(tick);

            if (currectRect == null)
                return false;

            var destInTexture = new Point(destination.X - (objectPoint.X + Map.CellMeasure/2 - currectRect.Value.Width/2), 
                            destination.Y - (objectPoint.Y - currectRect.Value.Height + Map.CellMeasure));
            
            if (destInTexture.X >= 0  && destInTexture.Y >= 0 && destInTexture.X < currectRect.Value.Width && destInTexture.Y < currectRect.Value.Height)
                return false;
            
            var buffer = new Color[1];
            _texture.GetData(0, new Rectangle(destInTexture.X + currectRect.Value.X, destInTexture.Y + currectRect.Value.Y, 1, 1), buffer, 0, 1);
            return buffer.Any(c => c != Color.Transparent);

        }
    }
}
