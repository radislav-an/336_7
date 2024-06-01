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
    public partial class FormEdit : Form
    {
        private SqlConnection sqlConnection = null;
        
        private string surname;
        private string name;
        private string patronymic;
        private DateTime dateOfBirth;

      
        public FormEdit(string surname, string name, string patronymic, DateTime dateOfBirth)
        {
            InitializeComponent();

            this.surname = surname;
            this.name = name;
            this.patronymic = patronymic;
            this.dateOfBirth = dateOfBirth;
        }

        private void FormEdit_Load(object sender, EventArgs e)
        {
            // Заповнення полів форми даними пацієнта вибраними для редагування
            textBox1.Text = surname;
            textBox2.Text = name;
            textBox3.Text = patronymic;
            dateTimePicker1.Value = dateOfBirth;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /// Отримання нових даних пацієнта з полів форми
            string newSurname = textBox1.Text.Trim();
            string newName = textBox2.Text.Trim();
            string newPatronymic = textBox3.Text.Trim();
            DateTime newDateOfBirth = dateTimePicker1.Value;

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

            // Оновлення даних пацієнта в базі даних 
            try
            {

                    if (patientID > 0)
                    {
                        // Оновлення даних пацієнта в базі даних
                        string updateQuery = "UPDATE Patients SET Surname = @NewSurname, Name = @NewName, Patronymic = @NewPatronymic, DateOfBirth = @NewDateOfBirth WHERE ID = @ID";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, sqlConnection);
                        updateCommand.Parameters.AddWithValue("@NewSurname", newSurname);
                        updateCommand.Parameters.AddWithValue("@NewName", newName);
                        updateCommand.Parameters.AddWithValue("@NewPatronymic", newPatronymic);
                        updateCommand.Parameters.AddWithValue("@NewDateOfBirth", newDateOfBirth);
                        updateCommand.Parameters.AddWithValue("@ID", patientID);

                        int rowsAffected = updateCommand.ExecuteNonQuery();


                    if (rowsAffected > 0)
                        {
                            MessageBox.Show("Дані пацієнта успішно оновлені в базі даних.");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Не вдалося оновити дані пацієнта в базі даних.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пацієнта з такими даними не знайдено в базі даних.");
                    }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при оновленні даних пацієнта в базі даних: " + ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
