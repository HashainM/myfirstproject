using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Configuration;

namespace AutoSpa_Xpress
{
    public partial class Category : Form
    {
        OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["AutoSpa_Xpress.Properties.Settings.AutoSpaConnectionString"].ConnectionString);

        public Category()
        {
            InitializeComponent();
        }       

        private void Category_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT CCODE, CNAME FROM Category", con);
            da.Fill(dt);
            cmbcCode.DataSource = dt;
            cmbcCode.DisplayMember = "CCODE";
            cmbcName.DataSource = dt;
            cmbcName.DisplayMember = "CNAME";
            cmbcCode.Text = null;
            cmbcName.Text = null;
        }

        public void AutoSearch(ComboBox xcb, System.Windows.Forms.KeyPressEventArgs e, bool blnLimitToList)
        {
            string strFindStr = "";
            if (e.KeyChar == (char)8)
            {
                if (xcb.SelectionStart <= 1)
                {
                    xcb.Text = "";
                    return;
                }

                if (xcb.SelectionLength == 0)
                {
                    strFindStr = xcb.Text.Substring(0, xcb.Text.Length - 1);
                }
                else
                {
                    strFindStr = xcb.Text.Substring(0, xcb.SelectionStart - 1);
                }
            }
            else
            {
                if (xcb.SelectionLength == 0)
                {
                    strFindStr = xcb.Text + e.KeyChar;
                }
                else
                {
                    strFindStr = xcb.Text.Substring(0, xcb.SelectionStart) + e.KeyChar;
                }

                int intIdx = -1;

                intIdx = xcb.FindString(strFindStr);
                if (intIdx != -1)
                {
                    xcb.SelectedText = "";
                    xcb.SelectedIndex = intIdx;
                    xcb.SelectionStart = strFindStr.Length;
                    xcb.SelectionLength = xcb.Text.Length;
                    e.Handled = true;
                }
                else
                {
                    e.Handled = blnLimitToList;
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            int pi = 0;
            String cCode = null;
            btnNew.Enabled = false;
            btnDelete.Enabled = false;
            cmbcName.Text = null;
            try
            {
                con.Open();
                OleDbDataReader reader = null;
                OleDbCommand cmd = new OleDbCommand("Select * From Category Where ID = (Select MAX(ID) From Category)", con);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cCode = reader["CCODE"].ToString();
                }

                pi = Int32.Parse(cCode);

                if (pi == 0)
                {
                    pi = 1;
                }
                else
                {
                    pi++;
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("error" + ex);
            }

            cmbcCode.Text = pi.ToString().PadLeft(6, '0');
            cmbcCode.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbcCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb != null && cmbcCode.DataSource != null && cmbcName.DataSource != null)
            {
                int index = cb.SelectedIndex;
                if (index >= 0)
                {
                    if (cmbcCode.SelectedIndex != index)
                        cmbcCode.SelectedIndex = index;
                    if (cmbcName.SelectedIndex != index)
                        cmbcName.SelectedIndex = index;
                }
            }  
        }

        private void cmbcName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbcCode.Enabled = true;
            ComboBox cb = sender as ComboBox;
            if (cb != null && cmbcCode.DataSource != null && cmbcName.DataSource != null)
            {
                int index = cb.SelectedIndex;
                if (index >= 0)
                {
                    if (cmbcCode.SelectedIndex != index)
                        cmbcCode.SelectedIndex = index;
                    if (cmbcName.SelectedIndex != index)
                        cmbcName.SelectedIndex = index;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cmbcCode.Enabled = true;
            cmbcCode.Text = null;
            cmbcCode.Text = null;
            btnNew.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String cId = null;
            String cCode = cmbcCode.Text.ToString();
            String cName = cmbcName.Text.ToString();

            if (cCode == String.Empty || cName == String.Empty)
            {
                MessageBox.Show("At least one of the property values is invalid. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    OleDbDataReader reader = null;
                    OleDbCommand cmd = new OleDbCommand("Select ID From Category WHERE CCODE = '" + cCode + "'", con);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cId = reader["ID"].ToString();
                    }
                    if (cId == null)
                    {
                        OleDbCommand cmd2 = con.CreateCommand();
                        cmd2.CommandText = "INSERT INTO Category (CCODE,CNAME) VALUES('" + cCode + "','" + cName + "')";
                        cmd2.Connection = con;
                        cmd2.ExecuteNonQuery();
                    }
                    else
                    {
                        OleDbCommand cmd2 = con.CreateCommand();
                        cmd2.CommandText = "UPDATE Category SET CNAME ='" + cName + "' WHERE CCODE = '" + cCode + "'";
                        cmd2.Connection = con;
                        cmd2.ExecuteNonQuery();
                    }

                    con.Close();

                    cmbcCode.Enabled = true;
                    cmbcCode.Text = null;
                    cmbcName.Text = null;
                    btnNew.Enabled = true;
                    btnDelete.Enabled = true;
                    this.Category_Load(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR " + ex);
                }


            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                String Ccode = cmbcCode.Text.ToString();
                con.Open();
                OleDbCommand cmd = con.CreateCommand();
                cmd.CommandText = "DELETE FROM Category WHERE CCODE = '" + Ccode + "'";
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("error " + ex);
            }
            cmbcCode.Enabled = true;
            cmbcCode.Text = null;
            cmbcName.Text = null;
            btnNew.Enabled = true;
            this.Category_Load(null, null);
        }

        private void cmbcName_KeyPress(object sender, KeyPressEventArgs e)
        {
            AutoSearch(cmbcName, e, false);
        }
    }
}
