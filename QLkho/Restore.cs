using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace QLkho
{
    public partial class Restore : DevExpress.XtraEditors.XtraForm
    {
        public Restore()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "SQL SEVER database backup file|*.bak";
            dlg.Title = "Phục hồi Database";
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                textEdit1.Text = dlg.FileName;
                simpleButton2.Enabled = true;
            }    
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = string.Format("alter database [QLKhoIT] set single_user with rollback immediate");
                ConnectDB.Query(sql);
            }catch
            {

            }
        }
    }
}