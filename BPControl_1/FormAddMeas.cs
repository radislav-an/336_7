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

namespace BPControl
{
    public partial class FormAddMeas : Form
    {
        private SqlConnection sqlConnection = null;

        private string surname;
        private string name;
        private string patronymic;
        private DateTime dateOfBirth;

        public FormAddMeas(string surname, string name, string patronymic, DateTime dateOfBirth)
        {
            InitializeComponent();

            this.surname = surname;
            this.name = name;
            this.patronymic = patronymic;
            this.dateOfBirth = dateOfBirth;

        }
        private void FormAddMeas_Load(object sender, EventArgs e)
        {
            // Заповнення полів форми даними пацієнта 
            textBox1.Text = surname;
            textBox2.Text = name;
            textBox3.Text = patronymic;
            dateTimePicker1.Value = dateOfBirth;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Закриття форми
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /// Отримання даних вимірювання з полів форми
            string SystolicPressure = textBox4.Text.Trim();
            string DiastolicPressure = textBox5.Text.Trim();
            string HeartRate = textBox6.Text.Trim();
            DateTime MeasurementDate = dateTimePicker2.Value;


            //З'єднання з базою даних
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PatientsDB"].ConnectionString);
            sqlConnection.Open();

            // Запит для отримання ID пацієнта по П.І.Б і даті народження
            string selectQuery = "SELECT ID FROM Patients WHERE Surname = @Surname AND Name = @Name AND Patronymic = @Patronymic AND DateOfBirth = @DateOfBirth";
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("@Surname", surname);
            selectCommand.Parameters.AddWithValue("@Name", name);
            selectCommand.Parameters.AddWithValue("@Patronymic", patronymic);
            selectCommand.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);


            int patientID = 0;


            using (SqlDataReader reader = selectCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    patientID = reader.GetInt32(0);
                }
            }

            // Запис даних в базі даних вимірювання
            try
            {

                if (patientID > 0)
                {
                    // Запис даних в базі даних
                    SqlCommand command = new SqlCommand(
                    $"INSERT INTO Measurements (PatientID, MeasurementDate, SystolicPressure, DiastolicPressure, HeartRate, Surname, Name, Patronymic) VALUES (@patientID, @MeasurementDate, @SystolicPressure, @DiastolicPressure, @HeartRate, @Surname, @Name, @Patronymic)",
                    sqlConnection);

                    command.Parameters.AddWithValue("Surname", surname);
                    command.Parameters.AddWithValue("Name", name);
                    command.Parameters.AddWithValue("Patronymic", patronymic);

                    command.Parameters.AddWithValue("PatientID", patientID);
                    command.Parameters.AddWithValue("SystolicPressure", textBox4.Text);
                    command.Parameters.AddWithValue("DiastolicPressure", textBox5.Text);
                    command.Parameters.AddWithValue("HeartRate", textBox6.Text);
                    command.Parameters.AddWithValue("MeasurementDate", dateTimePicker2.Value);

                    if (command.ExecuteNonQuery() == 1)
                        MessageBox.Show("Дані вимірювання успішно додано до бази даних.");

                    else
                        MessageBox.Show("Помилка додавання даних");


                }
                else
                {
                    MessageBox.Show("Пацієнта з такими даними не знайдено в базі даних.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при додаванні даних: " + ex.Message);
            }

            this.Close();


        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
