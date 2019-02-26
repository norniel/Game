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
        private readonly SpriteFont _font;
        private readonly MonoKnowledgeList _changableKnowledgesList;
        
        private bool _isVisible;
        public bool IsVisible => _isVisible;

        private int _pointsToForget;
        private int _pointsToRemember;

        private readonly MonoItem _okButton;
        private readonly MonoItem _topButton;
        private readonly MonoItem _bottomButton;
        private bool _okButtonShow;

        private Action<Dictionary<string, uint>> _rewriteKnowledges;

        public MonoKnowledges(GraphicsDevice graphicsDevice, SpriteFont font)
        {
            _font = font;
            var graphicsDevice1 = graphicsDevice;
            var texture = new Texture2D(graphicsDevice1, 1, 1, false, SurfaceFormat.Color);
            Color[] c = new Color[1];
            c[0] = Color.White;
            texture.SetData<Color>(c);

            var textHeight = (int)font.MeasureString(_pointsToForget.ToString()).Y;

            _topButton = new MonoItem(new MonoItemInfo(texture, null, "<", this.MovePrev), _font, Color.Black, MonoDrawer.SCREEN_WIDTH - (int)font.MeasureString("<".ToString()).X - 10 - 40, MonoDrawer.SCREEN_HEIGHT - 40);
            _bottomButton = new MonoItem(new MonoItemInfo(texture, null, ">", this.MoveNext), _font, Color.Black, MonoDrawer.SCREEN_WIDTH - 40, MonoDrawer.SCREEN_HEIGHT - 40);

            _okButton = new MonoItem(new MonoItemInfo(texture, null, "Ok", DoRewrite), font, Color.Black, MonoDrawer.SCREEN_WIDTH - 40, MonoDrawer.SCREEN_HEIGHT - 40 - _topButton.Height - 10);
            _changableKnowledgesList = new MonoKnowledgeList(graphicsDevice1, 0, textHeight, MonoDrawer.SCREEN_WIDTH, MonoDrawer.SCREEN_HEIGHT - textHeight, font, texture);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_isVisible)
                return;

            spriteBatch.DrawString(_font, $"Points to forget: {_pointsToForget}", new Vector2(0, 0), Color.Black);

            _changableKnowledgesList.Draw(spriteBatch);
            _topButton.Draw(spriteBatch);
            _bottomButton.Draw(spriteBatch);

            if(_okButtonShow)
                _okButton.Draw(spriteBatch);
        }

        public void Init(bool isVisible, Dictionary<string, uint> knowledges, Action<Dictionary<string, uint>> newKnowledgesAction = null)
        {
            if (!isVisible)
            {
                Hide();
                return;
            }

            _isVisible = true;
            _okButtonShow = false;
            
            _rewriteKnowledges = newKnowledgesAction;

            if (knowledges != null)
            {
                var totalPoints = (int)(knowledges.Values.Sum(t => t));
                _pointsToRemember = totalPoints/2;
                _pointsToForget = totalPoints - _pointsToRemember;
                _changableKnowledgesList.SetItems(knowledges.Where(kn => kn.Value != 0).Select(kn => new MonoKnowledgeItemInfo(kn.Key, 0, (int)kn.Value, (int)kn.Value, false)).ToList());
            }
        }

        private void Hide()
        {
            _isVisible = false;
            _rewriteKnowledges = null;
            _pointsToForget = 0;
            _pointsToRemember = 0;
            _okButtonShow = false;
        }

        private void DoRewrite()
        {
            var newKnowledges = _changableKnowledgesList.NewKnowledges();

            _rewriteKnowledges?.Invoke(newKnowledges);

            Hide();
        }

        public bool MouseLClick(MouseState mouseState)
        {
            return _isVisible && ((_okButtonShow && _okButton.MouseLClick(mouseState)) || _topButton.MouseLClick(mouseState) || _bottomButton.MouseLClick(mouseState)) ;
        }

        public bool LButtonDown(int mouseX, int mouseY)
        {
            return _isVisible && _changableKnowledgesList.LButtonDown(mouseX, mouseY);
        }

        public bool LButtonUp(int mouseX, int mouseY)
        {
            if(!_isVisible)
                return false;
                    
            var isSliderReleased = _changableKnowledgesList.LButtonUp(mouseX, mouseY);
            if (isSliderReleased)
            {
                var knowledgeSum = _changableKnowledgesList.CurrentSum();
                _pointsToForget = Math.Max(0, knowledgeSum - _pointsToRemember);

                _okButtonShow = knowledgeSum == _pointsToRemember;

                _changableKnowledgesList.SetMin(_pointsToForget);
            }

            return isSliderReleased;
        }

        public bool MouseMove(int mouseX, int mouseY)
        {
            if (!_isVisible)
                return false;

            var isSliderPressed = _changableKnowledgesList.MouseMove(mouseX, mouseY);
            if (isSliderPressed)
            {
                var knowledgeSum = _changableKnowledgesList.CurrentSum();
                _pointsToForget = knowledgeSum - _pointsToRemember;

                _okButtonShow = knowledgeSum == _pointsToRemember;
            }

            return isSliderPressed;
        }

        private void MovePrev()
        {
            _changableKnowledgesList.MovePrev();
        }

        private void MoveNext()
        {
            _changableKnowledgesList.MoveNext();
        }
    }
}
