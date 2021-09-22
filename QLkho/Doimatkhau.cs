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
            user = Form1.taikhoan; // tạo biến user để nhận dữ liệu từ biến public taikhoan từ form chính, gán dữ liệu cho textbox txtuser
            txtuser.Text = user;  
        }

        private void bnt_doimk_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = @"select * from NhanVien where username = '" + user + "' and password = '" + txtpassht.Text + "'";  // lọc Nhân viên có username và password nhập vào
                DataTable data = ConnectDB.getTable(sql);
                if (data.Rows.Count <= 0)
                {
                    XtraMessageBox.Show("Mật khẩu hiện tại sai !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                if (txtpassmoi.Text != txtxacnhan.Text) // kiểm tra người dùng nhập mã mới có trùng với mã xác nhận không
                {
                    MessageBox.Show("Xác nhận lại mật khẩu mới không đúng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string sql1 = @"update NhanVien set password='" + txtpassmoi.Text + "' where username= '" + user + "'"; // cập nhật mật khẩu mới
                    if (ConnectDB.Query(sql1) == -1)
                    {
                        XtraMessageBox.Show("Đổi mật khẩu không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        XtraMessageBox.Show("Đổi mật khẩu thành công (^-^)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        string sql2 = "insert into LichSu values('" + user + "',N'Đổi mật khẩu thành ("+txtpassmoi.Text+ ")','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')"; // Ghi lại thao tác thay đổi mật khẩu vào bảng LichSu
                        ConnectDB.Query(sql2);
                        this.Close();
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
            // hàm kiểm tra các textbox có rỗng hay không
            if (txtpassht.Text == "" || txtpassmoi.Text == "" || txtxacnhan.Text == "")
            {
                XtraMessageBox.Show("Bạn phải điền đầy đủ các trường !", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                return false;
            }
            return true;
        }
    }
}