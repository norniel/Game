using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Engine.BridgeObjects;

namespace MonoBrJozik.Controls
{
    internal class MonoInventoryItem: MonoItem
    {
        private readonly MonoMenu _menu;
        private readonly List<MonoItemInfo> _clientActions;
        public MonoInventoryItem(MonoInvItemInfo itemInfo, MonoMenu menu, SpriteFont font, Color fontColor, int x, int y):base(itemInfo, font, fontColor, x, y)
        {
            _menu = menu;
            _clientActions = itemInfo.ClientActions;
        }

        public override bool MouseRClick(MouseState mouseState)
        {
            if (!base.MouseRClick(mouseState))
                return false;

            if (!_clientActions?.Any() ?? true)
                return true;

            _menu.Show(_clientActions, mouseState.X, mouseState.Y);

            return true;
        }
    }
}
