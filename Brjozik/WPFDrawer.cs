using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Engine;
using Engine.BridgeObjects;
using Engine.Interfaces;
using Point = Engine.Point;
using Rect = System.Windows.Rect;
using Size = Engine.Size;

namespace Game
{
    class WpfDrawer : IDrawer
    {
        private Dictionary<uint, IObjectDrawer> _drawSamples;

        private readonly Canvas _canvas;
        private readonly ListBox _listBox;
        private readonly ListBox _heroListBox;
        private readonly ListBox _listBoxDateTime;
        private Path _appearance;
        private Path _horizontalAppearance;
        private LineGeometry _hands;
        private EllipseGeometry _hat;
        private EllipseGeometry _nose;
        private readonly int _dcenter;
        private RotateTransform _t;
        private readonly PointCollection _visWayCollection;
        private readonly Polyline _visibleWay;
        private readonly BitmapImage _appletreeImage;
        private readonly BitmapImage _appletree1Image;
        private readonly BitmapImage _appletree2Image;

        private readonly BitmapImage _plantImage;
        private readonly BitmapImage _dryplantImage;
        private readonly BitmapImage _growingPlantImage;
        private readonly BitmapImage _rockImage;
        private readonly BitmapImage _fireImage;

        private readonly BitmapImage _appleImage;
        private readonly BitmapImage _branchImage;
        private readonly BitmapImage _bushImage;
        private readonly BitmapImage _raspberryImage;
        private readonly BitmapImage _stoneAxeImage;
        private readonly BitmapImage _logImage;
        private readonly BitmapImage _attenuatingFireImage;
        private readonly BitmapImage _spruceTreeImage;
        private readonly BitmapImage _coneImage;

        private readonly BitmapImage _dikabrozikImage;
        private readonly BitmapImage _dikabrozikWithBundleImage;

        private readonly BitmapImage _mushroomImage;
        private readonly BitmapImage _growingMushroomImage;

        private readonly BitmapImage _roastedMushroomImage;
        private readonly BitmapImage _roastedAppleImage;

        private readonly BitmapImage _twigImage;
        private readonly BitmapImage _grassBed0;
        private readonly BitmapImage _grassBed1;
        private readonly BitmapImage _grassBed2;
        private readonly BitmapImage _grassBed3;

        private readonly BitmapImage _diggingStickImage;
        private readonly BitmapImage _sharpStoneImage;

        private readonly BitmapImage _rootImage;

        private readonly BitmapImage _wickiupImage0;
        private readonly BitmapImage _wickiupImage1;
        private readonly BitmapImage _wickiupImage2;
        private readonly BitmapImage _wickiupImage3;
        private readonly BitmapImage _wickiupImage4;
        private readonly BitmapImage _wickiupImage5;
        private readonly BitmapImage _wickiupImage6;

        private readonly BitmapImage _nutTreeImage;
        private readonly BitmapImage _nutImage;

        private readonly TextBlock _acting = new TextBlock();
        private int _drawCount;

