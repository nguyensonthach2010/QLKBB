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
    public partial class Dangnhap : DevExpress.XtraEditors.XtraForm
    {
        public Dangnhap()
        {
            InitializeComponent();
        }
        private bool validate()
        {   //hàm kiểm tra dữ liệu nhập vào có rỗng hay k
            if (txtusername.Text == "" || txtpass.Text == "")
            {
                XtraMessageBox.Show("Bạn phải điền đầy đủ các trường !", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        public static string tk = ""; // tạo biến public để truyền tham số của biến username sang các form khác
        private void btn_dn_Click(object sender, EventArgs e)
        {
            try
            {
                if (validate())  //kiểm tra dữ liệu k rỗng thì :
                {
                    //khai báo chuỗi lệnh sql
                    string sql = @"select * from NhanVien where username = '" + txtusername.Text + "' and password = '" + txtpass.Text + "'";
                    DataTable data = ConnectDB.getTable(sql);

                    if (data.Rows.Count <= 0)  //gọi hàm getTable từ lớp DbHelper có giá trị truyền vào là chuỗi lênh select để lấy thông tin từ bảng nếu có số dòng <= 0 thì:
                    {
                        XtraMessageBox.Show("Sai tài khoản hoặc mật khẩu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else  //nếu số dòng lấy được > 0 thì :
                        if(data.Rows[0]["trangthai"].ToString() == "Active") // kiểm tra dữ liệu cột trạng thái trong sql nếu là Active thì mới được quyền truy cập
                        {
                            XtraMessageBox.Show("Đăng nhập thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Quyen.nhomnd = data.Rows[0]["nhomnd"].ToString();
                            ;
                            tk = txtusername.Text;
                            this.Visible = false;  //cho form này ẩn đi
                            new Form1().ShowDialog();  //hiện form chinh
                            string sql1 = "insert into LichSu values('" + txtusername.Text + "','Login','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')"; // thêm lich sử hoạt động là Login của username vào bảng lịch sử User
                            ConnectDB.Query(sql1);
                        }else
                    {
                        XtraMessageBox.Show("Tài khoản này hiện đang bị khoá. Liên hệ admin để mở khoá tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (tb == DialogResult.Yes)
                Application.Exit();
        }

        private void Dangnhap_Load(object sender, EventArgs e)
        {
            DevExpress.LookAndFeel.DefaultLookAndFeel themes = new DevExpress.LookAndFeel.DefaultLookAndFeel(); // cài đặt giao diện mặc định cho form
            themes.LookAndFeel.SkinName = "Glass Oceans";
        }
    }
}