using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBrJozik.Controls
{
    internal class MonoKnowledgeList:MonoList
    {
        private Texture2D _btexture;
        private Texture2D _blueLightTexture;
        private Texture2D _bltexture;

        public MonoKnowledgeList(GraphicsDevice graphicsDevice, int x, int y, int width, int height, SpriteFont font, Texture2D texture) : base(x, y, width, height, font, Color.Black, texture)
        {
            _btexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] c1 = new Color[1];
            c1[0] = Color.Black;
            _btexture.SetData<Color>(c1);

            _blueLightTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] c3 = new Color[1];
            c3[0] = Color.LightBlue;
            _blueLightTexture.SetData<Color>(c3);

            _bltexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] c2 = new Color[1];
            c2[0] = Color.Blue;
            _bltexture.SetData<Color>(c2);
        }

        public override MonoControl ProduceItem(MonoItemInfoBase itemInfo, SpriteFont font, Color fontColor, int x, int y)
        {
            return !(itemInfo is MonoKnowledgeItemInfo info) ? null : new MonoSlider(x, y, info.Text, info.End, info.Start, info.Current, font, _btexture, _bltexture, _blueLightTexture);
        }

        public bool LButtonDown(int mouseX, int mouseY)
        {
            return childControls.OfType<MonoSlider>().Any(sl => sl.LButtonDown(mouseX, mouseY));
        }

        public bool LButtonUp(int mouseX, int mouseY)
        {
            return childControls.OfType<MonoSlider>().Any(sl => sl.LButtonUp(mouseX, mouseY));
        }

        public bool MouseMove(int mouseX, int mouseY)
        {
            return childControls.OfType<MonoSlider>().Any(sl => sl.MouseMove(mouseX, mouseY));
        }
    }
}
