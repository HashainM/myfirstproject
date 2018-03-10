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
    public partial class Brands : Form
    {       
        
        OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["AutoSpa_Xpress.Properties.Settings.AutoSpaConnectionString"].ConnectionString);
        public Brands()
        {
            InitializeComponent();
        }        

        private void Brands_Load(object sender, EventArgs e)
        {         

            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT BCODE, BNAME FROM Brand", con);
            da.Fill(dt);
            cmbbCode.DataSource = dt;
            cmbbCode.DisplayMember = "BCODE";
            cmbbName.DataSource = dt;
            cmbbName.DisplayMember = "BNAME";            
            cmbbCode.Text = null;
            cmbbName.Text = null;
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
            String bCode = null;
            btnNew.Enabled = false;
            btnDelete.Enabled = false;
            cmbbName.Text = null;
            try
            {
                con.Open();                
                OleDbDataReader reader = null;
                OleDbCommand cmd = new OleDbCommand("Select * From Brand Where ID = (Select MAX(ID) From Brand)", con);
                reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    bCode = reader["BCODE"].ToString();                
                }

                pi = Int32.Parse(bCode);

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
            
            cmbbCode.Text = pi.ToString().PadLeft(6, '0');
            cmbbCode.Enabled = false;
            
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            String bId = null;
            String bCode = cmbbCode.Text.ToString();
            String bName = cmbbName.Text.ToString();           

            if (bCode == String.Empty || bName == String.Empty){
               MessageBox.Show("At least one of the property values is invalid. ","Error",MessageBoxButtons.OK,MessageBoxIcon.Error); 
            }
            else
            {                
                try
                {
                    con.Open();
                    OleDbDataReader reader = null;
                    OleDbCommand cmd = new OleDbCommand("Select ID From Brand WHERE BCODE = '" + bCode + "'", con);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        bId = reader["ID"].ToString();
                    }                    
                    if (bId == null)
                    {
                        OleDbCommand cmd2 = con.CreateCommand();
                        cmd2.CommandText = "INSERT INTO Brand (BCODE,BNAME) VALUES('" + bCode + "','" + bName + "')";
                        cmd2.Connection = con;
                        cmd2.ExecuteNonQuery();
                    }
                    else
                    {                        
                        OleDbCommand cmd2 = con.CreateCommand();
                        cmd2.CommandText = "UPDATE Brand SET BNAME ='" + bName + "' WHERE BCODE = '" + bCode + "'";
                        cmd2.Connection = con;
                        cmd2.ExecuteNonQuery();
                    }  

                    con.Close();

                    cmbbCode.Enabled = true;
                    cmbbCode.Text = null;
                    cmbbName.Text = null;
                    btnNew.Enabled = true;
                    btnDelete.Enabled = true;                    
                    this.Brands_Load(null, null);
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
                String Bcode = cmbbCode.Text.ToString();
                con.Open();
                OleDbCommand cmd2 = con.CreateCommand();
                cmd2.CommandText = "DELETE FROM Brand WHERE BCODE = '"+Bcode+"'";
                cmd2.Connection = con;
                cmd2.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("eeror " + ex);
            }
            cmbbCode.Enabled = true;
            cmbbCode.Text = null;
            cmbbName.Text = null;
            btnNew.Enabled = true;                        
            this.Brands_Load(null, null);
        }

        private void cmbbCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb != null && cmbbCode.DataSource != null && cmbbName.DataSource != null)
            {
                int index = cb.SelectedIndex;
                if (index >= 0)
                {
                    if (cmbbCode.SelectedIndex != index)
                        cmbbCode.SelectedIndex = index;
                    if (cmbbName.SelectedIndex != index)
                        cmbbName.SelectedIndex = index;
                }
            }            
        }

        private void cmbbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbbCode.Enabled = true;
            ComboBox cb = sender as ComboBox;
            if (cb != null && cmbbCode.DataSource != null && cmbbName.DataSource != null)
            {
                int index = cb.SelectedIndex;
                if (index >= 0)
                {
                    if (cmbbCode.SelectedIndex != index)
                        cmbbCode.SelectedIndex = index;
                    if (cmbbName.SelectedIndex != index)
                        cmbbName.SelectedIndex = index;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cmbbCode.Enabled = true;
            cmbbCode.Text = null;
            cmbbName.Text = null;
            btnNew.Enabled = true;
            btnDelete.Enabled = true;
        }

        
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            AutoSearch(cmbbName, e, false);
        }

    }
}
