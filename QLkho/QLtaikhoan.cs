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
    public partial class QLtaikhoan : DevExpress.XtraEditors.XtraForm
    {
        public QLtaikhoan()
        {
            InitializeComponent();
        }
        private bool validate()
        {   //hàm kiểm tra dữ liệu nhập vào có rỗng hay k
            if (txtmanv.Text == "" || txttennv.Text == "" || txtusername.Text == "" || txtpassword.Text == "")
            {
                XtraMessageBox.Show("Bạn phải điền đầy đủ các trường !", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void hien()
        {
            try
            {
                string sql = "select *from NhanVien";
                gridControl1.DataSource = ConnectDB.getTable(sql);
            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            
        }

        private void QLtaikhoan_Load(object sender, EventArgs e)
        {
            hien();
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (validate())
                {
                    string sql = "insert into NhanVien values('" + txtmanv.Text + "',N'" + txttennv.Text + "','" + txtusername.Text + "','" + txtpassword.Text + "',N'" + cb_quyen.Text + "')";

                    if (ConnectDB.Query(sql) == -1)
                    {
                        XtraMessageBox.Show("Thêm không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Thêm thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        hien();
                    }
                }
            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            try
            {
                DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn sửa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (tb == DialogResult.Yes)
                {
                    if (validate())
                    {
                        string sql = "update NhanVien set tennv = N'" + txttennv.Text + "', username ='" + txtusername.Text + "',password='" + txtpassword.Text + "',nhomnd=N'" + cb_quyen.Text + "' where manv = '" + txtmanv.Text + "'";

                        if (ConnectDB.Query(sql) == -1)
                        {
                            XtraMessageBox.Show("Cập nhật thông tin không thành công. Hãy load lại dữ liệu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            XtraMessageBox.Show("Cập nhật thông tin thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            hien();
                        }
                    }
                }
            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            try
            {
                DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (tb == DialogResult.Yes)
                {
                    if (validate())
                    {
                        string sql = "delete from NhanVien where manv = '" + txtmanv.Text + "'";

                        if (ConnectDB.Query(sql) == -1)
                        {
                            XtraMessageBox.Show("Xóa thông tin không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            XtraMessageBox.Show("Xóa thông tin thành công (^-^)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            hien();
                        }
                    }
                }
            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

           
        }

        private void simpleButton5_Click_1(object sender, EventArgs e)
        {
            txtmanv.Text = "";
            txttennv.Text = "";
            txtusername.Text = "";
            txtpassword.Text = "";
            cb_quyen.Text = "";
            txtmanv.ReadOnly = false;
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            hien();
        }

        private void gridControl1_Click_1(object sender, EventArgs e)
        {
            txtmanv.ReadOnly = true;
            txtmanv.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "manv").ToString();
            txttennv.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "tennv").ToString();
            txtusername.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "username").ToString();
            txtpassword.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "password").ToString();
            cb_quyen.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "nhomnd").ToString();
        }
    }
}