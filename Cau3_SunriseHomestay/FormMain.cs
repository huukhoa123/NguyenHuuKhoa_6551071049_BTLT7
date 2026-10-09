using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cau3_SunriseHomestay.Models;
using Microsoft.EntityFrameworkCore;

namespace Cau3_SunriseHomestay
{
    public partial class FormMain : Form
    {
        private SunriseHomestayContext _context;
        private string? _selectedImagePath = null;
        private string? _selectedImageFileName = null;
        private string _imagesDir;

        // UI Controls
        private Label lblSoPhong;
        private TextBox txtSoPhong;

        private Label lblTangSo;
        private NumericUpDown nudTangSo;

        private Label lblLoaiPhong;
        private ComboBox cboLoaiPhong;

        private Label lblTinhTrang;
        private ComboBox cboTinhTrang;

        private PictureBox picHinhAnh;
        private Button btnChonAnh;

        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnQuanLyLoaiPhong;

        private Label lblLocLoai;
        private ComboBox cboLocLoaiPhong;
        private Label lblLocTinhTrang;
        private ComboBox cboLocTinhTrang;
        private Button btnTimKiem;

        private DataGridView dgvPhong;

        public FormMain()
        {
            _imagesDir = Path.Combine(Application.StartupPath, "Images");
            if (!Directory.Exists(_imagesDir))
            {
                Directory.CreateDirectory(_imagesDir);
            }

            InitializeComponentLayout();
            _context = new SunriseHomestayContext();
            _context.Database.EnsureCreated();
        }

        private async void FormMain_Load(object? sender, EventArgs e)
        {
            await LoadLoaiPhongComboBoxesAsync();
            await LoadDataAsync();
        }

