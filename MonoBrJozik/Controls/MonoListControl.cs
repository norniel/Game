using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoBrJozik.Controls
{
    internal class MonoListControl:MonoControl
    {
        public MonoListControl()
        {}

        protected MonoListControl(List<MonoControl> monoControls)
        {
            childControls.AddRange(monoControls);
        }
        
        protected readonly List<MonoControl> childControls = new List<MonoControl>();
        public MonoListControl(List<MonoControl> monoControls, int x, int y, int width, int height) : base(x, y, width, height)
        {
            childControls = monoControls;
        }

        public override bool MouseClick(MouseState mouseState)
        {
            if (!base.MouseClick(mouseState))
            {
                return false;
            }

            return childControls.Any(ctrl => ctrl.MouseClick(mouseState));
        }

        public override void Draw(SpriteBatch spriteBatch) => childControls.ForEach(ctrl => ctrl.Draw(spriteBatch));
    }
}
