using System;
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

        int selectedRow;
        string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\kenne\Source\Repos\ApplicationStorer\ApplicationStorer\ApplicationStorer\Data\ApplicationData.mdf; Integrated Security = True; Connect Timeout = 30";
        

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
            string query = @"
                INSERT INTO ApplicationTable 
                    (Company, WorkTitle, Duration, AppliedDate, DeadlineDate, Webpage, Information)
                VALUES
                    (@Company, @WorkTitle, @Duration, @AppliedDate, @DeadlineDate, @Webpage, @Information)";


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
                cmd.Parameters.AddWithValue("@Information", InformationRichTextBox1.Text);

                cmd.ExecuteNonQuery();
                dataGridView1.DataSource = GetApplicationData();

                Clear();
            }
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            string query = @"
                            DELETE FROM ApplicationTable 
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
            string query = @"
                            UPDATE ApplicationTable 
                            SET 
                                Company = @Company, 
                                WorkTitle = @WorkTitle, 
                                Duration = @Duration, 
                                AppliedDate = @AppliedDate, 
                                DeadlineDate = @DeadlineDate, 
                                Webpage = @Webpage,
                                Information = @Information 
                            WHERE Id = @rowId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();


                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                int rowId = Convert.ToInt32(dataGridView1[0, selectedIndex].Value);

                cmd.Parameters.Add("@rowId", SqlDbType.Int).Value = rowId;

                cmd.Parameters.AddWithValue("@Company", CompanyTextBox.Text);
                cmd.Parameters.AddWithValue("@WorkTitle", WorkingTitleTextBox.Text);
                cmd.Parameters.AddWithValue("@Duration", DurationComboBox.Text);
                cmd.Parameters.AddWithValue("@AppliedDate", AppliedDateTimePicker.Value);
                cmd.Parameters.AddWithValue("@DeadlineDate", DeadlineDateTimePicker.Value);
                cmd.Parameters.AddWithValue("@Webpage", WebpageTextBox.Text);
                cmd.Parameters.AddWithValue("@Information", InformationRichTextBox1.Text);

                cmd.ExecuteNonQuery();
                dataGridView1.DataSource = GetApplicationData();

                Clear();
            }

        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                selectedRow = e.RowIndex;
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                CompanyTextBox.Text = row.Cells[1].Value.ToString();
                WorkingTitleTextBox.Text = row.Cells[2].Value.ToString();
                DurationComboBox.Text = row.Cells[3].Value.ToString();
                AppliedDateTimePicker.Value = DateTime.Parse(row.Cells[4].Value.ToString());
                DeadlineDateTimePicker.Value = DateTime.Parse(row.Cells[5].Value.ToString());
                WebpageTextBox.Text = row.Cells[6].Value.ToString();
                InformationRichTextBox1.Text = row.Cells[7].Value.ToString();
            }
            catch (Exception)
            {

            }

        }

        private void Clear()
        {
            CompanyTextBox.Text = "";
            WorkingTitleTextBox.Text = "";
            DurationComboBox.Text = "";
            AppliedDateTimePicker.Value = DateTime.Now;
            DeadlineDateTimePicker.Value = DateTime.Now;
            WebpageTextBox.Text = "";
            InformationRichTextBox1.Clear();

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.kennethblix.no");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start($"{WebpageTextBox.Text}");
            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect website!", "Work application saver", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                                   0);
            }

        }
    }
}
