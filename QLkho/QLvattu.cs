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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraGrid;

namespace QLkho
{
    public partial class QLvattu : DevExpress.XtraEditors.XtraForm
    {
        public QLvattu()
        {
            InitializeComponent();
            usvt = Dangnhap.tk;
        }

        private void QLvattu_Load(object sender, EventArgs e)
        {
            hien();
        }
        string usvt = "";
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (validate())
                {
                    string sql = "insert into VatTu values('" + txtmavt.Text + "',N'" + txttenvt.Text + "',N'" + txtbarcode.Text + "')";

                    if (ConnectDB.Query(sql) == -1)
                    {
                        XtraMessageBox.Show("Thêm không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        XtraMessageBox.Show("Thêm thành công (^-^)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        hien();
                        string sql2 = "insert into LichSu values('" + usvt + "',N'Thêm hàng mới: [Mã hàng]:(" +txtmavt.Text + ") || [Tên hàng]:("+txttenvt.Text+") || [Barcode]:("+txtbarcode.Text+")','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                        ConnectDB.Query(sql2);
                    }
                }
            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
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
                        string sql = "update VatTu set tenvt = N'" + txttenvt.Text + "', barcode ='" + txtbarcode.Text + "' where mavt = '" + txtmavt.Text + "'";

                        if (ConnectDB.Query(sql) == -1)
                        {
                            XtraMessageBox.Show("Cập nhật thông tin không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            XtraMessageBox.Show("Cập nhật thông tin thành công (^-^)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            hien();
                            string sql2 = "insert into LichSu values('" + usvt + "',N'Sửa thông tin hàng có [Mã hàng]:(" + txtmavt.Text + ") thành [Tên hàng]:(" + txttenvt.Text + ") || [Barcode]:(" + txtbarcode.Text + ")','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                            ConnectDB.Query(sql2);
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
                        string sql = "delete from VatTu where mavt = '" + txtmavt.Text + "'";

                        if (ConnectDB.Query(sql) == -1)
                        {
                            XtraMessageBox.Show("Xóa thông tin không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            XtraMessageBox.Show("Xóa thông tin thành công (^-^)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            hien();
                            string sql2 = "insert into LichSu values('" + usvt + "',N'Xoá hàng có [Mã hàng]:(" + txtmavt.Text + ") || [Tên hàng]:(" + txttenvt.Text + ") || [Barcode]:(" + txtbarcode.Text + ")','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                            ConnectDB.Query(sql2);
                        }
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
            txtmavt.Text = "";
            txttenvt.Text = "";
            txtbarcode.Text = "";
            txtmavt.ReadOnly = false;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            hien();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            txtmavt.ReadOnly = true;
            txtmavt.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaVT").ToString();
            txttenvt.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "TenVT").ToString();
            txtbarcode.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Barcode").ToString();
        }
        private bool validate()
        {   //hàm kiểm tra dữ liệu nhập vào có rỗng hay k
            if (txtmavt.Text == "" || txttenvt.Text == "" || txtbarcode.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đủ các trường !", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void Quanlyvattu_Load(object sender, EventArgs e)
        {
            hien();
        }
        private void hien()
        {
            try
            {
                try
                {
                    string sql = "select MaVT, TenVT, Barcode ,Sum(Nhap) as tongnhhap , SUM(Xuat) as tongxuat, (SUM(Nhap) - SUM(Xuat)) as Ton from (select mavt as MaVT, tenvt as TenVT, barcode as Barcode, 0 as Nhap, 0 as Xuat From VatTu union Select N.mavt as MaVT, H.tenvt as TenVT,  N.barcodenhap as Barcode, Sum(N.slnhap) as Nhap, 0 as Xuat  From NhapKho N, VatTu H Where N.mavt = H.mavt Group By N.mavt, H.tenvt, N.barcodenhap having SUM(N.slnhap) > 0 union Select X.mavt as MaVT, H.tenvt as TenVT, X.barcodexuat as Barcode, 0 as Nhap, Sum(X.slxuat) as Xuat   From XuatKho X, VatTu H Where X.mavt = H.mavt Group By X.mavt, H.tenvt, X.barcodexuat having SUM(X.slxuat) > 0) as hangton Group by MaVT, TenVT, Barcode";
                    gridControl1.DataSource = ConnectDB.getTable(sql);

                }
                catch
                {
                    XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
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