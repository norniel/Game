using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoBrJozik.Controls
{
    internal class MonoItem : MonoControl
    {
        private const int _innerOffset = 2;
        private const int _textOffset = 5;
        private readonly SpriteFont _font;
        private readonly Action _action;
        private readonly Texture2D _texture;
        private readonly Texture2D _innerTexture;
        private readonly string _text;
        private Color _textColor;// = Color.MintCream;
        private Color _fontColor;// = Color.MintCream;

        public MonoItem(MonoItemInfo itemInfo, SpriteFont font, Color fontColor, int x, int y)
        {
            LeftTopX = x;
            LeftTopY = y;
            _font = font;
            _action = itemInfo.Action;
            _texture = itemInfo.Texture;
            _innerTexture = itemInfo.InnerTexture;
            _text = itemInfo.Text;
            _textColor = fontColor;
            _fontColor = fontColor;

            var height = 0;
            var width = 0;

            if (_innerTexture != null)
            {
                height = _innerOffset + _innerTexture.Height;
                width = _innerTexture.Width + _innerOffset;
            }

            if (_text != null)
            {
                var stringSize = _font.MeasureString(_text);
                width = width + _innerOffset + (int)stringSize.X;
                height = Math.Max(height, 2 * _textOffset + (int) stringSize.Y);
            }

            Height = height + _innerOffset;
            Width = width + _innerOffset;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
            {
                spriteBatch.Draw(_texture, new Rectangle(LeftTopX, LeftTopY, Width, Height), Color.White);
            }

            var textResultOffset = 0;

            if (_innerTexture != null)
            {
                spriteBatch.Draw(_innerTexture, new Vector2(LeftTopX + _innerOffset, LeftTopY + _innerOffset), Color.White);
                textResultOffset = _innerTexture.Width + _innerOffset;
            }
            
            if(_text != null)
                spriteBatch.DrawString(_font, _text, new Vector2(LeftTopX + textResultOffset, LeftTopY + _textOffset), _textColor);
        }

        public override bool MouseLClick(MouseState mouseState)
        {
            var result = base.MouseLClick(mouseState);

            if (result)
                _action?.Invoke();

            return result;
        }

        public override bool MouseOver(MouseState mouseState)
        {
            var result = base.MouseOver(mouseState);

            _textColor = result ? Color.LightBlue : _fontColor;

            return result;
        }
    }
}