        public WpfDrawer(Canvas canvas, ListBox listBox, ListBox heroListBox, ListBox listBoxDateTime)
        {
            _drawSamples = new Dictionary<uint, IObjectDrawer>();
            //_drawSamples[0x00000100] = new TreeDrawer(); // automize

            _dcenter = 8;

            _canvas = canvas;
            _listBox = listBox;
            _heroListBox = heroListBox;
            _listBoxDateTime = listBoxDateTime;

            _appletreeImage = CreateBitmapImage(@"apple tree icon.png");
            _appletree1Image = CreateBitmapImage(@"apple-tree1 icon.png");

            _appletree2Image = CreateBitmapImage(@"apple-tree2 icon.png");
            _plantImage = CreateBitmapImage(@"plant icon.png");
            _dryplantImage = CreateBitmapImage(@"dry plant icon.png");
            _growingPlantImage = CreateBitmapImage(@"growing plant icon.png");

            _rockImage = CreateBitmapImage(@"rock icon2.png");
            _fireImage = CreateBitmapImage(@"fire icon.png");
            _appleImage = CreateBitmapImage(@"apple icon.png");
            _branchImage = CreateBitmapImage(@"branch icon.png");
            _bushImage = CreateBitmapImage(@"brush icon.png");
            _raspberryImage = CreateBitmapImage(@"Raspberry icon.png");
            _stoneAxeImage = CreateBitmapImage(@"Stone axe icon.png");
            _logImage = CreateBitmapImage(@"Log icon.png");
            _attenuatingFireImage = CreateBitmapImage(@"attenuating fire small.png");
            _spruceTreeImage = CreateBitmapImage(@"spruce tree.png");
            _coneImage = CreateBitmapImage(@"cone small.png");

            _dikabrozikImage = CreateBitmapImage(@"dikabroyozik small.png");
            _dikabrozikWithBundleImage = CreateBitmapImage(@"dikabroyozik with bundle small.png");

            _mushroomImage = CreateBitmapImage(@"mushroom small.png");
            _growingMushroomImage = CreateBitmapImage(@"mushroom growing small.png");

            _roastedMushroomImage = CreateBitmapImage(@"roasted mushroom small.png");
            _roastedAppleImage = CreateBitmapImage(@"roasted apple icon.png");

            _twigImage = CreateBitmapImage(@"twig icon.png");

            _grassBed0 = CreateBitmapImage(@"grassbed0.png");
            _grassBed1 = CreateBitmapImage(@"grassbed1.png");
            _grassBed2 = CreateBitmapImage(@"grassbed2.png");
            _grassBed3 = CreateBitmapImage(@"grassbed3.png");

            _diggingStickImage = CreateBitmapImage(@"digging stick icon.png");
            _sharpStoneImage = CreateBitmapImage(@"sharp stone icon.png");

            _rootImage = CreateBitmapImage(@"root icon.png");

            _nutTreeImage = CreateBitmapImage(@"nut tree icon.png");
            _nutImage = CreateBitmapImage(@"nut.png");

            _wickiupImage0 = CreateBitmapImage(@"Wickiup0 icon.png");
            _wickiupImage1 = CreateBitmapImage(@"Wickiup1 icon.png");
            _wickiupImage2 = CreateBitmapImage(@"Wickiup2 icon.png");
            _wickiupImage3 = CreateBitmapImage(@"Wickiup3 icon.png");
            _wickiupImage4 = CreateBitmapImage(@"Wickiup4 icon.png");
            _wickiupImage5 = CreateBitmapImage(@"Wickiup5 icon.png");
            _wickiupImage6 = CreateBitmapImage(@"Wickiup6 icon.png");

            CreateActing();

            PrepareAppearance();

            // polyline
            _visibleWay = new Polyline
            {
                Stroke = Brushes.DarkSlateGray,
                StrokeThickness = 2//,
                                   // FillRule = FillRule.EvenOdd
            };
            _visWayCollection = new PointCollection();
            _visibleWay.Points = _visWayCollection;
        }

