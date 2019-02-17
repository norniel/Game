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
        private readonly MonoList _knowledgesList;
        private readonly Texture2D texture;

        private readonly GraphicsDevice _graphicsDevice;
        private Dictionary<string, uint> _originalKnowledges;
        private bool _showButton;
        private bool _isVisible = false;
        private readonly MonoItem _okButton;
        public bool IsHaltShown => _showButton;

        private Action<Dictionary<string, uint>> _rewriteKnowledges;

        public MonoKnowledges(GraphicsDevice graphicsDevice, SpriteFont font)
        {
            _graphicsDevice = graphicsDevice;
            texture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] c = new Color[1];
            c[0] = Color.White;
            texture.SetData<Color>(c);

            _knowledgesList = new MonoList(0, 0, MonoDrawer.SCREEN_WIDTH, MonoDrawer.SCREEN_HEIGHT, font, Color.Black, texture);
            _okButton = new MonoItem(new MonoItemInfo(texture, null, "Ok", DoRewrite), font, Color.Black, MonoDrawer.SCREEN_WIDTH - 40, MonoDrawer.SCREEN_HEIGHT - 40);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_isVisible)
                return;

            _knowledgesList.Draw(spriteBatch);

            if(_showButton)
                _okButton.Draw(spriteBatch);
        }

        public void Init(bool isVisible, Dictionary<string, uint> knowledges, bool showButton = false, Action<Dictionary<string, uint>> newKnowledgesAction = null)
        {
            if (!isVisible)
            {
                Hide();
                return;
            }

            _isVisible = isVisible;
            _showButton = showButton;
            _originalKnowledges = knowledges;
            _rewriteKnowledges = newKnowledgesAction;

            if (knowledges != null)
                _knowledgesList.SetItems(knowledges.Select(kn => new MonoInvItemInfo(texture, null, $"{kn.Key}({kn.Value})", null, null)).ToList());
        }

        private void Hide()
        {
            _isVisible = false;
            _rewriteKnowledges = null;
            _showButton = false;
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
    }
}
