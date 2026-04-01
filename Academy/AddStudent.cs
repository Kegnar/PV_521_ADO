using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        
        public AddStudent()
        {
            InitializeComponent();
        }

        public AddStudent(MainForm mainForm):base() 
        {
            studentData = new TextBox[] { tbFirstName, tbLastName, tbMiddleName, tbEmail, tbPhoneNumber };
            studentBirthDate = dtpBirthDate;
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
            // тут нужно передать в основную форму
            
            throw new NotImplementedException();
            this.Close();
        }
    }
}
