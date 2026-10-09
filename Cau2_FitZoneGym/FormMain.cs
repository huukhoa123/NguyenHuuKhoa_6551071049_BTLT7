using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cau2_FitZoneGym.Models;
using Microsoft.EntityFrameworkCore;

namespace Cau2_FitZoneGym
{
    public partial class FormMain : Form
    {
        private FitZoneContext _context;

        // UI Controls
        private Label lblMaHV;
        private TextBox txtMaHV;
        private Label lblHoTen;
        private TextBox txtHoTen;
        private Label lblSDT;
        private TextBox txtSDT;
        private Label lblEmail;
        private TextBox txtEmail;

        private GroupBox gbGioiTinh;
        private RadioButton rdoNam;
        private RadioButton rdoNu;

        private Label lblNgaySinh;
        private DateTimePicker dtpNgaySinh;

        private Label lblHangThanhVien;
        private ComboBox cboHangThanhVien;

        private CheckBox chkTrangThai;

        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;

        private Label lblLocHang;
        private ComboBox cboLocHangThanhVien;
        private TextBox txtTimHoTen;
        private Button btnTimKiem;

        private DataGridView dgvHoiVien;

        public FormMain()
        {
            InitializeComponentLayout();
            _context = new FitZoneContext();
            _context.Database.EnsureCreated();
        }

