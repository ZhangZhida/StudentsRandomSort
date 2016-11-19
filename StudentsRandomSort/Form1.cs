using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentsRandomSort
{
    public partial class Form1 : Form
    {
        string filePath = Application.StartupPath + "\\edudiants.txt";
        Font myFont = new Font("Verdana", 18);

        Graphics g;
        
        SolidBrush brush1 = new SolidBrush(Color.FromArgb(0, 0, 200));  
        Pen pen = new Pen(Color.Black);
        Color fontColor = Color.FromArgb(0,0,200);

        double w;
        double h;
        string[] names;
        int cols = 6; //必须是偶数
        int rows = 6;
        int cases = 1; //默认为分组模式
        int groupCount = 4; //默认每组四个人
        int oldMemberCount = 1;
        int nameCount = 0;
        int howManyGroup = 0;
        string[][] groups;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            load_edudiant(filePath);
            itemsCalculateToRefresh();
            names = randomSort(names, oldMemberCount);
        }
        private void load_edudiant(string filePath)
        {
            StreamReader src1 = new StreamReader(@filePath, Encoding.UTF8);
            ArrayList namesAL = new ArrayList();
            //string[] names;
            //string name = src1.ReadLine();
            //while (name != null && name.Length != 0) {
            //    namesAL.Add(name);
            //    name = src1.ReadLine();
            //}
            //names = (string[])namesAL.ToArray();
            string name = src1.ReadToEnd();
            int idx = name.IndexOf("f");
            while (idx != -1) {
                namesAL.Add(name.Substring(0, idx));
                name = name.Substring(idx+1);
                idx = name.IndexOf("f");
            }
            names = (string[])namesAL.ToArray(typeof(string));
            nameCount = names.Length;
            howManyGroup = (int)(Math.Ceiling((double)nameCount / (double)groupCount));
            formGroups();
            this.pictureBox1.Update();
            this.pictureBox1.Refresh();
        }

        private string[][] formGroups() {
            object[] tmpGroupObj;
            string[] tmpGroupStr;
            int idx = 0;
            Queue myq = new Queue(names);
            string[][] groups = new string[howManyGroup][];
            for (int i = 0; i < howManyGroup; i++) {
                Queue tmpGroup = new Queue();
                for (int j = 0; j < groupCount; j++) {
                    try
                    {
                        tmpGroup.Enqueue(myq.Dequeue());
                    }
                    catch (Exception e) {
                        
                    }
                }
                tmpGroupObj = tmpGroup.ToArray();
                tmpGroupStr = new string[tmpGroup.Count];
                foreach (object item in tmpGroupObj) {
                    tmpGroupStr[idx] = item.ToString();
                    idx++;
                }
                groups[i] = tmpGroupStr;
                idx = 0;
            }

            return groups;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            itemsCalculateToRefresh();
            
            names = randomSort(names, oldMemberCount);

            this.pictureBox1.Update();
            this.pictureBox1.Refresh();
        }

        //private string[] randomSort(string[] names) {
        //    if (names.Length == 0) {
        //        return null;
        //    }
        //    int nameCount = names.Length;
        //    int[] a = new int[nameCount];
        //    string[] newNames = new string[nameCount];
        //    for (int i = 0; i < nameCount; i++)
        //    {
        //        a[i] = i;
        //    }
        //    int[] b = a.OrderBy(x => Guid.NewGuid()).ToArray();
        //    int idx = 0;
        //    foreach (int i in b)
        //    {
        //        newNames[idx] = names[i];
        //        idx++;
        //    }
        //    names = newNames;
        //    return names;
        //}

        private string[] randomSort(string[] names, int oldMemberCount) 
        {
            if (names.Length == 0 || howManyGroup == 0 || nameCount == 0)
            {
                MessageBox.Show("names.Length == 0 || howManyGroup == 0 || nameCount == 0");
                return null;
            }
            string[] tmpNames = new string[nameCount];
            string[] oneGroup;
            string[][] oldMembers = new string[howManyGroup][];
            string[][] newMembers = new string[howManyGroup][];
            int[] randomizedIdx1;
            int[] randomizedIdx2;
            bool flag1 = true;
            int itr = 0;
            //
            for (int i = 0; i < howManyGroup; i++) {
                oneGroup = groups[i];
                randomizedIdx1 = randomizeIndexs(oneGroup.Length);
                oldMembers[i] = new string[oldMemberCount];
                for (int j = 0; j < oldMemberCount; j++) {
                    
                    oldMembers[i][j] = oneGroup[randomizedIdx1[j]];
                }
                newMembers[i] = new string[groupCount - oldMemberCount];
                for (int j = 0; j < oneGroup.Length - oldMemberCount; j++) {
                    
                    newMembers[i][j] = oneGroup[randomizedIdx1[j+oldMemberCount]];
                }
            }
            randomizedIdx2 = new int[howManyGroup];
            //newMembers和oldMembers随机组合
            while (flag1) {
                flag1 = false;
                randomizedIdx2 = randomizeIndexs(howManyGroup);
                for (int i = 0; i < randomizedIdx2.Length; i++) {
                    if (randomizedIdx2[i] == i) {
                        flag1 = true;
                    }
                }
            }
            for (int i = 0; i < howManyGroup; i++) {
                string[] tmpGroup = new string[(oldMembers[i].Length + newMembers[i].Length)];
                oldMembers[i].CopyTo(tmpGroup, 0);
                newMembers[randomizedIdx2[i]].CopyTo(tmpGroup, oldMembers[i].Length);
                groups[i] = tmpGroup;
            }

            tmpNames = (string[])names.Clone();
            foreach (string[] oneGroup2 in groups) {
                foreach (string name in oneGroup2) {
                    if (name != null) {
                        tmpNames[itr] = name;
                        itr++;
                    }
                    
                }
            }
            names = tmpNames;
            itr = 0;

            return names;
        }

        private int[] randomizeIndexs(int a) {
            int[] c = new int[a];
            for (int i = 0; i < a; i++) {
                c[i] = i;
            }
            int[] b = c.OrderBy(x => Guid.NewGuid()).ToArray();
            return b;
        }

        private int[] randomizeIndexs(int[] a) {
            int[] b = a.OrderBy(x => Guid.NewGuid()).ToArray();
            return b;
        }

        public void drawDesks(int cols, int rows, int cases, int groupCount, Graphics g, Pen pen, Brush brush)
        {
            int nameCount = names.Length;
            string[] namesTmp = new string[nameCount];
            names.CopyTo(namesTmp, 0);
            int nameItr = 0;
            string name;
            
            switch (cases) {
                //情况一是分组，默认每组四个人
                case 1:
                    int groupColCount1 = (int)System.Math.Ceiling(Convert.ToDouble(cols) / 2);
                    int groupRowCount1= (int)System.Math.Ceiling(Convert.ToDouble(rows) / 2);
                    double widthGroup1 = (w * 0.98) / (groupColCount1 * 1.2);
                    double heightGroup1 = (h * 0.98) / (groupRowCount1 *1.2);
                    for (int j = 0; j < groupRowCount1; j++){
                        for (int i = 0; i < groupColCount1; i++) {
                            float left = (float)(w * 0.01 + widthGroup1 * 0.2 * i + widthGroup1*i);
                            float right = (float)(left + widthGroup1);
                            float up = (float)(h * 0.01 + heightGroup1 * 0.2 * j + heightGroup1 * j);
                            float down = (float)(up + heightGroup1);
                            drawGroupDesk(left, right, up, down, g, pen);
                            name = getName(names, nameItr);
                            nameItr++;
                            drawName(name, left, (float)(left + (right - left) / 2.0), up, (float)(up + (down - up) / 2.0), g, brush);

                            name = getName(names, nameItr);
                            nameItr++;
                            drawName(name, left + (float)((right - left) / 2.0), right, up, (float)(up + (down - up) / 2.0), g, brush);

                            name = getName(names, nameItr);
                            nameItr++;
                            drawName(name, left, (float)(left + (right - left) / 2.0), (float)(up + (down - up) / 2.0), down, g, brush);

                            name = getName(names, nameItr);
                            nameItr++;
                            drawName(name, (float)(left + (right - left) / 2.0), right, (float)(up + (down - up) / 2.0), down, g, brush);
                        }
                    }
                    return;
                case 2:
                    int groupColCount2 = cols;
                    int groupRowCount2 = rows;
                    double widthGroup2 = (w * 0.98) / (groupColCount2 * 1.2);
                    double heightGroup2 = (h * 0.98) / (groupRowCount2 *1.2);
                    for (int j = 0; j < groupRowCount2; j++){
                        for (int i = 0; i < groupColCount2; i++) {
                            float left = (float)(w * 0.01 + widthGroup2 * 0.2 * i + widthGroup2*i);
                            float right = (float)(left + widthGroup2);
                            float up = (float)(h * 0.01 + heightGroup2 * 0.2 * j + heightGroup2 * j);
                            float down = (float)(up + heightGroup2);
                            drawSingleDesk(left, right, up, down, g, pen);
                            name = getName(names, nameItr);
                            nameItr++;
                            drawName(name, left, right, up, down, g, brush);

                        }
                    }
                    return;
            }
        }

        private string getName(string[] names, int i) {
            if (i < names.Length)
            {
                return names[i];
            }
            else {
                return "";
            }

        }

        private void drawSingleDesk(float left, float right, float up, float down, Graphics g, Pen pen) {
            g.DrawLine(pen, new PointF(left, up), new PointF(right, up));
            g.DrawLine(pen, new PointF(right, up), new PointF(right, down));
            g.DrawLine(pen, new PointF(right, down), new PointF(left, down));
            g.DrawLine(pen, new PointF(left, down), new PointF(left, up));
        }

        private void drawGroupDesk(float left, float right, float up, float down, Graphics g, Pen pen) {
            g.DrawLine(pen, new PointF(left, up), new PointF(right, up));
            g.DrawLine(pen, new PointF(right, up), new PointF(right, down));
            g.DrawLine(pen, new PointF(right, down), new PointF(left, down));
            g.DrawLine(pen, new PointF(left, down), new PointF(left, up));
            g.DrawLine(pen, new PointF(left, (float)((up + down) / 2.0)), new PointF(right, (float)((up + down) / 2.0)));
            g.DrawLine(pen, new PointF((float)((left + right) / 2.0), up), new PointF((float)((left + right) / 2.0), down));
        }

        private void drawName(string name, float left, float right, float up, float down, Graphics g, Brush brush) { 
            SizeF size = new SizeF((float)((right-left)* 1.2), (float)((down - up)*1.2));
            //PointF Location = new PointF((float)((left+right)/2.0), (float)((up+down)/2.0));
            PointF Location = new PointF((float)(left+(right-left)*0.18), (float)(up + (down-up)*0.18));
            myFont = new Font("Verdana", (float)(size.Width/7.5));
            g.DrawString(name, myFont, brush, new RectangleF(Location, size));
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            itemPaintToRefresh();
            g = e.Graphics;
            w = pictureBox1.Width;
            h = pictureBox1.Height;

            int count = names.Length;
            if (count == 0)
            {
                MessageBox.Show("edudiants.txt为空，或者没有加载edudiants.txt");
                return;
            }
            //names = randomSort(names, oldMemberCount);
            drawDesks(cols, rows, cases, groupCount, g, pen, brush1);
            //formGroups();
        }

        private void RandomSortForExamination() { 
            
        }

        private void itemsCalculateToRefresh() {
            this.oldMemberCount = NewGroupWithOldMembersCount.oldMemberCount;
            this.groups = formGroups();
            
        }

        private void itemPaintToRefresh() {
            this.brush1.Color = fontColor;
        }

        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.Text == "重新分小组")
            {
                cases = 1;
            }
            if (this.comboBox1.Text == "考试排座位") {
                cases = 2;
            }
            this.pictureBox1.Update();
            this.pictureBox1.Refresh();
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void 新分组人数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FormSettings FormSettings = new FormSettings();
            //FormSettings.Show();
            NewGroupWithOldMembersCount groupWithOldMembersCount = new NewGroupWithOldMembersCount();
            groupWithOldMembersCount.Show();
            
            
        }

        private void 字体颜色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                fontColor = colorDialog1.Color;
            }
            this.pictureBox1.Update();
            this.pictureBox1.Refresh();
        }

        private void duToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 读取名单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = "";
            openFileDialog1.InitialDirectory = Application.StartupPath;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
            }
            if (filePath == "") {
                return;
            }
            load_edudiant(filePath);
        }

        private void 保存名单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = "";
            saveFileDialog1.InitialDirectory = Application.StartupPath;
            saveFileDialog1.Filter = "文本文件(*.txt)|*.txt";
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;

            saveFileDialog1.FileName = currentTime.Year + "-" + currentTime.Month + "-" + currentTime.Day + "名单";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                filePath = saveFileDialog1.FileName;
            }
            //MessageBox.Show(filePath);
            if (filePath == "") {
                return;
            }
            StreamWriter sw = new StreamWriter(filePath);
            foreach (string s in names) {
                sw.Write(s + "f");
            }
            sw.Close();
        }
        

    }
}
