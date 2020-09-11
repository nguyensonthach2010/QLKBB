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
    public partial class Theokics : DevExpress.XtraEditors.XtraForm
    {
        public Theokics()
        {
            InitializeComponent();
        }
        private bool validate()
        {   //hàm kiểm tra dữ liệu nhập vào có rỗng hay k
            if (txtmavt.Text == "")
            {
                MessageBox.Show("Vui lòng nhập vào mã vật tư cần tìm kiếm !", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void hien()
        {
            try
            {
                string sql = "SELECT Tonct.mavt as MaVT, Tonct.tenvt as TenVT, sum(Tonct.Tondk) AS TonDau, sum(Tonct.Nhaptk) AS Nhap, sum(Tonct.Xuattk) AS Xuat, (sum(Tonct.Tondk)+sum(Tonct.Nhaptk)- sum(Tonct.Xuattk)) AS TonCuoi FROM(Select dk.mavt, dk.tenvt, Tondk, 0 as Nhaptk, 0 as Xuattk  From (Select a.mavt, a.tenvt, (Sum(a.Nhap) - Sum(a.Xuat)) AS Tondk  From (Select N.mavt, H.tenvt, Sum(N.slnhap) as Nhap, 0 as Xuat  From NhapKho N, VatTu H Where N.mavt = H.mavt and N.ngaynhap < '" + date_bd.Text + "' and N.mavt = '" + txtmavt.Text + "' Group By N.mavt, H.tenvt UNION (Select X.mavt, H.tenvt, 0 as Nhap, sum(X.slxuat) as Xuat From XuatKho X, VatTu H Where X.mavt = H.mavt and X.ngayxuat < '" + date_bd.Text + "' and X.mavt = '" + txtmavt.Text + "' Group By X.mavt, H.tenvt)) a GROUP BY a.mavt, a.tenvt HAVING(Sum(a.Nhap - a.Xuat)) <> 0) dk Union Select mavt, tenvt, 0 as Tondk, 0 as Nhaptk, 0 as Xuattk From VatTu Union Select N.mavt, H.tenvt, 0 as Tondk, Sum(N.slnhap) as Nhaptk, 0 as Xuattk  From NhapKho N, VatTu H Where N.mavt = H.mavt and N.ngaynhap >= '" + date_bd.Text + "' and N.ngaynhap <= '" + date_kt.Text + "' and N.mavt = '" + txtmavt.Text + "' Group By N.mavt, H.tenvt Union Select X.mavt, H.tenvt, 0 as Tondk, 0 as Nhaptk, sum(X.slxuat) as Xuattk  From XuatKho X, VatTu H Where X.mavt = H.mavt and X.ngayxuat >= '" + date_bd.Text + "' and X.ngayxuat <= '" + date_kt.Text + "' and X.mavt = '" + txtmavt.Text + "' Group By X.mavt, H.tenvt )  AS Tonct GROUP BY Tonct.mavt, Tonct.tenvt HAVING(sum(Tonct.Tondk) + sum(Tonct.Nhaptk) - sum(Tonct.Xuattk)) <> 0; ";
                gridControl1.DataSource = ConnectDB.getTable(sql);

            }
            catch
            {
                XtraMessageBox.Show("Không thể kết nối tới CSDL", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            hien();
        }

    }
}