using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using BayraktarGame;

namespace Bayraktar
{
    public partial class MilitaryUnit : UserControl
    {
        private readonly Unit _unit;
        public MilitaryUnit(Unit unit)
        {
            this._unit = unit;
            InitializeComponent();
            _setImageSource(UnitImg, _unit.Image);
        }

        public double X =>UnitImg.Width;
        public double Y =>UnitImg.Height;
        private void _setImageSource(Image target, byte[] source)
        {
            if (source == null)
                return;
            target.Source = (BitmapSource)new ImageSourceConverter().ConvertFrom(source);
        }
        private void MU_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Destroy();
            ((Image)sender).MouseLeftButtonUp -= MU_MouseLeftButtonUp;
        }

        public Action IsDestroyed;
        public void Destroy()
        {
            _setImageSource(UnitImg, _unit.ImageDestroyed);
            IsDestroyed?.Invoke();
        }
    }
}