        private void PrepareAppearance()
        {
            _appearance = new Path { Fill = Brushes.Yellow, Stroke = Brushes.Brown, Height = 16, Width = 16 };
            Canvas.SetTop(_appearance, 0);
            Canvas.SetLeft(_appearance, 0);

            _hands = new LineGeometry();
            _hands.StartPoint = new System.Windows.Point(8, 0);
            _hands.EndPoint = new System.Windows.Point(8, 16);


            // Create the ellipse geometry to add to the Path
            _hat = new EllipseGeometry();

            _hat.Center = new System.Windows.Point(8, 8);
            _hat.RadiusX = 5;
            _hat.RadiusY = 5;


            _nose = new EllipseGeometry();
            _nose.Center = new System.Windows.Point(14, 8);
            _nose.RadiusX = 2;
            _nose.RadiusY = 2;

            GeometryGroup ggroup = new GeometryGroup();

            ggroup.Children.Add(_hands);
            ggroup.Children.Add(_hat);
            ggroup.Children.Add(_nose);
            ggroup.FillRule = FillRule.Nonzero;

            _appearance.Data = ggroup;

            _t = new RotateTransform();
            _appearance.RenderTransform = _t;
            _appearance.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
            _canvas.Children.Add(_appearance);

            _horizontalAppearance = new Path { Fill = Brushes.Yellow, Stroke = Brushes.Brown, Height = 36, Width = 16 };

            var hands2 = new LineGeometry
            {
                StartPoint = new System.Windows.Point(0, 11),
                EndPoint = new System.Windows.Point(16, 11)
            };

            // Create the ellipse geometry to add to the Path
            var hat2 = new EllipseGeometry();
            hat2.Center = new System.Windows.Point(8, 5);
            hat2.RadiusX = 5;
            hat2.RadiusY = 5;

            var body = new LineGeometry();
            body.StartPoint = new System.Windows.Point(8, 10);
            body.EndPoint = new System.Windows.Point(8, 18);

            var legLeft = new LineGeometry
            {
                StartPoint = new System.Windows.Point(8, 18),
                EndPoint = new System.Windows.Point(6, 30)
            };

            var legRight = new LineGeometry();
            legRight.StartPoint = new System.Windows.Point(8, 18);
            legRight.EndPoint = new System.Windows.Point(10, 30);

            GeometryGroup ggroup2 = new GeometryGroup();

            ggroup2.Children.Add(hands2);
            ggroup2.Children.Add(hat2);
            ggroup2.Children.Add(body);
            ggroup2.Children.Add(legLeft);
            ggroup2.Children.Add(legRight);
            ggroup2.FillRule = FillRule.Nonzero;

            _horizontalAppearance.Data = ggroup2;
        }

        private BitmapImage CreateBitmapImage(string uri)
        {
            var packUrl = new Uri("pack://application:,,,/Icons/" + uri);

            var bi = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            bi.BeginInit();
            bi.UriSource = packUrl;
            bi.EndInit();

            bi.Freeze();

            return bi;
        }


        #region Implementation of IDrawer

        public Func<int, int, List<string>> GetAction { get; set; }

        public void Clear()
        {
            _canvas.UpdateLayout();
            foreach (var disposableChild in _canvas.Children.OfType<IDisposable>())
            {
                disposableChild.Dispose();
            }
            _canvas.Children.Clear();
        }

        public void DrawHero(Point position, double angle, List<Point> wayList, bool isHorizontal)
        {
            _canvas.Children.Add(_visibleWay);
            _visWayCollection.Clear();

            System.Windows.Point point = new System.Windows.Point(position.X, position.Y);
            _visWayCollection.Add(point);
            foreach (var wayPoint in wayList)
            {/*
                if (weakReference.Target == null)
                    continue;
                */
                point = new System.Windows.Point(wayPoint.X, wayPoint.Y);
                _visWayCollection.Add(point);
            }
            if (isHorizontal)
            {
                _canvas.Children.Add(_horizontalAppearance);
                Canvas.SetLeft(_horizontalAppearance, position.X - _dcenter);
                Canvas.SetTop(_horizontalAppearance, position.Y - _dcenter);
            }
            else
            {
                _canvas.Children.Add(_appearance);
                Canvas.SetLeft(_appearance, position.X - _dcenter);
                Canvas.SetTop(_appearance, position.Y - _dcenter);
                _t.Angle = angle;
            }
        }

