using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BayraktarGame;
using GameEntities;

namespace GameServerInterface
{
    public partial class UnitsEdit : Form
    {
        public UnitsEdit()
        {
            InitializeComponent();
        }


        private void _init()
        {
            unitsDGV.AutoGenerateColumns = false;
            unitsDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _updateDgv();

        }

        private GameContext _dataBase = new GameContext();

        private Task _updateDgvAsync() => Task.Factory.StartNew(() =>
        {
            if (InvokeRequired)
                Invoke(new Action(_updateDgv));
            else
            {
                _updateDgv();
            }
        });
        private void _updateDgv()
        {
            unitsDGV.DataSource = _dataBase.Units.ToList();
        }


        private void _uploadPic(PictureBox box)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image|*.png;*.jpg";
            openFileDialog.Title = "Select Picture";
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            try
            {
                Bitmap b = new Bitmap(Image.FromStream(openFileDialog.OpenFile()), new Size(300, 300));
                box.Image = b;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                MessageBox.Show(e.Message);
            }
        }
        private void normalPicBtn_Click(object sender, EventArgs e)
        {
            _uploadPic(normalPic);
        }

        private void destroyedPicBtn_Click(object sender, EventArgs e)
        {
            _uploadPic(destroyedPic);
        }

        #region Add

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (!_check())
                return;
            _addNewUnit();
            _updateDgvAsync();
        }

        private void _addNewUnit()
        {
            Unit unit = _newUnit();
            _dataBase.Units.Add(unit);
            _dataBase.SaveChanges();
        }

        private Unit _newUnit()
        {
            ImageConverter converter = new ImageConverter();

            Unit unit = new Unit
            {
                Name = nameBox.Text,
                CoolDown = decimal.ToInt32(coolDownNum.Value),
                Price = decimal.ToInt32(priceNum.Value),
                Image = (byte[])converter.ConvertTo(normalPic.Image, typeof(byte[])),
                ImageDestroyed = (byte[])converter.ConvertTo(destroyedPic.Image, typeof(byte[]))
            };

            return unit;
        }


        private bool _check()
        {
            if (string.IsNullOrEmpty(nameBox.Text))
            {
                tip.Show("Empty box", nameBox);
                return false;
            }

            if (priceNum.Value < 0)
            {
                tip.Show("Incorrect Price", priceNum);
                return false;
            }
            if (coolDownNum.Value < 0)
            {
                tip.Show("Incorrect Cooldown", coolDownNum);
                return false;
            }

            if (normalPic.Image == null)
            {
                tip.Show("No pic", normalPic);
                return false;
            }
            if (destroyedPic.Image == null)
            {
                tip.Show("No pic", destroyedPic);
                return false;
            }

            return true;
        }

        #endregion

        private void updBtn_Click(object sender, EventArgs e)
        {
            if (!_check())
                return;
            if (unitsDGV.SelectedRows.Count == 0)
                return;
            if (!(unitsDGV.SelectedRows[0].DataBoundItem is Unit unit)) return;
            _fillUnit(unit);
            _dataBase.SaveChanges();
            _updateDgvAsync();
        }

        private void _fillUnit(Unit unit)
        {
            ImageConverter converter = new ImageConverter();

            unit.CoolDown = decimal.ToInt32(coolDownNum.Value);
            unit.Price = decimal.ToInt32(priceNum.Value);
            unit.Name = nameBox.Text;
            unit.Image = (byte[])converter.ConvertTo(normalPic.Image, typeof(byte[]));
            unit.ImageDestroyed = (byte[])converter.ConvertTo(destroyedPic.Image, typeof(byte[]));

        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (unitsDGV.SelectedRows.Count == 0)
                return;
            if (!(unitsDGV.SelectedRows[0].DataBoundItem is Unit unit)) return;
            _dataBase.Units.Remove(unit);
            _dataBase.SaveChanges();
            _updateDgvAsync();
        }

        private void unitsDGV_SelectionChanged(object sender, EventArgs e)
        {
            if (unitsDGV.SelectedRows.Count == 0)
                return;
            _fillBoxes();
        }

        private void _fillBoxes()
        {
            var unit = unitsDGV.SelectedRows[0].DataBoundItem as Unit;
            if (unit == null)
                return;
            nameBox.Text = unit.Name;
            priceNum.Value = unit.Price;
            coolDownNum.Value = unit.CoolDown;
            _bytesToBox(unit.Image, normalPic);
            _bytesToBox(unit.ImageDestroyed, destroyedPic);
        }

        private void _bytesToBox(byte[] data, PictureBox box)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                box.Image = Image.FromStream(stream);
            }
        }

        private void UnitsEdit_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                if (InvokeRequired)
                    Invoke(new Action(_init));
                else
                {
                    _init();
                }
            });
        }

        private void dbButtons_Load(object sender, EventArgs e)
        {
            dbButtons.delBtn.Click += delBtn_Click;
            dbButtons.addBtn.Click += addBtn_Click;
            dbButtons.updBtn.Click += updBtn_Click;
        }
    }
}
