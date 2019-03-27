using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoBrJozik.Animation
{
    class CharacterEntity
    {
        private readonly Texture2D _texture;
        private readonly int _frameCount;
        private readonly Animation[] _walkAnimations = new Animation[8];
        private readonly Animation[] _standAnimations = new Animation[8];
        private static readonly TimeSpan _t25 = TimeSpan.FromSeconds(.25);
        private readonly int _width = 32;
        private readonly int _height = 32;

        public CharacterEntity(Texture2D texture, int frameCount)
      {
          _texture = texture;
          _frameCount = frameCount;

          var walkTopRight = InitFrame(7);
          _walkAnimations[0] = walkTopRight;

          var walkRight = InitFrame(3);
          _walkAnimations[1] = walkRight;

          var walkBottomRight = InitFrame(4);
          _walkAnimations[2] = walkBottomRight;

          var walkBottom = InitFrame(1);
          _walkAnimations[3] = walkBottom;

          var walkBottomLeft = InitFrame(5);
          _walkAnimations[4] = walkBottomLeft;

          var walkLeft = InitFrame(2);
          _walkAnimations[5] = walkLeft;

          var walkTopLeft = InitFrame(6);
          _walkAnimations[6] = walkTopLeft;

          var walkTop = InitFrame(0);
          _walkAnimations[7] = walkTop;

          // Standing animations only have a single frame of animation:
          var standTopRight = new Animation();
          standTopRight.AddFrame(new Rectangle(0, 224, _width, _height), _t25);
          _standAnimations[0] = standTopRight;

          var standRight = new Animation();
          standRight.AddFrame(new Rectangle(_width, _height*8, _width, _height), _t25);
          _standAnimations[1] = standRight;

          var standBottomRight = new Animation();
          standBottomRight.AddFrame(new Rectangle(0, 128, _width, _height), _t25);
          _standAnimations[2] = standBottomRight;

          var standBottom = new Animation();
          standBottom.AddFrame(new Rectangle(0, _height * 8, _width, _height), _t25);
          _standAnimations[3] = standBottom;

          var standBottomLeft = new Animation();
          standBottomLeft.AddFrame(new Rectangle(0, 160, _width, _height), _t25);
          _standAnimations[4] = standBottomLeft;

          var standLeft = new Animation();
          standLeft.AddFrame(new Rectangle(_width*3, _height * 8, _width, _height), _t25);
          _standAnimations[5] = standLeft;

          var standTopLeft = new Animation();
          standTopLeft.AddFrame(new Rectangle(0, 192, _width, _height), _t25);
          _standAnimations[6] = standTopLeft;

          var standTop = new Animation();
          standTop.AddFrame(new Rectangle(_width*2, _height * 8, _width, _height), _t25);
          _standAnimations[7] = standTop;
        }

        private Animation InitFrame(int j)
        {
            var walk = new Animation();
            for (int i = 0; i < _frameCount; i++)
            {
                walk.AddFrame(new Rectangle(_width * i, _height * j, _width, _height), _t25);
            }

            return walk;
        }

        public void Draw(SpriteBatch spriteBatch, int tick, Vector2 position, double angle90, bool isMoving,
          bool isHorizontal)
      {
          var angle = (angle90 + 90) % 360;
          var index = (int) angle == 45 ? 7 : ((int) (2 * angle - 45 + 720) % 720) / 90;
          var sourceRectangle = (isMoving ? _walkAnimations[index] : _standAnimations[index]).CurrentRectangle(tick);

          spriteBatch.Draw(_texture, new Vector2(position.X - _width/2, position.Y - _height), sourceRectangle, Color.White);
      }
    }
}