        public void DrawObject(uint id, long x, long y)
        {
            if (id == 0x00000100)
            {
                // _canvas.Children

                DrawImage(_appletreeImage, x, y);/*
                Ellipse rec = new Ellipse()
                                  {Fill = Brushes.ForestGreen, Stroke = Brushes.Green, Height = 20, Width = 20};
                rec.ContextMenu = _canvas.ContextMenu;
                _canvas.Children.Add(rec);
                Canvas.SetLeft(rec, x);
                Canvas.SetTop(rec, y);*/
            }
            else if (id == 0x00000200)
            {
                // _canvas.Children

                DrawImage(_appletree1Image, x, y);
            }
            else if (id == 0x00000300)
            {
                // _canvas.Children

                DrawImage(_appletree2Image, x, y);
            }
            else if (id == 0x00001000)
            {
                // _canvas.Children
                /*      Ellipse rec = new Ellipse() { Fill = Brushes.GreenYellow, Stroke = Brushes.Green, Height = 10, Width = 10 };
                      rec.ContextMenu = _canvas.ContextMenu;
                      _canvas.Children.Add(rec);
                      Canvas.SetLeft(rec, x+10);
                      Canvas.SetTop(rec, y+10);*/

                DrawImage(_rockImage, x, y);
            }
            else if (id == 0x00001100)
            {
                // _canvas.Children

                DrawImage(_plantImage, x, y);
            }
            else if (id == 0x10001100)
            {
                DrawImage(_growingPlantImage, x, y);
            }
            else if (id == 0x20001100)
            {
                DrawImage(_dryplantImage, x, y);
            }
            else if (id == 0x00001300)
            {
                // _canvas.Children

                DrawImage(_stoneAxeImage, x, y);
            }
            else if (id == 0x00001400)
            {
                // _canvas.Children

                DrawImage(_logImage, x, y);
            }
            else if (id == 0x00000600)
            {
                // _canvas.Children
                DrawImage(_fireImage, x, y);
            }
            else if (id == 0x00000700)
            {
                // _canvas.Children
                DrawImage(_appleImage, x, y);
            }
            else if (id == 0x00000800)
            {
                // _canvas.Children
                DrawImage(_branchImage, x, y);
            }
            else if (id == 0x00001200)
            {
                // _canvas.Children
                DrawImage(_bushImage, x, y);
            }
            else if (id == 0x00000900)
            {
                // _canvas.Children
                DrawImage(_raspberryImage, x, y);
            }
            else if (id == 0x00001500)
            {
                // _canvas.Children
                DrawImage(_attenuatingFireImage, x, y);
            }
            else if (id == 0x00001600)
            {
                // _canvas.Children
                DrawImage(_spruceTreeImage, x, y);
            }
            else if (id == 0x00001700)
            {
                // _canvas.Children
                DrawImage(_coneImage, x, y);
            }
            else if (id == 0x00001900)
            {
                // _canvas.Children
                DrawImage(_mushroomImage, x, y);
            }
            else if (id == 0x10001900)
            {
                // _canvas.Children
                DrawImage(_growingMushroomImage, x, y);
            }
            else if (id == 0x00001A00)
            {
                // _canvas.Children
                DrawImage(_roastedMushroomImage, x, y);
            }
            else if (id == 0x00001B00)
            {
                // _canvas.Children
                DrawImage(_roastedAppleImage, x, y);
            }
            else if (id == 0x00001C00)
            {
                // _canvas.Children
                DrawImage(_twigImage, x, y);
            }
            else if ((id / 0x100) == 0x00001D)
            {
                var innerId = id % 0x10;

                switch (innerId)
                {
                    case 0: DrawImage(_grassBed0, x, y); break;
                    case 1: DrawImage(_grassBed1, x, y); break;
                    case 2: DrawImage(_grassBed2, x, y); break;
                    case 3: DrawImage(_grassBed3, x, y); break;
                }
                // _canvas.Children
            }
            else if ((id / 0x100) == 0x00001E)
            {
                var innerId = id % 0x10;

                switch (innerId)
                {
                    case 0: DrawImage(_wickiupImage0, x, y); break;
                    case 1: DrawImage(_wickiupImage1, x, y); break;
                    case 2: DrawImage(_wickiupImage2, x, y); break;
                    case 3: DrawImage(_wickiupImage3, x, y); break;
                    case 4: DrawImage(_wickiupImage4, x, y); break;
                    case 5: DrawImage(_wickiupImage5, x, y); break;
                    case 6: DrawImage(_wickiupImage6, x, y); break;
                }
                // _canvas.Children
            }
            else if (id == 0x00002200)
            {
                // _canvas.Children
                DrawImage(_sharpStoneImage, x, y);
            }
            else if (id == 0x00002300)
            {
                // _canvas.Children
                DrawImage(_diggingStickImage, x, y);
            }
            else if (id == 0x00002400)
            {
                // _canvas.Children
                DrawImage(_rootImage, x, y);
            }
            else if (id == 0x00002500)
            {
                // _canvas.Children
                DrawImage(_nutTreeImage, x, y);
            }
            else if (id == 0x00002600)
            {
                // _canvas.Children
                DrawImage(_nutImage, x, y);
            }
            else if ((id / 0x1000) == 0x00018)
            {
                DrawRotatedImage(_dikabrozikImage, x, y, id % 0x1000);
            }
            else if ((id / 0x1000) == 0x10018)
            {
                DrawRotatedImage(_dikabrozikWithBundleImage, x, y, id % 0x1000);
            }
            else if (id == 0x00002000)
            {
                // _canvas.Children
                Rectangle rec = new Rectangle { Fill = Brushes.DarkBlue, Stroke = Brushes.DarkBlue, Height = 20, Width = 20 };
                _canvas.Children.Add(rec);
                Canvas.SetLeft(rec, x);
                Canvas.SetTop(rec, y);

            }
            else if (id == 0x00002100)
            {
                // _canvas.Children
                Rectangle rec = new Rectangle { Fill = Brushes.Blue, Stroke = Brushes.Blue, Height = 20, Width = 20 };
                _canvas.Children.Add(rec);
                Canvas.SetLeft(rec, x);
                Canvas.SetTop(rec, y);
            }/*
            else
            {
                // _canvas.Children
                Rectangle rec = new Rectangle() { Fill = Brushes.LightGreen, Stroke = Brushes.Black, Height = 20, Width = 20 };
                _canvas.Children.Add(rec);
                Canvas.SetLeft(rec, x);
                Canvas.SetTop(rec, y);
            }*/
        }

