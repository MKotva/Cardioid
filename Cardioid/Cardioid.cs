using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace Carcioid
{
    class Cardioid
    {
        public Bitmap _canvas;
        private int _canvasCenter;
        private double _radius;
        private int _ellCoord; //Coordinates of ellipse
        private double _factor;
        private Color[] _colorList;
        private Color _finalColor;
        private Color _actualColor;
        private bool _isColorChanging;
        private Random _rnd;

        public Cardioid(int? size = 1900) //Its square.
        {
            _canvas = new Bitmap(size.Value, size.Value);
            _canvasCenter = _canvas.Width / 2;
            _radius = (double)(size.Value) / 2 - 16.0;
            _ellCoord = (_canvas.Width / 2) - ((int)(_radius));
            _factor = 0;
            
            var properties = typeof(Color).GetProperties().Where(x => x.PropertyType == typeof(Color));
            var colorList = new List<Color>();
            foreach(var prop in properties)
            {
                colorList.Add((Color)prop.GetValue(prop.Name));
            }
            _colorList = colorList.ToArray();

            _rnd = new Random();
            _finalColor = _colorList[_rnd.Next(0, _colorList.Length)];
            _actualColor = _colorList[_rnd.Next(0, _colorList.Length)];
            _isColorChanging = false;
        }

        /// <summary>
        /// Normalize value from fRange to tRange.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fRangeStart"></param>
        /// <param name="fRangeEnd"></param>
        /// <param name="tRangeStart"></param>
        /// <param name="tRangeEnd"></param>
        /// <returns></returns>
        /// 
        private double Map(double value, double fRangeStart, double fRangeEnd, double tRangeStart, double tRangeEnd)
        {
            return (((value - fRangeStart) / (fRangeEnd - fRangeStart)) * (tRangeEnd - tRangeStart)) + tRangeStart;
        }


        /// <summary>
        /// Set and draw canvas.
        /// </summary>
        public Bitmap DrawCardioid(double? focusCount = 200.0, double? factor = 0.015)
        {
            Graphics g = Graphics.FromImage(_canvas);
            g.Clear(Color.Black);

            if (_factor == 0.015 && factor == -0.015)
            {
                _factor = 0.015;
            }
            else
            {
                _factor += factor.Value;
            }

            using (Pen myPen = new Pen(Color.White))
            {
                g.DrawEllipse(myPen, _ellCoord, _ellCoord, (int)(_radius) * 2, (int)(_radius) * 2);
            }

            using (Pen myPen = new Pen(_actualColor)) //TODO: Change color system;
            {
                myPen.Width = 1;
                for (int i = 0; i < focusCount.Value; i++)
                {
                    var a = GetVector(i, focusCount.Value);
                    var b = GetVector(i * _factor, focusCount.Value);
                    g.DrawLine(myPen, _canvasCenter + a.X, _canvasCenter + a.Y, _canvasCenter + b.X, _canvasCenter + b.Y);
                }
            }
            
            if(_isColorChanging)
                SetTransitionColor();
            return new Bitmap(_canvas);
        }

        private Vector2 GetVector(double index, double focusCout)
        {
            double angle = Map(index % focusCout, 0, focusCout, 0, Math.PI * 2);
            var nVector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            nVector = Vector2.Multiply(nVector, (float)_radius);
            return nVector;
        }

        /// <summary>
        /// Sets colors closer to the final color.
        /// </summary>
        private void SetTransitionColor()
        {
            var newR = 0;
            var newG = 0;
            var newB = 0;
            var newA = 0;

            if(_actualColor.ToArgb() == _finalColor.ToArgb())
            {
                SetRandomFinalColor();
            }

            //Red
            if (_actualColor.R > _finalColor.R)
                newR = _actualColor.R - 1;
            else if (_actualColor.R < _finalColor.R)
                newR = _actualColor.R + 1;
            else
                newR = _actualColor.R;

            //Green
            if (_actualColor.G > _finalColor.G)
                newG = _actualColor.G - 1;
            else if (_actualColor.G < _finalColor.G)
                newG = _actualColor.G + 1;
            else
                newG = _actualColor.G;

            //Blue
            if (_actualColor.B > _finalColor.B)
                newB = _actualColor.B - 1;
            else if (_actualColor.B < _finalColor.B)
                newB = _actualColor.B + 1;
            else
                newB = _actualColor.B;

            //Alpha
            if (_actualColor.A > _finalColor.A)
                newA = _actualColor.A - 1;
            else if (_actualColor.A < _finalColor.A)
                newA = _actualColor.A + 1;
            else
                newA = _actualColor.A;

            _actualColor = Color.FromArgb(newA, newR, newG, newB);
        }

        /// <summary>
        /// Sets random final color
        /// </summary>
        private void SetRandomFinalColor()
        {
            _finalColor = _colorList[_rnd.Next(0, _colorList.Length)];
        }

        public void SetColor(Color color)
        {
            _actualColor = color;
            _isColorChanging = false;
        }

        public void SetChanging()
        {
            _isColorChanging = true;
        }
    }
}
