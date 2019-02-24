using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace MonoBrJozik.Controls
{
    internal class MonoInventory: MonoControl
    {
        private readonly SpriteFont _font;
        private readonly Color _fontColor;
        private readonly MonoItem _topButton;
        private readonly MonoItem _bottomButton;
        private readonly MonoList _monoList;

        public MonoInventory(int x, int y, int width, int height, SpriteFont font, Color fontColor, Texture2D texture)
        {
            _font = font;
            _fontColor = fontColor;
            LeftTopX = x;
            LeftTopY = y;
            Width = width;
            Height = height;

            var textSize = _font.MeasureString(">");
            var bottomY = height - (int)textSize.Y;

            _topButton = new MonoItem(new MonoItemInfo(null, null, "<", () => this.MovePrev()), _font, _fontColor, x, 0);
            _bottomButton = new MonoItem(new MonoItemInfo(null, null, ">", () => this.MoveNext()), _font, _fontColor, x, bottomY);
            _monoList = new MonoInventoryList(x, _topButton.Height, width, bottomY - _topButton.Height, _font, _fontColor, texture);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _topButton.Draw(spriteBatch);
            _bottomButton.Draw(spriteBatch);
            _monoList.Draw(spriteBatch);
        }

        private void MovePrev()
        {
            _monoList.MovePrev();
        }

        private void MoveNext()
        {
            _monoList.MoveNext();
        }

        public void SetItems(List<MonoInvItemInfo> itemsInfo)
        {
            _monoList.SetItems(itemsInfo);
        }

        public override bool MouseLClick(MouseState mouseState)
        {
            if(_monoList.MouseLClick(mouseState))
                return true;

            if (_topButton.MouseLClick(mouseState))
                return true;

            return _bottomButton.MouseLClick(mouseState);
        }

        public override bool MouseRClick(MouseState mouseState)
        {
            if (_monoList.MouseRClick(mouseState))
                return true;

            if (_topButton.MouseRClick(mouseState))
                return true;

            return _bottomButton.MouseRClick(mouseState);
        }

        public override bool MouseOver(MouseState mouseState)
        {
            if (_monoList.MouseOver(mouseState))
                return true;

            if (_topButton.MouseOver(mouseState))
                return true;

            return _bottomButton.MouseOver(mouseState);
        }
    }
}
