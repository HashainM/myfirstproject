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
    public partial class Product : Form
    {
        OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["AutoSpa_Xpress.Properties.Settings.AutoSpaConnectionString"].ConnectionString);
        OleDbDataAdapter da;
        public Product()
        {
            InitializeComponent();
        }

        private void Product_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            da = new OleDbDataAdapter("SELECT PNo, Mno FROM Product", con);
            da.Fill(dt);
            cmbPno.DataSource = dt;
            cmbPno.DisplayMember = "PNo";
            cmbMno.DataSource = dt;
            cmbMno.DisplayMember = "Mno";
            cmbPno.Text = null;            
            cmbMno.Text = null;
            txtDes.Text = null;
            txtPdes.Text = null;
            txtUnit.Text = null;
            txtCost.Text = null;
            txtSell.Text = null;
            txtReOrder.Text = null;
            rbStock.Checked = true;

            FillBrand();
            FillCategory();
        }

        public void FillBrand()
        { 
            DataTable dtb = new DataTable();
            da = new OleDbDataAdapter("SELECT BNAME FROM Brand", con);
            da.Fill(dtb);
            cmbBname.DataSource = dtb;
            cmbBname.DisplayMember = "BNAME";
            cmbBname.Text = null;
        }

        public void FillCategory()
        {
            DataTable dtc = new DataTable();
            da = new OleDbDataAdapter("SELECT CNAME FROM Category", con);
            da.Fill(dtc);
            cmbCname.DataSource = dtc;
            cmbCname.DisplayMember = "CNAME";
            cmbCname.Text = null;
        }

        public void Reset()
        {
            cmbMno.Text = null;
            cmbCname.Text = null;
            cmbBname.Text = null;
            txtDes.Text = null;
            txtPdes.Text = null;
            txtUnit.Text = null;
            txtCost.Text = null;
            txtSell.Text = null;
            txtReOrder.Text = null;
            rbStock.Checked = true;
        }

        public void SetDescription()
        {
            String mno = cmbMno.Text.ToString();
            String cat = cmbCname.Text.ToString();
            String brand = cmbBname.Text.ToString();

            txtDes.Text = brand + " " + mno + " " + cat;
        }

        public void SetPrintDescription()
        {            
            String cat = cmbCname.Text.ToString();
            String brand = cmbBname.Text.ToString();

            txtPdes.Text = brand + " " + cat;
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbPno_SelectedIndexChanged(object sender, EventArgs e)
        {
            String pno = null;
            ComboBox cb = sender as ComboBox;
            DataTable dt = new DataTable();
            if (cb != null && cmbPno.DataSource != null && cmbMno.DataSource != null)
            {
                int index = cb.SelectedIndex;
                if (index >= 0)
                {
                    if (cmbPno.SelectedIndex != index)
                    {
                        cmbPno.SelectedIndex = index;
                        pno = cmbPno.Text.ToString();

                        try
                        {
                            con.Open();
                            OleDbDataReader reader = null;
                            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Product WHERE PNo = '" + pno + "'", con);
                            reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                cmbCname.Text = reader["CName"].ToString();
                                cmbBname.Text = reader["BName"].ToString();
                                txtDes.Text = reader["Description"].ToString();
                                txtPdes.Text = reader["PDescription"].ToString();
                                txtUnit.Text = reader["Unit"].ToString();
                                txtCost.Text = reader["Cprice"].ToString();
                                txtSell.Text = reader["SellPrice"].ToString();
                                txtReOrder.Text = reader["ROQ"].ToString();

                                String pType = reader["PType"].ToString();
                                if (pType == "s")
                                {
                                    rbStock.Checked = true;
                                }
                                if (pType == "ns")
                                {
                                    rbNStock.Checked = true;
                                }
                                if (pType == "sr")
                                {
                                    rbService.Checked = true;
                                }

                            }
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("error " + ex);
                        }

                    }
                    if (cmbMno.SelectedIndex != index)
                    {
                        cmbMno.SelectedIndex = index;
                        pno = cmbMno.Text.ToString();
                        try
                        {
                            con.Open();
                            OleDbDataReader reader = null;
                            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Product WHERE Mno = '" + pno + "'", con);
                            reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                cmbCname.Text = reader["CName"].ToString();
                                cmbBname.Text = reader["BName"].ToString();
                                txtDes.Text = reader["Description"].ToString();
                                txtPdes.Text = reader["PDescription"].ToString();
                                txtUnit.Text = reader["Unit"].ToString();
                                txtCost.Text = reader["Cprice"].ToString();
                                txtSell.Text = reader["SellPrice"].ToString();
                                txtReOrder.Text = reader["ROQ"].ToString();

                                String pType = reader["PType"].ToString();
                                if (pType == "s")
                                {
                                    rbStock.Checked = true;
                                }
                                if (pType == "ns")
                                {
                                    rbNStock.Checked = true;
                                }
                                if (pType == "sr")
                                {
                                    rbService.Checked = true;
                                }
                            }
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("error " + ex);
                        }
                    }
                }
            }
        }

        private void cmbMno_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbPno.Enabled = true;
            btnNew.Enabled = true;
            btnDelete.Enabled = true;

            String pno = null;
            DataTable dt = new DataTable();
            ComboBox cb = sender as ComboBox;
            if (cb != null && cmbPno.DataSource != null && cmbMno.DataSource != null)
            {
                int index = cb.SelectedIndex;
                if (index >= 0)
                {
                    if (cmbPno.SelectedIndex != index)
                    {
                        cmbPno.SelectedIndex = index;
                        pno = cmbPno.Text.ToString();
                        try
                        {
                            con.Open();
                            OleDbDataReader reader = null;
                            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Product WHERE PNo = '" + pno + "'", con);
                            reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                cmbCname.Text = reader["CName"].ToString();
                                cmbBname.Text = reader["BName"].ToString();
                                txtDes.Text = reader["Description"].ToString();
                                txtPdes.Text = reader["PDescription"].ToString();
                                txtUnit.Text = reader["Unit"].ToString();
                                txtCost.Text = reader["Cprice"].ToString();
                                txtSell.Text = reader["SellPrice"].ToString();
                                txtReOrder.Text = reader["ROQ"].ToString();

                                String pType = reader["PType"].ToString();
                                if (pType == "s")
                                {
                                    rbStock.Checked = true;
                                }
                                if (pType == "ns")
                                {
                                    rbNStock.Checked = true;
                                }
                                if (pType == "sr")
                                {
                                    rbService.Checked = true;
                                }
                            }
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("error " + ex);
                        }
                    }
                    if (cmbMno.SelectedIndex != index)
                    {
                        cmbMno.SelectedIndex = index;
                        pno = cmbMno.Text.ToString();
                        try
                        {
                            con.Open();
                            OleDbDataReader reader = null;
                            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Product WHERE Mno = '" + pno + "'", con);
                            reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                cmbCname.Text = reader["CName"].ToString();
                                cmbBname.Text = reader["BName"].ToString();
                                txtDes.Text = reader["Description"].ToString();
                                txtPdes.Text = reader["PDescription"].ToString();
                                txtUnit.Text = reader["Unit"].ToString();
                                txtCost.Text = reader["Cprice"].ToString();
                                txtSell.Text = reader["SellPrice"].ToString();
                                txtReOrder.Text = reader["ROQ"].ToString();

                                String pType = reader["PType"].ToString();
                                if (pType == "s")
                                {
                                    rbStock.Checked = true;
                                }
                                if (pType == "ns")
                                {
                                    rbNStock.Checked = true;
                                }
                                if (pType == "sr")
                                {
                                    rbService.Checked = true;
                                }
                            }
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("error " + ex);
                        }
                    }
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            int pi = 0;
            String pno = null;
            btnNew.Enabled = false;
            btnDelete.Enabled = false;
            Reset();
            try
            {
                con.Open();
                OleDbDataReader reader = null;
                OleDbCommand cmd = new OleDbCommand("Select * From Product Where ID = (Select MAX(ID) From Product)", con);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    pno = reader["PNo"].ToString();
                }

                pi = Int32.Parse(pno);

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

            cmbPno.Text = pi.ToString().PadLeft(6, '0');
            cmbPno.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cmbPno.Enabled = true;
            cmbPno.Text = null;
            Reset();
            btnNew.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String id = null;
            String Ptype = null;
            String pno = cmbPno.Text.ToString();
            String mno = cmbMno.Text.ToString();
            String category = cmbCname.Text.ToString();
            String brand = cmbBname.Text.ToString();
            String des = txtDes.Text.ToString();
            String Pdes = txtDes.Text.ToString();
            String unitVal = txtUnit.Text.ToString();
            String CpriceVal = txtCost.Text.ToString();
            String SpriceVal = txtSell.Text.ToString();
            String roqVal = txtReOrder.Text.ToString();
            int unit = int.Parse(unitVal);
            double Cprice = double.Parse(CpriceVal);
            double Sprice = double.Parse(SpriceVal);
            int roq = int.Parse(roqVal);

             
            if (rbStock.Checked)Ptype = "s";            
            if (rbNStock.Checked)Ptype = "ns";
            if (rbService.Checked)Ptype = "sr";

            if (pno == String.Empty || mno == String.Empty || category == String.Empty || brand == String.Empty)
            {
                MessageBox.Show("At least one of the property values is invalid. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult result = MessageBox.Show("Do you want to save data?","",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();
                        OleDbDataReader reader = null;
                        OleDbCommand cmd = new OleDbCommand("Select ID From Product WHERE PNo = '" + pno + "'",con);
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            id = reader["ID"].ToString();
                        }
                        if (id == null)
                        {
                            OleDbCommand cmd2 = con.CreateCommand();
                            cmd2.CommandText = "INSERT INTO Product (PNo,Mno,Description,PDescription,Unit,Cprice,SellPrice,ROQ,PType,CName,BName) VALUES('" + pno + "','" + mno + "','" + des + "','" + Pdes + "'," + unit + "," + Cprice + "," + Sprice + "," + roq + ",'" + Ptype + "','" + category + "','" + brand + "')";
                            cmd2.Connection = con;
                            cmd2.ExecuteNonQuery();
                        }
                        else
                        {
                            OleDbCommand cmd2 = con.CreateCommand();
                            cmd2.CommandText = "UPDATE Product SET Mno ='" + mno + "', Description = '" + des + "', PDescription='" + Pdes + "', Unit=" + unit + ", Cprice=" + Cprice + ", SellPrice=" + Sprice + ", ROQ=" + roq + ", PType='" + Ptype + "', CName='" + category + "', BName='" + brand + "' WHERE PNo = '" + pno + "'";
                            cmd2.Connection = con;
                            cmd2.ExecuteNonQuery();                                                        
                        }
                        con.Close();
                                                
                        cmbPno.Enabled = true;
                        cmbPno.Text = null;                        
                        btnNew.Enabled = true;
                        btnDelete.Enabled = true;                        
                        this.Product_Load(null, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("error " + ex);
                    }
                }                
            }
            
            
        }

        private void cmbMno_TextChanged(object sender, EventArgs e)
        {
            SetDescription();
            SetPrintDescription();
        }

        private void cmbCname_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDescription();
            SetPrintDescription();
        }

        private void cmbBname_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDescription();
            SetPrintDescription();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                String pno = cmbPno.Text.ToString();
                con.Open();
                OleDbCommand cmd2 = con.CreateCommand();
                cmd2.CommandText = "DELETE FROM Product WHERE PNo = '" + pno + "'";
                cmd2.Connection = con;
                cmd2.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("eeror " + ex);
            }
            cmbPno.Enabled = true;
            cmbPno.Text = null;
            btnNew.Enabled = true;
            btnDelete.Enabled = true;
            this.Product_Load(null, null);
        }

        private void cmbCname_KeyPress(object sender, KeyPressEventArgs e)
        {
            AutoSearch(cmbCname, e, false);
        }

        private void cmbBname_KeyPress(object sender, KeyPressEventArgs e)
        {
            AutoSearch(cmbBname, e, false);
        }
    }
}
