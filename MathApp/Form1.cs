using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp
{
    public partial class Form1 : Form
    {
        private TextBox txtSum = null!;
        private TextBox txtRate = null!;
        private TextBox txtTerm = null!;
        private Button btnCalculate = null!;
        private DataGridView grid = null!;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Кредитний калькулятор";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblSum = new Label { Text = "Сума (грн):", Location = new Point(20, 20), AutoSize = true };
            txtSum = new TextBox { Text = "100000", Location = new Point(110, 17), Width = 90 };

            Label lblRate = new Label { Text = "Ставка (%):", Location = new Point(220, 20), AutoSize = true };
            txtRate = new TextBox { Text = "15", Location = new Point(300, 17), Width = 70 };

            Label lblTerm = new Label { Text = "Термін (міс):", Location = new Point(390, 20), AutoSize = true };
            txtTerm = new TextBox { Text = "12", Location = new Point(480, 17), Width = 70 };

            btnCalculate = new Button { Text = "Розрахувати графік", Location = new Point(20, 50), Width = 530, Height = 32 };
            btnCalculate.Click += BtnCalculate_Click;

            grid = new DataGridView
            {
                Location = new Point(20, 95),
                Size = new Size(645, 340),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };

            this.Controls.AddRange(new Control[] { lblSum, txtSum, lblRate, txtRate, lblTerm, txtTerm, btnCalculate, grid });
        }

        private void BtnCalculate_Click(object? sender, EventArgs e)
        {
            if (!double.TryParse(txtSum.Text, out double totalSum) ||
                !double.TryParse(txtRate.Text, out double annualRate) ||
                !int.TryParse(txtTerm.Text, out int months) || months <= 0)
            {
                MessageBox.Show("Будь ласка, введіть коректні числові значення!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            grid.Rows.Clear();
            grid.Columns.Clear();
            grid.Columns.Add("Month", "Місяць");
            grid.Columns.Add("MainDebt", "Основний борг (грн)");
            grid.Columns.Add("Interest", "Відсотки (грн)");
            grid.Columns.Add("TotalPayment", "Загальний платіж (грн)");
            grid.Columns.Add("Balance", "Залишок боргу (грн)");

            double monthlyMainDebt = totalSum / months;
            double monthlyRate = (annualRate / 100.0) / 12.0;
            double remainingBalance = totalSum;

            for (int month = 1; month <= months; month++)
            {
                double interestPayment = remainingBalance * monthlyRate;
                double totalMonthlyPayment = monthlyMainDebt + interestPayment;
                remainingBalance -= monthlyMainDebt;

                if (remainingBalance < 0 || month == months) remainingBalance = 0;

                grid.Rows.Add(
                    month,
                    monthlyMainDebt.ToString("N2"),
                    interestPayment.ToString("N2"),
                    totalMonthlyPayment.ToString("N2"),
                    remainingBalance.ToString("N2")
                );
            }
        }
    }
}