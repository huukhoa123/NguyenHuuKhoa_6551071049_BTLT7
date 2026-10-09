using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cau4_AnKhangClinic.Models;
using Microsoft.EntityFrameworkCore;

namespace Cau4_AnKhangClinic
{
    public partial class FormBacSi : Form
    {
        private AnKhangClinicContext _context;

        private Label lblMaBS;
        private TextBox txtMaBS;
        private Label lblHoTen;
        private TextBox txtHoTen;
        private Label lblChuyenKhoa;
        private TextBox txtChuyenKhoa;
        private Label lblSDT;
        private TextBox txtSDT;

        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;

        private DataGridView dgvBacSi;

        public FormBacSi()
        {
            InitializeComponentLayout();
            _context = new AnKhangClinicContext();
        }

        private async void FormBacSi_Load(object? sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void InitializeComponentLayout()
        {
            this.Text = "Quản Lý Bác Sĩ";
            this.Width = 650;
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterParent;

            lblMaBS = new Label { Text = "Mã bác sĩ", Left = 20, Top = 20, Width = 80 };
            txtMaBS = new TextBox { Left = 110, Top = 18, Width = 150, ReadOnly = true, Text = "ReadOnly" };

            lblHoTen = new Label { Text = "Họ tên BS", Left = 20, Top = 55, Width = 80 };
            txtHoTen = new TextBox { Left = 110, Top = 53, Width = 150 };

            lblChuyenKhoa = new Label { Text = "Chuyên khoa", Left = 20, Top = 90, Width = 80 };
            txtChuyenKhoa = new TextBox { Left = 110, Top = 88, Width = 150 };

            lblSDT = new Label { Text = "Số điện thoại", Left = 20, Top = 125, Width = 80 };
            txtSDT = new TextBox { Left = 110, Top = 123, Width = 150 };

            btnThem = new Button { Text = "Thêm", Left = 300, Top = 18, Width = 80, Height = 30 };
            btnSua = new Button { Text = "Sửa", Left = 390, Top = 18, Width = 80, Height = 30 };
            btnXoa = new Button { Text = "Xóa", Left = 480, Top = 18, Width = 80, Height = 30 };
            btnLamMoi = new Button { Text = "Làm mới", Left = 300, Top = 55, Width = 80, Height = 30 };

            dgvBacSi = new DataGridView
            {
                Left = 20,
                Top = 175,
                Width = 590,
                Height = 220,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(lblMaBS);
            this.Controls.Add(txtMaBS);
            this.Controls.Add(lblHoTen);
            this.Controls.Add(txtHoTen);
            this.Controls.Add(lblChuyenKhoa);
            this.Controls.Add(txtChuyenKhoa);
            this.Controls.Add(lblSDT);
            this.Controls.Add(txtSDT);

            this.Controls.Add(btnThem);
            this.Controls.Add(btnSua);
            this.Controls.Add(btnXoa);
            this.Controls.Add(btnLamMoi);

            this.Controls.Add(dgvBacSi);

            this.Load += FormBacSi_Load;
            dgvBacSi.SelectionChanged += DgvBacSi_SelectionChanged;

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
        }

        private async Task LoadDataAsync()
        {
            var list = await _context.BacSis.Select(x => new
            {
                MaBS = x.MaBS,
                HoTen = x.HoTen,
                ChuyenKhoa = x.ChuyenKhoa,
                SDT = x.SDT
            }).ToListAsync();

            dgvBacSi.DataSource = list;

            if (dgvBacSi.Columns["MaBS"] != null) dgvBacSi.Columns["MaBS"].HeaderText = "Mã BS";
            if (dgvBacSi.Columns["HoTen"] != null) dgvBacSi.Columns["HoTen"].HeaderText = "Họ tên";
            if (dgvBacSi.Columns["ChuyenKhoa"] != null) dgvBacSi.Columns["ChuyenKhoa"].HeaderText = "Chuyên khoa";
            if (dgvBacSi.Columns["SDT"] != null) dgvBacSi.Columns["SDT"].HeaderText = "SĐT";
        }

        private void DgvBacSi_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvBacSi.SelectedRows.Count > 0)
            {
                var row = dgvBacSi.SelectedRows[0];
                txtMaBS.Text = row.Cells["MaBS"].Value?.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();
                txtChuyenKhoa.Text = row.Cells["ChuyenKhoa"].Value?.ToString();
                txtSDT.Text = row.Cells["SDT"].Value?.ToString();
            }
        }

        private async void BtnThem_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập Họ tên Bác sĩ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var bs = new BacSi
            {
                HoTen = txtHoTen.Text.Trim(),
                ChuyenKhoa = txtChuyenKhoa.Text.Trim(),
                SDT = txtSDT.Text.Trim()
            };

            _context.BacSis.Add(bs);
            await _context.SaveChangesAsync();
            MessageBox.Show("Thêm Bác sĩ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadDataAsync();
            ClearInputs();
        }

        private async void BtnSua_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaBS.Text, out int maBS)) return;

            var bs = await _context.BacSis.FindAsync(maBS);
            if (bs != null)
            {
                if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Họ tên Bác sĩ không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bs.HoTen = txtHoTen.Text.Trim();
                bs.ChuyenKhoa = txtChuyenKhoa.Text.Trim();
                bs.SDT = txtSDT.Text.Trim();

                await _context.SaveChangesAsync();
                MessageBox.Show("Cập nhật thông tin Bác sĩ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync();
            }
        }

        private async void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaBS.Text, out int maBS)) return;

            var confirm = MessageBox.Show("Bạn có chắc muốn xóa Bác sĩ này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    var bs = await _context.BacSis.FindAsync(maBS);
                    if (bs != null)
                    {
                        _context.BacSis.Remove(bs);
                        await _context.SaveChangesAsync();
                        MessageBox.Show("Xóa Bác sĩ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadDataAsync();
                        ClearInputs();
                    }
                }
                catch (DbUpdateException)
                {
                    MessageBox.Show("Không thể xóa Bác sĩ đang có lịch khám đặt trước!", "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            ClearInputs();
            await LoadDataAsync();
        }

        private void ClearInputs()
        {
            txtMaBS.Text = "ReadOnly";
            txtHoTen.Clear();
            txtChuyenKhoa.Clear();
            txtSDT.Clear();
        }
    }
}
