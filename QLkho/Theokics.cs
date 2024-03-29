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
                string sql = "SELECT Tonct.mavt as MaVT, Tonct.tenvt as TenVT, Tonct.Barcode, sum(Tonct.Tondk) AS TonDau, sum(Tonct.Nhaptk) AS Nhap, sum(Tonct.Xuattk) AS Xuat, (sum(Tonct.Tondk)+sum(Tonct.Nhaptk)- sum(Tonct.Xuattk)) AS TonCuoi FROM(Select dk.mavt, dk.tenvt, dk.Barcode, Tondk, 0 as Nhaptk, 0 as Xuattk  From (Select a.mavt, a.tenvt, a.Barcode ,(Sum(a.Nhap) - Sum(a.Xuat)) AS Tondk  From (Select N.mavt, H.tenvt, N.barcodenhap as Barcode ,Sum(N.slnhap) as Nhap, 0 as Xuat  From NhapKho N, VatTu H Where N.mavt = H.mavt and N.ngaynhap < '" + Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy HH:mm:ss") + "' and N.barcodenhap = '" + txtmavt.Text + "' Group By N.mavt, H.tenvt, N.barcodenhap UNION (Select X.mavt, H.tenvt , X.barcodexuat as Barcode , 0 as Nhap, sum(X.slxuat) as Xuat From XuatKho X, VatTu H Where X.mavt = H.mavt and X.ngayxuat < '" + Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy HH:mm:ss")  + "' and X.barcodexuat = '" + txtmavt.Text + "' Group By X.mavt, H.tenvt, X.barcodexuat)) a GROUP BY a.mavt, a.tenvt , a.Barcode HAVING(Sum(a.Nhap - a.Xuat)) <> 0) dk Union Select mavt, tenvt, barcode as Barcode ,0 as Tondk, 0 as Nhaptk, 0 as Xuattk From VatTu Union Select N.mavt, H.tenvt, N.barcodenhap as Barcode, 0 as Tondk, Sum(N.slnhap) as Nhaptk, 0 as Xuattk  From NhapKho N, VatTu H Where N.mavt = H.mavt and N.ngaynhap >= '" + Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy HH:mm:ss") + "' and N.ngaynhap <= '" + Convert.ToDateTime(date_kt.Text).ToString("MM/dd/yyyy HH:mm:ss") + "' and N.barcodenhap = '" + txtmavt.Text + "' Group By N.mavt, H.tenvt, N.barcodenhap Union Select X.mavt, H.tenvt, X.barcodexuat as Barcode, 0 as Tondk, 0 as Nhaptk, sum(X.slxuat) as Xuattk  From XuatKho X, VatTu H Where X.mavt = H.mavt and X.ngayxuat >= '" + Convert.ToDateTime(date_bd.Text).ToString("MM/dd/yyyy HH:mm:ss") + "' and X.ngayxuat <= '" + Convert.ToDateTime(date_kt.Text).ToString("MM/dd/yyyy HH:mm:ss") + "' and X.barcodexuat = '" + txtmavt.Text + "' Group By X.mavt, H.tenvt, X.barcodexuat )  AS Tonct GROUP BY Tonct.mavt, Tonct.tenvt, Tonct.Barcode HAVING(sum(Tonct.Tondk) + sum(Tonct.Nhaptk) - sum(Tonct.Xuattk)) <> 0; ";
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