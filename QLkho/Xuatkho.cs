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
using System.Data.SqlClient;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraGrid;

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
            string sqln = @"select MaVT, TenVT, Barcode ,Sum(Nhap) as tongnhhap , SUM(Xuat) as tongxuat, (SUM(Nhap) - SUM(Xuat)) as Ton from (select mavt as MaVT, tenvt as TenVT, barcode as Barcode, 0 as Nhap, 0 as Xuat From VatTu union Select N.mavt as MaVT, H.tenvt as TenVT,  N.barcodenhap as Barcode, Sum(N.slnhap) as Nhap, 0 as Xuat  From NhapKho N, VatTu H Where N.mavt = H.mavt Group By N.mavt, H.tenvt, N.barcodenhap having SUM(N.slnhap) > 0 union Select X.mavt as MaVT, H.tenvt as TenVT, X.barcodexuat as Barcode, 0 as Nhap, Sum(X.slxuat) as Xuat   From XuatKho X, VatTu H Where X.mavt = H.mavt Group By X.mavt, H.tenvt, X.barcodexuat having SUM(X.slxuat) > 0) as hangton where Barcode = '"+txtbarcode.Text+"' Group by MaVT, TenVT, Barcode";
            DataTable dat = ConnectDB.getTable(sqln);
            if (dat.Rows.Count > 0 && Convert.ToInt64(dat.Rows[0]["Ton"].ToString()) >= Convert.ToInt64(txtslnhap.Text))
            {
                try
                {
                    if (validate())
                    {
                        DialogResult tb = XtraMessageBox.Show("Bạn có muốn in phiếu xuất mặt hàng này sau khi xuất kho không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (tb == DialogResult.Yes)
                        {
                            string sql2 = "insert into XuatKho values('" + txtmahd.Text.Trim() + "','" + txtmvt.Text + "','" + txtbarcode.Text + "','" + txtslnhap.Text + "','" + txtdvtinh.Text + "','" + Convert.ToDateTime(date_nhap.Text).ToString("MM/dd/yyyy HH:mm:ss") + "',N'" + cb_user.EditValue.ToString() + "',N'" + txtdvgiaonhan.Text + "',N'" + txtghichu.Text + "')";
                            if (ConnectDB.Query(sql2) == -1)
                            {
                                XtraMessageBox.Show("Xuất không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                XtraMessageBox.Show("Xuất kho thành công (^-^)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                string sqll = "insert into LichSu values('" + userxuat + "',N'Xuất kho mặt hàng có: [Mã HĐ]:(" + txtmahd + ") ||[Mã hàng]:(" + txtmvt.Text + ") || [Barcode]:(" + txtbarcode.Text + ") || [Số lượng xuất]:(" + txtslnhap.Text + ") || [Đơn vị tính]:(" + txtdvtinh.Text + ") || [Ngày xuất]:(" + Convert.ToDateTime(date_nhap.Text).ToString("dd/MM/yyyy HH:mm:ss") + ") || [Người xuất]:(" + cb_user.EditValue.ToString() + ") || [Đơn vị giao nhận]:(" + txtdvgiaonhan.Text + ")','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                                ConnectDB.Query(sqll);
                                hien();
                            }
                            SqlConnection con = new SqlConnection(@"Data Source=192.168.1.53,1433;Initial Catalog=QLKhoBB;User ID=sa;Password=123456789");
                            con.Open();
                            SqlCommand cmd = new SqlCommand("select * from XuatKho where sohd  = '" + txtmahd.Text + "'", con);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            con.Close();
                            XtraReport2 rp = new XtraReport2();
                            rp.DataSource = dt;
                            rp.ShowPreviewDialog();
                        }
                        else
                        {
                            try
                            {
                                if (validate())
                                {
                                    string sql2 = "insert into XuatKho values('" + txtmahd.Text.Trim() + "','" + txtmvt.Text + "','" + txtbarcode.Text + "','" + txtslnhap.Text + "','" + txtdvtinh.Text + "','" + Convert.ToDateTime(date_nhap.Text).ToString("MM/dd/yyyy HH:mm:ss") + "',N'" + cb_user.EditValue.ToString() + "',N'" + txtdvgiaonhan.Text + "',N'" + txtghichu.Text + "')";

                                    if (ConnectDB.Query(sql2) == -1)
                                    {
                                        XtraMessageBox.Show("Xuất không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else
                                    {
                                        XtraMessageBox.Show("Xuất kho thành công (^-^)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        string sqll = "insert into LichSu values('" + userxuat + "',N'Xuất kho mặt hàng có: [Mã HĐ]:(" + txtmahd + ") ||[Mã hàng]:(" + txtmvt.Text + ") || [Barcode]:(" + txtbarcode.Text + ") || [Số lượng xuất]:(" + txtslnhap.Text + ") || [Đơn vị tính]:(" + txtdvtinh.Text + ") || [Ngày xuất]:(" + Convert.ToDateTime(date_nhap.Text).ToString("dd/MM/yyyy HH:mm:ss") + ") || [Người xuất]:(" + cb_user.EditValue.ToString() + ") || [Đơn vị giao nhận]:(" + txtdvgiaonhan.Text + ")','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                                        ConnectDB.Query(sqll);
                                        hien();
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
                catch
                {
                    XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                XtraMessageBox.Show("Mặt hàng này hiện trong kho còn ít hơn so với số lượng xuất!");
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
            txtmvt.Enabled = false;
            txttenvt.Enabled = false;
            txtslnhap.Enabled = false;
            txtmahd.Enabled = false;
            txtghichu.Enabled = false;
            txtdvtinh.Enabled = false;
            txtdvgiaonhan.Enabled = false;
            cb_user.Enabled = false;
            date_nhap.Enabled = false;
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
                    string sql2 = "insert into LichSu values('" + userxuat + "',N'Xuất file Excel của các mặt hàng đã xuất kho','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                    ConnectDB.Query(sql2);
                }
            }
            catch
            {
                XtraMessageBox.Show("Không thể Xuất file Excel", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (validate())
                {
                    string sql4 = "update XuatKho set mavt = '" + txtmvt.Text + "',barcodexuat ='" + txtbarcode.Text + "',slxuat ='" + txtslnhap.Text + "',dvt ='" + txtdvtinh.Text + "',ngayxuat ='" + Convert.ToDateTime(date_nhap.Text).ToString("MM/dd/yyyy HH:mm:ss") + "', manv = '" + cb_user.EditValue.ToString() + "',dvgiaonhan = N'" + txtdvgiaonhan.Text + "',ghichu = N'" + txtghichu.Text + "' where sohd ='" + txtmahd.Text + "'";

                    if (ConnectDB.Query(sql4) == -1)
                    {
                        XtraMessageBox.Show("Update không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        XtraMessageBox.Show("Update thành công (^-^)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        hien();
                        string sqll = "insert into LichSu values('" + userxuat + "',N'Sửa lại mặt hàng có: [Mã HĐ]:(" + txtmahd + ") ||[Mã hàng]:(" + txtmvt.Text + ") || [Barcode]:(" + txtbarcode.Text + ") || [Số lượng xuất]:(" + txtslnhap.Text + ") || [Đơn vị tính]:(" + txtdvtinh.Text + ") || [Ngày xuất]:(" + Convert.ToDateTime(date_nhap.Text).ToString("dd/MM/yyyy HH:mm:ss") + ") || [Người xuất]:(" + cb_user.EditValue.ToString() + ") || [Đơn vị giao nhận]:(" + txtdvgiaonhan.Text + ")','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                        ConnectDB.Query(sqll);
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
                            string sqll = "insert into LichSu values('" + userxuat + "',N'Xoá mặt hàng có: [Mã HĐ]:(" + txtmahd + ") ||[Mã hàng]:(" + txtmvt.Text + ") || [Barcode]:(" + txtbarcode.Text + ") || [Số lượng xuất]:(" + txtslnhap.Text + ") || [Đơn vị tính]:(" + txtdvtinh.Text + ") || [Ngày xuất]:(" + Convert.ToDateTime(date_nhap.Text).ToString("dd/MM/yyyy HH:mm:ss") + ") || [Người xuất]:(" + cb_user.EditValue.ToString() + ") || [Đơn vị giao nhận]:(" + txtdvgiaonhan.Text + ")','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                            ConnectDB.Query(sqll);
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
            string sql5 = @"select tenvt,mavt from VatTu where barcode = '" + txtbarcode.Text.Trim() + "' ";
            DataTable tb = ConnectDB.getTable(sql5);
            if (tb.Rows.Count > 0)
            {
                txttenvt.Text = tb.Rows[0]["tenvt"].ToString().Trim();
                txtmvt.Text = tb.Rows[0]["mavt"].ToString().Trim();
                DialogResult dr = XtraMessageBox.Show("Chi tiết: [Mã vật tư] là : [" + tb.Rows[0]["mavt"].ToString().Trim() +"] và [Tên vật tư] : ["+ tb.Rows[0]["tenvt"].ToString().Trim() + "] ! ", "Barcode hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    txtmvt.Enabled = true;
                    txttenvt.Enabled = true;
                    txtslnhap.Enabled = true;
                    txtmahd.Enabled = true;
                    txtghichu.Enabled = true;
                    txtdvtinh.Enabled = true;
                    txtdvgiaonhan.Enabled = true;
                    cb_user.Enabled = true;
                    date_nhap.Enabled = true;
                }
            }
            else
            {
                XtraMessageBox.Show("Không tìm thấy mặt hàng có barcode này trong CSDL");
            }
        }

        private void gridControl1_Click_1(object sender, EventArgs e)
        {
            txtmvt.Enabled = true;
            txttenvt.Enabled = true;
            txtslnhap.Enabled = true;
            txtghichu.Enabled = true;
            txtdvtinh.Enabled = true;
            txtdvgiaonhan.Enabled = true;
            cb_user.Enabled = true;
            date_nhap.Enabled = true;
            txtmahd.ReadOnly = true;
            txtmahd.Enabled = true;
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

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=192.168.1.53,1433;Initial Catalog=QLKhoBB;User ID=sa;Password=123456789");
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from XuatKho where sohd  = '" + txtmahd.Text + "'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            XtraReport2 rp = new XtraReport2();
            rp.DataSource = dt;
            rp.ShowPreviewDialog();
        }
        bool indicatorIcon = true;

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    string sText = (e.RowHandle + 1).ToString();
                    Graphics gr = e.Info.Graphics;
                    gr.PageUnit = GraphicsUnit.Pixel;
                    GridView gridView = ((GridView)sender);
                    SizeF size = gr.MeasureString(sText, e.Info.Appearance.Font);
                    int nNewSize = Convert.ToInt32(size.Width) + GridPainter.Indicator.ImageSize.Width + 10;
                    if (gridView.IndicatorWidth < nNewSize)
                    {
                        gridView.IndicatorWidth = nNewSize;
                    }

                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    e.Info.DisplayText = sText;
                }
                if (!indicatorIcon)
                    e.Info.ImageIndex = -1;

                if (e.RowHandle == GridControl.InvalidRowHandle)
                {
                    Graphics gr = e.Info.Graphics;
                    gr.PageUnit = GraphicsUnit.Pixel;
                    GridView gridView = ((GridView)sender);
                    SizeF size = gr.MeasureString("STT", e.Info.Appearance.Font);
                    int nNewSize = Convert.ToInt32(size.Width) + GridPainter.Indicator.ImageSize.Width + 10;
                    if (gridView.IndicatorWidth < nNewSize)
                    {
                        gridView.IndicatorWidth = nNewSize;
                    }

                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    e.Info.DisplayText = "STT";
                }
            }
            catch
            {
                XtraMessageBox.Show("Lỗi cột STT");
            }
        }

        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            GridView gridview = ((GridView)sender);
            if (!gridview.GridControl.IsHandleCreated) return;
            Graphics gr = Graphics.FromHwnd(gridview.GridControl.Handle);
            SizeF size = gr.MeasureString(gridview.RowCount.ToString(), gridview.PaintAppearance.Row.GetFont());
            gridview.IndicatorWidth = Convert.ToInt32(size.Width + 0.999f) + GridPainter.Indicator.ImageSize.Width + 10;
        }
    }
}