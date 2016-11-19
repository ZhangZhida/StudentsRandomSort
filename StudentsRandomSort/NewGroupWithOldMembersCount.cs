using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentsRandomSort
{
    public partial class NewGroupWithOldMembersCount : Form
    {
        public int groupCount = 4;
        public static int oldMemberCount = 1;

        public NewGroupWithOldMembersCount()
        {
            InitializeComponent();
            if (groupCount < 2) {
                MessageBox.Show("每组人数过少，无法分组");
                return;
            }

            for (int i = 0; i < groupCount-1; i++)
            {
                this.comboBox2.Items.Add((i + 1).ToString());
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            oldMemberCount = comboBox2.SelectedIndex + 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
