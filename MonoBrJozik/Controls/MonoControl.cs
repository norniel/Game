using Microsoft.Xna.Framework;
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

        public virtual bool MouseClick(MouseState mouseState)
        {
            return mouseState.X >= LeftTopX && mouseState.X < RightBottomX && mouseState.Y >= LeftTopY &&
                   mouseState.Y < RightBottomY;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
