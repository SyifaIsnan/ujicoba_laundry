using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ujicoba
{
    public partial class Order : Form
    {
        public Order()
        {
            InitializeComponent();
            tampildata();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "0";
            textBox4.Text = "0";
            textBox5.Text = "0";
            textBox6.Text = "0";
            textBox7.Text = "0";
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            textBox6.ReadOnly = true;
            textBox7.ReadOnly = true;
            richTextBox1.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            comboBox1.SelectedIndex = -1;

            using (var conn = P.conn())
            {
                SqlCommand cmd = new SqlCommand("select kodepetugas, namapetugas from [PetugasAntar]", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                DataTable dt = new DataTable();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                comboBox1.DataSource = dt;
                comboBox1.ValueMember = "kodepetugas";
                comboBox1.DisplayMember = "namapetugas";
                comboBox1.SelectedIndex = -1;
                conn.Close();


                DataGridViewComboBoxColumn status = new DataGridViewComboBoxColumn();
                status.DataSource = new string[] { "PENDING", "DIPROSES", "DIANTAR" };
                status.Name = "Status";
                status.HeaderText = "Status";
                status.DataPropertyName = "Status";

                DataGridViewLinkColumn link = new DataGridViewLinkColumn();
                link.Name = "Pilih layanan";
                link.HeaderText = "Pilih layanan";
                link.Text = "Pilih layanan";
                link.UseColumnTextForLinkValue = true;

                dataGridView1.Columns.Add(status);
                dataGridView1.Columns.Add(link);
            }

        }

        private void tampildata()
        {
            using (var conn = P.conn())
            {
                try
                {


                    SqlCommand cmd = new SqlCommand("select * from [Order]", conn);
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


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Order_Load(object sender, EventArgs e)
        {
            
        }

        private void nomortelepon(string nomortelepon)
        {
            using (var conn = P.conn())
            {
                try
                {


                    SqlCommand cmd = new SqlCommand("select * from [Pelanggan] where nomortelepon = @nomortelepon", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        string nama = dr["nama"].ToString();
                        string alamat = dr["alamat"].ToString();
                        caripelanggan(nama, alamat);
                    }
                    conn.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void caripelanggan(string nama, string alamat)
        {
            textBox2.Text = nama;
            richTextBox1.Text = alamat;
            dateTimePicker1.Focus();
            dateTimePicker2.Focus();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
                                                                                                                        var row = dataGridView1.CurrentRow.Cells;
            textBox1.Text = row["nomortelepon"].Value.ToString();
            textBox4.Text = row["biayajemput"].Value.ToString();
            textBox5.Text = row["biayaantar"].Value.ToString();
            textBox6.Text = row["biayahari"].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(row["tanggalorder"].Value.ToString());
            dateTimePicker2.Value = Convert.ToDateTime(row["tanggalselesai"].Value.ToString());
            comboBox1.SelectedValue = row["petugasantar"].Value.ToString() ;

            DateTime order = dateTimePicker1.Value;
            DateTime selesai = dateTimePicker2.Value;
            TimeSpan selisih = selesai - order;
            int lamahari = selisih.Days;
            textBox3.Text = lamahari.ToString();

            if (lamahari > 3)
            {
                using (var conn = P.conn())
                {
                    SqlCommand cmd = new SqlCommand("select biaya from [BiayaTambahan] where kodebiaya = 1", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBox6.Text = dr["biaya"].ToString();
                    }

                }
            }
            else
            {
                textBox6.Text = "0";
            }

            if (textBox6.Text != "0")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }

            if (textBox7.Text != "0")
            {
                checkBox2.Checked = true;
            }
            else
            {
                checkBox2.Checked = false;
            }

            using (var conn = P.conn())
            {
                try
                {


                    SqlCommand cmd = new SqlCommand("select * from [Pelanggan] where nomortelepon = @nomortelepon", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        string nama = dr["nama"].ToString();
                        string alamat = dr["alamat"].ToString();
                        caripelanggan(nama, alamat);
                    }
                    conn.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }


            hitungtotal();

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

                        SqlCommand cekdata = new SqlCommand("select count(*) from [Pelanggan] where nomortelepon = @nomortelepon", conn);
                        cekdata.CommandType = CommandType.Text;
                        conn.Open();
                        cekdata.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
                        int datapelanggan = (int)cekdata.ExecuteScalar();
                        if (datapelanggan == 0)
                        {
                            SqlCommand tambah = new SqlCommand("insert into [Pelanggan] values (@nomortelepon , @nama, @alamat)", conn);
                            tambah.CommandType = CommandType.Text;
                            conn.Open();
                            tambah.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
                            tambah.Parameters.AddWithValue("@nama", textBox2.Text);
                            tambah.Parameters.AddWithValue("@alamat", richTextBox1.Text);
                            tambah.ExecuteNonQuery();
                            conn.Close();
                        }


                        SqlCommand cmd = new SqlCommand("insert into [Order] values (@nomortelepon, @tanggalorder, @tanggalselesai, @biayaantar, @biayajemput , @biayahari, @petugasantar, @statusorder)", conn);
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
                        cmd.Parameters.AddWithValue("@tanggalorder", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@tanggalselesai", dateTimePicker2.Value);
                        cmd.Parameters.AddWithValue("@biayaantar", textBox5.Text);
                        cmd.Parameters.AddWithValue("@biayajemput", textBox4.Text);
                        cmd.Parameters.AddWithValue("@biayahari", textBox6.Text);
                        cmd.Parameters.AddWithValue("@petugasantar", comboBox1.SelectedValue);
                        cmd.Parameters.AddWithValue("@statusorder", "PENDING");
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
               using(var conn = P.conn())
               {
                    SqlCommand cmd = new SqlCommand("select biaya from [biayatambahan] where kodebiaya = 3", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBox4.Text = dr["biaya"].ToString();
                    }
                    conn.Close();
               }
            }
            else
            {
                textBox4.Text = "0";
            }
            hitungtotal();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                using (var conn = P.conn())
                {
                    SqlCommand cmd = new SqlCommand("select biaya from [biayatambahan] where kodebiaya = 2", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBox5.Text = dr["biaya"].ToString();
                    }
                    conn.Close();
                    comboBox1.Enabled = true;
                }
            }
            else
            {
                textBox5.Text = "0";
            }
            hitungtotal();
        }

        private void hitungtotal()
        {
            int jemput = Convert.ToInt32(textBox4.Text);
            int antar = Convert.ToInt32(textBox5.Text);
            int hari = Convert.ToInt32(textBox6.Text);

            int total = jemput + antar + hari;
            textBox7.Text = total.ToString("C", CultureInfo.GetCultureInfo("id-ID"));

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime order = dateTimePicker1.Value;
            DateTime selesai = dateTimePicker2.Value;
            TimeSpan selisih = selesai - order;
            int lamahari = selisih.Days;
            textBox3.Text = lamahari.ToString();

            if(lamahari > 3)
            {
                using (var conn = P.conn())
                {
                    SqlCommand cmd = new SqlCommand("select biaya from [BiayaTambahan] where kodebiaya = 1", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBox6.Text = dr["biaya"].ToString();
                    }
                    
                }
            }
            else
            {
                textBox6.Text = "0";
            }

            hitungtotal();
      
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string nomor = textBox1.Text;
                nomortelepon(nomor);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "Pilih layanan")
            {
                Pilihlayanan l = new Pilihlayanan();
                l.ShowDialog();
            }
        }
    }
}
