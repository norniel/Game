using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBrJozik.Controls
{
    internal class MonoKnowledges
    {
        private readonly MonoList _knowledgesList;
        private readonly Texture2D texture;

        private readonly GraphicsDevice _graphicsDevice;
        private Dictionary<string, uint> _originalKnowledges;

        public MonoKnowledges(GraphicsDevice graphicsDevice, SpriteFont font)
        {
            _graphicsDevice = graphicsDevice;
            texture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] c = new Color[1];
            c[0] = Color.White;
            texture.SetData<Color>(c);

            _knowledgesList = new MonoList(0, 0, MonoDrawer.SCREEN_WIDTH, MonoDrawer.SCREEN_HEIGHT, font, Color.Black, texture);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _knowledgesList.Draw(spriteBatch);
        }

        public void SetItems(Dictionary<string, uint> knowledges)
        {
            _originalKnowledges = knowledges;
            _knowledgesList.SetItems(knowledges.Select(kn => new MonoInvItemInfo(texture, null, $"{kn.Key}({kn.Value})", null, null)).ToList());
        }
    }
}
