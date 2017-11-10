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

        private DataTable GetApplicationData()
        {
            DataTable dtApplication = new DataTable();

            string connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\kenne\Source\Repos\ApplicationStorer\ApplicationStorer\ApplicationStorer\Data\ApplicationData.mdf; Integrated Security = True; Connect Timeout = 30";
            string query = "SELECT * FROM ApplicationTable";


            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dtApplication.Load(reader);
            }

            return dtApplication;

        }
    }
}
