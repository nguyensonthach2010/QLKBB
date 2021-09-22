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
    public partial class Import : DevExpress.XtraEditors.XtraForm
    {
        public Import()
        {
            InitializeComponent();
            userthem = Dangnhap.tk;
        }
        string userthem = "";
        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = tableCollection[comboBoxEdit1.SelectedItem.ToString()];
                if (dt != null)
                {
                    List<Nhap> nhap = new List<Nhap>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Nhap nhap1 = new Nhap();
                        nhap1.sohd = dt.Rows[i]["Số hóa đơn"].ToString();
                        nhap1.mavt = dt.Rows[i]["Mã hàng"].ToString();
                        nhap1.barcodenhap = dt.Rows[i]["Barcode"].ToString();
                        nhap1.slnhap = dt.Rows[i]["Số lượng nhập"].ToString();
                        nhap1.dvt = dt.Rows[i]["Đơn vị tính"].ToString();
                        nhap1.ngaynhap = Convert.ToDateTime(dt.Rows[i]["Ngày nhập"].ToString()).ToString("MM/dd/yyyy HH:mm:ss");                        
                        nhap1.manv = dt.Rows[i]["Người nhập"].ToString();
                        nhap1.dvgiaonhan = dt.Rows[i]["Đơn vị giao nhận"].ToString();
                        nhap1.ghichu = dt.Rows[i]["Ghi chú"].ToString();
                        nhap.Add(nhap1);
                    }
                    nhapBindingSource1.DataSource = nhap;
                }
            }catch
            {
                XtraMessageBox.Show("Vui lòng kiểm tra lại định dạng hoặc các trường của file excel","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            
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
            }catch
            {
                XtraMessageBox.Show("Có lỗi xảy ra! ", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string conn = "Data Source=192.168.1.53,1433;Initial Catalog=QLKhoBB;User ID=sa;Password=123456789";
                DapperPlusManager.Entity<Nhap>().Table("NhapKho");
                List<Nhap> nhaps = nhapBindingSource1.DataSource as List<Nhap>;
                if (nhaps != null)
                {
                    using (IDbConnection db = new SqlConnection(conn))
                    {
                        db.BulkInsert(nhaps);
                    }
                }
                DialogResult tb = XtraMessageBox.Show("Import thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (tb == DialogResult.OK)
                {
                    this.Close();
                }
                string sql2 = "insert into LichSu values('" + userthem + "',N'Import file Excel vào nhập kho','" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "')";
                ConnectDB.Query(sql2);
            }
            catch 
            {
                XtraMessageBox.Show("Có lỗi xảy ra!. Không thể Import vào CSDL!. Lưu ý thoát file excel trước khi import và số hóa đơn không được trùng trong CSDL!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}