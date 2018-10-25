using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace KriptoProje
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
       
        }
        private string password = "1";
        private byte[] Sifrele(byte[] SifresizVeri, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();

            alg.Key = Key;
            alg.IV = IV;

            CryptoStream cs = new CryptoStream(ms,alg.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(SifresizVeri, 0, SifresizVeri.Length);
            cs.Close();

            byte[] sifrelenmisVeri = ms.ToArray();
            return sifrelenmisVeri;
        }
        private byte[] SifreCoz(byte[] SifreliVeri, byte[] Key, byte[] IV)

        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();

            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(SifreliVeri, 0, SifreliVeri.Length);
            cs.Close();

            byte[] SifresiCozulmusVeri = ms.ToArray();
            return SifresiCozulmusVeri;

        }
        public string TextSifrele(string sifrelenecekMetin)
        {
            byte[] sifrelenecekByteDizisi = System.Text.Encoding.Unicode.GetBytes(sifrelenecekMetin);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
            byte[] SifrelenmisVeri = Sifrele(sifrelenecekByteDizisi,pdb.GetBytes(32), pdb.GetBytes(16));
            return Convert.ToBase64String(SifrelenmisVeri);
            
        }
        public string TextSifreCoz(string text)
        {

            byte[] SifrelenmisByteDizisi = Convert.FromBase64String(text);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65,0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            byte[] SifresiCozulmusVeri = SifreCoz(SifrelenmisByteDizisi,pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.Unicode.GetString(SifresiCozulmusVeri);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Width = 200;
            textBox1.Height = 50;
            textBox2.Width = 200;
            textBox2.Height = 50;
            label1.Text = "Şifrelenmiş Metin";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = textBox2.Text = TextSifrele(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = TextSifreCoz(textBox2.Text);
        }
    }
}
