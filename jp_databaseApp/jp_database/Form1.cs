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
                MessageBox.Show("Can't connect to the database [", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close(); //this stop the empty UI to pop up.
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StateTextBoxes(false);
            ReadRecords();
            InitButtons();
        }

        //-----------------
        //--DELETE RECORD--
        //-----------------

        private void DeleteRecord(int id)       // D in CRUD
        {
            try
            {
                using (NpgsqlConnection myPGConnection = new NpgsqlConnection(stdPGcon))
                {
                    myPGConnection.Open();
                    string sql_query = "DELETE FROM tbl_jpdb WHERE jpdb_id=" + id;
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd = new NpgsqlCommand(sql_query, myPGConnection);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Record has been deleted");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't connect to the database [" + ex.Message + "]", "Dadabase Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        //------------------
        //Buttons
        //------------------

        private void btnNew_Click(object sender, EventArgs e)
        {
            StateTextBoxes(true);
            btnSave.Enabled = true;
        }

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
            InitButtons();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecord();
            ReadRecords();
            ClearTextBoxes();
            StateTextBoxes(false);
            btnSave.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int recordId = Convert.ToInt32(dgv.CurrentRow.Cells["jpdb_id"].Value);
            UpdateRecord(recordId);
            ReadRecords();
            ClearTextBoxes();
            StateTextBoxes(false);
            InitButtons();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                int recordId = Convert.ToInt32(dgv.CurrentRow.Cells["jpdb_id"].Value);
                DeleteRecord(recordId);
                ReadRecords();
            }
            ClearTextBoxes();
            StateTextBoxes(false);
            InitButtons();
        }

        // ------------------
        // Helper methods
        // ------------------
        private void dgv_DoubleClick(object sender, EventArgs e)
        {
            StateTextBoxes(true);
            btnNew.Enabled = false;
            btnUpdate.Enabled = true;
            btnClear.Enabled = true;
            btnDelete.Enabled = true;
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
            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                tb.Text = string.Empty;
            }
            StateTextBoxes(false);
        }

        private void StateTextBoxes(bool state)
        {
            if (state == false)
            {
                foreach (TextBox tb in this.Controls.OfType<TextBox>())
                {
                    tb.Enabled = false;
                }
            }
            else if (state == true)
            {
                foreach (TextBox tb in this.Controls.OfType<TextBox>())
                {
                    tb.Enabled = true;
                }
            }
        }

        private void InitButtons()
        {
            btnNew.Enabled = true;
            btnSave.Enabled = false;
            btnUpdate.Enabled = false;
            btnClear.Enabled = false;
            btnDelete.Enabled = false;
            btnQuit.Enabled = true;      // Always true
        }


        private void SaveRecord()        // C in CRUD
        {
            try
            {
                using (NpgsqlConnection myPGConnection = new NpgsqlConnection(stdPGcon))
                {
                    myPGConnection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();

                    string sql_query = "INSERT INTO tbl_jpdb(Company,first_name,last_name,title,email,website,workphone,mobilephone,address,zipcode,city,country,notes) VALUES" +
                        "('" + txtCompany.Text + "','" + txtFirstname.Text + "','" + txtLastname.Text + "','" + txtTitle.Text + "','" + txtEmail.Text + "','" + txtWebsite.Text + "','" + txtWorkphone.Text + "','" + txtMobilephone.Text + "','" +
                    "'" + txtAddress.Text + "','" + txtZipcode.Text + "','" + txtCity.Text + "','" + txtCountry.Text + "','" + txtNotes.Text + "')";

                    cmd = new NpgsqlCommand(sql_query, myPGConnection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record has been saved.");


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't connect to the database [" + ex.Message + "]", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close(); // this stop the empty UI to pop up.
            }
        }

        //-------------------
        //-- UPDATE RECORD --
        //-------------------

        private void UpdateRecord(int id)      //U in CRUD
        {
            try
            {
                using (NpgsqlConnection myPGConnection = new NpgsqlConnection(stdPGcon))
                {
                    myPGConnection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();

                    string sql_query = ("UPDATE tbl_jpdb SET company='" + txtCompany.Text + "', first_name='" + txtFirstname.Text + "', last_name='" + txtLastname.Text + "', title='" + txtTitle.Text + "', email='" + txtEmail.Text + "', website='" + txtWebsite.Text + "', workphone='" + txtWorkphone.Text + "', mobilephone='" + txtMobilephone.Text + "', Address='" + txtAddress.Text + "', zipcode='" + txtZipcode.Text + "', city='" + txtCity.Text + "', country='" + txtCountry.Text + "', notes='" + txtNotes.Text + "'WHERE jpdb_id=" + id);
                    cmd = new NpgsqlCommand(sql_query, myPGConnection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record has been updated.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't connect to the database [" + ex.Message + "]", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {

                using (NpgsqlConnection myPGConnection = new NpgsqlConnection(stdPGcon))
                {
                    string sql_query = "";
                    var datatable = new DataTable();

                    // Building the query string(s)
                    if (rdbCompany.Checked)
                        sql_query = "SELECT * FROM tbl_jpdb WHERE company LIKE '%" + txtSearch.Text + "%'";
                    if (rdbFirstname.Checked)
                        sql_query = "SELECT * FROM tbl_jpdb WHERE first_name LIKE '%" + txtSearch.Text + "%'";
                    if (rdbLastname.Checked)
                        sql_query = "SELECT * FROM tbl_jpdb WHERE last_name LIKE '%" + txtSearch.Text + "%'";

                    myPGConnection.Open();
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql_query, myPGConnection);
                    da.Fill(datatable);
                    dgv.DataSource = datatable;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Can't connect to the database", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    } 
}