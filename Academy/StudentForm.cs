using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Academy
{
    public partial class StudentForm : HumanForm
    {
        public StudentForm()
        {
            InitializeComponent();

#if DEBUG
            tbLastName.Text = "Жук";
            tbFirstName.Text = "Василий";
            tbMiddleName.Text = "Петрович";
            dtpBirthDate.Text = "1977.10.24";
            tbEmail.Text = "bazilik_spb@mail.ru";
            tbPhone.Text = "+7(911)024-56-78"; 
#endif
            //tbLastName.Text = "+7(911)024-56-78";

            DataTable groups = DataBase.Connector.Select("SELECT * FROM Groups");
            cbGroup.DataSource = groups;
            cbGroup.DisplayMember = "group_name";
            cbGroup.ValueMember = "group_id";

        }

        public StudentForm(int student_id)
        {
            tbLastName.Text = "Жук";
            tbFirstName.Text = "Василий";
            tbMiddleName.Text = "Петрович";
            dtpBirthDate.Text = "1977.10.24";
            tbEmail.Text = "bazilik_spb@mail.ru";
            tbPhone.Text = "+7(911)024-56-78";
        }



        protected override void buttonOK_Click(object sender, EventArgs e)
        {

            var student = new Dictionary<string, object>
            {
                { "last_name",    tbLastName.Text.Trim() },
                { "first_name",   tbFirstName.Text.Trim() },
                { "middle_name",  tbMiddleName.Text.Trim() },
                { "birth_date",   dtpBirthDate.Value.Date },
                { "email",        tbEmail.Text.Trim() },
                { "phone",        tbPhone.Text.Trim() },
                { "group",        cbGroup.SelectedValue ?? DBNull.Value }

            };

            if (DataBase.Connector.Insert("Students", student))
            {
                MessageBox.Show("Запись добавлена.");
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Невозможно добавить существующего студента.");
            }

        }
        
    }
}

