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
using ExcelDataReader;
using System.Data.SqlClient;
using System.IO;
using Z.Dapper.Plus;

namespace QLkho
{
    public partial class Importx : DevExpress.XtraEditors.XtraForm
    {
        public Importx()
        {
            InitializeComponent();
        }
        DataTableCollection tableCollection;

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel Workbook(*.xlsx;*.xls)|*.xlsx;*.xls" })
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        textEdit1.Text = openFileDialog.FileName;
                        using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                        {
                            using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                                });
                                tableCollection = result.Tables;
                                comboBoxEdit1.Properties.Items.Clear();
                                foreach (DataTable table in tableCollection)
                                    comboBoxEdit1.Properties.Items.Add(table.TableName);
                            }
                        }
                    }
                }
            }
            catch
            {
                XtraMessageBox.Show("Có lỗi xảy ra!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string conn = "Data Source=192.168.1.53,1433;Initial Catalog=QLKhoBB;User ID=sa;Password=123456789";
                DapperPlusManager.Entity<Xuat>().Table("XuatKho");
                List<Xuat> xuats = xuatBindingSource.DataSource as List<Xuat>;
                if (xuats != null)
                {
                    using (IDbConnection db = new SqlConnection(conn))
                    {
                        db.BulkInsert(xuats);
                    }
                }
                DialogResult tb = XtraMessageBox.Show("Import thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (tb == DialogResult.OK)
                {
                    this.Close();
                }
            }
            catch
            {
                XtraMessageBox.Show("Có lỗi xảy ra!. Không thể Import vào CSDL!. Lưu ý thoát file excel trước khi import và số hóa đơn không được trùng trong CSDL!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void comboBoxEdit1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = tableCollection[comboBoxEdit1.SelectedItem.ToString()];
                if (dt != null)
                {
                    List<Xuat> xuat = new List<Xuat>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string sqln = @"select MaVT, TenVT, Barcode ,Sum(Nhap) as tongnhhap , SUM(Xuat) as tongxuat, (SUM(Nhap) - SUM(Xuat)) as Ton from (select mavt as MaVT, tenvt as TenVT, barcode as Barcode, 0 as Nhap, 0 as Xuat From VatTu union Select N.mavt as MaVT, H.tenvt as TenVT,  N.barcodenhap as Barcode, Sum(N.slnhap) as Nhap, 0 as Xuat  From NhapKho N, VatTu H Where N.mavt = H.mavt Group By N.mavt, H.tenvt, N.barcodenhap having SUM(N.slnhap) > 0 union Select X.mavt as MaVT, H.tenvt as TenVT, X.barcodexuat as Barcode, 0 as Nhap, Sum(X.slxuat) as Xuat   From XuatKho X, VatTu H Where X.mavt = H.mavt Group By X.mavt, H.tenvt, X.barcodexuat having SUM(X.slxuat) > 0) as hangton where Barcode = '" + dt.Rows[i]["Barcode"].ToString() + "' Group by MaVT, TenVT, Barcode";
                        DataTable dat = ConnectDB.getTable(sqln);
                        if (dat.Rows.Count > 0 && Convert.ToInt64(dat.Rows[0]["Ton"].ToString()) >= Convert.ToInt64(dt.Rows[i]["Số lượng xuất"].ToString()))
                        {
                            Xuat xuat1 = new Xuat();
                            xuat1.sohd = dt.Rows[i]["Số hóa đơn"].ToString();
                            xuat1.mavt = dt.Rows[i]["Mã hàng"].ToString();
                            xuat1.barcodexuat = dt.Rows[i]["Barcode"].ToString();
                            xuat1.slxuat = dt.Rows[i]["Số lượng xuất"].ToString();
                            xuat1.dvt = dt.Rows[i]["Đơn vị tính"].ToString();
                            xuat1.ngayxuat = Convert.ToDateTime(dt.Rows[i]["Ngày xuất"].ToString()).ToString("MM/dd/yyyy HH:mm:ss");
                            xuat1.manv = dt.Rows[i]["Người xuất"].ToString();
                            xuat1.dvgiaonhan = dt.Rows[i]["Đơn vị giao nhận"].ToString();
                            xuat1.ghichu = dt.Rows[i]["Ghi chú"].ToString();
                            xuat.Add(xuat1);
                        }
                        else
                        {
                            XtraMessageBox.Show("Số lượng xuất không được vượt quá lượng tồn trong kho!");
                        }
                    }
                    xuatBindingSource.DataSource = xuat;
                }
            }
            catch
            {
                XtraMessageBox.Show("Vui lòng kiểm tra lại định dạng hoặc các trường của file excel", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}