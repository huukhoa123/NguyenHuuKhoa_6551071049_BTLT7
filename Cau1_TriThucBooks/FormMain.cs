using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cau1_TriThucBooks.Models;
using Microsoft.EntityFrameworkCore;

namespace Cau1_TriThucBooks
{
    public partial class FormMain : Form
    {
        private TriThucBooksContext _context;

        // UI Controls
        private Label lblTitle;
        private Label lblMaTL;
        private TextBox txtMaTL;
        private Label lblTenTheLoai;
        private TextBox txtTenTheLoai;
        private Label lblMoTa;
        private TextBox txtMoTa;
        private Label lblNgayTaoHeader;
        private Label lblNgayTaoValue;

        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;

        private TextBox txtTimKiem;
        private Button btnTimKiem;

        private DataGridView dgvTheLoai;

        public FormMain()
        {
            InitializeComponentLayout();
            _context = new TriThucBooksContext();
            _context.Database.EnsureCreated();
        }

        private async void FormMain_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void InitializeComponentLayout()
        {
            this.Text = "Quản Lý Thể Loại Sách - Tri Thức Books";
            this.Width = 850;
            this.Height = 550;
            this.StartPosition = FormStartPosition.CenterScreen;

            lblMaTL = new Label { Text = "Mã thể loại", Left = 20, Top = 20, Width = 90 };
            txtMaTL = new TextBox { Left = 120, Top = 18, Width = 200, ReadOnly = true, Text = "ReadOnly" };

            lblTenTheLoai = new Label { Text = "Tên thể loại", Left = 20, Top = 55, Width = 90 };
            txtTenTheLoai = new TextBox { Left = 120, Top = 53, Width = 200 };

            lblMoTa = new Label { Text = "Mô tả", Left = 20, Top = 90, Width = 90 };
            txtMoTa = new TextBox { Left = 120, Top = 88, Width = 200, Height = 70, Multiline = true, ScrollBars = ScrollBars.Vertical };

            lblNgayTaoHeader = new Label { Text = "Ngày tạo:", Left = 340, Top = 90, Width = 70 };
            lblNgayTaoValue = new Label { Text = "--", Left = 410, Top = 90, Width = 150, AutoSize = true };

            btnThem = new Button { Text = "Thêm", Left = 480, Top = 18, Width = 75, Height = 30 };
            btnSua = new Button { Text = "Sửa", Left = 565, Top = 18, Width = 75, Height = 30 };
            btnXoa = new Button { Text = "Xóa", Left = 650, Top = 18, Width = 75, Height = 30 };
            btnLamMoi = new Button { Text = "Làm mới", Left = 735, Top = 18, Width = 75, Height = 30 };

            txtTimKiem = new TextBox { Left = 20, Top = 175, Width = 280 };
            btnTimKiem = new Button { Text = "Tìm kiếm", Left = 310, Top = 173, Width = 85, Height = 28 };

            dgvTheLoai = new DataGridView
            {
                Left = 20,
                Top = 215,
                Width = 790,
                Height = 270,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(lblMaTL);
            this.Controls.Add(txtMaTL);
            this.Controls.Add(lblTenTheLoai);
            this.Controls.Add(txtTenTheLoai);
            this.Controls.Add(lblMoTa);
            this.Controls.Add(txtMoTa);
            this.Controls.Add(lblNgayTaoHeader);
            this.Controls.Add(lblNgayTaoValue);

            this.Controls.Add(btnThem);
            this.Controls.Add(btnSua);
            this.Controls.Add(btnXoa);
            this.Controls.Add(btnLamMoi);

            this.Controls.Add(txtTimKiem);
            this.Controls.Add(btnTimKiem);

            this.Controls.Add(dgvTheLoai);

            // Events
            this.Load += FormMain_Load;
            dgvTheLoai.SelectionChanged += DgvTheLoai_SelectionChanged;

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
            btnTimKiem.Click += BtnTimKiem_Click;
            txtTimKiem.TextChanged += TxtTimKiem_TextChanged;
        }

        private async Task LoadDataAsync(string keyword = "")
        {
            var query = _context.TheLoaiSachs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.TenTheLoai.Contains(keyword));
            }

            var list = await query.Select(x => new
            {
                MaTL = x.MaTL,
                TenTheLoai = x.TenTheLoai,
                MoTa = x.MoTa,
                SoLuongSach = x.SoLuongSach,
                NgayTao = x.NgayTao.ToString("dd/MM/yyyy HH:mm:ss")
            }).ToListAsync();

