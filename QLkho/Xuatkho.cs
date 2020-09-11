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
    public partial class Xuatkho : DevExpress.XtraEditors.XtraForm
    {
        public Xuatkho()
        {
            InitializeComponent();
        }
        private bool validate()
        {   //hàm kiểm tra dữ liệu nhập vào có rỗng hay k
            if (txtmahd.Text == "" || txtlosx.Text == "" || txtdgxuat.Text == "" || txtnguoixuat.Text == "" || txtslxuat.Text == "" || txtvitri.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin !", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void hien()
        {
            try
            {
                string sql = "SELECT sohd , tenvt,losx,vitri,slxuat,dgxuat,ngayxuat,username,nguoixuat,(slxuat*dgxuat) as thanhtien FROM XuatKho INNER JOIN VatTu ON VatTu.mavt = XuatKho.mavt INNER JOIN NhanVien ON NhanVien.manv = XuatKho.manv";
                gridControl1.DataSource = ConnectDB.getTable(sql);
            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void Xuatkho_Load(object sender, EventArgs e)
        {
            userxuat = Form1.taikhoan;
            string sql1 = "select *from NhanVien where username ='" + userxuat + "'";
            cb_nhanvien.Properties.DataSource = ConnectDB.getTable(sql1);
            cb_nhanvien.Properties.DisplayMember = "username";
            cb_nhanvien.Properties.ValueMember = "manv";

            string sql2 = "select*from VatTu";
            cb_tenvt.Properties.DataSource = ConnectDB.getTable(sql2);
            cb_tenvt.Properties.DisplayMember = "tenvt";
            cb_tenvt.Properties.ValueMember = "mavt";
            hien();
            DateTime dt = DateTime.Now;
            date_xuat.DateTime = dt;
        }
        string userxuat = "";

        private void gridControl1_Click(object sender, EventArgs e)
        {
            txtmahd.ReadOnly = true;
            txtmahd.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "sohd").ToString();
            cb_tenvt.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "tenvt").ToString();
            txtlosx.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "losx").ToString();
            txtvitri.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "vitri").ToString();
            txtslxuat.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "slxuat").ToString();
            txtdgxuat.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "dgxuat").ToString();
            txtnguoixuat.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "nguoixuat").ToString();
            cb_nhanvien.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "username").ToString();
            date_xuat.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ngayxuat").ToString();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (validate())
                {
                    string sql = "insert into XuatKho values('" + txtmahd.Text + "',N'" + cb_tenvt.EditValue.ToString() + "',N'" + txtlosx.Text + "','" + txtvitri.Text + "','" + txtslxuat.Text + "','" + txtdgxuat.Text + "','" + date_xuat.Text + "','" + cb_nhanvien.EditValue.ToString() + "',N'" + txtnguoixuat.Text + "')";

                    if (ConnectDB.Query(sql) == -1)
                    {
                        XtraMessageBox.Show("Xuất không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        XtraMessageBox.Show("Xuất kho thành công (^-^)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        hien();
                    }
                }
            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            txtmahd.Text = "";
            txtlosx.Text = "";
            txtdgxuat.Text = "";
            txtmahd.ReadOnly = false;
            txtnguoixuat.Text = "";
            txtslxuat.Text = "";
            txtvitri.Text = "";
            cb_nhanvien.Text = "";
            cb_tenvt.Text = "";
            date_xuat.Text = "";
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            hien();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                string sql3 = "SELECT sohd , tenvt,losx,vitri,slxuat,dgxuat,ngayxuat,username,nguoixuat,(slxuat*dgxuat) as thanhtien FROM XuatKho INNER JOIN VatTu ON VatTu.mavt = XuatKho.mavt INNER JOIN NhanVien ON NhanVien.manv = XuatKho.manv";
                SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    string exportFilePath = saveFileDialogExcel.FileName;
                    gridControl1.DataSource = ConnectDB.getTable(sql3);
                    gridControl1.ExportToXlsx(exportFilePath);
                }
            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Importx im = new Importx();
            im.Show();
            this.Hide();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn sửa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (tb == DialogResult.Yes)
                {
                    if (validate())
                    {
                        string sql = "update XuatKho set mavt = N'" + cb_tenvt.EditValue.ToString() + "',losx = N'" + txtlosx.Text + "',vitri = '" + txtvitri.Text + "',slxuat = '" + txtslxuat.Text + "',dgxuat = '" + txtdgxuat.Text + "',ngayxuat = '" + date_xuat.Text + "',nguoixuat = N'" + txtnguoixuat.Text + "' where sohd='" + txtmahd.Text + "'";

                        if (ConnectDB.Query(sql) == -1)
                        {
                            XtraMessageBox.Show("Cập nhật không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            XtraMessageBox.Show("Cập nhật thành công (^-^)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (tb == DialogResult.Yes)
                {
                    if (validate())
                    {
                        string sql = "delete from XuatKho where sohd = '" + txtmahd.Text + "'";

                        if (ConnectDB.Query(sql) == -1)
                        {
                            XtraMessageBox.Show("Xóa hóa đơn không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            XtraMessageBox.Show("Xóa hóa đơn thành công (^-^)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}