        public void DrawSurface(uint width, uint height)
        {
            /* Rectangle rec = new Rectangle() { Fill = Brushes.DarkBlue, Stroke = Brushes.Blue, Height = 40, Width = width };
             _canvas.Children.Add(rec);
             Canvas.SetLeft(rec, 0);
             Canvas.SetTop(rec, height - 40);*/
        }

        public void DrawMenu(int x, int y, IEnumerable<ClientAction> actions)
        {

            if (_canvas.ContextMenu == null)
            {
                _canvas.ContextMenu = new ContextMenu();
            }

            var cm = _canvas.ContextMenu;
            if (cm != null)
            {
                if (cm.IsOpen)
                    cm.IsOpen = false;

                cm.Items.Clear();

                foreach (var act in actions)
                {
                    var action = act;
                    var menuItem = new MenuItem
                    {
                        Header = action.Name,
                        IsEnabled = action.CanDo
                    };

                    menuItem.Click += (sender, args) => action.Do();

                    cm.Items.Add(menuItem);
                }

                cm.IsOpen = true;
            }
        }

        public void DrawContainer(IEnumerable<MenuItems> objects)
        {
            _listBox.Items.Clear();

            foreach (var gameObject in objects)
            {
                var image = new Image();
                image.Source = GetBitmapImageById(gameObject.Id);

                TextBlock gameObjecttextBlock = new TextBlock();

                gameObjecttextBlock.TextAlignment = TextAlignment.Center;
                gameObjecttextBlock.Text = gameObject.Name;
                gameObjecttextBlock.DataContext = gameObject.GetClientActions;
                gameObjecttextBlock.ContextMenuOpening += HandlerForCMO;

                _listBox.Items.Add(image);
                _listBox.Items.Add(gameObjecttextBlock);
            }
        }

