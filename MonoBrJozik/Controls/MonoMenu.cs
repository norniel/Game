using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoBrJozik.Controls
{
    internal delegate void MenuHandler();

    internal class MonoMenu: MonoItemsControl
    {
        private readonly SpriteFont _font;
        private readonly Color _fontColor;
        private readonly Texture2D _texture;
        readonly int _screenWidth; 
        readonly int _screenHeight;
        private int Offset => 10;
        private int OffsetItems => 2;
        public bool IsShown { get; private set; }
        
        private static event MenuHandler MenuIsShown;

        public MonoMenu(SpriteFont font, Color fontColor, int screenWidth, int screenHeight)
        {
            _font = font;
            _fontColor = fontColor;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            MenuIsShown += Clear;
        }

        public MonoMenu(SpriteFont font, Color fontColor, int screenWidth, int screenHeight, Texture2D texture):this(font, fontColor, screenWidth, screenHeight)
        {
            _texture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsShown)
                return;

            if (_texture != null)
            {
                spriteBatch.Draw(_texture, new Rectangle(LeftTopX, LeftTopY, Width, Height), Color.White);
            }

            base.Draw(spriteBatch);
        }

        public override bool MouseLClick(MouseState mouseState)
        {
            if (!IsShown)
                return false;

            var result = base.MouseLClick(mouseState);

            Clear();
            return result;
        }

        public override bool MouseRClick(MouseState mouseState)
        {
            if (!IsShown)
                return false;

            Clear();

            return false;
        }

        public override bool MouseOver(MouseState mouseState)
        {
            if (!IsShown)
                return false;

            return base.MouseOver(mouseState);
        }

        public void Clear()
        {
            if (!IsShown)
                return;

            IsShown = false;
            childControls.Clear();
        }

        public void Show(List<MonoItemInfo> monoItemInfos, int parentX, int parentY)
        {
            SignalMenuIsShown();
         //   if (IsShown)
         //       Clear();

            var x = 0;
            var y = 0;
            var width = 0;
            var height = 0;

            var monoItems = monoItemInfos.Select(info =>
            {
                var monoItem = new MonoItem(info, _font, _fontColor, x, y);
                height = height + OffsetItems + monoItem.Height;
                y = height;
                width = Math.Max(width, monoItem.Width);
                return monoItem;
            }).ToList();

            childControls.AddRange(monoItems);

            Height = height;
            Width = width;

            LeftTopX = parentX + Offset + Width >= _screenWidth ? parentX - Offset - Width : parentX + Offset;

            var minY = Math.Max(0, parentY - Height / 2);
            LeftTopY = minY + Height >= _screenHeight ? _screenHeight - Height - Offset : minY;

            childControls.ForEach(ctrl =>
            {
                ctrl.LeftTopX = LeftTopX;
                ctrl.LeftTopY = LeftTopY + ctrl.LeftTopY;
            });

            IsShown = true;
        }

        static void SignalMenuIsShown()
        {
            MenuIsShown?.Invoke();
        }
    }
}
