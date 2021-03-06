﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoBrJozik.Controls
{
    internal class MonoItemsControl:MonoControl
    {
        private bool _isOver = false;

        public MonoItemsControl()
        {}

        protected MonoItemsControl(List<MonoControl> monoControls)
        {
            childControls.AddRange(monoControls);
        }
        
        protected readonly List<MonoControl> childControls = new List<MonoControl>();
        public MonoItemsControl(List<MonoControl> monoControls, int x, int y, int width, int height) : base(x, y, width, height)
        {
            childControls = monoControls;
        }

        public override bool MouseLClick(MouseState mouseState)
        {
            if (!base.MouseLClick(mouseState))
            {
                return false;
            }

            return childControls.Any(ctrl => ctrl.MouseLClick(mouseState));
        }

        public override bool MouseRClick(MouseState mouseState)
        {
            if (!base.MouseRClick(mouseState))
            {
                return false;
            }

            return childControls.Any(ctrl => ctrl.MouseRClick(mouseState));
        }

        public override bool MouseOver(MouseState mouseState)
        {
            var prevIsOver = _isOver;
            _isOver = base.MouseOver(mouseState);
            
            if( _isOver || prevIsOver)
            {
                childControls.ForEach(ctrl => ctrl.MouseOver(mouseState));
            }

            return _isOver;
        }

        public override void Draw(SpriteBatch spriteBatch) => childControls.ForEach(ctrl => ctrl.Draw(spriteBatch));
    }
}
