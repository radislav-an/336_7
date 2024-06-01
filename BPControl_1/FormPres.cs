using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPControl
{
    public partial class FormPres : Form
    {

        private SqlConnection sqlConnection = null;

        private string surname;
        private string name;
        private string patronymic;
        private DateTime dateOfBirth;

        public FormPres(string surname, string name, string patronymic, DateTime dateOfBirth)
        {
            InitializeComponent();

            this.surname = surname;
            this.name = name;
            this.patronymic = patronymic;
            this.dateOfBirth = dateOfBirth;

        }

        private void FormPres_Load(object sender, EventArgs e)
        {
            // Заповнення полів форми даними пацієнта 
            textBox1.Text = surname;
            textBox2.Text = name;
            textBox3.Text = patronymic;
            dateTimePicker1.Value = dateOfBirth;


            //З'єднання з базою даних
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PatientsDB"].ConnectionString);
            sqlConnection.Open();


            // Запит для отримання ID пацієнта по П.І.Б і даті народження

            int patientID = 0;
          
            string selectQuery = "SELECT ID FROM Patients WHERE Surname = @Surname AND Name = @Name AND Patronymic = @Patronymic AND DateOfBirth = @DateOfBirth";
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("@Surname", surname);
            selectCommand.Parameters.AddWithValue("@Name", name);
            selectCommand.Parameters.AddWithValue("@Patronymic", patronymic);
            selectCommand.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);


             

            using (SqlDataReader reader = selectCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    patientID = reader.GetInt32(0);
                    
                }
            }


            //Загрузка бази даних в таблицю

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT Surname AS \"Призвище\", Name AS \"Ім'я\", Patronymic AS \"По-батькові\", MeasurementDate AS \"Дата Час\",  SystolicPressure AS \"Систолічний тиск\", DiastolicPressure AS \"Діастолічний тиск\", HeartRate AS \"Серцебиття\" FROM Measurements", sqlConnection);
            
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet); 
            dataGridView1.DataSource = dataSet.Tables[0];

            //(dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"SystolicPressure >= 140";
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Призвище LIKE '%{textBox1.Text}%'";


            //Режим наявності строки вибору в таблиці
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //(dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Surname LIKE '%{textBox1.Text}%'";
        }
    }
}
