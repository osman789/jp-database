using Npgsql;
using System.Configuration;
using System.Data;

namespace jp_database
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //ConnectionString(s) are handled here
        //------------------------------------
        readonly string stdPGcon = ConfigurationManager.ConnectionStrings["PGcon"].ConnectionString;

        private void ReadRecords()
        {
            try
            {
                using (NpgsqlConnection myPGConnection = new NpgsqlConnection(stdPGcon))
                {
                    myPGConnection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand("SELECT jpdb_id,company,first_name,last_name,title,email,website,workphone,mobilephone,address,zipcode,city,country,notes FROM tbl_jpdb ORDER BY jpdb_id ASC", myPGConnection);
                    DataTable dt = new DataTable();
                    NpgsqlDataReader dr = cmd.ExecuteReader();
                    dt.Load(dr);
                    dgv.DataSource = dt;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Can't connect to the database", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close(); //this stop the empty UI to pop up.
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReadRecords();
        }   
    }
}