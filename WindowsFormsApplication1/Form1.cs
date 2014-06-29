using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;
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
        private System.Timers.Timer _timer = new System.Timers.Timer(5000);
        private HydraliticErosion _erosionGenerator = null;

        private MapGeneratorEnum _currentGenerator = MapGeneratorEnum.DiamondSquare;
        public Form1()
        {
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
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
                case MapGeneratorEnum.WithErosion:
                    UpdateCombinedWithErosion();
                    break;
            }
        }

        private void UpdateCombinedWithErosion()
        {
            var combinedMap = GetCombinedMap();
            _erosionGenerator = new HydraliticErosion(combinedMap, 20);

            _timer.Enabled = true;

            _timer.Start();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var resultMap = _erosionGenerator.ApplyErosion();
            DrawMap(resultMap);
        }

        private void UpdateCombined()
        {
            var resultMap = GetCombinedMap();
            DrawMap(resultMap);
        }

        private void DrawMap(float[,] resultMap)
        {
            var scene = new ILScene { new ILPlotCube { new ILSurface(ILMath.tosingle((ILArray<float>)resultMap)) } };
            ilPanel1.Scene = scene;
            ilPanel1.Scene.First<ILPlotCube>().TwoDMode = false;
        }

        private float[,] GetCombinedMap()
        {
            var combinedGenerator = new CombinedMapGenerator();
            var resultMap = combinedGenerator.GetCombinedMap(tbPower.Value, tbHeight.Value, trackBar3.Value);
            return resultMap;
        }

        private void UpdateVoronoy()
        {
            var voronoyGenerator = new VoronoyGenerator();
            var resultMap = voronoyGenerator.GenerateMapWithVoronoyDiagrams(tbPower.Value, trackBar3.Value);
            DrawMap(resultMap);
        }

        private void UpdateDiamondSquare()
        {
            var diamondSquaredGenerator = new DiamondSquareGenerator();
            var resultMap = diamondSquaredGenerator.GenerateBaseMap(tbPower.Value, tbHeight.Value, trackBar3.Value);//GetFloatMap(tbPower.Value, tbHeight.Value, trackBar3.Value);
            DrawMap(resultMap);
        }

        private void Update()
        {
            var resultMap = GetFloatMap(tbPower.Value, tbHeight.Value, trackBar3.Value);
            //var ilPlote = new ILPlotCube {new ILSurface(ILMath.tosingle((ILArray<float>) resultMap))};

            var scene = new ILScene { new ILPlotCube { new ILSurface(ILMath.tosingle((ILArray<float>)resultMap)) } };
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
            float[,] points = new float[iWidth + 1, iHeight + 1];

            //Assign the four corners of the intial grid random color values  
            //These will end up being the colors of the four corners          
            c1 = (float)rnd.NextDouble();
            c2 = (float)rnd.NextDouble();
            c3 = (float)rnd.NextDouble();
            c4 = (float)rnd.NextDouble();
            var gRoughness = height;
            var gBigSize = iWidth + iHeight;
            DivideGrid(ref points, 0, 0, iWidth, iHeight, c1, c2, c3, c4);
            return points;
        }
        public void DivideGrid(ref float[,] points, float x, float y, float width, float height, float c1, float c2, float c3, float c4)
        {
            float Edge1, Edge2, Edge3, Edge4, Middle;

            float newWidth = (float)Math.Floor(width / 2);
            float newHeight = (float)Math.Floor(height / 2);

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
            return (float)((rnd.NextDouble() - 0.5) * Max);
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (_erosionGenerator == null)
            {
                _currentGenerator = MapGeneratorEnum.WithErosion;
                SetElementsEnabled(false);
                UpdateMap();
            }
            else
            {
                _timer.Stop();
                _timer.Enabled = false;
                _erosionGenerator = null;
                SetElementsEnabled(true);
            }
        }

        private void SetElementsEnabled(bool enabled)
        {
            this.button1.Enabled = enabled;
            this.button2.Enabled = enabled;
            this.button3.Enabled = enabled;
            this.button1.Enabled = enabled;
            this.tbPower.Enabled = enabled;
            this.tbHeight.Enabled = enabled;
            this.trackBar3.Enabled = enabled;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var combinedMap = GetCombinedMap();
            var erosionGenerator = new HydraliticErosion(combinedMap, 250);
            var resultMap = erosionGenerator.ApplyErosion();
            var waterMap = erosionGenerator.GetWaterMap();

            float maxHeight = 0;
            float minHeight = 0;
            float maxWater = 0;
            float minWater = 0;

            for (int i = 0; i < resultMap.GetLength(0); i++)
            {
                for (int j = 0; j < resultMap.GetLength(1); j++)
                {
                    maxHeight = Math.Max(maxHeight, resultMap[i, j]);
                    minHeight = Math.Min(minHeight, resultMap[i, j]);
                    maxWater = Math.Max(maxWater, waterMap[i, j]);
                    minWater = Math.Min(minWater, waterMap[i, j]);
                }
            }

            using (Bitmap flag = new Bitmap(resultMap.GetLength(0), resultMap.GetLength(1)))
            {
                using (Graphics flagGraphics = Graphics.FromImage(flag))
                {
                    for (int i = 0; i < resultMap.GetLength(0); i++)
                    {
                        for (int j = 0; j < resultMap.GetLength(1); j++)
                        {
                            using (var brush = GetBrushFromHeight(resultMap[i, j], maxHeight, minHeight))
                            {
                                flagGraphics.FillRectangle(brush, i, j, 1, 1);
                            }

                            using (var brush = GetBlueBrushFromWater(waterMap[i, j], maxWater, minWater))
                            {
                                flagGraphics.FillRectangle(brush, i, j, 1, 1);
                            }
                        }
                    }
                }

                flag.Save(@"D:\test.bmp", ImageFormat.Bmp);
            }

        }

        private Brush GetBlueBrushFromWater(float water, float maxWater, float minWater)
        {
            var dif = maxWater - minWater;
            var difWater = water - minWater;

            return new SolidBrush(Color.FromArgb((int)((255f * difWater)/dif), Color.MediumBlue));
        }

        private Brush GetBrushFromHeight(float height, float maxHeight, float minHeight)
        {
            var dif = maxHeight - minHeight;
            var difHight = height - minHeight;

            return new SolidBrush(ColorFromAhsb(100, (160f - ((160 * difHight) / dif)), 0.5f, 0.5f));
        }

        public static Color ColorFromAhsb(int a, float h, float s, float b)
        {

            if (0 > a || 255 < a)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (0f > h || 360f < h)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (0f > s || 1f < s)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (0f > b || 1f < b)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (0 == s)
            {
                return Color.FromArgb(a, Convert.ToInt32(b * 255),
                  Convert.ToInt32(b * 255), Convert.ToInt32(b * 255));
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < b)
            {
                fMax = b - (b * s) + s;
                fMin = b + (b * s) - s;
            }
            else
            {
                fMax = b + (b * s);
                fMin = b - (b * s);
            }

            iSextant = (int)Math.Floor(h / 60f);
            if (300f <= h)
            {
                h -= 360f;
            }
            h /= 60f;
            h -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = h * (fMax - fMin) + fMin;
            }
            else
            {
                fMid = fMin - h * (fMax - fMin);
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(a, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(a, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(a, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(a, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(a, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(a, iMax, iMid, iMin);
            }
        }
    }

    public enum MapGeneratorEnum
    {
        DiamondSquare,
        Voronoy,
        Combined,
        WithErosion
    }
}
