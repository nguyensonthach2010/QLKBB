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
    public partial class QLtaikhoan : DevExpress.XtraEditors.XtraForm
    {
        public QLtaikhoan()
        {
            InitializeComponent();
            us = Dangnhap.tk;
        }
        private bool validate()
        {   //hàm kiểm tra dữ liệu nhập vào có rỗng hay k
            if (txtmanv.Text == "" || txttennv.Text == "" || txtusername.Text == "" || txtpassword.Text == "" || cb_quyen.Text =="" || cb_tt.Text =="")
            {
                XtraMessageBox.Show("Bạn phải điền đầy đủ các thông tin tài khoản !", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        string us = "";
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
                    string sql = "insert into NhanVien values('" + txtmanv.Text + "',N'" + txttennv.Text + "','" + txtusername.Text + "','" + txtpassword.Text + "',N'" + cb_quyen.Text + "',N'"+cb_tt.Text+"')";

                    if (ConnectDB.Query(sql) == -1)
                    {
                        XtraMessageBox.Show("Thêm không thành công (T_T) !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Thêm thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        hien();
                        string sql2 = "insert into LichSu values('" + us + "',N'Thêm tài khoản: [Mã NV]:(" + txtmanv.Text + ") || [Tên NV]:(" + txttennv.Text + ") || [username]:(" + txtusername.Text + ") || [password]:(" + txtpassword.Text + ") || [Quyền]:(" + cb_quyen.Text + ") || [Trạng thái]:(" + cb_tt.Text + ")','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                        ConnectDB.Query(sql2);
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
                        string sql = "update NhanVien set tennv = N'" + txttennv.Text + "', username ='" + txtusername.Text + "',nhomnd=N'" + cb_quyen.Text + "', trangthai = '"+cb_tt.Text+"' where manv = '" + txtmanv.Text + "'";

                        if (ConnectDB.Query(sql) == -1)
                        {
                            XtraMessageBox.Show("Cập nhật thông tin không thành công. Hãy load lại dữ liệu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            XtraMessageBox.Show("Cập nhật thông tin thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            hien();
                            string sql2 = "insert into LichSu values('" + us + "',N'Update tài khoản: [Mã NV]:(" + txtmanv.Text + ") thành [Tên NV]:(" + txttennv.Text + ") || [username]:(" + txtusername.Text + ") || [Quyền]:(" + cb_quyen.Text + ") || [Trạng thái]:(" + cb_tt.Text + ")','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
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
                            string sql2 = "insert into LichSu values('" + us + "',N'Xoá tài khoản: [Mã NV]:(" + txtmanv.Text + ") || [Tên NV]:(" + txttennv.Text + ") || [username]:(" + txtusername.Text + ") || [Quyền]:(" + cb_quyen.Text + ") || [Trạng thái]:(" + cb_tt.Text + ")','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
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

        private void simpleButton5_Click_1(object sender, EventArgs e)
        {
            txtmanv.Text = "";
            txttennv.Text = "";
            txtusername.Text = "";
            txtpassword.Text = "";
            cb_quyen.Text = "";
            cb_tt.Text = "";
            txtmanv.ReadOnly = false;
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            hien();
        }

        private void gridControl1_Click_1(object sender, EventArgs e)
        {
            txtmanv.ReadOnly = true;
            txtpassword.ReadOnly = true;
            txtmanv.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "manv").ToString();
            txttennv.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "tennv").ToString();
            txtusername.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "username").ToString();
            cb_quyen.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "nhomnd").ToString();
            cb_tt.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "trangthai").ToString();
            txtpassword.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "password").ToString();
        }
        bool indicatorIcon = true;
        private void gridView1_CustomDrawRowIndicator_1(object sender, RowIndicatorCustomDrawEventArgs e)
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

        private void gridView1_RowCountChanged_1(object sender, EventArgs e)
        {
            GridView gridview = ((GridView)sender);
            if (!gridview.GridControl.IsHandleCreated) return;
            Graphics gr = Graphics.FromHwnd(gridview.GridControl.Handle);
            SizeF size = gr.MeasureString(gridview.RowCount.ToString(), gridview.PaintAppearance.Row.GetFont());
            gridview.IndicatorWidth = Convert.ToInt32(size.Width + 0.999f) + GridPainter.Indicator.ImageSize.Width + 10;
        }
    }
}