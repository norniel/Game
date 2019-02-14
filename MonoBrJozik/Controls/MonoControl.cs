using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoBrJozik.Controls
{
    internal abstract class MonoControl
    {
        public int LeftTopX { get; set; }
        public int LeftTopY { get; set; }

        public int RightBottomX => LeftTopX + Width;
        public int RightBottomY => LeftTopY + Height;

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        protected MonoControl()
        {
        }


        public MonoControl(int x, int y, int width, int height)
        {
            LeftTopX = x;
            LeftTopY = y;
            Width = width;
            Height = height;
        }

        protected virtual bool IsInside(MouseState mouseState)
        {
            return mouseState.X >= LeftTopX && mouseState.X < RightBottomX && mouseState.Y >= LeftTopY &&
                   mouseState.Y < RightBottomY;
        }

        public virtual bool MouseLClick(MouseState mouseState)
        {
            return IsInside(mouseState);
        }

        public virtual bool MouseRClick(MouseState mouseState)
        {
            return IsInside(mouseState);
        }

        public virtual bool MouseOver(MouseState mouseState)
        {
            return IsInside(mouseState);
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
