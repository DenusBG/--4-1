using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuizApp
{
    public partial class Form1 : Form
    {
        // Запитання 1 (RadioButton)
        private RadioButton q1Opt1 = null!;
        private RadioButton q1Opt2 = null!;
        private RadioButton q1Opt3 = null!;

        // Запитання 2 (RadioButton)
        private RadioButton q2Opt1 = null!;
        private RadioButton q2Opt2 = null!;
        private RadioButton q2Opt3 = null!;

        // Запитання 3 (CheckBox)
        private CheckBox q3Opt1 = null!;
        private CheckBox q3Opt2 = null!;
        private CheckBox q3Opt3 = null!;

        private Button btnSubmit = null!;

        public Form1()
        {
            InitializeComponent();
            InitializeQuizComponents();
        }

        private void InitializeQuizComponents()
        {
            this.Text = "Тестування знань C# та WinForms";
            this.Size = new Size(520, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AutoScroll = true;

            int top = 20;

            // Запитання 1
            Label lblQ1 = new Label { Text = "1. Яка мова програмування використовується у WinForms?", Location = new Point(20, top), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };
            top += 25;
            GroupBox gbQ1 = new GroupBox { Location = new Point(20, top), Size = new Size(460, 90) };
            q1Opt1 = new RadioButton { Text = "C#", Location = new Point(15, 20), AutoSize = true };
            q1Opt2 = new RadioButton { Text = "Python", Location = new Point(15, 42), AutoSize = true };
            q1Opt3 = new RadioButton { Text = "JavaScript", Location = new Point(15, 64), AutoSize = true };
            gbQ1.Controls.AddRange(new Control[] { q1Opt1, q1Opt2, q1Opt3 });
            top += 100;

            // Запитання 2
            Label lblQ2 = new Label { Text = "2. Яка платформа є основою для запуску C# додатка?", Location = new Point(20, top), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };
            top += 25;
            GroupBox gbQ2 = new GroupBox { Location = new Point(20, top), Size = new Size(460, 90) };
            q2Opt1 = new RadioButton { Text = "JVM", Location = new Point(15, 20), AutoSize = true };
            q2Opt2 = new RadioButton { Text = ".NET", Location = new Point(15, 42), AutoSize = true };
            q2Opt3 = new RadioButton { Text = "Node.js", Location = new Point(15, 64), AutoSize = true };
            gbQ2.Controls.AddRange(new Control[] { q2Opt1, q2Opt2, q2Opt3 });
            top += 100;

            // Запитання 3
            Label lblQ3 = new Label { Text = "3. Оберіть елементи керування Windows Forms (декілька варіантів):", Location = new Point(20, top), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };
            top += 25;
            Panel panelQ3 = new Panel { Location = new Point(20, top), Size = new Size(460, 90) };
            q3Opt1 = new CheckBox { Text = "Button", Location = new Point(15, 10), AutoSize = true };
            q3Opt2 = new CheckBox { Text = "DataGridView", Location = new Point(15, 32), AutoSize = true };
            q3Opt3 = new CheckBox { Text = "<div> tag", Location = new Point(15, 54), AutoSize = true };
            panelQ3.Controls.AddRange(new Control[] { q3Opt1, q3Opt2, q3Opt3 });
            top += 100;

            // Кнопка перевірки
            btnSubmit = new Button { Text = "Завершити тест та дізнатися результат", Location = new Point(20, top), Size = new Size(460, 40), Font = new Font(this.Font, FontStyle.Bold) };
            btnSubmit.Click += BtnSubmit_Click;

            this.Controls.AddRange(new Control[] { lblQ1, gbQ1, lblQ2, gbQ2, lblQ3, panelQ3, btnSubmit });
        }

        private void BtnSubmit_Click(object? sender, EventArgs e)
        {
            int score = 0;

            // Перевірка Q1 (1 бал)
            if (q1Opt1.Checked) score += 1;

            // Перевірка Q2 (1 бал)
            if (q2Opt2.Checked) score += 1;

            // Перевірка Q3 (по 1 балу за кожну правильну галочку)
            if (q3Opt1.Checked) score += 1;
            if (q3Opt2.Checked) score += 1;
            if (q3Opt3.Checked) score -= 1; // Штраф за невірний варіант

            if (score < 0) score = 0;

            MessageBox.Show($"Ви набрали: {score} з 4 можливих балів!", "Результати тестування", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}