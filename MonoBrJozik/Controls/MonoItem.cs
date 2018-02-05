using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoBrJozik.Controls
{
    internal class MonoItem : MonoControl
    {
        private readonly Texture2D _texture;
        private readonly Texture2D _innerTexture;
        private readonly string _text;

        public SpriteFont Font { get; set; }

        public MonoItem(Texture2D texture, Texture2D innerTexture, string text)
        {
            _texture = texture;
            _innerTexture = innerTexture;
            _text = text;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Vector2(LeftTopX, LeftTopY), Color.White);
            var textOffset = 0;

            if (_innerTexture != null)
            {
                spriteBatch.Draw(_innerTexture, new Vector2(LeftTopX + 2, LeftTopY + 2), Color.White);
                textOffset = _innerTexture.Width + 2;
            }
            
            if(_text != null)
                spriteBatch.DrawString(Font, _text, new Vector2(LeftTopX + textOffset + 5, LeftTopY + 10), Color.Black);
        }

        public override bool MouseClick(MouseState mouseState)
        {
            var isInside = base.MouseClick(mouseState);

            return isInside;
        }
    }
}
