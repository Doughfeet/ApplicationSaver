﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationStorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetApplicationData();
        }

        private DataTable GetApplicationData()
        {
            DataTable dtApplication = new DataTable();

            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\kenne\Source\Repos\ApplicationStorer\ApplicationStorer\ApplicationStorer\Data\ApplicationData.mdf; Integrated Security = True; Connect Timeout = 30";
            string query = "SELECT * FROM ApplicationTable";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dap = new SqlDataAdapter(query, connection);
                dap.Fill(dtApplication);
                return dtApplication;
            }
        }

        private void AddButton1_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\kenne\Source\Repos\ApplicationStorer\ApplicationStorer\ApplicationStorer\Data\ApplicationData.mdf; Integrated Security = True; Connect Timeout = 30";
            string query = "" +
                "INSERT INTO ApplicationTable" +
                    "(Company, WorkTitle, Duration, AppliedDate, DeadlineDate, Webpage)" +
                "VALUES" +
                    "(@Company, @WorkTitle, @Duration, @AppliedDate, @DeadlineDate, @Webpage)";


            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();

                cmd.Parameters.AddWithValue("@Company", CompanyTextBox.Text);
                cmd.Parameters.AddWithValue("@WorkTitle", WorkingTitleTextBox.Text);
                cmd.Parameters.AddWithValue("@Duration", DurationComboBox.Text);
                cmd.Parameters.AddWithValue("@AppliedDate", AppliedDateTimePicker.Value);
                cmd.Parameters.AddWithValue("@DeadlineDate", DeadlineDateTimePicker.Value);
                cmd.Parameters.AddWithValue("@Webpage", WebpageTextBox.Text);
  
                cmd.ExecuteNonQuery();
                dataGridView1.DataSource = GetApplicationData();

                Clear();
            }
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\kenne\Source\Repos\ApplicationStorer\ApplicationStorer\ApplicationStorer\Data\ApplicationData.mdf; Integrated Security = True; Connect Timeout = 30";
            string query = @"DELETE FROM ApplicationTable 
                            WHERE Id = @rowId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();
                try
                {
                    int selectedIndex = dataGridView1.SelectedRows[0].Index;
                    int rowId = Convert.ToInt32(dataGridView1[0, selectedIndex].Value);

                    cmd.Parameters.Add("@rowId", SqlDbType.Int).Value = rowId;
                    cmd.ExecuteNonQuery();
                    dataGridView1.Rows.RemoveAt(selectedIndex);
                    Clear();
                }
                catch (Exception)
                {
                    MessageBox.Show("There is no data", "Work application storer", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                                   0);
                }
            }
        }
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\kenne\Source\Repos\ApplicationStorer\ApplicationStorer\ApplicationStorer\Data\ApplicationData.mdf; Integrated Security = True; Connect Timeout = 30";
            string query = @"
                            SELECT * FROM ApplicationTable
                            UPDATE ";
        }


        //private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    try
        //    {
        //        selectedRow =

        //        CompanyTextBox.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        //        WorkingTitleTextBox.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
        //        DurationComboBox.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
        //        AppliedDateTimePicker.Value = DateTime.Parse(dataGridView1.SelectedRows[0].Cells[4].Value.ToString());
        //        DeadlineDateTimePicker.Value = DateTime.Parse(dataGridView1.SelectedRows[0].Cells[5].Value.ToString());
        //        WebpageTextBox.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("No data!");
        //    }

        //}

        private void Clear()
        {
            CompanyTextBox.Text = "";
            WorkingTitleTextBox.Text = "";
            DurationComboBox.Text = "";
            AppliedDateTimePicker.Value = DateTime.Now;
            DeadlineDateTimePicker.Value = DateTime.Now;
            WebpageTextBox.Text = "";

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[selectedRow];

            CompanyTextBox.Text = row.Cells[1].Value.ToString();
            WorkingTitleTextBox.Text = row.Cells[2].Value.ToString();
            DurationComboBox.Text = row.Cells[3].Value.ToString();
            AppliedDateTimePicker.Value = DateTime.Parse(row.Cells[4].Value.ToString());
            DeadlineDateTimePicker.Value = DateTime.Parse(row.Cells[5].Value.ToString());
            WebpageTextBox.Text = row.Cells[6].Value.ToString();
        }
    }
}
