using System;
using System.Collections.Generic;

namespace bir
{
    public class PriorityQueue<TUrunsayisi, TMusteri> // TUrunsayisi oncelik derecesini TMusteri ise Musteri ismini temsil eder
    {
        private List<KeyValuePair<TUrunsayisi, TMusteri>> _heap; // Heap veri yapısına uygun olacak şekilde içeriğimizi tutacağımız koleksiyon
        private IComparer<TUrunsayisi> _kiyasla; // Max-Heap veya Min-Heap' e göre bir uyarlamaya hizmet verebilmek için kullanılacak arayüz referansı
        private const string Uyari = "Koleksiyonda hiç eleman yok"; //Hata mesajimiz

        #region Constructors 

        // TUrunsayisi ile gelen tipin varsayılan karşılaştırma kriterine göre bir yol izlenir
        public PriorityQueue()
            :this(Comparer<TUrunsayisi>.Default) 
        {
        }

        // TUrunsayisi tipinin karşılaştırma işlevselliğini üstlenen bir IComparer implementasyonu ile bir Construct işlemi
        public PriorityQueue(IComparer<TUrunsayisi> karsilastirici)
        {
            if (karsilastirici == null)
                throw new ArgumentNullException();

            _heap = new List<KeyValuePair<TUrunsayisi, TMusteri>>();
            _kiyasla = karsilastirici;
        }

        #endregion

        #region Temel Fonksiyonlar

        // Koleksiyona bir veri eklemek için kullanılacak. Goruldugu gibi Enqueue ve Enqueue2 olmak uzere iki tur vardir. bunlarin temel farki da
        //sirasiyla Lastfirst ve Lastfirst2 ye gondermeleridir. Birden fazla class olusturmak istemedigim icin maxheap ve minheap teknikleri kulanacak
        //Durumlari ayni class icerisinde inceliyorum. Yeni bir class olusturmak yerine methodlari ciftlestiriyor ve kucuktur buyuktur isaretlerini
        //Ve gonderilen method refaranslarini degistirerek cok daha az memory allocate i isteyen ve cok daha islevsel ve anlasilabilir ve kisa bir
        //Proje olusturmaya calisiyorum. Metodlarin yaninda bir sey yazmayanlari min-heap. 2 yazanlari ise max-heap icindir.
        public void Enqueue(TUrunsayisi oncelik, TMusteri deger)
        {
            KeyValuePair<TUrunsayisi, TMusteri> veri = new KeyValuePair<TUrunsayisi, TMusteri>(oncelik, deger);
            _heap.Add(veri);
            // Sondan basa dogru yeniden bir siralama yaptirilir.
            LastToFirst(_heap.Count - 1);
        }
        public void Enqueue2(TUrunsayisi oncelik, TMusteri deger)
        {
            KeyValuePair<TUrunsayisi, TMusteri> veri = new KeyValuePair<TUrunsayisi, TMusteri>(oncelik, deger);
            _heap.Add(veri);
            // Sondan basa dogru yeniden bir siralama yaptirilir.
            LastToFirst2(_heap.Count - 1);
        }
        public KeyValuePair<TUrunsayisi, TMusteri> Dequeue()
        {
            if (!Bosmu) //Koleksiyon bos degilse girmeli, Bos ise hata mesaji yollaybiliriz.
            {
                FirstToLast(0); // Kolaysiyonu baston sona yeniden uzerinden gec ve en kucugu roota getir
                KeyValuePair<TUrunsayisi, TMusteri> sonuc = _heap[0]; // Heapin Root-undaki elemani al
                // Dequeue roottaki elemani dondurken onu koleksiyondan da cikartmali
                if (_heap.Count <= 1) // 1 veya daha az eleman soz konusu ise temizle
                {
                    _heap.Clear();
                }
                else // 1 den fazla eleman var ise ilgili elemani koleksiyondan cikart
                {
                    _heap[0] = _heap[_heap.Count - 1];
                    _heap.RemoveAt(_heap.Count - 1);
                }
                return sonuc;
            }
            else
                throw new InvalidOperationException(Uyari);
        }
        public KeyValuePair<TUrunsayisi, TMusteri> Dequeue2() //Method referansini degistirip ayni adimlari uygula
        {
            if (!Bosmu) 
            {
                FirstToLast2(0); 
                KeyValuePair<TUrunsayisi, TMusteri> sonuc = _heap[0];
                if (_heap.Count <= 1) 
                {
                    _heap.Clear();
                }
                else 
                {
                    _heap[0] = _heap[_heap.Count - 1];
                    _heap.RemoveAt(_heap.Count - 1);
                }
                return sonuc;
            }
            else
                throw new InvalidOperationException(Uyari);
        }


        //Peek ile varsayilan ilk elemani geriye dondurebiliriz. Elaman heapden silinmez 
        public KeyValuePair<TUrunsayisi, TMusteri> Peek()
        {
            if (!Bosmu)
                return _heap[0];
            else
                throw new InvalidOperationException(Uyari);
        }

        //Koleksiyonda eleman olup olmadiginin kontrolu icindir
        public bool Bosmu
        {
            get { return _heap.Count == 0; }
        }

        #endregion

