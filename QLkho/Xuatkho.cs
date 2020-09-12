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
            if (txtmahd.Text == "" || txttenvt.Text == "" || txtdvtinh.Text == "" || txtslnhap.Text == "" || txtdvgiaonhan.Text == "" || txtmvt.Text == "" || txtbarcode.Text == "")
            {
                XtraMessageBox.Show("Vui lòng nhập đầy đủ thông tin !", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void hien()
        {
            try
            {
                string sql = "SELECT sohd , XuatKho.mavt, tenvt, barcodexuat,slxuat,dvt,ngayxuat,username,dvgiaonhan,ghichu FROM XuatKho INNER JOIN VatTu ON VatTu.mavt = XuatKho.mavt INNER JOIN NhanVien ON NhanVien.manv = XuatKho.manv";
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
            cb_user.Properties.DataSource = ConnectDB.getTable(sql1);
            cb_user.Properties.DisplayMember = "username";
            cb_user.Properties.ValueMember = "manv";
            hien();
            DateTime dt = DateTime.Now;
            date_nhap.DateTime = dt;
        }
        string userxuat = "";

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (validate())
                {
                    string sql = "insert into XuatKho values('" + txtmahd.Text.Trim() + "','" + txtmvt.Text + "','" + txtbarcode.Text + "','" + txtslnhap.Text + "','" + txtdvtinh.Text + "','" + Convert.ToDateTime(date_nhap.Text).ToString("dd-MM-yyyy HH:mm:ss") + "',N'" + cb_user.EditValue.ToString() + "',N'" + txtdvgiaonhan.Text + "',N'" + txtghichu.Text + "')";

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
            txttenvt.Text = "";
            txtdvtinh.Text = "";
            txtmahd.ReadOnly = false;
            txtghichu.Text = "";
            txtslnhap.Text = "";
            txtdvgiaonhan.Text = "";
            cb_user.Text = userxuat;
            txtbarcode.Text = "";
            date_nhap.Text = "";
            txtmvt.Text = "";
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            hien();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                string sql3 = "SELECT sohd , XuatKho.mavt, tenvt, barcodexuat,slxuat,dvt,ngayxuat,username,dvgiaonhan,ghichu FROM XuatKho INNER JOIN VatTu ON VatTu.mavt = XuatKho.mavt INNER JOIN NhanVien ON NhanVien.manv = XuatKho.manv";
                SaveFileDialog saveFileDialogExcel = new SaveFileDialog();
                saveFileDialogExcel.Filter = "Excel files (*.xlsx)|*.xlsx";
                if (saveFileDialogExcel.ShowDialog() == DialogResult.OK)
                {
                    string exportFilePath = saveFileDialogExcel.FileName;
                    gridControl1.DataSource = ConnectDB.getTable(sql3);
                    gridControl1.ExportToXlsx(exportFilePath);
                    XtraMessageBox.Show("Xuất file Excel thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                XtraMessageBox.Show("Không thể Xuất file Excel", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Import im = new Import();
            im.Show();
            this.Hide();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (validate())
                {
                    string sql = "update XuatKho set mavt = '" + txtmvt.Text + "',barcodexuat ='" + txtbarcode.Text + "',slxuat ='" + txtslnhap.Text + "',dvt ='" + txtdvtinh.Text + "',ngayxuat ='" + Convert.ToDateTime(date_nhap.Text).ToString("dd-MM-yyyy HH:mm:ss") + "', manv = '" + cb_user.EditValue.ToString() + "',dvgiaonhan = N'" + txtdvgiaonhan.Text + "',ghichu = N'" + txtghichu.Text + "' where sohd ='" + txtmahd.Text + "'";

                    if (ConnectDB.Query(sql) == -1)
                    {
                        XtraMessageBox.Show("Update không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        XtraMessageBox.Show("Update thành công (^-^)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        hien();
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

        private void simpleButton81_Click(object sender, EventArgs e)
        {
            string sql2 = @"select tenvt,mavt from VatTu where barcode = '" + txtbarcode.Text.Trim() + "' ";
            DataTable tb = ConnectDB.getTable(sql2);
            if (tb.Rows.Count > 0)
            {
                txttenvt.Text = tb.Rows[0]["tenvt"].ToString().Trim();
                txtmvt.Text = tb.Rows[0]["mavt"].ToString().Trim();
                DialogResult dr = XtraMessageBox.Show("Barcode có trong CSDL! Bạn có thể nhập các trường còn lại! ", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {

                }
            }
            else
            {
                XtraMessageBox.Show("Không tìm thấy thông tin hàng có barcode này trong CSDL");
            }
        }

        private void gridControl1_Click_1(object sender, EventArgs e)
        {
            txtmahd.ReadOnly = true;
            txtmahd.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "sohd").ToString();
            txttenvt.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "tenvt").ToString();
            txtmvt.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "mavt").ToString();
            txtdvgiaonhan.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "dvgiaonhan").ToString();
            txtslnhap.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "slxuat").ToString();
            txtdvtinh.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "dvt").ToString();
            cb_user.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "username").ToString();
            txtbarcode.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "barcodexuat").ToString();
            date_nhap.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ngayxuat").ToString();
            txtghichu.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ghichu").ToString();
        }
    }
}