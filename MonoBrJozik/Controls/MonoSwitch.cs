using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoBrJozik.Controls
{
    internal class MonoSwitch:MonoItem
    {
        private readonly Texture2D _originalTexture;
        private bool _isSwitched = false;
        private readonly Texture2D _pressedTexture;

        public MonoSwitch(MonoItemInfo itemInfo, bool isSwitched, Texture2D pressedTexture, SpriteFont font, Color fontColor, int x, int y) : base(itemInfo, font, fontColor, x, y)
        {
            _pressedTexture = pressedTexture;
            _originalTexture = _texture;

            SetSwitched(isSwitched);
        }

        public override bool MouseLClick(MouseState mouseState)
        {
            var result = base.MouseLClick(mouseState);
/*
            if(result)
                SetSwitched(!_isSwitched);
*/
            return result;
        }

        public void SetSwitched(bool isSwitched)
        {
            _isSwitched = isSwitched;
            _texture = _isSwitched ? _pressedTexture : _originalTexture;
            _textColor = _isSwitched ? Color.DarkSlateGray : _fontColor;
        }

        public override bool MouseOver(MouseState mouseState)
        {
            return IsInside(mouseState);
        }
    }
}