        private void InitializeComponentLayout()
        {
            this.Text = "Quản Lý Phòng - Sunrise Homestay";
            this.Width = 950;
            this.Height = 650;
            this.StartPosition = FormStartPosition.CenterScreen;

            lblSoPhong = new Label { Text = "Số phòng", Left = 20, Top = 20, Width = 70 };
            txtSoPhong = new TextBox { Left = 95, Top = 18, Width = 110 };

            lblTangSo = new Label { Text = "Tầng số", Left = 215, Top = 20, Width = 60 };
            nudTangSo = new NumericUpDown { Left = 280, Top = 18, Width = 60, Minimum = 1, Maximum = 100, Value = 1 };

            lblLoaiPhong = new Label { Text = "Loại phòng", Left = 355, Top = 20, Width = 75 };
            cboLoaiPhong = new ComboBox { Left = 435, Top = 18, Width = 140, DropDownStyle = ComboBoxStyle.DropDownList };

            lblTinhTrang = new Label { Text = "Tình trạng", Left = 585, Top = 20, Width = 75 };
            cboTinhTrang = new ComboBox { Left = 665, Top = 18, Width = 110, DropDownStyle = ComboBoxStyle.DropDownList };
            cboTinhTrang.Items.AddRange(new object[] { "Trống", "Đang ở", "Đang dọn" });
            cboTinhTrang.SelectedIndex = 0;

            picHinhAnh = new PictureBox
            {
                Left = 435,
                Top = 50,
                Width = 110,
                Height = 80,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            btnChonAnh = new Button { Text = "Chọn ảnh...", Left = 435, Top = 135, Width = 110, Height = 28 };

            // CRUD Buttons
            btnThem = new Button { Text = "Thêm", Left = 665, Top = 60, Width = 80, Height = 32 };
            btnSua = new Button { Text = "Sửa", Left = 755, Top = 60, Width = 80, Height = 32 };
            btnXoa = new Button { Text = "Xóa", Left = 665, Top = 100, Width = 80, Height = 32 };
            btnLamMoi = new Button { Text = "Làm mới", Left = 755, Top = 100, Width = 80, Height = 32 };
            btnQuanLyLoaiPhong = new Button { Text = "Quản lý Loại Phòng", Left = 665, Top = 140, Width = 170, Height = 30 };

            // Filters
            lblLocLoai = new Label { Text = "Lọc theo loại phòng", Left = 20, Top = 182, Width = 130 };
            cboLocLoaiPhong = new ComboBox { Left = 150, Top = 179, Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };

            lblLocTinhTrang = new Label { Text = "Lọc theo tình trạng", Left = 295, Top = 182, Width = 120 };
            cboLocTinhTrang = new ComboBox { Left = 420, Top = 179, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cboLocTinhTrang.Items.AddRange(new object[] { "Tất cả", "Trống", "Đang ở", "Đang dọn" });
            cboLocTinhTrang.SelectedIndex = 0;

            btnTimKiem = new Button { Text = "Tìm kiếm", Left = 555, Top = 177, Width = 100, Height = 30 };

            // DataGridView
            dgvPhong = new DataGridView
            {
                Left = 20,
                Top = 220,
                Width = 890,
                Height = 370,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                RowTemplate = { Height = 50 }
            };

            this.Controls.Add(lblSoPhong);
            this.Controls.Add(txtSoPhong);
            this.Controls.Add(lblTangSo);
            this.Controls.Add(nudTangSo);
            this.Controls.Add(lblLoaiPhong);
            this.Controls.Add(cboLoaiPhong);
            this.Controls.Add(lblTinhTrang);
            this.Controls.Add(cboTinhTrang);

            this.Controls.Add(picHinhAnh);
            this.Controls.Add(btnChonAnh);

            this.Controls.Add(btnThem);
            this.Controls.Add(btnSua);
            this.Controls.Add(btnXoa);
            this.Controls.Add(btnLamMoi);
            this.Controls.Add(btnQuanLyLoaiPhong);

            this.Controls.Add(lblLocLoai);
            this.Controls.Add(cboLocLoaiPhong);
            this.Controls.Add(lblLocTinhTrang);
            this.Controls.Add(cboLocTinhTrang);
            this.Controls.Add(btnTimKiem);

            this.Controls.Add(dgvPhong);

            this.Load += FormMain_Load;
            dgvPhong.SelectionChanged += DgvPhong_SelectionChanged;

            btnChonAnh.Click += BtnChonAnh_Click;
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
            btnQuanLyLoaiPhong.Click += BtnQuanLyLoaiPhong_Click;
            btnTimKiem.Click += BtnTimKiem_Click;
        }

        private async Task LoadLoaiPhongComboBoxesAsync()
        {
            var list = await _context.LoaiPhongs.ToListAsync();

            cboLoaiPhong.DataSource = list;
            cboLoaiPhong.DisplayMember = "TenLoai";
            cboLoaiPhong.ValueMember = "MaLoai";

            var filterList = list.Select(x => new { x.MaLoai, x.TenLoai }).ToList();
            filterList.Insert(0, new { MaLoai = 0, TenLoai = "Tất cả" });

            cboLocLoaiPhong.DataSource = filterList;
            cboLocLoaiPhong.DisplayMember = "TenLoai";
            cboLocLoaiPhong.ValueMember = "MaLoai";
            cboLocLoaiPhong.SelectedIndex = 0;
        }

        private async Task LoadDataAsync()
        {
            int selectedMaLoai = (cboLocLoaiPhong.SelectedValue is int val) ? val : 0;
            string selectedTinhTrang = cboLocTinhTrang.SelectedItem?.ToString() ?? "Tất cả";

            var query = _context.Phongs.Include(p => p.MaLoaiNavigation).AsQueryable();

            if (selectedMaLoai > 0)
            {
                query = query.Where(p => p.MaLoai == selectedMaLoai);
            }

            if (selectedTinhTrang != "Tất cả")
            {
                query = query.Where(p => p.TinhTrang == selectedTinhTrang);
            }

            var list = await query.ToListAsync();

            DataTable dt = new DataTable();
            dt.Columns.Add("MaPhong", typeof(int));
            dt.Columns.Add("Anh", typeof(Image));
            dt.Columns.Add("SoPhong", typeof(string));
            dt.Columns.Add("TangSo", typeof(int));
            dt.Columns.Add("TenLoai", typeof(string));
            dt.Columns.Add("GiaMoiDem", typeof(string));
            dt.Columns.Add("TinhTrang", typeof(string));
            dt.Columns.Add("HinhAnhFileName", typeof(string));

            foreach (var item in list)
            {
                Image? img = null;
                if (!string.IsNullOrEmpty(item.HinhAnh))
                {
                    string imgPath = Path.Combine(_imagesDir, item.HinhAnh);
                    if (File.Exists(imgPath))
                    {
                        try
                        {
                            using (var stream = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                            {
                                img = Image.FromStream(stream);
                            }
                        }
                        catch { }
                    }
                }

                dt.Rows.Add(
                    item.MaPhong,
                    img ?? CreatePlaceholderImage(),
                    item.SoPhong,
                    item.TangSo,
                    item.MaLoaiNavigation?.TenLoai ?? "",
                    item.MaLoaiNavigation?.GiaMoiDem.ToString("N0") ?? "0",
                    item.TinhTrang,
                    item.HinhAnh
                );
            }

            dgvPhong.DataSource = dt;

            if (dgvPhong.Columns["HinhAnhFileName"] != null) dgvPhong.Columns["HinhAnhFileName"].Visible = false;

            if (dgvPhong.Columns["MaPhong"] != null) dgvPhong.Columns["MaPhong"].HeaderText = "Mã phòng";
            if (dgvPhong.Columns["Anh"] != null)
            {
                var imgCol = (DataGridViewImageColumn)dgvPhong.Columns["Anh"];
                imgCol.HeaderText = "Ảnh";
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                imgCol.Width = 70;
            }
            if (dgvPhong.Columns["SoPhong"] != null) dgvPhong.Columns["SoPhong"].HeaderText = "Số phòng";
            if (dgvPhong.Columns["TangSo"] != null) dgvPhong.Columns["TangSo"].HeaderText = "Tầng";
            if (dgvPhong.Columns["TenLoai"] != null) dgvPhong.Columns["TenLoai"].HeaderText = "Loại phòng";
            if (dgvPhong.Columns["GiaMoiDem"] != null) dgvPhong.Columns["GiaMoiDem"].HeaderText = "Giá/đêm";
            if (dgvPhong.Columns["TinhTrang"] != null) dgvPhong.Columns["TinhTrang"].HeaderText = "Tình trạng";
        }

        private Image CreatePlaceholderImage()
        {
            Bitmap bmp = new Bitmap(60, 40);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);
                g.DrawString("No Image", new Font("Arial", 8), Brushes.DarkGray, 5, 12);
            }
            return bmp;
        }

        private void DgvPhong_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvPhong.SelectedRows.Count > 0)
            {
                var row = dgvPhong.SelectedRows[0];
                txtSoPhong.Text = row.Cells["SoPhong"].Value?.ToString();

                if (int.TryParse(row.Cells["TangSo"].Value?.ToString(), out int tang))
                {
                    nudTangSo.Value = Math.Max(nudTangSo.Minimum, Math.Min(nudTangSo.Maximum, tang));
                }

                string tenLoai = row.Cells["TenLoai"].Value?.ToString() ?? "";
                cboLoaiPhong.Text = tenLoai;

                string tt = row.Cells["TinhTrang"].Value?.ToString() ?? "Trống";
                int ttIndex = cboTinhTrang.Items.IndexOf(tt);
                if (ttIndex >= 0) cboTinhTrang.SelectedIndex = ttIndex;

                string fileName = row.Cells["HinhAnhFileName"].Value?.ToString() ?? "";
                _selectedImageFileName = fileName;

                if (!string.IsNullOrEmpty(fileName))
                {
                    string fullPath = Path.Combine(_imagesDir, fileName);
                    if (File.Exists(fullPath))
                    {
                        using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                        {
                            picHinhAnh.Image = Image.FromStream(stream);
                        }
                    }
                    else
                    {
                        picHinhAnh.Image = null;
                    }
                }
                else
                {
                    picHinhAnh.Image = null;
                }
            }
        }

