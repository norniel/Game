using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoBrJozik.Controls
{
    internal class MonoMenu: MonoListControl
    {
        private int Offset => 10;
        public bool IsShown { get; private set; }

        public MonoMenu(List<MonoControl> monoControls, int parentX, int parentY, int screenWidth, int screenHeight):base(monoControls)
        {
            LeftTopX = parentX + Offset + Width >= screenWidth ? parentX - Offset - Width : parentX + Offset;

            var minY = Math.Max(0, parentY - Height / 2);
            LeftTopY = minY + Height >= screenHeight ? screenHeight - Height - Offset : minY;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsShown)
                return;

            base.Draw(spriteBatch);
        }

        public bool MouseLClick(MouseState mouseState)
        {
            var result = base.MouseClick(mouseState);

            Clear();
            return result;
        }

        public bool MouseRClick(MouseState mouseState)
        {
            return base.MouseClick(mouseState);
        }

        public void Clear()
        {
            IsShown = false;
            childControls.Clear();
        }

        public void Show(List<MonoControl> monoControls)
        {
            if(IsShown)
                Clear();
        }
    }
}