        private async void FormMain_Load(object? sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void InitializeComponentLayout()
        {
            this.Text = "Quản Lý Hội Viên - Phòng Tập Gym FitZone";
            this.Width = 900;
            this.Height = 620;
            this.StartPosition = FormStartPosition.CenterScreen;

            lblMaHV = new Label { Text = "Mã HV", Left = 20, Top = 20, Width = 80 };
            txtMaHV = new TextBox { Left = 110, Top = 18, Width = 200, ReadOnly = true, Text = "ReadOnly" };

            lblHoTen = new Label { Text = "Họ tên", Left = 20, Top = 55, Width = 80 };
            txtHoTen = new TextBox { Left = 110, Top = 53, Width = 200 };

            lblSDT = new Label { Text = "Số điện thoại", Left = 20, Top = 90, Width = 80 };
            txtSDT = new TextBox { Left = 110, Top = 88, Width = 200 };

            lblEmail = new Label { Text = "Email", Left = 20, Top = 125, Width = 80 };
            txtEmail = new TextBox { Left = 110, Top = 123, Width = 200 };

            gbGioiTinh = new GroupBox { Text = "Giới tính", Left = 330, Top = 18, Width = 220, Height = 65 };
            rdoNam = new RadioButton { Text = "Nam", Left = 20, Top = 25, Width = 70, Checked = true };
            rdoNu = new RadioButton { Text = "Nữ", Left = 110, Top = 25, Width = 70 };
            gbGioiTinh.Controls.Add(rdoNam);
            gbGioiTinh.Controls.Add(rdoNu);

            lblNgaySinh = new Label { Text = "Ngày sinh", Left = 20, Top = 160, Width = 80 };
            dtpNgaySinh = new DateTimePicker { Left = 110, Top = 158, Width = 200, Format = DateTimePickerFormat.Short };

            lblHangThanhVien = new Label { Text = "Hạng thành viên", Left = 20, Top = 195, Width = 90 };
            cboHangThanhVien = new ComboBox { Left = 110, Top = 193, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cboHangThanhVien.Items.AddRange(new object[] { "Basic", "VIP", "Premium" });
            cboHangThanhVien.SelectedIndex = 0;

            chkTrangThai = new CheckBox { Text = "Đang hoạt động", Left = 110, Top = 230, Width = 150, Checked = true };

            // CRUD Buttons
            btnThem = new Button { Text = "Thêm", Left = 600, Top = 20, Width = 90, Height = 32 };
            btnSua = new Button { Text = "Sửa", Left = 600, Top = 60, Width = 90, Height = 32 };
            btnXoa = new Button { Text = "Xóa", Left = 600, Top = 100, Width = 90, Height = 32 };
            btnLamMoi = new Button { Text = "Làm mới", Left = 600, Top = 140, Width = 90, Height = 32 };

            // Search Bar
            lblLocHang = new Label { Text = "Hạng thành viên", Left = 330, Top = 232, Width = 100 };
            cboLocHangThanhVien = new ComboBox { Left = 435, Top = 229, Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            cboLocHangThanhVien.Items.AddRange(new object[] { "Tất cả", "Basic", "VIP", "Premium" });
            cboLocHangThanhVien.SelectedIndex = 0;

            txtTimHoTen = new TextBox { Left = 575, Top = 229, Width = 150, PlaceholderText = "Nhập họ tên cần tìm..." };
            btnTimKiem = new Button { Text = "Tìm kiếm", Left = 735, Top = 227, Width = 110, Height = 30 };

            // DataGridView
            dgvHoiVien = new DataGridView
            {
                Left = 20,
                Top = 275,
                Width = 840,
                Height = 280,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(lblMaHV);
            this.Controls.Add(txtMaHV);
            this.Controls.Add(lblHoTen);
            this.Controls.Add(txtHoTen);
            this.Controls.Add(lblSDT);
            this.Controls.Add(txtSDT);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);

            this.Controls.Add(gbGioiTinh);
            this.Controls.Add(lblNgaySinh);
            this.Controls.Add(dtpNgaySinh);
            this.Controls.Add(lblHangThanhVien);
            this.Controls.Add(cboHangThanhVien);
            this.Controls.Add(chkTrangThai);

            this.Controls.Add(btnThem);
            this.Controls.Add(btnSua);
            this.Controls.Add(btnXoa);
            this.Controls.Add(btnLamMoi);

            this.Controls.Add(lblLocHang);
            this.Controls.Add(cboLocHangThanhVien);
            this.Controls.Add(txtTimHoTen);
            this.Controls.Add(btnTimKiem);

            this.Controls.Add(dgvHoiVien);

            // Events
            this.Load += FormMain_Load;
            dgvHoiVien.SelectionChanged += DgvHoiVien_SelectionChanged;

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
            btnTimKiem.Click += BtnTimKiem_Click;
            cboLocHangThanhVien.SelectedIndexChanged += CboLocHangThanhVien_SelectedIndexChanged;
        }

        private async Task LoadDataAsync()
        {
            string hoTenSearch = txtTimHoTen.Text.Trim();
            string hangSearch = cboLocHangThanhVien.SelectedItem?.ToString() ?? "Tất cả";

            var query = _context.HoiViens.AsQueryable();

            if (!string.IsNullOrWhiteSpace(hoTenSearch))
            {
                query = query.Where(x => x.HoTen.Contains(hoTenSearch));
            }

            if (hangSearch != "Tất cả")
            {
                query = query.Where(x => x.HangThanhVien == hangSearch);
            }

            var rawList = await query.ToListAsync();

            var list = rawList.Select(x => new
            {
                MaHV = x.MaHV,
                HoTen = x.HoTen,
                GioiTinh = x.GioiTinh ? "Nam" : "Nữ",
                NgaySinh = x.NgaySinh.ToString("dd/MM/yyyy"),
                SDT = x.SDT,
                Email = x.Email,
                HangThanhVien = x.HangThanhVien,
                TrangThai = x.TrangThai ? "Đang hoạt động" : "Tạm ngưng"
            }).ToList();

            dgvHoiVien.DataSource = list;

            if (dgvHoiVien.Columns["MaHV"] != null) dgvHoiVien.Columns["MaHV"].HeaderText = "Mã HV";
            if (dgvHoiVien.Columns["HoTen"] != null) dgvHoiVien.Columns["HoTen"].HeaderText = "Họ tên";
            if (dgvHoiVien.Columns["GioiTinh"] != null) dgvHoiVien.Columns["GioiTinh"].HeaderText = "Giới tính";
            if (dgvHoiVien.Columns["NgaySinh"] != null) dgvHoiVien.Columns["NgaySinh"].HeaderText = "Ngày sinh";
            if (dgvHoiVien.Columns["SDT"] != null) dgvHoiVien.Columns["SDT"].HeaderText = "SĐT";
            if (dgvHoiVien.Columns["Email"] != null) dgvHoiVien.Columns["Email"].HeaderText = "Email";
            if (dgvHoiVien.Columns["HangThanhVien"] != null) dgvHoiVien.Columns["HangThanhVien"].HeaderText = "Hạng thành viên";
            if (dgvHoiVien.Columns["TrangThai"] != null) dgvHoiVien.Columns["TrangThai"].HeaderText = "Trạng thái";
        }

        private void DgvHoiVien_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvHoiVien.SelectedRows.Count > 0)
            {
                var row = dgvHoiVien.SelectedRows[0];
                txtMaHV.Text = row.Cells["MaHV"].Value?.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();
                txtSDT.Text = row.Cells["SDT"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();

                string gt = row.Cells["GioiTinh"].Value?.ToString() ?? "Nam";
                if (gt == "Nam") rdoNam.Checked = true;
                else rdoNu.Checked = true;

                if (DateTime.TryParse(row.Cells["NgaySinh"].Value?.ToString(), out DateTime ns))
                {
                    dtpNgaySinh.Value = ns;
                }

                string hang = row.Cells["HangThanhVien"].Value?.ToString() ?? "Basic";
                int index = cboHangThanhVien.Items.IndexOf(hang);
                if (index >= 0) cboHangThanhVien.SelectedIndex = index;

                string tt = row.Cells["TrangThai"].Value?.ToString() ?? "";
                chkTrangThai.Checked = (tt == "Đang hoạt động");
            }
        }

        private bool ValidateData()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên không được để trống!", "Lỗi kiểm tra dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string sdt = txtSDT.Text.Trim();
            if (!Regex.IsMatch(sdt, @"^\d{9,11}$"))
            {
                MessageBox.Show("Số điện thoại chỉ được chứa chữ số và có độ dài từ 9 đến 11 ký tự!", "Lỗi SĐT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string email = txtEmail.Text.Trim();
            if (!email.Contains('@'))
            {
                MessageBox.Show("Email không hợp lệ (phải chứa ký tự '@')!", "Lỗi Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int age = DateTime.Now.Year - dtpNgaySinh.Value.Year;
            if (dtpNgaySinh.Value.Date > DateTime.Now.AddYears(-age)) age--;
            if (age < 15)
            {
                MessageBox.Show("Tuổi hội viên phải từ 15 tuổi trở lên!", "Lỗi Tuổi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private async void BtnThem_Click(object? sender, EventArgs e)
        {
            if (!ValidateData()) return;

            var hv = new HoiVien
            {
                HoTen = txtHoTen.Text.Trim(),
                GioiTinh = rdoNam.Checked,
                NgaySinh = dtpNgaySinh.Value,
                SDT = txtSDT.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                HangThanhVien = cboHangThanhVien.SelectedItem?.ToString() ?? "Basic",
                NgayDangKy = DateTime.Now,
                TrangThai = chkTrangThai.Checked
            };

            _context.HoiViens.Add(hv);
            await _context.SaveChangesAsync();

            MessageBox.Show("Thêm mới hội viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadDataAsync();
            ClearInputs();
        }

        private async void BtnSua_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaHV.Text, out int maHV))
            {
                MessageBox.Show("Vui lòng chọn hội viên cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateData()) return;

            var hv = await _context.HoiViens.FindAsync(maHV);
            if (hv != null)
            {
                hv.HoTen = txtHoTen.Text.Trim();
                hv.GioiTinh = rdoNam.Checked;
                hv.NgaySinh = dtpNgaySinh.Value;
                hv.SDT = txtSDT.Text.Trim();
                hv.Email = txtEmail.Text.Trim();
                hv.HangThanhVien = cboHangThanhVien.SelectedItem?.ToString() ?? "Basic";
                hv.TrangThai = chkTrangThai.Checked;

                await _context.SaveChangesAsync();
                MessageBox.Show("Cập nhật thông tin hội viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync();
            }
        }

        private async void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaHV.Text, out int maHV))
            {
                MessageBox.Show("Vui lòng chọn hội viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa hội viên {txtHoTen.Text} (Mã: {maHV}) không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var hv = await _context.HoiViens.FindAsync(maHV);
                if (hv != null)
                {
                    _context.HoiViens.Remove(hv);
                    await _context.SaveChangesAsync();
                    MessageBox.Show("Xóa hội viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                    ClearInputs();
                }
            }
        }

        private async void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            ClearInputs();
            txtTimHoTen.Clear();
            cboLocHangThanhVien.SelectedIndex = 0;
            await LoadDataAsync();
        }

        private async void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async void CboLocHangThanhVien_SelectedIndexChanged(object? sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void ClearInputs()
        {
            txtMaHV.Text = "ReadOnly";
            txtHoTen.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            rdoNam.Checked = true;
            dtpNgaySinh.Value = DateTime.Now.AddYears(-20);
            cboHangThanhVien.SelectedIndex = 0;
            chkTrangThai.Checked = true;
        }
    }
}
