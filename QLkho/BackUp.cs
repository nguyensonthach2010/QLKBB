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
    public partial class BackUp : DevExpress.XtraEditors.XtraForm
    {
        public BackUp()
        {
            InitializeComponent();
        }
        ConnectDB con = new ConnectDB();
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textEdit11.Text = dlg.SelectedPath;
                simpleButton2.Enabled = true;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if(textEdit11.Text == string.Empty)
                {
                    XtraMessageBox.Show("Vui lòng chọn đường dẫn lưu file backup","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else
                {
                    string sql =  "BACKUP DATABASE [QLKhoBB] TO DISK ='"+textEdit11.Text+"\\"+"DATABASE"+"-"+DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") +".bak'";
                    ConnectDB.Query(sql);
                    XtraMessageBox.Show("Back up dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    simpleButton2.Enabled = false;
                }    
            }catch
            {
                XtraMessageBox.Show("Có lỗi xảy ra!. ", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BackUp_Load(object sender, EventArgs e)
        {

        }
    }
}