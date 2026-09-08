using System;
using System.Drawing;
using System.Windows.Forms;

namespace SubscriptionApp
{
    public partial class Form1 : Form
    {
        private TextBox txtSalary = null!;
        private TextBox txtSubName = null!;
        private TextBox txtSubPrice = null!;
        private Button btnAdd = null!;
        private DataGridView grid = null!;
        private Label lblTotalExpenses = null!;
        private Label lblRemaining = null!;

        private double totalExpenses = 0;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Калькулятор підписок та бюджету";
            this.Size = new Size(600, 550);
            this.StartPosition = FormStartPosition.CenterScreen;

            // 1. Поле введення зарплати
            Label lblSalary = new Label { Text = "Щомісячна зарплата (грн):", Location = new Point(20, 20), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };
            txtSalary = new TextBox { Text = "30000", Location = new Point(210, 17), Width = 110 };

            // 2. Поля введення підписки
            Label lblSubName = new Label { Text = "Назва підписки:", Location = new Point(20, 60), AutoSize = true };
            txtSubName = new TextBox { Text = "Netflix", Location = new Point(125, 57), Width = 120 };

            Label lblSubPrice = new Label { Text = "Ціна (грн):", Location = new Point(260, 60), AutoSize = true };
            txtSubPrice = new TextBox { Text = "350", Location = new Point(335, 57), Width = 80 };

            btnAdd = new Button { Text = "Додати", Location = new Point(430, 55), Width = 120, Height = 27 };
            btnAdd.Click += BtnAdd_Click;

            // 3. Таблиця підписок
            grid = new DataGridView
            {
                Location = new Point(20, 100),
                Size = new Size(530, 260),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };
            grid.Columns.Add("SubName", "Назва підписки");
            grid.Columns.Add("SubPrice", "Вартість (грн)");

            // 4. Підсумкові мітки
            lblTotalExpenses = new Label { Text = "Загальні витрати на підписки: 0.00 грн", Location = new Point(20, 380), AutoSize = true, Font = new Font(this.Font.FontFamily, 10, FontStyle.Bold) };
            lblRemaining = new Label { Text = "Залишок від зарплати: 0.00 грн", Location = new Point(20, 410), AutoSize = true, Font = new Font(this.Font.FontFamily, 10, FontStyle.Bold) };

            this.Controls.AddRange(new Control[] { lblSalary, txtSalary, lblSubName, txtSubName, lblSubPrice, txtSubPrice, btnAdd, grid, lblTotalExpenses, lblRemaining });

            // Перерахунок залишку при зміні зарплати
            txtSalary.TextChanged += TxtSalary_TextChanged;

            UpdateSummary();
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            string name = txtSubName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Будь ласка, введіть назву підписки!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(txtSubPrice.Text, out double price) || price <= 0)
            {
                MessageBox.Show("Будь ласка, введіть коректну ціну підписки!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Додаємо в таблицю
            grid.Rows.Add(name, price.ToString("N2"));
            totalExpenses += price;

            // Очищення полів
            txtSubName.Clear();
            txtSubPrice.Clear();
            txtSubName.Focus();

            UpdateSummary();
        }

        private void TxtSalary_TextChanged(object? sender, EventArgs e)
        {
            UpdateSummary();
        }

        private void UpdateSummary()
        {
            double.TryParse(txtSalary.Text, out double salary);
            double remaining = salary - totalExpenses;

            lblTotalExpenses.Text = $"Загальні витрати на підписки: {totalExpenses:N2} грн";
            lblRemaining.Text = $"Залишок від зарплати: {remaining:N2} грн";

            // Якщо витрати перевищують зарплату — виділяємо червоним
            if (remaining < 0)
            {
                lblRemaining.ForeColor = Color.Red;
            }
            else
            {
                lblRemaining.ForeColor = Color.DarkGreen;
            }
        }
    }
}