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
    public partial class FormAll : Form
    {

        private SqlConnection sqlConnection = null;

        public FormAll()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FormAll_Load(object sender, EventArgs e)
        {
             
             //З'єднання з базою даних
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PatientsDB"].ConnectionString);
            sqlConnection.Open();

            //Загрузка бази даних в таблицю

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT Surname AS \"Призвище\", Name AS \"Ім'я\", Patronymic AS \"По-батькові\", MeasurementDate AS \"Дата Час\",  SystolicPressure AS \"Систолічний тиск\", DiastolicPressure AS \"Діастолічний тиск\", HeartRate AS \"Серцебиття\" FROM Measurements", sqlConnection);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];

            //(dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"SystolicPressure >= 140";

            //Режим наявності строки вибору в таблиці
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


        }
    }
}
