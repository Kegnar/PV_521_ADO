using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Academy
{
    public partial class AddStudent : Form
    {
        private TextBox[] studentData;
        private DateTimePicker studentBirthDate;
        private DBtools.Connector connector;
        private Dictionary<string, int> g_list;
        int groupId;

        public AddStudent()
        {
            InitializeComponent();
        }

        public AddStudent(MainForm mainForm):base() 
        {
            connector = new DBtools.Connector(ConfigurationManager.ConnectionStrings["PV_521_Import"].ConnectionString);
            studentData = new TextBox[] { tbFirstName, tbLastName, tbMiddleName, tbEmail, tbPhoneNumber };
            studentBirthDate = dtpBirthDate;
            g_list = connector.GetDictionary("Groups");
            
            cbAddStudentGroup.Items.AddRange(g_list.Keys.ToArray());
        }
        

        private void tbFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            var groupId = Convert.ToInt32(g_list[cbAddStudentGroup.SelectedItem.ToString()]);
            // доделать запрос по уму
            connector.Insert("Students", "first_name, last_name, middle_name, birth_date, email, phone, group", $@"{tbFirstName.ToString()}, {tbLastName.ToString()}, 
                            {tbMiddleName.ToString()}, {dtpBirthDate.ToString()}, {tbEmail.ToString()}, {tbPhoneNumber.ToString()}, {groupId}");

            this.Close();
        }
        // тут допилить получение индекса группы
        private void cbAddStudentGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            var idx = cbAddStudentGroup.SelectedIndex;
           
        }
    }
}
