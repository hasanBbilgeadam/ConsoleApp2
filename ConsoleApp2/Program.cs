using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Yemek> yemeks = new List<Yemek>()
            {
                new(){ Fiyat =100,Name="döner"},
                new(){ Fiyat =200,Name="kebap"},
                new(){ Fiyat =300,Name="tatlu"},
            }; 

            Müşteri müşteri = new Müşteri(12345);

            Aşçı aşçı = new Aşçı();

            Garson garson    = new Garson("hasan",aşçı);


            garson.SiparişAl(yemeks, müşteri, DateTime.Now);

            müşteri.ÖdemeYap(garson);



        
        }
    }


    //restoran



    public class Ödeme
    {

        public bool KartİleÖdeme(int kartNumarası)
        {
            if (kartNumarası>9999)
            {
              return true;
            }
            return false;
        }

    }


    public class Müşteri
    {
        /// <summary>
        /// kart numarası 5 haneli olmalı
        /// </summary>
        /// <param name="kartNumara"></param>
        public Müşteri(int kartNumara)
        {
            kartNumarasıF=kartNumara ;
        }

        private int kartNumarasıF;//12345
        private int bakiye;


        public string Adı { get; set; }
        public int MasaNumarası { get; set; }
        public int KartNumarası { get { return kartNumarasıF / 100; }  }//123

        public void ÖdemeYap(Garson garson)
        {

            garson.HesapAl(MasaNumarası);


            Ödeme ödeme = new Ödeme();

            if (ödeme.KartİleÖdeme(kartNumarasıF))
            {
                Console.WriteLine("ödeme başarılı");
            } 


        }

    }


    //müşteri 

    //yemek

    public class Yemek
    {
        public int Fiyat { get; set; }
        public string Name { get; set; }
    }

    //garson => şipariş - hesapal - yemekTesli Et 


    public class Garson
    {

        private Aşçı Aşçı;
        public  int GarsonNumarası { get; }
        public string Adı { get; set; }
        public Garson(string name,Aşçı aşçı)
        {
            Random random = new Random();
            GarsonNumarası = random.Next(10, 201);
            Adı = name;
            Aşçı = aşçı;

        }

        public List<Yemek> YemekSeç(List<Yemek> menü)
        {
            var list = new List<Yemek>();

            bool control = true;
            Console.WriteLine("yemek seçiniz ");
            Console.WriteLine("1 döner");
            Console.WriteLine("2 kebap");
            Console.WriteLine("3 tatlı");
            while (control)
            {
               string seçim = Console.ReadLine();

                switch (seçim)
                {
                    case "1":
                        {
                            list.Add(menü[0]);
                            break;
                        }

                    case "2":
                        {
                            list.Add(menü[1]);
                            break;
                        }

                    case "3":
                        {
                            list.Add(menü[2]);
                            break;
                        }

                    default:
                        {

                        Console.WriteLine("seçim işlemi tamamlandı siparişleriniz alındı");
                        control = false;
                        break;
                
                        }
                }


            }


            return list; 

           
        }
        public void SiparişAl(List<Yemek> menü, Müşteri müşteri,DateTime SiparişTarihi)
        {

            Sipariş sipariş = new Sipariş();
            sipariş.Yemekler = YemekSeç(menü);
            sipariş.SiparişTarih = SiparişTarihi;
            sipariş.Garson = this;
            sipariş.Müşteri = müşteri;


            Aşçı.SiparişAl(sipariş);

            Console.WriteLine("sipariş iletildi yapılmaya başlanıyoır");
            Thread.Sleep(1000);
            Aşçı.YemekYap();

           
        }

        public int HesapAl(int masaNumarası)
        {
            return Aşçı.HesapOluştur(masaNumarası);
        }


    }

    public class  Sipariş
    {

        public List<Yemek> Yemekler { get; set; }
        public DateTime SiparişTarih { get; set; }
        public Müşteri Müşteri { get; set; }
        public Garson Garson { get; set; }

        public bool Durum { get; set; }

    }

    public class Aşçı
    {

        public Aşçı()
        {
            Siparişler = new();
                
        }
        public string Name { get; set; }

        private List<Sipariş> Siparişler { get; set; }

        public void SiparişAl(Sipariş sipariş)
        {
            Siparişler.Add(sipariş);

            
            YemekYap();
        }


        public void YemekYap()
        {

            for (int i = 0; i < Siparişler.Count; i++)
            {
                
                    Console.WriteLine(Siparişler[i].SiparişTarih.ToString() + " olan şipariş yapılmaya başlandı");
                    Console.WriteLine("Masa Numarası :" + Siparişler[i].Müşteri.MasaNumarası);
                    Console.WriteLine("Garson adı :" + Siparişler[i].Garson.Adı);
                    Console.WriteLine("Yemek Sayısı :" + Siparişler[i].Yemekler.Count);
                  Thread.Sleep(1000);
                foreach (var item in Siparişler[i].Yemekler)
                    {
                        Thread.Sleep(500);

                        Console.WriteLine(item.Name + " yapılıyor");


                    }
                    Siparişler[i].Durum = true;

                    Console.WriteLine(Siparişler[i].Garson + " sipariş hazır götürebilirsin");
                    Console.WriteLine(Siparişler[i].Müşteri.MasaNumarası + " masa numarası");


                


            }

        }


        public int HesapOluştur(int masaNumarası)
        {
            int sum = 0;
            foreach (var item in Siparişler)
            {
                if (masaNumarası == item.Müşteri.MasaNumarası) 
                {

                    foreach (var item1 in item.Yemekler)
                    {
                        sum += item1.Fiyat;
                    }

                    break;
                }
            }

            return sum;
        }


    }
    //aşçı 
    //yemek yap - sipariş listesi 



    
}