        private void BtnChonAnh_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedImagePath = ofd.FileName;
                    _selectedImageFileName = Path.GetFileName(ofd.FileName);

                    using (var stream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        picHinhAnh.Image = Image.FromStream(stream);
                    }
                }
            }
        }

        private string? SaveImageToDirectory()
        {
            if (!string.IsNullOrEmpty(_selectedImagePath) && File.Exists(_selectedImagePath))
            {
                string destFileName = $"{Guid.NewGuid()}_{_selectedImageFileName}";
                string destPath = Path.Combine(_imagesDir, destFileName);
                File.Copy(_selectedImagePath, destPath, true);
                return destFileName;
            }
            return _selectedImageFileName;
        }

        private async void BtnThem_Click(object? sender, EventArgs e)
        {
            string soPhong = txtSoPhong.Text.Trim();
            if (string.IsNullOrEmpty(soPhong))
            {
                MessageBox.Show("Vui lòng nhập Số phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboLoaiPhong.SelectedValue == null || !(cboLoaiPhong.SelectedValue is int maLoai))
            {
                MessageBox.Show("Vui lòng chọn Loại phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string savedImageName = SaveImageToDirectory() ?? "";

            var phong = new Phong
            {
                SoPhong = soPhong,
                TangSo = (int)nudTangSo.Value,
                MaLoai = maLoai,
                TinhTrang = cboTinhTrang.SelectedItem?.ToString() ?? "Trống",
                HinhAnh = savedImageName
            };

            _context.Phongs.Add(phong);
            await _context.SaveChangesAsync();

            MessageBox.Show("Thêm phòng mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadDataAsync();
            ClearInputs();
        }

        private async void BtnSua_Click(object? sender, EventArgs e)
        {
            if (dgvPhong.SelectedRows.Count == 0) return;
            int maPhong = Convert.ToInt32(dgvPhong.SelectedRows[0].Cells["MaPhong"].Value);

            var phong = await _context.Phongs.FindAsync(maPhong);
            if (phong != null)
            {
                string soPhong = txtSoPhong.Text.Trim();
                if (string.IsNullOrEmpty(soPhong))
                {
                    MessageBox.Show("Số phòng không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboLoaiPhong.SelectedValue is int maLoai)
                {
                    phong.MaLoai = maLoai;
                }

                phong.SoPhong = soPhong;
                phong.TangSo = (int)nudTangSo.Value;
                phong.TinhTrang = cboTinhTrang.SelectedItem?.ToString() ?? "Trống";

                if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    phong.HinhAnh = SaveImageToDirectory();
                }

                await _context.SaveChangesAsync();
                MessageBox.Show("Cập nhật phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync();
            }
        }

        private async void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (dgvPhong.SelectedRows.Count == 0) return;
            int maPhong = Convert.ToInt32(dgvPhong.SelectedRows[0].Cells["MaPhong"].Value);

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa phòng {txtSoPhong.Text} không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                var phong = await _context.Phongs.FindAsync(maPhong);
                if (phong != null)
                {
                    _context.Phongs.Remove(phong);
                    await _context.SaveChangesAsync();
                    MessageBox.Show("Xóa phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                    ClearInputs();
                }
            }
        }

        private async void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            ClearInputs();
            await LoadLoaiPhongComboBoxesAsync();
            await LoadDataAsync();
        }

        private async void BtnQuanLyLoaiPhong_Click(object? sender, EventArgs e)
        {
            using (var formLP = new FormLoaiPhong())
            {
                formLP.ShowDialog();
            }
            await LoadLoaiPhongComboBoxesAsync();
            await LoadDataAsync();
        }

        private async void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void ClearInputs()
        {
            txtSoPhong.Clear();
            nudTangSo.Value = 1;
            cboTinhTrang.SelectedIndex = 0;
            picHinhAnh.Image = null;
            _selectedImagePath = null;
            _selectedImageFileName = null;
        }
    }
}
