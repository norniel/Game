using System;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
    }
}
