using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;


namespace MonoBrJozik.Controls
{
    internal class MonoInventory: MonoControl
    {
        private readonly SpriteFont _font;
        private readonly MonoItem _topButton;
        private readonly MonoItem _bottomButton;
        private readonly MonoList _monoList;

        public MonoInventory(int x, int y, int width, int height, SpriteFont font)
        {
            _font = font;
            LeftTopX = x;
            LeftTopY = y;
            Width = width;
            Height = height;

            var textSize = _font.MeasureString(">");
            var bottomY = height - (int)textSize.Y;

            _topButton = new MonoItem(new MonoItemInfo(null, null, "<", () => this.MovePrev()), _font, x, 0);
            _bottomButton = new MonoItem(new MonoItemInfo(null, null, ">", () => this.MoveNext()), _font, x, bottomY);


            _monoList = new MonoList(x, _topButton.Height, width, bottomY - _topButton.Height, _font);
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
