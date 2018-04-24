using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoBrJozik.Controls
{
    internal class MonoList: MonoItemsControl
    {
        private readonly int _virtualY;

        private int _realHeight;

        private MonoMenu _menu;

        private readonly SpriteFont _font;
        private readonly Color _fontColor;

        private int startIndex;
        private int visibleCount;
        
        public MonoList(int x, int y, int width, int height, SpriteFont font, Color fontColor, Texture2D texture)
        {
            LeftTopX = x;
            LeftTopY = y;
            Width = width;
            Height = height;
            _fontColor = fontColor;
            _font = font;

            _menu = new MonoMenu(_font, Color.Black, MonoDrawer.SCREEN_WIDTH + width, height, texture);
        }

        public void MoveNext()
        {
            if (startIndex + visibleCount >= childControls.Count)
                return;

            startIndex = startIndex + visibleCount;
            CalcVisibleCount();
        }

        private void CalcVisibleCount()
        {
            var delta = childControls[startIndex].LeftTopY - LeftTopY;

            //var delta = childControls.Skip(startIndex).Take(visibleCount).Aggregate(0, (sum, ctrl) => sum + ctrl.Height);

            childControls.ForEach(ctrl => ctrl.LeftTopY -= delta);

            visibleCount = childControls.Count(ctrl => ctrl.LeftTopY <= LeftTopY + Height) - startIndex;
        }

        public void MovePrev()
        {
            if (startIndex <= 0)
                return;

            startIndex = childControls.Select((ctrl, i) => new { Index = i, ctrl.LeftTopY }).First(item => item.LeftTopY + Height > LeftTopY)?.Index ?? 0 ;

            CalcVisibleCount();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var childCtrl in childControls.Skip(startIndex).Take(visibleCount))
            {
                childCtrl.Draw(spriteBatch);
            }

            _menu.Draw(spriteBatch);
        }

        public void SetItems(List<MonoInvItemInfo> itemInfo)
        {
            var y = LeftTopY;
            var height = 0;

            var monoItems = itemInfo.Select(info =>
            {
                var monoItem = new MonoInventoryItem(info, _menu, _font, _fontColor, LeftTopX, y);
                height = height + monoItem.Height;
                y = LeftTopY + height;
                return monoItem;
            }).ToList();

            childControls.Clear();
            childControls.AddRange(monoItems);

            startIndex = !childControls.Any()? 0 : startIndex >= childControls.Count ? childControls.Count - 1 : startIndex;

            if (childControls.Any())
                CalcVisibleCount();
            else
                visibleCount = 0;
        }

        public override bool MouseLClick(MouseState mouseState)
        {
            if (_menu.MouseLClick(mouseState))
                return true;

            return base.MouseLClick(mouseState);
        }

        public override bool MouseRClick(MouseState mouseState)
        {
            if (_menu.MouseRClick(mouseState))
                return true;

            return base.MouseRClick(mouseState);
        }

        public override bool MouseOver(MouseState mouseState)
        {
            if (_menu.MouseOver(mouseState))
                return true;

            return base.MouseOver(mouseState);
        }
    }
}
