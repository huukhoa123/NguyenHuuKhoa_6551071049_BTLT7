using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cau4_AnKhangClinic.Models;
using Microsoft.EntityFrameworkCore;

namespace Cau4_AnKhangClinic
{
    public partial class FormMain : Form
    {
        private AnKhangClinicContext _context;

        // UI Controls
        private Label lblTenBenhNhan;
        private TextBox txtTenBenhNhan;

        private Label lblSDT;
        private TextBox txtSDT;

        private Label lblNgayKham;
        private DateTimePicker dtpNgayKham;

        private Label lblGioKham;
        private DateTimePicker dtpGioKham;

        private Label lblBacSi;
        private ComboBox cboBacSi;

        private Label lblTrangThai;
        private ComboBox cboTrangThai;

        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnQuanLyBacSi;

        private Label lblTuNgay;
        private DateTimePicker dtpTuNgay;
        private Label lblDenNgay;
        private DateTimePicker dtpDenNgay;
        private ComboBox cboLocBacSi;
        private Button btnTimKiem;

        private DataGridView dgvLichKham;

        public FormMain()
        {
            InitializeComponentLayout();
            _context = new AnKhangClinicContext();
            _context.Database.EnsureCreated();
        }

        private async void FormMain_Load(object? sender, EventArgs e)
        {
            await LoadBacSiComboBoxesAsync();
            await LoadDataAsync();
        }

