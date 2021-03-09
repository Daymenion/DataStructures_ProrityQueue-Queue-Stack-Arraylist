using System;
using System.Collections;
using System.Collections.Generic;

namespace bir
{
    class Musterisinifi
    {
        public string Musteri;
        public string Urunsayisi;

        public Musterisinifi(string musteri, string urunsayisi)
        {
            this.Musteri = musteri;
            this.Urunsayisi = urunsayisi;
        }
    }
    class Program
    {
        public static string[] Duzenle(string[] dizi,int temp)
        {//kullanicidan aldigimiz urunsayisi ve musteriadi stringlerindeki, rastgele olarak sectigimiz kullanici
         //ve urunsayisindaki veriyi aldiktan sonra stringi yeniden duzenlemek ve secilene gore
         //siralamak icin yapilmis bir method.
            string[] dizi1 = new string[dizi.Length - 1];
            for (int i = 0; i < dizi.Length - 1; i++)
            {
                if (i >= temp)
                    dizi1[i] = dizi[i+1];
                else
                    dizi1[i] = dizi[i];
            }
            return dizi1;
        }
        static void Main()
        {
            //Console.BackgroundColor = ConsoleColor.White; //Ben siyah cmd kullaniyorum bu yuzden yorum satiri olarak biraktim.
            Console.ForegroundColor = ConsoleColor.Black;  
            string[] musteriAdi = {"Ali", "Merve", "Veli", "Gülay", "Okan", "Zekiye", "Kemal", "Banu", "İlker", "Songül", "Nuri", "Deniz"};
            string[] urunSayisi = { "8", "11", "16", "5", "15", "14", "19","3", "18", "17", "13", "15" };
            ArrayList arrayliste = new ArrayList();
            int sayac = 0;
            int temp;
            Musterisinifi musterisinifi;
            Random rastgele = new Random();
            List<Musterisinifi> genericListe;
            int a = musteriAdi.Length; //az sonra bircok for dongusunde kullanacagiz
            while (sayac < a)
            {
                genericListe = new List<Musterisinifi>();
                int sayac2;
                if(musteriAdi.Length < 5) //stringimizin suandaki uzunlugu 5 den kucukse max string uzunlugumuz kadar random donmeli
                    sayac2 = rastgele.Next(1, musteriAdi.Length + 1); //extra olarak bu kodu yazmamizdaki temel sebep genericlisteye alacagimiz random verileri de bu sayaca gore sececegiz
                else
                    sayac2 = rastgele.Next(1,6); //klasik random alma 
                int genericListeLength = sayac2;
                for (int i = 0; i < genericListeLength; i++) 
                {
                    if (sayac2 > 1)   //Burada donulen random sececegimiz verinin indexidir. Random dondukce sayac2yi azaltacagiz cunku sectigimiz veriyi duzenle methodu yardimiyla stringten siliyoruz 
                        temp = rastgele.Next(sayac2--);
                    else
                        temp = 0; // 1 kalincada direkt olarak 0 atayabiliriz.
                    musterisinifi = new Musterisinifi(musteriAdi[temp], urunSayisi[temp]); //Stringleri musteri sinifina donusturuyoruz
                    genericListe.Add(musterisinifi);    //ve genericlisteye ekliyoruz sonrada duzenle methoduyla stringleri duzenliyoruz
                    musteriAdi = Duzenle(musteriAdi,temp);
                    urunSayisi = Duzenle(urunSayisi, temp);
                    sayac++; //sayaci arttirarak whileden cikana kadar donmesini saglayabiliriz.
                }
                arrayliste.Add(genericListe);  //Genericlistemizi arraylistemize ekleyebiliriz
            }
            Stack<Musterisinifi> yigit = new Stack<Musterisinifi>(a); //soru 2 icin stack ve queue miz
            Queue<Musterisinifi> kuyruk = new Queue<Musterisinifi>(a);
            PriorityQueue<int, Musterisinifi> fifopQueue = new PriorityQueue<int, Musterisinifi>(); //Soru 3 ve 4 icin PQmuz
            PriorityQueue<int, Musterisinifi> lifopQueue = new PriorityQueue<int, Musterisinifi>();
            Console.WriteLine("Rastgele Sirada gelen Musteriler ve alacaklari urun sayisi:\n");
            foreach (List<Musterisinifi> item in arrayliste)
            {
                foreach (Musterisinifi item1 in item)
                {
                    yigit.Push(item1);   //Soru 2 deki yigita ve kuyrugumuza elemanlarin eklenmesi
                    kuyruk.Enqueue(item1);
                    fifopQueue.Enqueue(Convert.ToInt32(item1.Urunsayisi),item1); //Soru 3 ve 4 deki PQmuza elemanlarin gonderilmesi
                    lifopQueue.Enqueue2(Convert.ToInt32(item1.Urunsayisi), item1);
                    Console.WriteLine("\t{0}, {1}", item1.Musteri, item1.Urunsayisi); //Soru 1 in outputu, Arayliste icindeki generic listelerin konsolo yazilmasi
                }
                Console.WriteLine(); // Genericlisteler arasi bosluk birakilmasi
            }
            float b = arrayliste.Count;
            Console.WriteLine("Arraylist icindeki liste sayisi: " + b);
            Console.WriteLine("Ortalama eleman sayisi: " + a / b); //Soru 1in ortlama eleman outputu
            Console.WriteLine("\n\n*stack*\t\t\t*queue*\n");
            float toplam = 0;
            float toptoplam = 0; //Soru 4 icin queue ve PQ arasindaki ortlama islem suresi hesaplama icin icin
            for (int i = 0; i < a; i++)
            {//Once peek sonra pop ya da once peek sonra dequeue kullanmalıyım kı değer kaybolmasın ama sıralamada da ılerleyebıleyım.
                toplam += Convert.ToInt32(kuyruk.Peek().Urunsayisi); //kuyruk ile yaptigimiz sirlamanin ortlama islem suresi icin
                toptoplam += toplam;
                Console.WriteLine(yigit.Peek().Musteri + ", " + yigit.Pop().Urunsayisi + "   \t|\t" + kuyruk.Peek().Musteri + ", " + kuyruk.Dequeue().Urunsayisi + "    --->  \t bekleyecegi toplam sure: " + toplam);
            } //Yigit ve kuyrugumuz konsolo basilmasi
            
            Console.WriteLine("\nUrun sayisinin fazla olmasina gore oncelik verilirse siralama:");
            float toplam1 = 0;
            float toptoplam1 = 0;
            for (int i = 0; i < a; i++)
            {
                Musterisinifi item; //Soru 3 un outputu
                item = lifopQueue.Dequeue2().Value; //Elemanin PQdan cikarilip iteme atanmasi
                toplam1 += Convert.ToInt32(item.Urunsayisi); // 3. sorunun PQ icin ortlama islem suresi toplanmasi
                toptoplam1 += toplam1;
                Console.WriteLine("{0}. Siradaki kisi\t {1} , {2}  --->  \t bekleyecegi toplam sure: {3}", i + 1, item.Musteri, item.Urunsayisi, toplam1);
            }
            Console.WriteLine("\n\nUrun sayisinin az olmasina gore oncelik verilirse siralama:");
            float toplam2 = 0;
            float toptoplam2 = 0;//Soru 4 deki ortlama islem suresi icin PQ toptoplami
            for (int i = 0; i < a; i++)
            {
                Musterisinifi item;
                item = fifopQueue.Dequeue().Value; //Elemanin PQdan cikarilip iteme atanmasi ve bastirilmasi
                toplam2 += Convert.ToInt32(item.Urunsayisi); // 4. sorunun PQ icin ortlama islem suresi toplanmasi
                toptoplam2 += toplam2;
                Console.WriteLine("{0}. Siradaki kisi\t {1} , {2}  --->  \t bekleyecegi toplam sure: {3}", i + 1, item.Musteri, item.Urunsayisi, toplam2);
            }
            Console.WriteLine("\n\nKuyruk ile yapilan once gelenin one gectigi siralamada ortalama islem suresi: {0}",toptoplam/a);
            Console.WriteLine("\nOncelikli Kuyruk ile urunsayisi coktan aza yapilan siralamada ortalama islem suresi: {0}", toptoplam1 / a);
            Console.WriteLine("\nOncelikli Kuyruk ile urunsayisi azdan coga yapilan siralamada ortalama islem suresi: {0}", toptoplam2 / a);
            //Ortlama islem surelerinin outputuyla beraber proje programinin biti.
            Console.ReadKey();
        }
    }
}
