using System;
using Microsoft.Xna.Framework.Graphics;

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
            _monoList = new MonoList();

            _topButton = new MonoItem(new MonoItemInfo(null, null, "<", () => _monoList.MovePrev()), _font, x, 0);
            _bottomButton = new MonoItem(new MonoItemInfo(null, null, ">", () => _monoList.MoveNext()), _font, x, bottomY);

            _monoList.LeftTopY = _topButton.LeftTopY + _topButton.Height;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _topButton.Draw(spriteBatch);
            _bottomButton.Draw(spriteBatch);
            _monoList.Draw(spriteBatch);
        }
    }
}
