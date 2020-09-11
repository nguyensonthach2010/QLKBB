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
                XtraMessageBox.Show("Có lỗi xảy ra!. Lưu ý thoát file excel trước khi import và số hóa đơn không được trùng trong CSDL!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string conn = "Data Source=192.168.1.53,1433;Initial Catalog=QLKhoIT;User ID=sa;Password=123456789";
                DapperPlusManager.Entity<Xuat>().Table("XuatKho");
                List<Xuat> xuats = xuatkhoBindingSource.DataSource as List<Xuat>;
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
                XtraMessageBox.Show("Lưu ý thoát file excel trước khi import và số hóa đơn không được trùng trong CSDL", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        Xuat xuat1 = new Xuat();
                        xuat1.sohd = dt.Rows[i]["Số hóa đơn"].ToString();
                        xuat1.mavt = dt.Rows[i]["Mã vật tư"].ToString();
                        xuat1.losx = dt.Rows[i]["Lô sản xuất"].ToString();
                        xuat1.vitri = dt.Rows[i]["Vị trí"].ToString();
                        xuat1.slxuat = dt.Rows[i]["Số lượng xuất"].ToString();
                        xuat1.dgxuat = dt.Rows[i]["Đơn giá xuất (VNĐ)"].ToString();
                        xuat1.ngayxuat = dt.Rows[i]["Ngày xuất"].ToString();
                        xuat1.manv = dt.Rows[i]["Mã nhân viên"].ToString();
                        xuat1.nguoixuat = dt.Rows[i]["Người nhận"].ToString();
                        xuat.Add(xuat1);
                    }
                    xuatkhoBindingSource.DataSource = xuat;
                }
            }
            catch
            {
                XtraMessageBox.Show("Vui lòng kiểm tra lại định dạng hoặc các trường của file excel", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}