using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBrJozik.Controls
{
    internal class MonoKnowledgesSimple
    {
        private readonly MonoInventoryList _simpleKnowledgesList;
        private readonly Texture2D _texture;
        
        private bool _isVisible;

        public MonoKnowledgesSimple(GraphicsDevice graphicsDevice, SpriteFont font)
        {
            _texture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] c = new Color[1];
            c[0] = Color.White;
            _texture.SetData<Color>(c);

            _simpleKnowledgesList = new MonoInventoryList(0, 0, MonoDrawer.SCREEN_WIDTH, MonoDrawer.SCREEN_HEIGHT, font, Color.Black, _texture);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_isVisible)
                return;

            _simpleKnowledgesList.Draw(spriteBatch);
        }

        public void Init(bool isVisible, Dictionary<string, uint> knowledges, bool showButton = false, Action<Dictionary<string, uint>> newKnowledgesAction = null)
        {
            if (!isVisible)
            {
                Hide();
                return;
            }

            _isVisible = true;

            if (knowledges != null)
                _simpleKnowledgesList.SetItems(knowledges.Select(kn => new MonoInvItemInfo(_texture, null, $"{kn.Key}({kn.Value})", null, null)).ToList());
        }

        private void Hide()
        {
            _isVisible = false;
        }
    }
}

