using Npgsql;
using System.Configuration;
using System.Data;

namespace jp_database
{
    public partial class frmMain : Form
    {
        public frmMain()
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

        //------------------
        //Buttons
        //------------------
        private void btnQuit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Leave Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecord();
        }


        private void dgv_DoubleClick(object sender, EventArgs e)
        {
            txtCompany.Text = dgv.CurrentRow.Cells["company"].Value.ToString();
            txtFirstname.Text = dgv.CurrentRow.Cells["first_name"].Value.ToString();
            txtLastname.Text = dgv.CurrentRow.Cells["last_name"].Value.ToString();
            txtTitle.Text = dgv.CurrentRow.Cells["title"].Value.ToString();
            txtEmail.Text = dgv.CurrentRow.Cells["email"].Value.ToString();
            txtWorkphone.Text = dgv.CurrentRow.Cells["workphone"].Value.ToString();
            txtMobile.Text = dgv.CurrentRow.Cells["mobilePhone"].Value.ToString();
            txtAddress.Text = dgv.CurrentRow.Cells["address"].Value.ToString();
            txtZipcode.Text = dgv.CurrentRow.Cells["zipcode"].Value.ToString();
            txtCity.Text = dgv.CurrentRow.Cells["city"].Value.ToString();
            txtCountry.Text = dgv.CurrentRow.Cells["country"].Value.ToString();
            txtWebsite.Text = dgv.CurrentRow.Cells["website"].Value.ToString();
            txtNotes.Text = dgv.CurrentRow.Cells["notes"].Value.ToString();
        }

        private void ClearTextBoxes()
        {
            foreach(TextBox tb in this.Controls.OfType<TextBox>())
            {
                tb.Text = string.Empty;
            }
        }

        private void SaveRecord()        // C in CRUD
        {
            try
            {
                using (NpgsqlConnection myPGConnection = new NpgsqlConnection(stdPGcon))
                {
                    myPGConnection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();

                    string sql_query = "INSERT INTO tbl_jpdb(Company,first_name,last_name,title,email,website,workphone,mobilephone,address,zipcode,city,country,website,notes) VALUES" +
                        "C'" + txtCompany.Text + "','" + txtFirstname.Text + "','" + txtLastname.Text + "','" + txtTitle.Text + "','" + txtEmail.Text + "','" + txtWebsite.Text + "','" + txtWorkphone.Text + "','" + txtMobilephone.Text + "','" +
                    "'" + txtAddress.Text + "','" + txtZipcode.Text + "','" + txtCity.Text + "','" + txtCountry.Text + "','" + txtNotes.Text + "')";

                    cmd = new NpgsqlCommand(sql_query, myPGConnection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record has been saved.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Can't connect to the database", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close(); // this stop the empty UI to pop up.
            }
        }
    }
}