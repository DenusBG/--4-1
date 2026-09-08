using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace FinanceApp
{
    public class TransactionItem
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public string Type { get; set; } = "Витрата";
        public string Category { get; set; } = "Інше";
        public double Amount { get; set; }
        public string Description { get; set; } = "";
    }

    public partial class Form1 : Form
    {
        private RadioButton rbExpense = null!;
        private RadioButton rbIncome = null!;
        private ComboBox cbCategory = null!;
        private TextBox txtAmount = null!;
        private TextBox txtDescription = null!;
        private Button btnAdd = null!;

        private ComboBox cbFilterCategory = null!;
        private DataGridView grid = null!;

        private Label lblTotalIncome = null!;
        private Label lblTotalExpense = null!;
        private Label lblBalance = null!;

        private Button btnSave = null!;
        private Button btnLoad = null!;
        private Button btnDelete = null!;

        private List<TransactionItem> transactions = new List<TransactionItem>();

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Налаштування форми
            this.Text = "Персональний фінансовий менеджер";
            this.Size = new Size(820, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

            // 1. Панель введення
            GroupBox gbInput = new GroupBox
            {
                Text = " Нова операція ",
                Location = new Point(15, 10),
                Size = new Size(775, 115),
                ForeColor = Color.FromArgb(44, 62, 80),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            rbExpense = new RadioButton { Text = "Витрата", Location = new Point(15, 30), Checked = true, AutoSize = true, ForeColor = Color.FromArgb(192, 57, 43) };
            rbIncome = new RadioButton { Text = "Дохід", Location = new Point(105, 30), AutoSize = true, ForeColor = Color.FromArgb(39, 174, 96) };

            Label lblCat = new Label { Text = "Категорія:", Location = new Point(200, 31), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            cbCategory = new ComboBox { Location = new Point(275, 27), Width = 130, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };
            cbCategory.Items.AddRange(new string[] { "Зарплата", "Продукти", "Транспорт", "Розваги", "Комунальні", "Інше" });
            cbCategory.SelectedIndex = 1;

            Label lblSum = new Label { Text = "Сума (грн):", Location = new Point(420, 31), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            txtAmount = new TextBox { Location = new Point(500, 27), Width = 90, Font = new Font("Segoe UI", 9F) };

            Label lblDesc = new Label { Text = "Опис:", Location = new Point(15, 73), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            txtDescription = new TextBox { Location = new Point(65, 70), Width = 525, Font = new Font("Segoe UI", 9F) };

            btnAdd = new Button
            {
                Text = "➕ Додати",
                Location = new Point(605, 25),
                Size = new Size(155, 72),
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;

            gbInput.Controls.AddRange(new Control[] { rbExpense, rbIncome, lblCat, cbCategory, lblSum, txtAmount, lblDesc, txtDescription, btnAdd });

            // 2. Фільтрація, Видалення та Файли
            Label lblFilter = new Label { Text = "Фільтр:", Location = new Point(15, 140), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            cbFilterCategory = new ComboBox { Location = new Point(70, 136), Width = 140, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };
            cbFilterCategory.Items.Add("Усі категорії");
            cbFilterCategory.Items.AddRange(new string[] { "Зарплата", "Продукти", "Транспорт", "Розваги", "Комунальні", "Інше" });
            cbFilterCategory.SelectedIndex = 0;
            cbFilterCategory.SelectedIndexChanged += (s, e) => RefreshGrid();

            btnDelete = new Button
            {
                Text = "🗑️ Видалити",
                Location = new Point(225, 134),
                Width = 120,
                Height = 30,
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;

            btnSave = new Button
            {
                Text = "💾 Зберегти як...",
                Location = new Point(485, 134),
                Width = 140,
                Height = 30,
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnLoad = new Button
            {
                Text = "📂 Відкрити файл",
                Location = new Point(635, 134),
                Width = 155,
                Height = 30,
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnLoad.FlatAppearance.BorderSize = 0;
            btnLoad.Click += BtnLoad_Click;

            // 3. Стилізована Таблиця
            grid = new DataGridView
            {
                Location = new Point(15, 175),
                Size = new Size(775, 380),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                EnableHeadersVisualStyles = false
            };
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grid.ColumnHeadersHeight = 32;

            grid.Columns.Add("Date", "Дата");
            grid.Columns.Add("Type", "Тип");
            grid.Columns.Add("Category", "Категорія");
            grid.Columns.Add("Amount", "Сума (грн)");
            grid.Columns.Add("Description", "Опис");

            // 4. Підсумки
            Panel pnlSummary = new Panel
            {
                Location = new Point(15, 568),
                Size = new Size(775, 50),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblTotalIncome = new Label { Text = "Доходи: 0.00 грн", Location = new Point(15, 13), AutoSize = true, ForeColor = Color.FromArgb(39, 174, 96), Font = new Font("Segoe UI", 10.5F, FontStyle.Bold) };
            lblTotalExpense = new Label { Text = "Витрати: 0.00 грн", Location = new Point(260, 13), AutoSize = true, ForeColor = Color.FromArgb(192, 57, 43), Font = new Font("Segoe UI", 10.5F, FontStyle.Bold) };
            lblBalance = new Label { Text = "Баланс: 0.00 грн", Location = new Point(510, 13), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold) };

            pnlSummary.Controls.AddRange(new Control[] { lblTotalIncome, lblTotalExpense, lblBalance });

            this.Controls.AddRange(new Control[] { gbInput, lblFilter, cbFilterCategory, btnDelete, btnSave, btnLoad, grid, pnlSummary });
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (!double.TryParse(txtAmount.Text, out double amount) || amount <= 0)
            {
                MessageBox.Show("Будь ласка, введіть коректну додатню суму!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TransactionItem item = new TransactionItem
            {
                Date = DateTime.Now,
                Type = rbIncome.Checked ? "Дохід" : "Витрата",
                Category = cbCategory.SelectedItem?.ToString() ?? "Інше",
                Amount = amount,
                Description = txtDescription.Text.Trim()
            };

            transactions.Add(item);
            txtAmount.Clear();
            txtDescription.Clear();

            RefreshGrid();
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (grid.CurrentRow != null && grid.CurrentRow.Tag is TransactionItem selectedItem)
            {
                var result = MessageBox.Show("Ви дійсно бажаєте видалити обраний запис?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    transactions.Remove(selectedItem);
                    RefreshGrid();
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, оберіть рядок у таблиці для видалення!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RefreshGrid()
        {
            grid.Rows.Clear();
            string selectedCategory = cbFilterCategory.SelectedItem?.ToString() ?? "Усі категорії";

            var filtered = selectedCategory == "Усі категорії"
                ? transactions
                : transactions.Where(t => t.Category == selectedCategory).ToList();

            foreach (var item in filtered)
            {
                int rowIndex = grid.Rows.Add(
                    item.Date.ToString("yyyy-MM-dd HH:mm"),
                    item.Type,
                    item.Category,
                    item.Amount.ToString("N2"),
                    item.Description
                );

                // Зберігаємо об'єкт у Tag для точного видалення
                grid.Rows[rowIndex].Tag = item;

                // Підсвітка рядка залежно від типу
                if (item.Type == "Дохід")
                {
                    grid.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(235, 247, 240);
                }
            }

            UpdateSummary();
        }

        private void UpdateSummary()
        {
            double income = transactions.Where(t => t.Type == "Дохід").Sum(t => t.Amount);
            double expense = transactions.Where(t => t.Type == "Витрата").Sum(t => t.Amount);
            double balance = income - expense;

            lblTotalIncome.Text = $"Доходи: {income:N2} грн";
            lblTotalExpense.Text = $"Витрати: {expense:N2} грн";
            lblBalance.Text = $"Баланс: {balance:N2} грн";

            lblBalance.ForeColor = balance >= 0 ? Color.FromArgb(41, 128, 185) : Color.FromArgb(192, 57, 43);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "JSON файли (*.json)|*.json|Усі файли (*.*)|*.*";
                sfd.Title = "Оберіть файл для збереження даних";
                sfd.FileName = "my_finance.json";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(sfd.FileName, json);
                        MessageBox.Show("Дані успішно збережено у файл!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка при збереженні: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnLoad_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "JSON файли (*.json)|*.json|Усі файли (*.*)|*.*";
                ofd.Title = "Оберіть файл для завантаження";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string json = File.ReadAllText(ofd.FileName);
                        var loaded = JsonSerializer.Deserialize<List<TransactionItem>>(json);
                        if (loaded != null)
                        {
                            transactions = loaded;
                            RefreshGrid();
                            MessageBox.Show("Дані успішно завантажено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка при завантаженні: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}