            dgvTheLoai.DataSource = list;

            if (dgvTheLoai.Columns["MaTL"] != null) dgvTheLoai.Columns["MaTL"].HeaderText = "Mã TL";
            if (dgvTheLoai.Columns["TenTheLoai"] != null) dgvTheLoai.Columns["TenTheLoai"].HeaderText = "Tên thể loại";
            if (dgvTheLoai.Columns["MoTa"] != null) dgvTheLoai.Columns["MoTa"].HeaderText = "Mô tả";
            if (dgvTheLoai.Columns["SoLuongSach"] != null) dgvTheLoai.Columns["SoLuongSach"].HeaderText = "Số lượng sách";
            if (dgvTheLoai.Columns["NgayTao"] != null) dgvTheLoai.Columns["NgayTao"].HeaderText = "Ngày tạo";
        }

        private void DgvTheLoai_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTheLoai.SelectedRows.Count > 0)
            {
                var row = dgvTheLoai.SelectedRows[0];
                txtMaTL.Text = row.Cells["MaTL"].Value?.ToString();
                txtTenTheLoai.Text = row.Cells["TenTheLoai"].Value?.ToString();
                txtMoTa.Text = row.Cells["MoTa"].Value?.ToString();
                lblNgayTaoValue.Text = row.Cells["NgayTao"].Value?.ToString();
            }
        }

        private async void BtnThem_Click(object sender, EventArgs e)
        {
            string tenTL = txtTenTheLoai.Text.Trim();
            if (string.IsNullOrEmpty(tenTL))
            {
                MessageBox.Show("Vui lòng nhập Tên thể loại!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool exists = await _context.TheLoaiSachs.AnyAsync(x => x.TenTheLoai.ToLower() == tenTL.ToLower());
            if (exists)
            {
                MessageBox.Show("Tên thể loại này đã tồn tại trong CSDL!", "Thông báo trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var item = new TheLoaiSach
            {
                TenTheLoai = tenTL,
                MoTa = txtMoTa.Text.Trim(),
                SoLuongSach = 0,
                NgayTao = DateTime.Now
            };

            _context.TheLoaiSachs.Add(item);
            await _context.SaveChangesAsync();

            MessageBox.Show("Thêm thể loại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadDataAsync();
            ClearInputs();
        }

        private async void BtnSua_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtMaTL.Text, out int maTL))
            {
                MessageBox.Show("Vui lòng chọn một thể loại để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenTL = txtTenTheLoai.Text.Trim();
            if (string.IsNullOrEmpty(tenTL))
            {
                MessageBox.Show("Tên thể loại không được bỏ trống!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var item = await _context.TheLoaiSachs.FindAsync(maTL);
            if (item != null)
            {
                bool exists = await _context.TheLoaiSachs.AnyAsync(x => x.MaTL != maTL && x.TenTheLoai.ToLower() == tenTL.ToLower());
                if (exists)
                {
                    MessageBox.Show("Tên thể loại mới bị trùng với một thể loại khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                item.TenTheLoai = tenTL;
                item.MoTa = txtMoTa.Text.Trim();

                await _context.SaveChangesAsync();
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync();
            }
        }

        private async void BtnXoa_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtMaTL.Text, out int maTL))
            {
                MessageBox.Show("Vui lòng chọn thể loại cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa thể loại có mã {maTL} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    var item = await _context.TheLoaiSachs.FindAsync(maTL);
                    if (item != null)
                    {
                        _context.TheLoaiSachs.Remove(item);
                        await _context.SaveChangesAsync();
                        MessageBox.Show("Xóa thể loại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadDataAsync();
                        ClearInputs();
                    }
                }
                catch (DbUpdateException)
                {
                    MessageBox.Show("Không thể xóa thể loại này vì đang được sách khác tham chiếu!", "Lỗi ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void BtnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
            txtTimKiem.Clear();
            await LoadDataAsync();
        }

        private async void BtnTimKiem_Click(object sender, EventArgs e)
        {
            await LoadDataAsync(txtTimKiem.Text.Trim());
        }

        private async void TxtTimKiem_TextChanged(object sender, EventArgs e)
        {
            await LoadDataAsync(txtTimKiem.Text.Trim());
        }

        private void ClearInputs()
        {
            txtMaTL.Text = "ReadOnly";
            txtTenTheLoai.Clear();
            txtMoTa.Clear();
            lblNgayTaoValue.Text = "--";
        }
    }
}
