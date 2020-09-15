using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.Data.SqlClient;


namespace QLkho
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            skin();
            taikhoan = Dangnhap.tk;
            barHeaderItem1.Caption = "Bạn đang đăng nhập với user :" + " " + taikhoan;
        }
        public static string taikhoan = "";
        private void skin()
        {
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.XtraBars.Helpers.SkinHelper.InitSkinGallery(skinRibbonGalleryBarItem1, true);
            DevExpress.LookAndFeel.DefaultLookAndFeel themes = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            themes.LookAndFeel.SkinName = "Glass Oceans";
        }
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult tb = XtraMessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (tb == DialogResult.Yes)
            {
                this.Close();
                new Dangnhap().Visible = true;
            }
            
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Doimatkhau doimk = new Doimatkhau();
            doimk.Show();
        }
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            QLvattu qlvt = new QLvattu();
            qlvt.MdiParent = this;
            qlvt.Show();
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Quyen.nhomnd == "Nhập" || Quyen.nhomnd == "Admin")
            {
                frmNhap nk = new frmNhap();
                nk.MdiParent = this;
                nk.Show();
            }
            else
            {
                XtraMessageBox.Show("Chỉ nhóm tài khoản Nhập và Admin mới có thể Nhập kho!!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Quyen.nhomnd == "Xuất" || Quyen.nhomnd == "Admin")
            {
                Xuatkho nk = new Xuatkho();
                nk.MdiParent = this;
                nk.Show();
            }
            else
            {
                XtraMessageBox.Show("Chỉ nhóm tài khoản Xuất và Admin mới có thể Xuất kho!!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Tonkho ton = new Tonkho();
            ton.MdiParent = this;
            ton.Show();
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Theokics tt = new Theokics();
            tt.MdiParent = this;
            tt.Show();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Quyen.nhomnd == "Admin")
            {
                BackUp bk = new BackUp();
                bk.Show();
            }
            else
            {
                XtraMessageBox.Show("Chỉ admin mới có thể backup dữ liệu", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItem5_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Quyen.nhomnd == "Admin")
            {
                QLtaikhoan qltk = new QLtaikhoan();
                qltk.MdiParent = this;
                qltk.Show();
            }
            else
            {
                XtraMessageBox.Show("Chỉ admin mới có thể quản lý tài khoản", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Quyen.nhomnd == "Nhập" || Quyen.nhomnd == "Admin")
            {
                frm_RPN rp1 = new frm_RPN();
                rp1.Show();
            }
            else
            {
                XtraMessageBox.Show("Chỉ nhóm tài khoản nhập và Admin mới có thể quản lý hàng nhập", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Quyen.nhomnd == "Admin" || Quyen.nhomnd == "Nhập")
            {
                frm_RPHN rphn = new frm_RPHN();
                rphn.Show();
            }
            else
            {
                XtraMessageBox.Show("Chỉ nhóm tài khoản nhập và Admin mới có thể quản lý hàng nhập", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Quyen.nhomnd == "Xuất" || Quyen.nhomnd == "Admin")
            {
                frm_RPX rp1 = new frm_RPX();
                rp1.Show();
            }
            else
            {
                XtraMessageBox.Show("Chỉ nhóm tài khoản Xuất và Admin mới có thể quản lý hàng xuất!!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Quyen.nhomnd == "Xuất" || Quyen.nhomnd == "Admin")
            {
                frm_RPHX rphn = new frm_RPHX();
                rphn.Show();
            }
            else
            {
                XtraMessageBox.Show("Chỉ nhóm tài khoản Xuất và Admin mới có thể quản lý hàng xuất !!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Quyen.nhomnd == "Admin")
            {
                Restore rs = new Restore();
                rs.Show();
            }
            else
            {
                XtraMessageBox.Show("Chỉ admin mới có thể phục hồi dữ liệu!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }        
        }
    }
}
