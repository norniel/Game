using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Game;
using Point = Engine.Point;

namespace Brjozik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private Engine.Game _game;
        private readonly WpfDrawer _drawer;
        public MainWindow()
        {
            InitializeComponent();
            /*
            CultureInfo newCulture = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
            */
            _drawer = new WpfDrawer(canvas1, listBox1, heroListBox, listBoxDateTime);
            _game = new Engine.Game(_drawer, (uint)canvas1.Width, (uint)canvas1.Height);

            CompositionTarget.Rendering += DrawSnapshot;
        }

        // Called just before frame is rendered to allow custom drawing.
        protected void DrawSnapshot(object sender, EventArgs e)
        {
            _game.DrawChanges();
        }

        private void canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _game.LClick(new Point((int)e.GetPosition(this).X, (int)e.GetPosition(this).Y));
        }

        private void canvas1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _game.RClick(new Point((int)e.GetPosition(this).X, (int)e.GetPosition(this).Y));
        }
    }
}
