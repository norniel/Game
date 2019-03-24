using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoBrJozik.Animation
{
    class CharacterEntity
    {
        private readonly Texture2D _texture;
        private readonly Animation[] walkAnimations = new Animation[8];
        private readonly Animation[] standAnimations = new Animation[8];
        private static readonly TimeSpan _t25 = TimeSpan.FromSeconds(.25);

        public CharacterEntity(Texture2D texture)
      {
          _texture = texture;

          var walkTopRight = new Animation();
          walkTopRight.AddFrame(new Rectangle(0, 224, 32, 32), _t25);
          walkTopRight.AddFrame(new Rectangle(32, 224, 32, 32), _t25);
          walkTopRight.AddFrame(new Rectangle(64, 224, 32, 32), _t25);
          walkTopRight.AddFrame(new Rectangle(96, 224, 32, 32), _t25);
          walkTopRight.AddFrame(new Rectangle(128, 224, 32, 32), _t25);
          walkTopRight.AddFrame(new Rectangle(160, 224, 32, 32), _t25);
          walkTopRight.AddFrame(new Rectangle(192, 224, 32, 32), _t25);
          walkTopRight.AddFrame(new Rectangle(224, 224, 32, 32), _t25);
          walkAnimations[0] = walkTopRight;

          var walkRight = new Animation();
          walkRight.AddFrame(new Rectangle(0, 96, 32, 32), _t25);
          walkRight.AddFrame(new Rectangle(32, 96, 32, 32), _t25);
          walkRight.AddFrame(new Rectangle(64, 96, 32, 32), _t25);
          walkRight.AddFrame(new Rectangle(96, 96, 32, 32), _t25);
          walkRight.AddFrame(new Rectangle(128, 96, 32, 32), _t25);
          walkRight.AddFrame(new Rectangle(160, 96, 32, 32), _t25);
          walkRight.AddFrame(new Rectangle(192, 96, 32, 32), _t25);
          walkRight.AddFrame(new Rectangle(224, 96, 32, 32), _t25);
          walkAnimations[1] = walkRight;

          var walkBottomRight = new Animation();
          walkBottomRight.AddFrame(new Rectangle(0, 128, 32, 32), _t25);
          walkBottomRight.AddFrame(new Rectangle(32, 128, 32, 32), _t25);
          walkBottomRight.AddFrame(new Rectangle(64, 128, 32, 32), _t25);
          walkBottomRight.AddFrame(new Rectangle(96, 128, 32, 32), _t25);
          walkBottomRight.AddFrame(new Rectangle(128, 128, 32, 32), _t25);
          walkBottomRight.AddFrame(new Rectangle(160, 128, 32, 32), _t25);
          walkBottomRight.AddFrame(new Rectangle(192, 128, 32, 32), _t25);
          walkBottomRight.AddFrame(new Rectangle(224, 128, 32, 32), _t25);
          walkAnimations[2] = walkBottomRight;

          var walkBottom = new Animation();
          walkBottom.AddFrame(new Rectangle(0, 32, 32, 32), _t25);
          walkBottom.AddFrame(new Rectangle(32, 32, 32, 32), _t25);
          walkBottom.AddFrame(new Rectangle(64, 32, 32, 32), _t25);
          walkBottom.AddFrame(new Rectangle(96, 32, 32, 32), _t25);
          walkBottom.AddFrame(new Rectangle(128, 32, 32, 32), _t25);
          walkBottom.AddFrame(new Rectangle(160, 32, 32, 32), _t25);
          walkBottom.AddFrame(new Rectangle(192, 32, 32, 32), _t25);
          walkBottom.AddFrame(new Rectangle(224, 32, 32, 32), _t25);
          walkAnimations[3] = walkBottom;

          var walkBottomLeft = new Animation();
          walkBottomLeft.AddFrame(new Rectangle(0, 160, 32, 32), _t25);
          walkBottomLeft.AddFrame(new Rectangle(32, 160, 32, 32), _t25);
          walkBottomLeft.AddFrame(new Rectangle(64, 160, 32, 32), _t25);
          walkBottomLeft.AddFrame(new Rectangle(96, 160, 32, 32), _t25);
          walkBottomLeft.AddFrame(new Rectangle(128, 160, 32, 32), _t25);
          walkBottomLeft.AddFrame(new Rectangle(160, 160, 32, 32), _t25);
          walkBottomLeft.AddFrame(new Rectangle(192, 160, 32, 32), _t25);
          walkBottomLeft.AddFrame(new Rectangle(224, 160, 32, 32), _t25);
          walkAnimations[4] = walkBottomLeft;

          var walkLeft = new Animation();
          walkLeft.AddFrame(new Rectangle(0, 64, 32, 32), _t25);
          walkLeft.AddFrame(new Rectangle(32, 64, 32, 32), _t25);
          walkLeft.AddFrame(new Rectangle(64, 64, 32, 32), _t25);
          walkLeft.AddFrame(new Rectangle(96, 64, 32, 32), _t25);
          walkLeft.AddFrame(new Rectangle(128, 64, 32, 32), _t25);
          walkLeft.AddFrame(new Rectangle(160, 64, 32, 32), _t25);
          walkLeft.AddFrame(new Rectangle(192, 64, 32, 32), _t25);
          walkLeft.AddFrame(new Rectangle(224, 64, 32, 32), _t25);
          walkAnimations[5] = walkLeft;

          var walkTopLeft = new Animation();
          walkTopLeft.AddFrame(new Rectangle(0, 192, 32, 32), _t25);
          walkTopLeft.AddFrame(new Rectangle(32, 192, 32, 32), _t25);
          walkTopLeft.AddFrame(new Rectangle(64, 192, 32, 32), _t25);
          walkTopLeft.AddFrame(new Rectangle(96, 192, 32, 32), _t25);
          walkTopLeft.AddFrame(new Rectangle(128, 192, 32, 32), _t25);
          walkTopLeft.AddFrame(new Rectangle(160, 192, 32, 32), _t25);
          walkTopLeft.AddFrame(new Rectangle(192, 192, 32, 32), _t25);
          walkTopLeft.AddFrame(new Rectangle(224, 192, 32, 32), _t25);
          walkAnimations[6] = walkTopLeft;

          var walkTop = new Animation();
          walkTop.AddFrame(new Rectangle(0, 0, 32, 32), _t25);
          walkTop.AddFrame(new Rectangle(32, 0, 32, 32), _t25);
          walkTop.AddFrame(new Rectangle(64, 0, 32, 32), _t25);
          walkTop.AddFrame(new Rectangle(96, 0, 32, 32), _t25);
          walkTop.AddFrame(new Rectangle(128, 0, 32, 32), _t25);
          walkTop.AddFrame(new Rectangle(160, 0, 32, 32), _t25);
          walkTop.AddFrame(new Rectangle(192, 0, 32, 32), _t25);
          walkTop.AddFrame(new Rectangle(224, 0, 32, 32), _t25);
          walkAnimations[7] = walkTop;

          // Standing animations only have a single frame of animation:
          var standTopRight = new Animation();
          standTopRight.AddFrame(new Rectangle(0, 224, 32, 32), _t25);
          standAnimations[0] = standTopRight;

          var standRight = new Animation();
          standRight.AddFrame(new Rectangle(32, 256, 32, 32), _t25);
          standAnimations[1] = standRight;

          var standBottomRight = new Animation();
          standBottomRight.AddFrame(new Rectangle(0, 128, 32, 32), _t25);
          standAnimations[2] = standBottomRight;

          var standBottom = new Animation();
          standBottom.AddFrame(new Rectangle(0, 256, 32, 32), _t25);
          standAnimations[3] = standBottom;

          var standBottomLeft = new Animation();
          standBottomLeft.AddFrame(new Rectangle(0, 160, 32, 32), _t25);
          standAnimations[4] = standBottomLeft;

          var standLeft = new Animation();
          standLeft.AddFrame(new Rectangle(96, 256, 32, 32), _t25);
          standAnimations[5] = standLeft;

          var standTopLeft = new Animation();
          standTopLeft.AddFrame(new Rectangle(0, 192, 32, 32), _t25);
          standAnimations[6] = standTopLeft;

          var standTop = new Animation();
          standTop.AddFrame(new Rectangle(64, 256, 32, 32), _t25);
          standAnimations[7] = standTop;
        }

      public void Draw(SpriteBatch spriteBatch, int tick, Vector2 position, double angle90, bool isMoving,
          bool isHorizontal)
      {
          var angle = (angle90 + 90) % 360;
          var index = (int) angle == 45 ? 7 : ((int) (2 * angle - 45 + 720) % 720) / 90;
          var sourceRectangle = (isMoving ? walkAnimations[index] : standAnimations[index]).CurrentRectangle(tick);

          spriteBatch.Draw(_texture, new Vector2(position.X - 16, position.Y - 28), sourceRectangle, Color.White);
      }
    }
}
