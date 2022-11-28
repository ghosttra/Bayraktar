using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using BayraktarGame;

namespace Bayraktar
{
    public partial class MilitaryUnit : UserControl
    {
        private Unit _unit { get; set; }
        public MilitaryUnit(Unit unit)
        {
            this._unit = unit;
            InitializeComponent();
            _setImageSource(UnitImg, _unit.Image);
            _setImageSource(UnitImg, _unit.Image);
        }

        public double UnitWidth => UnitImg.Width;
        public double UnitHeight => UnitImg.Height;
        
        

        public DoubleAnimation Animation;
       
        private void _setImageSource(Image target, byte[] source)
        {
            if (source == null)
                return;
            target.Source = (BitmapSource)new ImageSourceConverter().ConvertFrom(source);
        }
        public bool IsDestroying;
        public Action<MilitaryUnit> Destroyed;
        public void Destroy()
        {
            IsDestroying = true;
            _setImageSource(UnitImg, _unit.ImageDestroyed);
            
            Destroyed?.Invoke(this);
        }
    }
}
