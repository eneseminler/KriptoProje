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
       //Şifreleme boyunca kullanılmak üzere 128 bitlik şifrelenmiş metin, 32 bitlik A,B,C,D matrisleri
       //F fonksiyonunda kullanılacak 4 matris
       //Ana işlemde kullanılacak A1,A2 adlı matrisler oluşturulmuştur.
        private byte[] Data = Encoding.UTF8.GetBytes("00000000000000000000000000000100010000000010000001000001000000010000000001000000011000100000010000010000001100000000101000000010");
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
        private byte[] A4 = Encoding.UTF8.GetBytes("00000000000000000000000000000000");
        private byte[] A5 = Encoding.UTF8.GetBytes("00000000000000000000000000000000");
        private byte[] hash = Encoding.UTF8.GetBytes("kdgqrwtyxmojasfg");

        public Form1()
        {
            InitializeComponent();
        }
        private void Main()
        {

            //Şifrelenecek metin ve Hash textboxlardan alınıyor.
            byte[] SifrelenecekMetin = Encoding.UTF8.GetBytes(textBox1.Text);
            byte[] HashMetin = Encoding.UTF8.GetBytes(textBox2.Text);
            int SifrelenecekUzunluk = SifrelenecekMetin.Length;
            int HashUzunluk = HashMetin.Length;

            //Şifrelenecek metin 128 bitlik veri haline getiriliyor.
            for (int i = 0; i < SifrelenecekUzunluk; i++)
            {
                Data[i] = SifrelenecekMetin[i];
            }
            for (int i = 0; i < HashUzunluk; i++)
            {
                hash[i] = HashMetin[i];
            }
            
            //128 bitlik veri 32-32 4-e bölünüyor.
            Bol();


            //16 şar kez 4 parçada şifreleme işlemi yapılıyor. Her 16'lık parçada F fonksiyonu değişime uğruyor.
            for (int i = 0; i < 16; i++)
            {
                Ffonk();    //F fonksiyonunda gerekli işlemler yapılyor
                Afonk();    //Ana fonksiyon çalışıyor
                Degistir(); //A B C D Fonksiyonları yerlerini değiştiriyor
            }
            for (int i = 0; i < 16; i++)
            {
                Gfonk();    //G fonksiyonunda gerekli işlemler yapılyor
                Afonk();    //Ana fonksiyon çalışıyor
                Degistir(); //A B C D Fonksiyonları yerlerini değiştiriyor
            }
            for (int i = 0; i < 16; i++)
            {
                Hfonk();    //H fonksiyonunda gerekli işlemler yapılyor
                Afonk();    //Ana fonksiyon çalışıyor
                Degistir(); //A B C D Fonksiyonları yerlerini değiştiriyor
            }
            for (int i = 0; i < 16; i++)
            {
                Ifonk();    //G fonksiyonunda gerekli işlemler yapılyor
                Afonk();    //Ana fonksiyon çalışıyor
                Degistir(); //A B C D Fonksiyonları yerlerini değiştiriyor
            }

            Birlestir();    //32 Bitlik böldüğümüz veriyi 128 bit haline getiriyoruz
            Duzelt();       //Karakterleri anlamlı hale getirmek için aralık dışındaki unicode değerleriyle oynuyoruz

            //Şifrelenmiş metini yazdırıyoruz.
            textBox3.Text = System.Text.Encoding.UTF8.GetString(Data);
            

            ////Şifrelenmiş metin dilendiği taktirde dosyaya yazılabilir.
            //string dosya_yolu = @"D:\metin.txt";
            ////İşlem yapacağımız dosyanın yolunu belirtiyoruz.
            //FileStream fs = new FileStream(dosya_yolu, FileMode.OpenOrCreate, FileAccess.Write);
            ////Bir file stream nesnesi oluşturuyoruz. 1.parametre dosya yolunu,
            ////2.parametre dosya varsa açılacağını yoksa oluşturulacağını belirtir,
            ////3.parametre dosyaya erişimin veri yazmak için olacağını gösterir.
            //StreamWriter sw = new StreamWriter(fs);
            ////Yazma işlemi için bir StreamWriter nesnesi oluşturduk.
            ////Şifrelenmiş metin yazılıyor
            //sw.WriteLine(System.Text.Encoding.UTF8.GetString(Data));

            ////Şifrelenmiş metinin her bir karakterinin Unicode değerleri yazılıyor
            //for (int i = 0; i < 128; i++)
            //{
            //    sw.Write(Data[i] + " - ");
            //}
            
            
           
            ////Dosyaya ekleyeceğimiz iki satırlık yazıyı WriteLine() metodu ile yazacağız.
            //sw.Flush();
            ////Veriyi tampon bölgeden dosyaya aktardık.
            //sw.Close();
            //fs.Close();



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
            int a, b, c, d, e, f;
            for (int i = 0; i < 32; i++)
            {
                 a = A[i] ^ F4[i];
                A1[i] = Convert.ToByte(a);

                 b = ((i * 11) + (a * hash[i/2])) % 16;
                A2[i] = Data[b];
                
                a = A1[i] ^ (A2[i]);
                A2[i] = Convert.ToByte(a);

                 c = ((i * 7) + b) % 16;
                 d = A2[i] ^ hash[c];
                A5[i] = Convert.ToByte(d);

                 e = A5[i] << 1;
                if (e > 128)
                {
                    e = e - 128;
                }
                A4[i] = (byte)e;
                 f = A4[i] ^ B[i];
                A3[i] = Convert.ToByte(f);
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
                    b = b - 128;
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
                    b = b - 128;
                }
                F2[i] = (byte)b;

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
                    c = c - 128;
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
        private void Duzelt()
        {
            int a = 0;
            int b = 0;
            int c = 129;
            int d = 80;
            int e = 32;
            for (int i = 0; i < 128; i++)
            {
                if (Data[i] > 200)
                {
                    a = (int)Data[i];
                    b = a - c;
                    Data[i] = (byte)b;
                }
                else if (Data[i] > 126 && Data[i] <= 200)
                {
                    a = (int)Data[i];
                    b = a - d;
                    Data[i] = (byte)b;
                }
                else if (Data[i] >= 0 && Data[i] <= 32)
                {
                    a = (int)Data[i];
                    b = a + d;
                    Data[i] = (byte)b;
                }
                else
                {
                    Data[i] = Data[i];
                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Main();
            
        }

      
    }
}