        private ImageSource GetBitmapImageById(uint id)
        {
           // string name = key.Remove(key.IndexOf('('));
            switch (id)
            {
                case 0x00001100:
                    return _dryplantImage;
                case 0x00000700:
                    return _appleImage;
                case 0x00000800:
                    return _branchImage;
                case 0x00001000:
                    return _rockImage;
                case 0x00000100:
                    return _appletreeImage;
                case 0x00001200:
                    return _bushImage;
                case 0x00000900:
                    return _raspberryImage;
                case 0x00001300:
                    return _stoneAxeImage;
                case 0x00001700:
                    return _coneImage;
                case 0x00001900:
                    return _mushroomImage;
                case 0x00001A00:
                    return _roastedMushroomImage;
                case 0x00001B00:
                    return _roastedAppleImage;
                case 0x00001C00:
                    return _twigImage;
                case 0x00002300:
                    return _diggingStickImage;
                case 0x00002200:
                    return _sharpStoneImage;
                case 0x00002400:
                    return _rootImage;
                case 0x00002600:
                    return _nutImage;
            }
            return _appletreeImage;
        }

        public void DrawHeroProperties(IEnumerable<KeyValuePair<string, int>> objects)
        {
            _heroListBox.Items.Clear();

            foreach (var gameObject in objects)
            {
                _heroListBox.Items.Add($"{gameObject.Key} - {gameObject.Value}");
            }
        }

        public void DrawActing(bool showActing)
        {
            _drawCount = (_drawCount + 1) % 20;
            if (showActing)
            {
                if (_drawCount == 0)
                {
                    _acting.Text = _acting.Text.Length == 9 ? "Acting." : _acting.Text.Length == 8 ? "Acting..." : "Acting..";
                }

                _canvas.Children.Add(_acting);
                Canvas.SetLeft(_acting, _canvas.Width / 2);
                Canvas.SetTop(_acting, _canvas.Height / 2);
            }
        }

        public void DrawDayNight(double lightness, GameDateTime gameDateTime, List<BurningProps> lightObjects)
        {
            var drawingGroup = new DrawingGroup();

            // Create a DrawingBrush.
            DrawingBrush myDrawingBrush = new DrawingBrush();

            // Create a drawing.
            GeometryDrawing myBlackDrawing = new GeometryDrawing();
            // myGeometryDrawing.Brush
            myBlackDrawing.Brush = Brushes.Black;
            myBlackDrawing.Pen = new Pen(Brushes.Black, 1);
            GeometryGroup rectangle = new GeometryGroup {FillRule = FillRule.EvenOdd};
            rectangle.Children.Add(new RectangleGeometry(new Rect { Height = _canvas.Height, Width = _canvas.Width }));

            GeometryGroup rectangle11 = new GeometryGroup();
            rectangle11.FillRule = FillRule.Nonzero;
            foreach (var lightObject in lightObjects)
            {
                rectangle11.Children.Add(
                    new EllipseGeometry(new System.Windows.Point(lightObject.Point.X + 10, lightObject.Point.Y + 10),
                        20 * lightObject.LightRadius, 20 * lightObject.LightRadius));
            }
            rectangle.Children.Add(rectangle11);
            var combined = new CombinedGeometry(GeometryCombineMode.Exclude, rectangle, rectangle11);
            myBlackDrawing.Geometry = combined;

            drawingGroup.Children.Add(myBlackDrawing);
            myDrawingBrush.Drawing = drawingGroup;
            Rectangle rec = new Rectangle
            {
                Fill = myDrawingBrush,
                Stroke = Brushes.Black,
                Height = _canvas.Height,
                Width = _canvas.Width,
                Opacity = lightness,
                IsEnabled = false
            };

            _canvas.Children.Add(rec);
            Canvas.SetLeft(rec, 0);
            Canvas.SetTop(rec, 0);

            _listBoxDateTime.Items.Clear();
            _listBoxDateTime.Items.Add($"{gameDateTime.Day}:{gameDateTime.Hour}:{gameDateTime.Minute}");
        }

