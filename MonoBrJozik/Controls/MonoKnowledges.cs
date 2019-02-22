using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoBrJozik.Controls
{
    internal class MonoKnowledges
    {
        private readonly MonoKnowledgeList _changableKnowledgesList;
        private readonly Texture2D _texture;

        private Dictionary<string, uint> _originalKnowledges;
        private bool _isVisible;
        public bool IsVisible => _isVisible;
        private readonly MonoItem _okButton;

        private Action<Dictionary<string, uint>> _rewriteKnowledges;

        public MonoKnowledges(GraphicsDevice graphicsDevice, SpriteFont font)
        {
            var graphicsDevice1 = graphicsDevice;
            _texture = new Texture2D(graphicsDevice1, 1, 1, false, SurfaceFormat.Color);
            Color[] c = new Color[1];
            c[0] = Color.White;
            _texture.SetData<Color>(c);

            _okButton = new MonoItem(new MonoItemInfo(_texture, null, "Ok", DoRewrite), font, Color.Black, MonoDrawer.SCREEN_WIDTH - 40, MonoDrawer.SCREEN_HEIGHT - 40);
            _changableKnowledgesList = new MonoKnowledgeList(graphicsDevice1, 0, 0, MonoDrawer.SCREEN_WIDTH, MonoDrawer.SCREEN_HEIGHT, font, _texture);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_isVisible)
                return;

            _changableKnowledgesList.Draw(spriteBatch);
            _okButton.Draw(spriteBatch);

            //_slider.Draw(spriteBatch);
        }

        public void Init(bool isVisible, Dictionary<string, uint> knowledges, bool showButton = false, Action<Dictionary<string, uint>> newKnowledgesAction = null)
        {
            if (!isVisible)
            {
                Hide();
                return;
            }

            _isVisible = true;

            _originalKnowledges = knowledges;
            _rewriteKnowledges = newKnowledgesAction;

            if (knowledges != null)
                _changableKnowledgesList.SetItems(knowledges.Select(kn => new MonoKnowledgeItemInfo(kn.Key, 0, kn.Value, kn.Value/2)).ToList());
        }

        private void Hide()
        {
            _isVisible = false;
            _rewriteKnowledges = null;
            _originalKnowledges = null;
        }

        private void DoRewrite()
        {
            var newKnowledges = _originalKnowledges.Select(pair => new KeyValuePair<string, uint>(pair.Key, pair.Value / 2))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            _rewriteKnowledges?.Invoke(newKnowledges);

            Hide();
        }

        public bool MouseLClick(MouseState mouseState)
        {
            return _isVisible && _okButton.MouseLClick(mouseState);
        }

        public bool LButtonDown(int mouseX, int mouseY)
        {
            return _isVisible && _changableKnowledgesList.LButtonDown(mouseX, mouseY);
        }

        public bool LButtonUp(int mouseX, int mouseY)
        {
            return _isVisible && _changableKnowledgesList.LButtonUp(mouseX, mouseY); 
        }

        public bool MouseMove(int mouseX, int mouseY)
        {
            return _isVisible && _changableKnowledgesList.MouseMove(mouseX, mouseY); 
        }
    }
}
