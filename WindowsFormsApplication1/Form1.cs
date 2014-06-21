using System.Windows.Forms;
using Game.Engine.MapGenerator;

namespace WindowsFormsApplication1
{
    using ILNumerics;
    using ILNumerics.Drawing;
    using ILNumerics.Drawing.Plotting;
    using System;
    public partial class Form1 : Form
    {
        private MapGeneratorEnum _currentGenerator = MapGeneratorEnum.DiamondSquare;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ilPanel1_Load(object sender, EventArgs e)
        {
            _currentGenerator = MapGeneratorEnum.DiamondSquare;
            UpdateMap();
        }

        private void UpdateMap()
        {
            switch (_currentGenerator)
            {
                case MapGeneratorEnum.DiamondSquare:
                    UpdateDiamondSquare();
                    break;
                case MapGeneratorEnum.Voronoy:
                    UpdateVoronoy();
                    break;
                case MapGeneratorEnum.Combined:
                    UpdateCombined();
                    break;
            }
        }

        private void UpdateCombined()
        {
            var combinedGenerator = new CombinedMapGenerator();
            var resultMap = combinedGenerator.GetCombinedMap(tbPower.Value, tbHeight.Value, trackBar3.Value);
            var scene = new ILScene { new ILPlotCube { new ILSurface(ILMath.tosingle((ILArray<float>)resultMap)) } };
            ilPanel1.Scene = scene;
            ilPanel1.Scene.First<ILPlotCube>().TwoDMode = false;
        }

        private void UpdateVoronoy()
        {
            var voronoyGenerator = new VoronoyGenerator();
            var resultMap = voronoyGenerator.GenerateMapWithVoronoyDiagrams(tbPower.Value, trackBar3.Value);
            var scene = new ILScene { new ILPlotCube { new ILSurface(ILMath.tosingle((ILArray<float>)resultMap)) } };
            ilPanel1.Scene = scene;
            ilPanel1.Scene.First<ILPlotCube>().TwoDMode = false;
        }

        private void UpdateDiamondSquare()
        {
            var diamondSquaredGenerator = new DiamondSquareGenerator();
            var resultMap = diamondSquaredGenerator.GenerateBaseMap(tbPower.Value, tbHeight.Value, trackBar3.Value);//GetFloatMap(tbPower.Value, tbHeight.Value, trackBar3.Value);
            var scene = new ILScene { new ILPlotCube { new ILSurface(ILMath.tosingle((ILArray<float>)resultMap)) } };
            ilPanel1.Scene = scene;
            ilPanel1.Scene.First<ILPlotCube>().TwoDMode = false;
        }

        private void Update()
        {
            var resultMap = GetFloatMap(tbPower.Value, tbHeight.Value, trackBar3.Value);
            var scene = new ILScene {new ILPlotCube {new ILSurface(ILMath.tosingle((ILArray<float>) resultMap))}};
            ilPanel1.Scene = scene;
            ilPanel1.Scene.First<ILPlotCube>().TwoDMode = false;
        }

        Random rnd = new Random();

        public double gRoughness = 1;
        public double gBigSize = 1;  
        private ILArray<float> GetFloatMap(int power, int height, int seed)
        {
            var iWidth = 1 << power;
            var iHeight = 1 << power;
            float c1, c2, c3, c4;  
            float[,] points = new float[iWidth+1, iHeight+1];  
      
            //Assign the four corners of the intial grid random color values  
    //These will end up being the colors of the four corners          
    c1 = (float) rnd.NextDouble();  
    c2 = (float) rnd.NextDouble();  
    c3 = (float) rnd.NextDouble();  
    c4 = (float) rnd.NextDouble();  
    var gRoughness = height;  
    var gBigSize = iWidth + iHeight;  
    DivideGrid(ref points, 0, 0, iWidth, iHeight, c1, c2, c3, c4);  
    return points;  
}
        public void DivideGrid(ref float[,] points, float x, float y, float width, float height, float c1, float c2, float c3, float c4)
        {
            float Edge1, Edge2, Edge3, Edge4, Middle;

            float newWidth = (float) Math.Floor(width / 2);
            float newHeight = (float) Math.Floor(height / 2);

            if (width > 1 || height > 1)
            {
                Middle = ((c1 + c2 + c3 + c4) / 4) + Displace(newWidth + newHeight);  //Randomly displace the midpoint!  
                Edge1 = ((c1 + c2) / 2);    //Calculate the edges by averaging the two corners of each edge.  
                Edge2 = ((c2 + c3) / 2);
                Edge3 = ((c3 + c4) / 2);
                Edge4 = ((c4 + c1) / 2);//  
                //Make sure that the midpoint doesn't accidentally "randomly displaced" past the boundaries!  
                Middle = Rectify(Middle);
                Edge1 = Rectify(Edge1);
                Edge2 = Rectify(Edge2);
                Edge3 = Rectify(Edge3);
                Edge4 = Rectify(Edge4);
                //Do the operation over again for each of the four new grids.             
                DivideGrid(ref points, x, y, newWidth, newHeight, c1, Edge1, Middle, Edge4);
                DivideGrid(ref points, x + newWidth, y, width - newWidth, newHeight, Edge1, c2, Edge2, Middle);
                DivideGrid(ref points, x + newWidth, y + newHeight, width - newWidth, height - newHeight, Middle, Edge2, c3, Edge3);
                DivideGrid(ref points, x, y + newHeight, newWidth, height - newHeight, Edge4, Middle, Edge3, c4);
            }
            else    //This is the "base case," where each grid piece is less than the size of a pixel.  
            {
                //The four corners of the grid piece will be averaged and drawn as a single pixel.  
                float c = (c1 + c2 + c3 + c4) / 4;

                points[(int)(x), (int)(y)] = c;
                if (width == 2)
                {
                    points[(int)(x + 1), (int)(y)] = c;
                }
                if (height == 2)
                {
                    points[(int)(x), (int)(y + 1)] = c;
                }
                if ((width == 2) && (height == 2))
                {
                    points[(int)(x + 1), (int)(y + 1)] = c;
                }
            }
        }

        private float Rectify(float iNum)
        {
            if (iNum < 0)
            {
                iNum = 0;
            }
            else if (iNum > 1.0)
            {
                iNum = 1.0f;
            }
            return iNum;
        }

        private float Displace(float SmallSize)
        {

            double Max = SmallSize / gBigSize * gRoughness;
            return (float) ((rnd.NextDouble() - 0.5) * Max);
        }  


        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            UpdateMap();
        }

        private void tbHeight_Scroll(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _currentGenerator = MapGeneratorEnum.DiamondSquare;
            UpdateMap();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _currentGenerator = MapGeneratorEnum.Voronoy;
            UpdateMap();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _currentGenerator = MapGeneratorEnum.Combined;
            UpdateMap();
        }
    }

    public enum MapGeneratorEnum
    {
        DiamondSquare,
        Voronoy,
        Combined
    }
}
