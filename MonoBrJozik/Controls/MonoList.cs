using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoBrJozik.Controls
{
    internal class MonoList: MonoItemsControl
    {
        private readonly int _virtualY;

        private int _realHeight;

        private int startIndex;
        private int visibleCount;
        
        public MonoList(int x, int y, int width, int height)
        {
            LeftTopX = x;
            LeftTopY = y;
            Width = width;
            Height = height;
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
    }
}
