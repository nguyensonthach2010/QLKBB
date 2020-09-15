﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.Drawing;

namespace QLkho
{
    public partial class frm_RPN : DevExpress.XtraEditors.XtraForm
    {
        public frm_RPN()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string sql = "select sohd, mavt ,barcodenhap, slnhap, dvt, ngaynhap, manv, dvgiaonhan, ghichu from NhapKho where NhapKho.ngaynhap >='" + Convert.ToDateTime(txtdatebd.Text).ToString("MM/dd/yyyy HH:mm:ss")+"' and NhapKho.ngaynhap<='"+ Convert.ToDateTime(txtdatekt.Text).ToString("MM/dd/yyyy HH:mm:ss") + "'";
            gridControl1.DataSource = ConnectDB.getTable(sql);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=192.168.1.53,1433;Initial Catalog=QLKhoBB;User ID=sa;Password=123456789");
            con.Open();
            SqlCommand cmd = new SqlCommand("select sohd, mavt ,barcodenhap, slnhap, dvt, ngaynhap, manv, dvgiaonhan, ghichu from NhapKho where NhapKho.ngaynhap >='" + Convert.ToDateTime(txtdatebd.Text).ToString("MM/dd/yyyy HH:mm:ss") + "' and NhapKho.ngaynhap<='" + Convert.ToDateTime(txtdatekt.Text).ToString("MM/dd/yyyy HH:mm:ss") + "'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            RPN rp = new RPN();
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