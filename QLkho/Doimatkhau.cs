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
    public partial class Doimatkhau : DevExpress.XtraEditors.XtraForm
    {
        public Doimatkhau()
        {
            InitializeComponent();
        }

        private void Doimatkhau_Load(object sender, EventArgs e)
        {
            user = Form1.taikhoan;
            txtuser.Text = user;
        }

        private void bnt_doimk_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = @"select * from NhanVien where username = '" + user + "' and password = '" + txtpassht.Text + "'";
                DataTable data = ConnectDB.getTable(sql);
                if (data.Rows.Count <= 0)
                {
                    MessageBox.Show("Mật khẩu hiện tại sai !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                if (txtpassmoi.Text != txtxacnhan.Text)
                {
                    MessageBox.Show("Xác nhận lại mật khẩu mới không đúng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string sql1 = @"update NhanVien set password='" + txtpassmoi.Text + "' where username= '" + user + "'";
                    if (ConnectDB.Query(sql1) == -1)
                    {
                        MessageBox.Show("Đổi mật khẩu không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Đổi mật khẩu thành công (^-^)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string user = "";
        private bool validate()
        {
            if (txtpassht.Text == "" || txtpassmoi.Text == "" || txtxacnhan.Text == "")
            {
                MessageBox.Show("Bạn phải điền đầy đủ các trường !", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
    }
}