        public void DrawShaddow(Point innerPoint, Size innerSize)
        {
            var drawingGroup = new DrawingGroup();

            // Create a DrawingBrush.
            DrawingBrush myDrawingBrush = new DrawingBrush();

            // Create a drawing.
            GeometryDrawing myBlackDrawing = new GeometryDrawing();
            // myGeometryDrawing.Brush
            myBlackDrawing.Brush = Brushes.Black;
            myBlackDrawing.Pen = new Pen(Brushes.Black, 1);
            GeometryGroup rectangle = new GeometryGroup();
            rectangle.FillRule = FillRule.EvenOdd;
            rectangle.Children.Add(new RectangleGeometry(new Rect { Height = _canvas.Height, Width = _canvas.Width }));

            GeometryGroup rectangle11 = new GeometryGroup();
            rectangle11.FillRule = FillRule.Nonzero;

            rectangle11.Children.Add(
                    new RectangleGeometry(new Rect(new System.Windows.Point(innerPoint.X, innerPoint.Y), 
                    new System.Windows.Size(innerSize.Width * 20, innerSize.Height*20))));

            rectangle.Children.Add(rectangle11);
            var combined = new CombinedGeometry(GeometryCombineMode.Exclude, rectangle, rectangle11);
            myBlackDrawing.Geometry = combined;

            drawingGroup.Children.Add(myBlackDrawing);
            myDrawingBrush.Drawing = drawingGroup;
            Rectangle rec = new Rectangle
            {
                Fill = myDrawingBrush,
                Stroke = Brushes.Black,
                Height = _canvas.Height,
                Width = _canvas.Width,
                Opacity = 0.5,
                IsEnabled = false
            };

            _canvas.Children.Add(rec);
            Canvas.SetLeft(rec, 0);
            Canvas.SetTop(rec, 0);
        }

        private void CreateActing()
        {
            _acting.Foreground = Brushes.FloralWhite;
            _acting.TextAlignment = TextAlignment.Center;
            _acting.FontSize = 16;

            _acting.Text =
                "Acting...";
        }

        private void DrawImage(BitmapSource bitmapImage, long x, long y)
        {
            var image = new Image {Source = bitmapImage};
            _canvas.Children.Add(image);
            Canvas.SetLeft(image, x);
            Canvas.SetTop(image, y);
        }

        private void DrawRotatedImage(BitmapImage bitmapImage, long x, long y, uint number)
        {
            var appearance = new Path { Fill = Brushes.Yellow, Stroke = Brushes.Brown, Height = 16, Width = 16 };
            Canvas.SetTop(_appearance, 0);
            Canvas.SetLeft(_appearance, 0);
            var image = new Image();
            image.Source = bitmapImage;
            image.RenderTransform = new RotateTransform((int)number - 90);
            image.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
            _canvas.Children.Add(image);
            Canvas.SetLeft(image, x);
            Canvas.SetTop(image, y);
        }

        void HandlerForCMO(object sender, ContextMenuEventArgs e)
        {
            FrameworkElement fe = e.Source as FrameworkElement;
            IEnumerable<ClientAction> actions = ((Func<IEnumerable<ClientAction>>)fe.DataContext)();
            fe.ContextMenu = BuildMenu(actions);
        }
        ContextMenu BuildMenu(IEnumerable<ClientAction> actions)
        {
            ContextMenu theMenu = new ContextMenu();
            // var actions = this.
            foreach (var act in actions)
            {
                var action = act;
                var menuItem = new MenuItem
                {
                    Header = action.Name,
                    IsEnabled = action.CanDo
                };

                menuItem.Click += (sender, args) => action.Do();

                theMenu.Items.Add(menuItem);
            }

            theMenu.IsOpen = true;

            return theMenu;
        }

        #endregion
    }
}
