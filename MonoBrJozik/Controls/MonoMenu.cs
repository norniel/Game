using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoBrJozik.Controls
{
    internal class MonoMenu: MonoItemsControl
    {
        private readonly SpriteFont _font;
        private int Offset => 10;
        private int OffsetItems => 2;
        public bool IsShown { get; private set; }
        
        public MonoMenu(SpriteFont font)
        {
            _font = font;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsShown)
                return;

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

            return base.MouseRClick(mouseState);
        }

        public void Clear()
        {
            IsShown = false;
            childControls.Clear();
        }

        public void Show(List<MonoItemInfo> monoItemInfos, int parentX, int parentY, int screenWidth, int screenHeight)
        {
            if(IsShown)
                Clear();

            var x = 0;
            var y = 0;
            var width = 0;
            var height = 0;

            var monoItems = monoItemInfos.Select(info =>
            {
                var monoItem = new MonoItem(info, _font, x, y);
                height = height + OffsetItems + monoItem.Height;
                y = height;
                width = Math.Max(width, monoItem.Width);
                return monoItem;
            }).ToList();

            childControls.AddRange(monoItems);

            Height = height;
            Width = width;

            LeftTopX = parentX + Offset + Width >= screenWidth ? parentX - Offset - Width : parentX + Offset;

            var minY = Math.Max(0, parentY - Height / 2);
            LeftTopY = minY + Height >= screenHeight ? screenHeight - Height - Offset : minY;

            childControls.ForEach(ctrl =>
            {
                ctrl.LeftTopX = LeftTopX;
                ctrl.LeftTopY = LeftTopY + ctrl.LeftTopY;
            });

            IsShown = true;
        }
    }
}
