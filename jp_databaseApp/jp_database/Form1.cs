using Npgsql;
using System.Configuration;

namespace jp_database
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        readonly string stdPGcon = ConfigurationManager.ConnectionStrings["PGcon"].ConnectionString;
    }
}