using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoBrJozik.Controls
{
    internal class MonoInventoryList:MonoList
    {
        private MonoMenu _menu;

        public MonoInventoryList(int x, int y, int width, int height, SpriteFont font, Color fontColor, Texture2D texture) : base(x, y, width, height, font, fontColor, texture)
        {
            _menu = new MonoMenu(_font, Color.Black, MonoDrawer.SCREEN_WIDTH + width, height, texture);
        }

        public override MonoControl ProduceItem(MonoItemInfoBase itemInfo, SpriteFont font, Color fontColor, int x, int y)
        {
             return itemInfo == null ? null : new MonoInventoryItem(itemInfo as MonoInvItemInfo, _menu, _font, fontColor, x, y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _menu.Draw(spriteBatch);
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
