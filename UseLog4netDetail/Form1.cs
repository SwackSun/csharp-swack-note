﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UseLog4netDetail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int value = 1 / int.Parse("0");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(Form1), ex);
            }
        }
    }
}