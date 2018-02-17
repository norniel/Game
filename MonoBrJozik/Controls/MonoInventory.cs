using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;


namespace MonoBrJozik.Controls
{
    internal class MonoInventory: MonoControl
    {
        private readonly SpriteFont _font;
        private readonly Color _fontColor;
        private readonly MonoItem _topButton;
        private readonly MonoItem _bottomButton;
        private readonly MonoList _monoList;

        public MonoInventory(int x, int y, int width, int height, SpriteFont font, Color fontColor)
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


            _monoList = new MonoList(x, _topButton.Height, width, bottomY - _topButton.Height, _font, _fontColor);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _topButton.Draw(spriteBatch);
            _bottomButton.Draw(spriteBatch);
            _monoList.Draw(spriteBatch);
        }

        public void MovePrev()
        {
            _monoList.MovePrev();
        }

        public void MoveNext()
        {
            _monoList.MoveNext();
        }

        public void SetItems(List<MonoItemInfo> itemsInfo)
        {
            _monoList.SetItems(itemsInfo);
        }
    }
}
