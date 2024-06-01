using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace BPControl
{
   
    
    public partial class FormAdd : Form
    {
        private SqlConnection sqlConnection = null;
        
        public FormAdd()
        
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PatientsDB"].ConnectionString);
            sqlConnection.Open();

            SqlCommand command = new SqlCommand(
                $"INSERT INTO Patients (Surname, Name, Patronymic, DateOfBirth) VALUES (@Surname, @Name, @Patronymic, @DateOfBirth)",
                sqlConnection);

            
            command.Parameters.AddWithValue("Surname", textBox1.Text);
            command.Parameters.AddWithValue("Name", textBox2.Text);
            command.Parameters.AddWithValue("Patronymic", textBox3.Text);
            command.Parameters.AddWithValue("DateOfBirth", dateTimePicker1.Value);

            if (command.ExecuteNonQuery() == 1)
                MessageBox.Show("Дані пацієнта успішно додано до бази даних.");

            

            else
                MessageBox.Show("Помилка додавання даних");



           // Очистити поля для вводу даних
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            dateTimePicker1.Value = DateTime.Now;

        }

        private void FormAdd_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
