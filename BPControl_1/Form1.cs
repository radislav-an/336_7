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
using System.Xml.Linq;

namespace BPControl
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;


        public Form1()
        
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PatientsDB"].ConnectionString);
            sqlConnection.Open();

            //Загрузка бази даних в таблицю

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT Surname AS \"Прізвище\", Name AS \"Ім'я\", Patronymic AS \"По-батькові\", DateOfBirth AS \"Дата народження\" FROM Patients", sqlConnection);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];

            //Режим наявності строки вибору в таблиці
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Додавання даних пацієнта
            FormAdd formAdd = new FormAdd();
            formAdd.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Оновлення даних в таблиці

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT Surname AS \"Прізвище\", Name AS \"Ім'я\", Patronymic AS \"По-батькові\", DateOfBirth AS \"Дата народження\" FROM Patients", sqlConnection);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //Видалення пацієнта

            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Ви впевнені, що хочете вилучити цього пацієнта?", "Вилучення пацієнта", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string surname = dataGridView1.SelectedRows[0].Cells["Прізвище"].Value.ToString();
                    string name = dataGridView1.SelectedRows[0].Cells["Ім'я"].Value.ToString();
                    string patronymic = dataGridView1.SelectedRows[0].Cells["По-батькові"].Value.ToString();
                    DateTime dateOfBirth = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["Дата народження"].Value);

                    try
                    {
                        //sqlConnection.Open();

                        string deleteQuery = "DELETE FROM Patients WHERE Surname = @Surname AND Name = @Name AND Patronymic = @Patronymic AND DateOfBirth = @DateOfBirth";

                        SqlCommand sqlCommand = new SqlCommand(deleteQuery, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@Surname", surname);
                        sqlCommand.Parameters.AddWithValue("@Name", name);
                        sqlCommand.Parameters.AddWithValue("@Patronymic", patronymic);
                        sqlCommand.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);

                        sqlCommand.ExecuteNonQuery();

                        MessageBox.Show("Дані пацієнта успішно вилучено з бази даних.");

                        // Оновити DataGridView
                        SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT Surname AS \"Прізвище\", Name AS \"Ім'я\", Patronymic AS \"По-батькові\", DateOfBirth AS \"Дата народження\" FROM Patients", sqlConnection);
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                        dataGridView1.DataSource = dataSet.Tables[0];
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при вилученні даних пацієнта: " + ex.Message);
                    }
                    finally
                    {
                        //if (sqlConnection.State == System.Data.ConnectionState.Open)
                        //    sqlConnection.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Виберіть пацієнта для вилучення.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Редагування даних пацієнта
            
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string surname = dataGridView1.SelectedRows[0].Cells["Прізвище"].Value.ToString();
                string name = dataGridView1.SelectedRows[0].Cells["Ім'я"].Value.ToString();
                string patronymic = dataGridView1.SelectedRows[0].Cells["По-батькові"].Value.ToString();
                DateTime dateOfBirth = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["Дата народження"].Value);

                FormEdit formEdit = new FormEdit(surname, name, patronymic, dateOfBirth);
                formEdit.ShowDialog();

                // Оновити дані в dataGridView1 після редагування
                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT Surname AS \"Прізвище\", Name AS \"Ім'я\", Patronymic AS \"По-батькові\", DateOfBirth AS \"Дата народження\" FROM Patients", sqlConnection);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];

            }
            else
            {
                MessageBox.Show("Виберіть пацієнта для редагування.");

            }  
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Внесення даних вимірювання тиску у пацієнта

            if (dataGridView1.SelectedRows.Count > 0)
            {
                string surname = dataGridView1.SelectedRows[0].Cells["Прізвище"].Value.ToString();
                string name = dataGridView1.SelectedRows[0].Cells["Ім'я"].Value.ToString();
                string patronymic = dataGridView1.SelectedRows[0].Cells["По-батькові"].Value.ToString();
                DateTime dateOfBirth = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["Дата народження"].Value);

                FormAddMeas formAddMeas = new FormAddMeas(surname, name, patronymic, dateOfBirth);
                formAddMeas.ShowDialog();


            }
            else
            {
                MessageBox.Show("Виберіть пацієнта.");

            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Виведення даних вимірювання тиску у пацієнта

            if (dataGridView1.SelectedRows.Count > 0)
            {
                string surname = dataGridView1.SelectedRows[0].Cells["Прізвище"].Value.ToString();
                string name = dataGridView1.SelectedRows[0].Cells["Ім'я"].Value.ToString();
                string patronymic = dataGridView1.SelectedRows[0].Cells["По-батькові"].Value.ToString();
                DateTime dateOfBirth = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["Дата народження"].Value);

                FormPres formPres = new FormPres(surname, name, patronymic, dateOfBirth);
                formPres.ShowDialog();


            }
            else
            {
                MessageBox.Show("Виберіть пацієнта.");

            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormAll formAll = new FormAll();
            formAll.ShowDialog();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            FormCrit formCrit = new FormCrit();
            formCrit.ShowDialog();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Прізвище LIKE '%{textBox1.Text}%'";
            //(dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Surname LIKE '%{textBox1.Text}%'";

        }
    }
}
