using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carcioid
{
    public partial class Form1 : Form
    {
        private Cardioid _cardioid;
        private bool IsPlaying;
        private double _factor;
        private bool _isRandomColor;
        private bool _shouldSave;
        private int _fileIndex;
        private double _focusCount;

        public Form1()
        {
            InitializeComponent();
            _cardioid = new Cardioid();
            _fileIndex = 0;
            _focusCount = 250;
        }

        private async void playButton_Click(object sender, EventArgs e)
        {
            IsPlaying = true;
            _factor = 0.015;
            if(!_isRandomColor)
            {
                _cardioid.SetColor(GetColor());
            }

            if(focusCountTextBox.Text != "")
            {
                _focusCount = Convert.ToDouble(focusCountTextBox.Text);
            }

            if(SizeTextBox.Text != "")
            {
                var size = Convert.ToInt32(SizeTextBox.Text);
                if (size > 100 && size != _cardioid._canvas.Width)
                {
                    _cardioid = new Cardioid(size);
                    if (_isRandomColor)
                        _cardioid.SetChanging();
                }
            }

            while (IsPlaying)
            { 
                var bitmap = _cardioid.DrawCardioid(_focusCount, _factor);
                pictureBox1.Image = bitmap;
                pictureBox1.Show();
                if (_shouldSave)
                {
                    Save(bitmap);
                }
                await Task.Delay(5);
            }
        }

        private Color GetColor()
        {
            var newR = 0;
            var newB = 0;
            var newG = 0;

            if (RTextBox.Text != "")
            {
                var r = Convert.ToInt32(RTextBox.Text);
                if (r < 255 && r > 0)
                    newR = r;
                else return Color.Red;
            }

            if (RTextBox.Text != "")
            {
                var g = Convert.ToInt32(RTextBox.Text);
                if (g < 255 && g > 0)
                    newG = g;
                else return Color.Red;
            }

            if (RTextBox.Text != "")
            {
                var b = Convert.ToInt32(RTextBox.Text);
                if (b < 255 && b > 0)
                    newB = b;
                else return Color.Red;
            }
            return Color.Red;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            IsPlaying = false;
        }

        private void forwardButton_Click(object sender, EventArgs e)
        {
            _factor = 0.015;
        }

        private void backwardButoon_Click(object sender, EventArgs e)
        {
            _factor = -0.015;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(_isRandomColor)
            {
                RLabel.Visible = true;
                GLabel.Visible = true;
                BLabel.Visible = true;
                RTextBox.Visible = true;
                GTextBox.Visible = true;
                BTextBox.Visible = true;
                _isRandomColor = false;
                _cardioid.SetColor(GetColor());
            }
            else
            {
                RLabel.Visible = false;
                GLabel.Visible = false;
                BLabel.Visible = false;
                RTextBox.Visible = false;
                GTextBox.Visible = false;
                BTextBox.Visible = false;
                _isRandomColor = true;
                _cardioid.SetChanging();
            }
        }

        private void Save(Bitmap bitmap)
        {
            bitmap.Save(String.Format("{0}.png", _fileIndex), ImageFormat.Png);
            _fileIndex++;
        }

        private void SaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_shouldSave)
                _shouldSave = false;
            else
                _shouldSave = true;
        }
    }
}
