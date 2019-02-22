using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoBrJozik.Controls
{
    internal abstract class MonoList: MonoItemsControl
    {
        private readonly int _virtualY;

        private int _realHeight;

        protected readonly SpriteFont _font;
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
        }

        public void SetItems(IEnumerable<MonoItemInfoBase> itemInfo)
        {
            var y = LeftTopY;
            var height = 0;

            var monoItems = itemInfo.Select(info =>
            {
                var monoItem = ProduceItem(info, _font, _fontColor, LeftTopX, y);
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

        public abstract MonoControl ProduceItem(MonoItemInfoBase itemInfo, SpriteFont font, Color fontColor, int x, int y);
    }
}
