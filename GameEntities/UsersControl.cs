using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BayraktarGame;
using GameEntities;

namespace GameServerInterface
{
    public partial class UsersControl : Form
    {
        public UsersControl()
        {
            InitializeComponent();
            _init();
        }

        private void _init()
        {
            usersDgv.AutoGenerateColumns = false;
            usersDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
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
            usersDgv.DataSource = _dataBase.Users.ToList();
        }


        private void dbButtons_Load(object sender, EventArgs e)
        {
            dbButtons.delBtn.Click += DelBtn_Click;
            dbButtons.updBtn.Click += UpdBtn_Click;
            dbButtons.addBtn.Click += AddBtn_Click;
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if(!_check())
                return;
            _addNewUser();
            _updateDgvAsync();
        }

        private void _addNewUser()
        {
            var newUser = _newUser();
            if(newUser == null)
                return;
            passwordBox.Text = String.Empty;
            _dataBase.Users.Add(newUser);
            _dataBase.SaveChanges();
        }

        private User _newUser()
        {
            if (_dataBase.Users.Any(u => u.Login.Equals(loginBox.Text)))
            {
                tip.Show("Login has already exist", loginBox);
                return null;
            }
            var user = new User();
            _fillUser(user);
            return user;
        }

        private void UpdBtn_Click(object sender, EventArgs e)
        {
            if (!_check())
                return;
            if (usersDgv.SelectedRows.Count == 0)
                return;
            if (!(usersDgv.SelectedRows[0].DataBoundItem is User user)) return;
            _fillUser(user);
            passwordBox.Text = String.Empty;
            _dataBase.SaveChanges();
            _updateDgvAsync();
        }

        private void _fillUser(User user)
        {
            user.Login = loginBox.Text;
            user.PassWord = passwordBox.Text.GetHashCode().ToString();
        }

        private bool _check()
        {
            if (string.IsNullOrEmpty(loginBox.Text))
            {
                tip.Show("Empty box", loginBox);
                return false;
            }
          
            if (string.IsNullOrEmpty(passwordBox.Text))
            {
                tip.Show("Empty Password", passwordBox);
                return false;
            }
            
            return true;
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if (usersDgv.SelectedRows.Count == 0)
                return;
            if (!(usersDgv.SelectedRows[0].DataBoundItem is User user)) return;
            _dataBase.Users.Remove(user);
            _dataBase.SaveChanges();
            _updateDgvAsync();

        }

        private void usersDgv_SelectionChanged(object sender, EventArgs e)
        {
            if (usersDgv.SelectedRows.Count == 0)
                return;
            _fillBoxes();
        }

        private void _fillBoxes()
        {
            var user = usersDgv.SelectedRows[0].DataBoundItem as User;
            if(user == null)
                return;
            loginBox.Text = user.Login;
        }
    }
}