        #region Sıralama Fonksiyonları

        private void LastToFirst(int pozisyon) //heapi basitce en asagidan yukariya dogru tarar ve yukari pozisyona en kucuk elemani getirmeye ugrasir.
        { //Bunu yaparken Daha kucuk olan bazi verileride yukari tasimis olacaktir.
            if (pozisyon >= _heap.Count)
                return;
            int YukariPos;
            while (pozisyon > 0)
            {
                YukariPos = (pozisyon - 1) / 2;
                if (_kiyasla.Compare(_heap[YukariPos].Key, _heap[pozisyon].Key) > 0) //Yukaripos ile pozisyonu karsilastirir.
                { //Yukaripos'da daha kucuk elemani tutmayi ister.
                    YerleriDegis(YukariPos, pozisyon);
                    pozisyon = YukariPos;
                }
                else break;
            }
        }
        private void LastToFirst2(int pozisyon) //LastToFirst un yaptigini yapmaya calisir ama en yukari en buyuk olani getirmeye calisir.
        {
            if (pozisyon >= _heap.Count)
                return;

            int YukariPos;

            while (pozisyon > 0)
            {
                YukariPos = (pozisyon - 1) / 2;
                if (_kiyasla.Compare(_heap[YukariPos].Key, _heap[pozisyon].Key) < 0) //Lattofirst ile arasindaki fark.
                {
                    YerleriDegis(YukariPos, pozisyon);
                    pozisyon = YukariPos;
                }
                else break;
            }
        }
        //Bu fonksiyonlarimizla heapi taratip buyuk kucuk kiyasi yapip en buyugu ya da en kucugu return etmek yerine. Var olan en kucugu ya da en buyugu en basa getirir
        // Boylece hem en buyugu ya da en kucugu secmis hem silme islemimiz hem de tekrar siralama islemimiz kolaylasir
        // ve kodda basit duzenlemelerle islem suresi kolayca kisaltilabilir. Ve bu sayede Devasa liste boyutlarinin ustesinden kolayca gelecek
        //Her sefherinde tekrar tekrar tum heapi taratip en buyuk ve ne kucugu return etme geregi olmayacaktir. Bu yuzden bu tur bir fonksiyon kullanimina yoneldim
        //ve varolan algoritmayi daha da iyilestirmeye calistim.

        private void FirstToLast(int pozisyon) //heapi yukaridan asagiya dogru tarar ve en kucuk veriyi 0. elemana getirtir. Bunu yaparken
        { //Mevcut eleman siralamasinda da iyilestirme yapar ve karsilastigi elemanlari da yukariya tasimaya calisir. Hem mevcut en kucugu secmemizi saglarken
            if (pozisyon >= _heap.Count) //hem de bir sonraki secimimiz icin heapi iyilestirecek ve islem suresini kisaltacaktir.
                return;
            while (true)
            {
                int kucukPozisyon = pozisyon;
                int solPozisyon = 2 * pozisyon + 1;
                int sagPozisyon = 2 * pozisyon + 2;
                if (solPozisyon < _heap.Count &&
                    _kiyasla.Compare(_heap[kucukPozisyon].Key, _heap[solPozisyon].Key) > 0) //kucuk pozisyon ile sag ve sol pozisyonlari karsilastiri
                    kucukPozisyon = solPozisyon;
                if (sagPozisyon < _heap.Count &&
                    _kiyasla.Compare(_heap[kucukPozisyon].Key, _heap[sagPozisyon].Key) > 0) //kucuk pozisyona daha kucuk olani getirmeye calisir.
                    kucukPozisyon = sagPozisyon;

                if (kucukPozisyon != pozisyon)
                {
                    YerleriDegis(kucukPozisyon, pozisyon);
                    pozisyon = kucukPozisyon;
                }
                else break;
            }
        }
        private void FirstToLast2(int pozisyon) // Firstolast in yaptigini daha buyuk olan icin yapmaya calisir.
        {
            if (pozisyon >= _heap.Count)
                return;
            while (true)
            {
                int buyukPozisyon = pozisyon;
                int solPozisyon = 2*pozisyon + 1;
                int sagPozisyon = 2*pozisyon+ 2;
                if (solPozisyon < _heap.Count &&
                    _kiyasla.Compare(_heap[buyukPozisyon].Key, _heap[solPozisyon].Key) < 0)
                    buyukPozisyon = solPozisyon;
                if (sagPozisyon < _heap.Count &&
                    _kiyasla.Compare(_heap[buyukPozisyon].Key, _heap[sagPozisyon].Key) < 0)
                    buyukPozisyon = sagPozisyon;

                if (buyukPozisyon != pozisyon)
                {
                    YerleriDegis(buyukPozisyon, pozisyon);
                    pozisyon = buyukPozisyon;

                }
                else break;
            }
        }

        private void YerleriDegis(int pozisyon1, int pozisyon2) //heap icerisinde gonderilen iki indexin yerini degistirir.
        {
            KeyValuePair<TUrunsayisi, TMusteri> val = _heap[pozisyon1];
            _heap[pozisyon1] = _heap[pozisyon2];
            _heap[pozisyon2] = val;
        }

        #endregion
    }
}
