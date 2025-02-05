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
    public partial class datauser : Form
    {
        public datauser()
        {
            InitializeComponent();
            tampildata();
        }

        private void tampildata()
        {
            using (var conn = P.conn())
            {
                try
                {


                    SqlCommand cmd = new SqlCommand("select * from [User]", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    DataTable dt = new DataTable();
                    SqlDataReader dr = cmd.ExecuteReader();
                    dt.Load(dr);
                    dataGridView1.DataSource = dt;
                    conn.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var conn = P.conn())
            {
                try
                {
                    if (P.validasi(this.Controls))
                    {
                        MessageBox.Show("Data yang ingin diinput harus lengkap!");
                    }
                    else
                    {
                        var row = dataGridView1.CurrentRow.Cells;
                        int kodeuser = Convert.ToInt32(row["kodeuser"].Value.ToString());
                        SqlCommand cmd = new SqlCommand("update [User] set namauser=@namauser , email= @email, password=@password where kodeuser=@kodeuser", conn);
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.Parameters.AddWithValue("@kodeuser", kodeuser);
                        cmd.Parameters.AddWithValue("@namauser", textBox1.Text);
                        cmd.Parameters.AddWithValue("@email", textBox2.Text);
                        cmd.Parameters.AddWithValue("@password", textBox3.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data berhasil diubah!");
                        tampildata();
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var conn = P.conn())
            {
                try
                {
                    if (P.validasi(this.Controls))
                    {
                        MessageBox.Show("Data yang ingin diinput harus lengkap!");
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("insert into [User] values (@namauser , @email, @password)", conn);
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.Parameters.AddWithValue("@namauser", textBox1.Text);
                        cmd.Parameters.AddWithValue("@email", textBox2.Text);
                        cmd.Parameters.AddWithValue("@password", P.enkripsi(textBox3.Text));
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data berhasil ditambahkan!");
                        tampildata();
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridView1.CurrentRow.Cells;
            textBox1.Text = row["namauser"].Value.ToString();
            textBox2.Text = row["email"].Value.ToString();
            textBox3.Text = row["password"].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var conn = P.conn())
            {
                try
                {
                    if (P.validasi(this.Controls))
                    {
                        MessageBox.Show("Data yang ingin diinput harus lengkap!");
                    }
                    else
                    {
                        var row = dataGridView1.CurrentRow.Cells;
                        int kodeuser = Convert.ToInt32(row["kodeuser"].Value.ToString());
                        SqlCommand cmd = new SqlCommand("delete from [User] where kodeuser=@kodeuser", conn);
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.Parameters.AddWithValue("@kodeuser", kodeuser);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data berhasil dihapus!");
                        tampildata();
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }
    }
}