        private void InitializeComponentLayout()
        {
            this.Text = "Quản Lý Lịch Khám Bệnh - An Khang Clinic";
            this.Width = 960;
            this.Height = 650;
            this.StartPosition = FormStartPosition.CenterScreen;

            lblTenBenhNhan = new Label { Text = "Tên bệnh nhân", Left = 20, Top = 20, Width = 90 };
            txtTenBenhNhan = new TextBox { Left = 120, Top = 18, Width = 230 };

            lblSDT = new Label { Text = "Số điện thoại", Left = 20, Top = 55, Width = 90 };
            txtSDT = new TextBox { Left = 120, Top = 53, Width = 230 };

            lblNgayKham = new Label { Text = "Ngày khám", Left = 20, Top = 90, Width = 90 };
            dtpNgayKham = new DateTimePicker { Left = 120, Top = 88, Width = 150, Format = DateTimePickerFormat.Short };

            lblGioKham = new Label { Text = "Giờ khám", Left = 280, Top = 90, Width = 65 };
            dtpGioKham = new DateTimePicker { Left = 350, Top = 88, Width = 100, Format = DateTimePickerFormat.Custom, CustomFormat = "HH:mm", ShowUpDown = true };

            lblBacSi = new Label { Text = "Bác sĩ", Left = 470, Top = 20, Width = 50 };
            cboBacSi = new ComboBox { Left = 530, Top = 18, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            lblTrangThai = new Label { Text = "Trạng thái", Left = 470, Top = 55, Width = 60 };
            cboTrangThai = new ComboBox { Left = 530, Top = 53, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cboTrangThai.Items.AddRange(new object[] { "Chờ khám", "Đã khám", "Đã hủy" });
            cboTrangThai.SelectedIndex = 0;

            // Buttons
            btnThem = new Button { Text = "Thêm", Left = 750, Top = 18, Width = 80, Height = 30 };
            btnSua = new Button { Text = "Sửa", Left = 840, Top = 18, Width = 80, Height = 30 };
            btnXoa = new Button { Text = "Xóa", Left = 750, Top = 53, Width = 80, Height = 30 };
            btnLamMoi = new Button { Text = "Làm mới", Left = 840, Top = 53, Width = 80, Height = 30 };
            btnQuanLyBacSi = new Button { Text = "Quản lý Bác sĩ", Left = 750, Top = 88, Width = 170, Height = 30 };

            // Filters
            lblTuNgay = new Label { Text = "Từ ngày", Left = 20, Top = 152, Width = 55 };
            dtpTuNgay = new DateTimePicker { Left = 80, Top = 149, Width = 110, Format = DateTimePickerFormat.Short, Value = new DateTime(2022, 1, 1) };

            lblDenNgay = new Label { Text = "Đến ngày", Left = 200, Top = 152, Width = 60 };
            dtpDenNgay = new DateTimePicker { Left = 265, Top = 149, Width = 110, Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddDays(30) };

            cboLocBacSi = new ComboBox { Left = 385, Top = 149, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };

            btnTimKiem = new Button { Text = "Tìm kiếm", Left = 575, Top = 147, Width = 100, Height = 30 };

            // DataGridView
            dgvLichKham = new DataGridView
            {
                Left = 20,
                Top = 190,
                Width = 900,
                Height = 400,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(lblTenBenhNhan);
            this.Controls.Add(txtTenBenhNhan);
            this.Controls.Add(lblSDT);
            this.Controls.Add(txtSDT);
            this.Controls.Add(lblNgayKham);
            this.Controls.Add(dtpNgayKham);
            this.Controls.Add(lblGioKham);
            this.Controls.Add(dtpGioKham);
            this.Controls.Add(lblBacSi);
            this.Controls.Add(cboBacSi);
            this.Controls.Add(lblTrangThai);
            this.Controls.Add(cboTrangThai);

            this.Controls.Add(btnThem);
            this.Controls.Add(btnSua);
            this.Controls.Add(btnXoa);
            this.Controls.Add(btnLamMoi);
            this.Controls.Add(btnQuanLyBacSi);

            this.Controls.Add(lblTuNgay);
            this.Controls.Add(dtpTuNgay);
            this.Controls.Add(lblDenNgay);
            this.Controls.Add(dtpDenNgay);
            this.Controls.Add(cboLocBacSi);
            this.Controls.Add(btnTimKiem);

            this.Controls.Add(dgvLichKham);

            this.Load += FormMain_Load;
            dgvLichKham.SelectionChanged += DgvLichKham_SelectionChanged;

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
            btnQuanLyBacSi.Click += BtnQuanLyBacSi_Click;
            btnTimKiem.Click += BtnTimKiem_Click;
        }

        private async Task LoadBacSiComboBoxesAsync()
        {
            var doctors = await _context.BacSis.ToListAsync();

            cboBacSi.DataSource = doctors;
            cboBacSi.DisplayMember = "DisplayText";
            cboBacSi.ValueMember = "MaBS";

            var filterList = doctors.Select(x => new { x.MaBS, DisplayText = x.DisplayText }).ToList();
            filterList.Insert(0, new { MaBS = 0, DisplayText = "Tất cả bác sĩ" });

            cboLocBacSi.DataSource = filterList;
            cboLocBacSi.DisplayMember = "DisplayText";
            cboLocBacSi.ValueMember = "MaBS";
            cboLocBacSi.SelectedIndex = 0;
        }

        private async Task LoadDataAsync()
        {
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddTicks(-1);
            int selectedMaBS = (cboLocBacSi.SelectedValue is int val) ? val : 0;

            var query = _context.LichKhams.Include(l => l.MaBSNavigation).AsQueryable();

            query = query.Where(l => l.NgayKham >= tuNgay && l.NgayKham <= denNgay);

            if (selectedMaBS > 0)
            {
                query = query.Where(l => l.MaBS == selectedMaBS);
            }

            var rawList = await query.ToListAsync();

            var list = rawList.Select(x => new
            {
                MaLich = x.MaLich,
                TenBenhNhan = x.TenBenhNhan,
                SDT = x.SDT,
                NgayKham = x.NgayKham.ToString("dd/MM/yyyy"),
                GioKham = x.GioKham,
                BacSi = x.MaBSNavigation != null ? $"BS. {x.MaBSNavigation.HoTen}" : "",
                ChuyenKhoa = x.MaBSNavigation?.ChuyenKhoa ?? "",
                TrangThai = x.TrangThai
            }).ToList();

            dgvLichKham.DataSource = list;

            if (dgvLichKham.Columns["MaLich"] != null) dgvLichKham.Columns["MaLich"].HeaderText = "Mã lịch";
            if (dgvLichKham.Columns["TenBenhNhan"] != null) dgvLichKham.Columns["TenBenhNhan"].HeaderText = "Tên bệnh nhân";
            if (dgvLichKham.Columns["SDT"] != null) dgvLichKham.Columns["SDT"].HeaderText = "SĐT";
            if (dgvLichKham.Columns["NgayKham"] != null) dgvLichKham.Columns["NgayKham"].HeaderText = "Ngày khám";
            if (dgvLichKham.Columns["GioKham"] != null) dgvLichKham.Columns["GioKham"].HeaderText = "Giờ khám";
            if (dgvLichKham.Columns["BacSi"] != null) dgvLichKham.Columns["BacSi"].HeaderText = "Bác sĩ";
            if (dgvLichKham.Columns["ChuyenKhoa"] != null) dgvLichKham.Columns["ChuyenKhoa"].HeaderText = "Chuyên khoa";
            if (dgvLichKham.Columns["TrangThai"] != null) dgvLichKham.Columns["TrangThai"].HeaderText = "Trạng thái";
        }

        private void DgvLichKham_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvLichKham.SelectedRows.Count > 0)
            {
                var row = dgvLichKham.SelectedRows[0];
                txtTenBenhNhan.Text = row.Cells["TenBenhNhan"].Value?.ToString();
                txtSDT.Text = row.Cells["SDT"].Value?.ToString();

                if (DateTime.TryParse(row.Cells["NgayKham"].Value?.ToString(), out DateTime nk))
                {
                    dtpNgayKham.Value = nk;
                }

                if (TimeSpan.TryParse(row.Cells["GioKham"].Value?.ToString(), out TimeSpan gk))
                {
                    dtpGioKham.Value = DateTime.Today.Add(gk);
                }

                string bsName = row.Cells["BacSi"].Value?.ToString() ?? "";
                foreach (var item in cboBacSi.Items)
                {
                    if (item is BacSi bs && bs.DisplayText.Contains(bsName.Replace("BS. ", "")))
                    {
                        cboBacSi.SelectedItem = item;
                        break;
                    }
                }

                string tt = row.Cells["TrangThai"].Value?.ToString() ?? "Chờ khám";
                int ttIdx = cboTrangThai.Items.IndexOf(tt);
                if (ttIdx >= 0) cboTrangThai.SelectedIndex = ttIdx;
            }
        }

        private async void BtnThem_Click(object? sender, EventArgs e)
        {
            string tenBN = txtTenBenhNhan.Text.Trim();
            if (string.IsNullOrEmpty(tenBN))
            {
                MessageBox.Show("Vui lòng nhập Tên bệnh nhân!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboBacSi.SelectedValue == null || !(cboBacSi.SelectedValue is int maBS))
            {
                MessageBox.Show("Vui lòng chọn Bác sĩ phụ trách!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpNgayKham.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Không cho phép đặt lịch khám vào ngày trong quá khứ!", "Lỗi ngày khám", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var lk = new LichKham
            {
                TenBenhNhan = tenBN,
                SDT = txtSDT.Text.Trim(),
                NgayKham = dtpNgayKham.Value.Date,
                GioKham = dtpGioKham.Value.ToString("HH:mm"),
                MaBS = maBS,
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Chờ khám"
            };

            _context.LichKhams.Add(lk);
            await _context.SaveChangesAsync();

            MessageBox.Show("Đặt lịch khám mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadDataAsync();
            ClearInputs();
        }

        private async void BtnSua_Click(object? sender, EventArgs e)
        {
            if (dgvLichKham.SelectedRows.Count == 0) return;
            int maLich = Convert.ToInt32(dgvLichKham.SelectedRows[0].Cells["MaLich"].Value);

            string tenBN = txtTenBenhNhan.Text.Trim();
            if (string.IsNullOrEmpty(tenBN))
            {
                MessageBox.Show("Tên bệnh nhân không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboBacSi.SelectedValue == null || !(cboBacSi.SelectedValue is int maBS))
            {
                MessageBox.Show("Vui lòng chọn Bác sĩ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var lk = await _context.LichKhams.FindAsync(maLich);
            if (lk != null)
            {
                lk.TenBenhNhan = tenBN;
                lk.SDT = txtSDT.Text.Trim();
                lk.NgayKham = dtpNgayKham.Value.Date;
                lk.GioKham = dtpGioKham.Value.ToString("HH:mm");
                lk.MaBS = maBS;
                lk.TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Chờ khám";

                await _context.SaveChangesAsync();
                MessageBox.Show("Cập nhật lịch khám thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync();
            }
        }

        private async void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (dgvLichKham.SelectedRows.Count == 0) return;
            int maLich = Convert.ToInt32(dgvLichKham.SelectedRows[0].Cells["MaLich"].Value);

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa lịch khám mã {maLich} của bệnh nhân {txtTenBenhNhan.Text} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                var lk = await _context.LichKhams.FindAsync(maLich);
                if (lk != null)
                {
                    _context.LichKhams.Remove(lk);
                    await _context.SaveChangesAsync();
                    MessageBox.Show("Xóa lịch khám thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                    ClearInputs();
                }
            }
        }

        private async void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            ClearInputs();
            await LoadBacSiComboBoxesAsync();
            await LoadDataAsync();
        }

        private async void BtnQuanLyBacSi_Click(object? sender, EventArgs e)
        {
            using (var formBS = new FormBacSi())
            {
                formBS.ShowDialog();
            }
            await LoadBacSiComboBoxesAsync();
            await LoadDataAsync();
        }

        private async void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void ClearInputs()
        {
            txtTenBenhNhan.Clear();
            txtSDT.Clear();
            dtpNgayKham.Value = DateTime.Today;
            dtpGioKham.Value = DateTime.Today.AddHours(8);
            cboTrangThai.SelectedIndex = 0;
        }
    }
}
