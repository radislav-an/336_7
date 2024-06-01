using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BPControl
{
    public partial class FormCrit : Form
    {
        private SqlConnection sqlConnection = null;

        public FormCrit()
        {
            InitializeComponent();
        }

        private void FormCrit_Load(object sender, EventArgs e)
        {
            //З'єднання з базою даних
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PatientsDB"].ConnectionString);
            sqlConnection.Open();



            //Загрузка бази даних в таблицю

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT Surname AS \"Прізвище\", Name AS \"Ім'я\", Patronymic AS \"По-батькові\", MeasurementDate AS \"Дата_Час\",  SystolicPressure AS \"Систолічний_тиск\", DiastolicPressure AS \"Діастолічний_тиск\", HeartRate AS \"Серцебиття\" FROM Measurements", sqlConnection);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];

            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Систолічний_тиск >= 140 or Систолічний_тиск <= 100";
            //(dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Прізвище LIKE '%{textBox1.Text}%'";


            //Режим наявності строки вибору в таблиці
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Прізвище LIKE '%{textBox1.Text}%' and (Систолічний_тиск >= 140 or Систолічний_тиск <= 100)";
            //(dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Surname LIKE '%{textBox1.Text}%'";

        }
    }
}
