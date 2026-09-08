using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HabitExpensesApp
{
    public partial class Form1 : Form
    {
        private ComboBox cbHabits = null!;
        private TextBox txtDailyCost = null!;
        private Button btnCalculate = null!;

        private Label lblMonth = null!;
        private Label lblYear = null!;
        private Label lblFiveYears = null!;
        private ListBox lstComparisons = null!;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Калькулятор щоденних витрат та альтернативних покупок";
            this.Size = new Size(680, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

            // 1. Панель введення
            GroupBox gbInput = new GroupBox
            {
                Text = " Щоденна витрата ",
                Location = new Point(15, 10),
                Size = new Size(635, 100),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            Label lblPreset = new Label { Text = "Шаблон:", Location = new Point(15, 30), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            cbHabits = new ComboBox { Location = new Point(80, 26), Width = 160, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };
            cbHabits.Items.AddRange(new string[] { "☕ Кава (60 грн)", "🚬 Цигарки (100 грн)", "🚕 Таксі (250 грн)", "🍔 Фастфуд (180 грн)", "Свій варіант" });
            cbHabits.SelectedIndex = 0;
            cbHabits.SelectedIndexChanged += CbHabits_SelectedIndexChanged;

            Label lblCost = new Label { Text = "Сума в день (грн):", Location = new Point(260, 30), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            txtDailyCost = new TextBox { Text = "60", Location = new Point(380, 26), Width = 90, Font = new Font("Segoe UI", 9F) };

            btnCalculate = new Button
            {
                Text = "📊 Розрахувати",
                Location = new Point(485, 24),
                Size = new Size(135, 60),
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnCalculate.FlatAppearance.BorderSize = 0;
            btnCalculate.Click += BtnCalculate_Click;

            gbInput.Controls.AddRange(new Control[] { lblPreset, cbHabits, lblCost, txtDailyCost, btnCalculate });

            // 2. Результати підрахунку
            GroupBox gbResults = new GroupBox
            {
                Text = " Підсумок витрат ",
                Location = new Point(15, 120),
                Size = new Size(635, 110),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            lblMonth = new Label { Text = "За місяць (30 днів): 0 грн", Location = new Point(15, 30), AutoSize = true, ForeColor = Color.FromArgb(192, 57, 43), Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            lblYear = new Label { Text = "За 1 рік (365 днів): 0 грн", Location = new Point(15, 55), AutoSize = true, ForeColor = Color.FromArgb(192, 57, 43), Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            lblFiveYears = new Label { Text = "За 5 років: 0 грн", Location = new Point(15, 80), AutoSize = true, ForeColor = Color.FromArgb(192, 57, 43), Font = new Font("Segoe UI", 10.5F, FontStyle.Bold) };

            gbResults.Controls.AddRange(new Control[] { lblMonth, lblYear, lblFiveYears });

            // 3. Порівняння з покупками
            GroupBox gbComparisons = new GroupBox
            {
                Text = " 💡 За ці гроші за 5 років можна купити: ",
                Location = new Point(15, 240),
                Size = new Size(635, 280),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(39, 174, 96)
            };

            lstComparisons = new ListBox
            {
                Location = new Point(15, 25),
                Size = new Size(605, 240),
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                SelectionMode = SelectionMode.None
            };

            gbComparisons.Controls.Add(lstComparisons);

            this.Controls.AddRange(new Control[] { gbInput, gbResults, gbComparisons });

            CalculateExpenses();
        }

        private void CbHabits_SelectedIndexChanged(object? sender, EventArgs e)
        {
            switch (cbHabits.SelectedIndex)
            {
                case 0: txtDailyCost.Text = "60"; break;
                case 1: txtDailyCost.Text = "100"; break;
                case 2: txtDailyCost.Text = "250"; break;
                case 3: txtDailyCost.Text = "180"; break;
                default: break;
            }
        }

        private void BtnCalculate_Click(object? sender, EventArgs e)
        {
            CalculateExpenses();
        }

        private void CalculateExpenses()
        {
            if (!double.TryParse(txtDailyCost.Text, out double daily) || daily <= 0)
            {
                MessageBox.Show("Введіть коректну додатню суму щоденних витрат!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double monthCost = daily * 30;
            double yearCost = daily * 365;
            double fiveYearsCost = yearCost * 5;

            lblMonth.Text = $"За місяць (30 днів): {monthCost:N0} грн";
            lblYear.Text = $"За 1 рік (365 днів): {yearCost:N0} грн";
            lblFiveYears.Text = $"За 5 років: {fiveYearsCost:N0} грн";

            // Оновлюємо список порівнянь
            lstComparisons.Items.Clear();

            var items = new List<(string Name, double Price)>
            {
                ("🎧 Бездротові навушники", 6000),
                ("📱 Флагманський смартфон", 45000),
                ("💻 Потужний ноутбук", 35000),
                ("✈️ Путівка на відпочинок / відпустка", 40000),
                ("🛵 Електроскутер", 30000),
                ("🚗 Вживаний автомобіль", 180000)
            };

            foreach (var item in items)
            {
                if (fiveYearsCost >= item.Price)
                {
                    int count = (int)(fiveYearsCost / item.Price);
                    lstComparisons.Items.Add($"• {item.Name} ({item.Price:N0} грн) — {count} шт.");
                }
            }

            if (lstComparisons.Items.Count == 0)
            {
                lstComparisons.Items.Add("Суми поки замало для більшості великих покупок, але це чудовий початок заощаджень!");
            }
        }
    }
}