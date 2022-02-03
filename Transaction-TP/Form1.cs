using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Transaction_TP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection cn = new SqlConnection(@"Data Source=.;Initial Catalog=Transactione;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter ad;
        DataTable dt;
        SqlTransaction tr;
        private void Form1_Load(object sender, EventArgs e)
        {
            cmd.Connection = cn;
            AfficherInfo();

        }

        private void textcherche_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textcherche.Text != "")
                {
                    cn.Open();
                    cmd.CommandText = "select * from Client_tab where idC=" + int.Parse(textcherche.Text);
                    SqlDataReader dr;
                    dr =cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                            
                         {
                            label4.Text = dr[1].ToString();
                            label5.Text = dr[2].ToString();
                            
                        }
                    }
                    dr.Close();



                }
                else
                {
                    label4.Text = "**";
                    label5.Text = "**";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();
            }

        }
        void AfficherInfo()
        {
            cmd = new SqlCommand("select *from Client_tab ", cn);
            ad = new SqlDataAdapter(cmd);
            dt = new DataTable();
            ad.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                // le dibut de tronsaction (bigin) :

                tr = cn.BeginTransaction();
                cmd.Transaction = tr;

                cmd.CommandText = "update Client_tab set Solde-=@a where idC=@b";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", textBox1.Text);
                cmd.Parameters.AddWithValue("@b", textcherche.Text);

                cmd.ExecuteNonQuery();

                cmd.CommandText = "update Client_tabeer set Solde+=@a where idC=@c";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@a", textBox1.Text);
                cmd.Parameters.AddWithValue("@c", textBox2.Text);
                cmd.ExecuteNonQuery();

                //la validation des transaction :
                tr.Commit();
                AfficherInfo();
               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

                //Annuler la transaction :
                tr.Rollback();
            }
            finally
            {
                cn.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
