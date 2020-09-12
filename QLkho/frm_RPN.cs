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
using DevExpress.XtraGrid;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;

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
    }
}