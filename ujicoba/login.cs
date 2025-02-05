using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ujicoba
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using(var conn = P.conn())
            {
                try
                {
                    if (P.validasi(this.Controls))
                    {
                        MessageBox.Show("Data yang harus diinput tidak boleh kosong!");
                    }
                    else
                    {

                        SqlCommand cmd = new SqlCommand("select * from [User] where email= @email and password=@password", conn);
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.Parameters.AddWithValue("@email", textBox1.Text);
                        cmd.Parameters.AddWithValue("@password", P.enkripsi(textBox2.Text));
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            this.Hide();
                            string namauser = dr["namauser"].ToString();
                            utama utama = new utama(namauser);
                            utama.Show();

                        }
                        else
                        {
                            MessageBox.Show("User tidak ditemukan!");
                        }
                        //cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
