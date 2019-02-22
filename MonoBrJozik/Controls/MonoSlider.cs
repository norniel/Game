using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBrJozik.Controls
{
    internal class MonoSlider: MonoControl
    {
        private readonly uint _start;
        private uint _current;
        private readonly SpriteFont _font;
        private readonly string _text;
        private readonly uint _end;
        private readonly Texture2D _blackTexture;
        private readonly Texture2D _blueTexture;
        private readonly Texture2D _blueLightTexture;
        private readonly Texture2D _grayTexture;
        private Rectangle _lineRectangle;
        private Rectangle _squareRectangle;
        private int _textHeight;

        private int _diffLTX;
        private bool _isEnabled;

        private bool _isPressed = false;

        private const int _width = 100;
        private const int _sqWidth = 10;
        private const int _sqHeight = 10;
        private const int _sqOffset = 2;

        public MonoSlider(int x, int y, string text, uint end, uint start, uint current, bool isDisabled, SpriteFont font, Texture2D blackTexture,
            Texture2D blueTexture, Texture2D blueLightTexture, Texture2D grayTexture)
        {
            _isEnabled = !isDisabled;
            _text = text;

            _textHeight = (int)font.MeasureString(_text).Y;

            _end = end;
            _start = start;
            _current = current;
            _font = font;

            _blackTexture = blackTexture;
            _blueTexture = blueTexture;
            _blueLightTexture = blueLightTexture;
            _grayTexture = grayTexture;

            LeftTopX = x;
            LeftTopY = y;

            CalculateSquare();

            _lineRectangle = new Rectangle(LeftTopX + _sqWidth / 2, LeftTopY + _textHeight + _sqHeight/2-1, _width, 2);

            Height = _sqHeight + _sqOffset + 2*(int)_font.MeasureString(_current.ToString()).Y;
        }

        public uint Current => _current;

        public string Text => _text;

        private void CalculateSquare()
        {
            var currentLtx = LeftTopX + (int) (_width * ((double) (_current - _start) / (_end - _start)));
            _squareRectangle = new Rectangle(currentLtx, LeftTopY + _textHeight, _sqWidth, _sqHeight);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, _text, new Vector2(LeftTopX, LeftTopY), Color.Black);

            spriteBatch.Draw(_blackTexture, _lineRectangle, Color.White);
            spriteBatch.Draw(!_isEnabled ? _grayTexture : _isPressed ? _blueLightTexture :_blueTexture, _squareRectangle, Color.White);

            if (_current > _start)
            {
                spriteBatch.DrawString(_font, _start.ToString(), new Vector2(LeftTopX, LeftTopY + _textHeight + _sqHeight + _sqOffset), Color.Black);
            }

            if (_current < _end)
            {
                spriteBatch.DrawString(_font, _end.ToString(), new Vector2(LeftTopX + _width, LeftTopY + _textHeight + _sqHeight + _sqOffset), Color.Black);
            }

            spriteBatch.DrawString(_font, _current.ToString(), new Vector2(_squareRectangle.X, LeftTopY + _textHeight + _sqHeight + _sqOffset), Color.Black);
        }

        public bool LButtonDown(int mouseX, int mouseY)
        {
            if (_isEnabled && !_isPressed && _squareRectangle.Contains(mouseX, mouseY))
            {
                _diffLTX = mouseX - _squareRectangle.Left;
                _isPressed = true;
            }

            return _isEnabled && _isPressed;
        }

        public bool LButtonUp(int mouseX, int mouseY)
        {
            if (!_isEnabled || !_isPressed)
                return false;

            CalculateSquare();
            _isPressed = false;
            _diffLTX = 0;

            return true;
        }

        public bool MouseMove(int mouseX, int mouseY)
        {
            if (!_isEnabled || !_isPressed)
                return false;

            var tmpX = Math.Max(LeftTopX, Math.Min(mouseX - _diffLTX, LeftTopX + _width));
            _current = _start + (uint)Math.Round((_end - _start) * ((double)(tmpX - LeftTopX) / _width));

            _squareRectangle.X = tmpX;

            return true;
        }
    }
}
