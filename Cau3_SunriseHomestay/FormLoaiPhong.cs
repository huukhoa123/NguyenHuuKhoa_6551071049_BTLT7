using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cau3_SunriseHomestay.Models;
using Microsoft.EntityFrameworkCore;

namespace Cau3_SunriseHomestay
{
    public partial class FormLoaiPhong : Form
    {
        private SunriseHomestayContext _context;

        private Label lblMaLoai;
        private TextBox txtMaLoai;
        private Label lblTenLoai;
        private TextBox txtTenLoai;
        private Label lblGiaMoiDem;
        private TextBox txtGiaMoiDem;
        private Label lblMoTa;
        private TextBox txtMoTa;

        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;

        private DataGridView dgvLoaiPhong;

        public FormLoaiPhong()
        {
            InitializeComponentLayout();
            _context = new SunriseHomestayContext();
        }

        private async void FormLoaiPhong_Load(object? sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void InitializeComponentLayout()
        {
            this.Text = "Quản Lý Loại Phòng";
            this.Width = 650;
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterParent;

            lblMaLoai = new Label { Text = "Mã loại", Left = 20, Top = 20, Width = 80 };
            txtMaLoai = new TextBox { Left = 110, Top = 18, Width = 150, ReadOnly = true, Text = "ReadOnly" };

            lblTenLoai = new Label { Text = "Tên loại", Left = 20, Top = 55, Width = 80 };
            txtTenLoai = new TextBox { Left = 110, Top = 53, Width = 150 };

            lblGiaMoiDem = new Label { Text = "Giá / đêm", Left = 20, Top = 90, Width = 80 };
            txtGiaMoiDem = new TextBox { Left = 110, Top = 88, Width = 150 };

            lblMoTa = new Label { Text = "Mô tả", Left = 20, Top = 125, Width = 80 };
            txtMoTa = new TextBox { Left = 110, Top = 123, Width = 150, Height = 50, Multiline = true };

            btnThem = new Button { Text = "Thêm", Left = 300, Top = 18, Width = 80, Height = 30 };
            btnSua = new Button { Text = "Sửa", Left = 390, Top = 18, Width = 80, Height = 30 };
            btnXoa = new Button { Text = "Xóa", Left = 480, Top = 18, Width = 80, Height = 30 };
            btnLamMoi = new Button { Text = "Làm mới", Left = 300, Top = 55, Width = 80, Height = 30 };

            dgvLoaiPhong = new DataGridView
            {
                Left = 20,
                Top = 195,
                Width = 590,
                Height = 200,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(lblMaLoai);
            this.Controls.Add(txtMaLoai);
            this.Controls.Add(lblTenLoai);
            this.Controls.Add(txtTenLoai);
            this.Controls.Add(lblGiaMoiDem);
            this.Controls.Add(txtGiaMoiDem);
            this.Controls.Add(lblMoTa);
            this.Controls.Add(txtMoTa);

            this.Controls.Add(btnThem);
            this.Controls.Add(btnSua);
            this.Controls.Add(btnXoa);
            this.Controls.Add(btnLamMoi);

            this.Controls.Add(dgvLoaiPhong);

            this.Load += FormLoaiPhong_Load;
            dgvLoaiPhong.SelectionChanged += DgvLoaiPhong_SelectionChanged;

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
        }

        private async Task LoadDataAsync()
        {
            var list = await _context.LoaiPhongs.Select(x => new
            {
                MaLoai = x.MaLoai,
                TenLoai = x.TenLoai,
                GiaMoiDem = x.GiaMoiDem.ToString("N0"),
                MoTa = x.MoTa
            }).ToListAsync();

            dgvLoaiPhong.DataSource = list;

            if (dgvLoaiPhong.Columns["MaLoai"] != null) dgvLoaiPhong.Columns["MaLoai"].HeaderText = "Mã loại";
            if (dgvLoaiPhong.Columns["TenLoai"] != null) dgvLoaiPhong.Columns["TenLoai"].HeaderText = "Tên loại";
            if (dgvLoaiPhong.Columns["GiaMoiDem"] != null) dgvLoaiPhong.Columns["GiaMoiDem"].HeaderText = "Giá / đêm (VNĐ)";
            if (dgvLoaiPhong.Columns["MoTa"] != null) dgvLoaiPhong.Columns["MoTa"].HeaderText = "Mô tả";
        }

        private void DgvLoaiPhong_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvLoaiPhong.SelectedRows.Count > 0)
            {
                var row = dgvLoaiPhong.SelectedRows[0];
                txtMaLoai.Text = row.Cells["MaLoai"].Value?.ToString();
                txtTenLoai.Text = row.Cells["TenLoai"].Value?.ToString();
                txtGiaMoiDem.Text = row.Cells["GiaMoiDem"].Value?.ToString();
                txtMoTa.Text = row.Cells["MoTa"].Value?.ToString();
            }
        }

        private async void BtnThem_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenLoai.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên loại phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtGiaMoiDem.Text.Replace(",", "").Replace(".", ""), out decimal gia))
            {
                MessageBox.Show("Giá mỗi đêm không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var lp = new LoaiPhong
            {
                TenLoai = txtTenLoai.Text.Trim(),
                GiaMoiDem = gia,
                MoTa = txtMoTa.Text.Trim()
            };

            _context.LoaiPhongs.Add(lp);
            await _context.SaveChangesAsync();
            MessageBox.Show("Thêm loại phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadDataAsync();
            ClearInputs();
        }

        private async void BtnSua_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaLoai.Text, out int maLoai)) return;

            var lp = await _context.LoaiPhongs.FindAsync(maLoai);
            if (lp != null)
            {
                if (string.IsNullOrWhiteSpace(txtTenLoai.Text))
                {
                    MessageBox.Show("Tên loại phòng không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtGiaMoiDem.Text.Replace(",", "").Replace(".", ""), out decimal gia))
                {
                    MessageBox.Show("Giá mỗi đêm không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lp.TenLoai = txtTenLoai.Text.Trim();
                lp.GiaMoiDem = gia;
                lp.MoTa = txtMoTa.Text.Trim();

                await _context.SaveChangesAsync();
                MessageBox.Show("Cập nhật loại phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync();
            }
        }

        private async void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaLoai.Text, out int maLoai)) return;

            var confirm = MessageBox.Show("Bạn có chắc muốn xóa loại phòng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    var lp = await _context.LoaiPhongs.FindAsync(maLoai);
                    if (lp != null)
                    {
                        _context.LoaiPhongs.Remove(lp);
                        await _context.SaveChangesAsync();
                        MessageBox.Show("Xóa loại phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadDataAsync();
                        ClearInputs();
                    }
                }
                catch (DbUpdateException)
                {
                    MessageBox.Show("Không thể xóa loại phòng đang chứa các phòng thực tế!", "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtMaLoai.Text = "ReadOnly";
            txtTenLoai.Clear();
            txtGiaMoiDem.Clear();
            txtMoTa.Clear();
        }
    }
}
