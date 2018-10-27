using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace KriptoProje
{
    public partial class Form1 : Form
    {
        private string hash = "eneseminler";
        private byte[] Data = Encoding.UTF8.GetBytes("00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
        private byte[] A = Encoding.UTF8.GetBytes("00000000000000000000000000000000");
        private byte[] B = Encoding.UTF8.GetBytes("00000000000000000000000000000000");
        private byte[] C = Encoding.UTF8.GetBytes("00000000000000000000000000000000");
        private byte[] D = Encoding.UTF8.GetBytes("00000000000000000000000000000000");
        private byte[] F1 = Encoding.UTF8.GetBytes("00000000000000000000000000000000");
        private byte[] F2 = Encoding.UTF8.GetBytes("00000000000000000000000000000000");
        private byte[] F3 = Encoding.UTF8.GetBytes("00000000000000000000000000000000");
        private byte[] F4 = Encoding.UTF8.GetBytes("00000000000000000000000000000000");
        private byte[] A1 = Encoding.UTF8.GetBytes("00000000000000000000000000000000");
        private byte[] A2 = Encoding.UTF8.GetBytes("00000000000000000000000000000000");
        private byte[] A3 = Encoding.UTF8.GetBytes("00000000000000000000000000000000");
        public Form1()
        {
            InitializeComponent();
        }
        private void Main()
        {
            byte[] ta = Encoding.UTF8.GetBytes(textBox1.Text);
            int taUzunluk = ta.Length;

            for (int i = 0; i < taUzunluk; i++)
            {
                Data[i] = ta[i];
            }
           

            Bol();
           
            for (int i = 0; i < 16; i++)
            {
                Ffonk();
                Afonk();
                Degistir();
            }
            for (int i = 0; i < 16; i++)
            {
                Gfonk();
                Afonk();
                Degistir();
            }
            for (int i = 0; i < 16; i++)
            {
                Hfonk();
                Afonk();
                Degistir();
            }
            for (int i = 0; i < 16; i++)
            {
                Hfonk();
                Ifonk();
                Degistir();
            }
            Birlestir();


            textBox3.Text = System.Text.Encoding.UTF8.GetString(Data);
            
            textBox2.Text = Convert.ToString(Data[0]);
            //textBox3.Text = Convert.ToString(Data[105]);
            string dosya_yolu = @"D:\metin.txt";
            //İşlem yapacağımız dosyanın yolunu belirtiyoruz.
            FileStream fs = new FileStream(dosya_yolu, FileMode.OpenOrCreate, FileAccess.Write);
            //Bir file stream nesnesi oluşturuyoruz. 1.parametre dosya yolunu,
            //2.parametre dosya varsa açılacağını yoksa oluşturulacağını belirtir,
            //3.parametre dosyaya erişimin veri yazmak için olacağını gösterir.
            StreamWriter sw = new StreamWriter(fs);
            //Yazma işlemi için bir StreamWriter nesnesi oluşturduk.
            for (int i = 0; i < 128; i++)
            {
                sw.WriteLine(Data[i]);
            }
            
           
            //Dosyaya ekleyeceğimiz iki satırlık yazıyı WriteLine() metodu ile yazacağız.
            sw.Flush();
            //Veriyi tampon bölgeden dosyaya aktardık.
            sw.Close();
            fs.Close();



        }
        private void Degistir()
        {
            for (int i = 0; i < 32; i++)
            {
                A[i] = D[i];
                D[i] = C[i];
                C[i] = B[i];
                B[i] = A3[i];
            }
        }
        private void Afonk()
        {
            for (int i = 0; i < 32; i++)
            {
                int a = A[i] ^ F4[i];
                A1[i] = Convert.ToByte(a);
                int c = ~A1[i];
                if (c > 128)
                {
                    c = c - 127;
                }
                A2[i] = (byte)c;
                int b = A2[i] ^ B[i];
                A3[i] = Convert.ToByte(b);
            }
        }
        private void Ffonk()
        {
            for (int i = 0; i < 32; i++)
            {
                int a = B[i] & C[i];
                F1[i] = Convert.ToByte(a);

                int b = ~B[i];
                if (b > 128)
                {
                    b = b - 127;
                }
                F2[i] = (byte)b;

                int c = F2[i] & D[i];
                F3[i] = Convert.ToByte(c);

                int d = F1[i] | F3[i];
                F4[i] = Convert.ToByte(d);
            }
        }
        private void Gfonk()
        {
            for (int i = 0; i < 32; i++)
            {
                int a = B[i] & D[i];
                F1[i] = Convert.ToByte(a);

                int b = ~D[i];
                if (b > 128)
                {
                    b = b - 127;
                }
                F2[i] = (byte)~b;

                int c = F2[i] & C[i];
                F3[i] = Convert.ToByte(c);

                int d = F1[i] | F3[i];
                F4[i] = Convert.ToByte(d);

            }

        }
        private void Hfonk()
        {
            for (int i = 0; i < 32; i++)
            {
                int a = A[i] ^ B[i];
                F1[i] = Convert.ToByte(a);

                int b = F1[i] ^ D[i];
                F4[i] = Convert.ToByte(b);
            }
            
        }
        private void Ifonk()
        {
            for (int i = 0; i < 16; i++)
            {
                int c = ~D[i];
                if (c > 128)
                {
                    c = c - 127;
                }
                F1[i] = (byte)c;

                int a = A[i] | F1[i];
                F2[i] = Convert.ToByte(a);

                int b = F2[i] ^ B[i];
                F4[i] = Convert.ToByte(b);
            }
        }
        private void Bol()
        {
            for (int i = 0; i < 32; i++)
            {
                A[i] = Data[i];   
            }
            for (int i = 0; i < 32; i++)
            {
                B[i] = Data[i + 32];
            }
            for (int i = 0; i < 32; i++)
            {
                C[i] = Data[i + 64];
            }
            for (int i = 0; i < 32; i++)
            {
                D[i] = Data[i + 96];
            }
        }
        private void Birlestir()
        {
            for (int i = 0; i < 32; i++)
            {
                Data[i] = A[i];
                Data[i + 32] = B[i];
                Data[i + 64] = C[i];
                Data[i + 96] = D[i];

            }
        }




        private void button1_Click(object sender, EventArgs e)
        {
            Main();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] data = Convert.FromBase64String(textBox2.Text);
           
        }
    }
}
