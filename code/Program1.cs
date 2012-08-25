using System;
using System.IO;
namespace ahmet
{
    class Stack
    {
        public string[] yigitDizisi;
        public int indis;
        public int boyut;

        public Stack(int uzunluk)
        {
            boyut = uzunluk;
            yigitDizisi = new string[boyut];
            indis = -1;
        }
        public int push(string eleman)
        {
            if (indis == (boyut - 1))
                return -1;
            yigitDizisi[++indis] = eleman;
            return indis;
        }

        public string pop()
        {
            if (indis == -1)
            {
                return null;
            }
            return yigitDizisi[indis--];
        }

        public bool isEmpty()
        {
            return indis == -1;
        }

        public string peek()
        {
            if (indis == -1)
            {
                return null;
            }
            return yigitDizisi[indis];
        }
    }



    abstract class Gercekleme
    {

        protected static string hataliMİ(string metin)
        {
            Stack s = new Stack(1000);
            bool hatasiz = true;
            int i = 0;

            while ((i < metin.Length) && (hatasiz))
            {
                if ((metin[i] == '<') && (metin[i + 1] == '/'))
                { //kapama durumu geldiğinde yığıttan cek

                    //her kapama etiketi için yığıtta acma durumunu ifade eden bicimlendirme etiketi
                    // var mı.yani yığıt bossa dengesizdir
                    if (s.isEmpty())
                    {
                        hatasiz = false;
                    }
                    //her acma etiketi kendi etiketiyle  sınanır.esit değilse dengesizdir
                    else
                    {
                        string top = s.pop();
                        if (metin[i + 2] == top[0])
                        { hatasiz = true; }
                        else
                        {
                            hatasiz = false;
                        }
                    }
                }
                //acma durumu geldiğinde acma bicimlendirme etiketini yığıta it
                else
                {
                    if (metin[i] == '<')
                    {
                        s.push(metin[i + 1].ToString());
                    }
                }
                i = i + 1;
            }
            //yığıt boskenken metin taramasının sonuna gelindiğinde  ve 
            //dengeli true ise hatalidir değilse hatasizdir
            if ((s.isEmpty()) && (hatasiz))
            {
                return "hatasiz";
            }
            else
            {
                return "hatali";
            }
        }

        protected static string Oku(string DosyaAdresi)
        {
            string okuma = "";
            // StreamReader sinifi nesnesi yarat
            StreamReader referans = new StreamReader(DosyaAdresi);
            // nesneye mesaj gondererek ReadToEnd fonk. cagir
            okuma = referans.ReadToEnd();
            // okunan dosyayi serbest birakmak icin gerekli
            referans.Close();
            return okuma;
        }


        protected static string bicimlendir(string metin)
        {
            Stack s = new Stack(1000);
            string konrol;
            string bicimli_gecici;
            string bicimli = "";
            int j = 0;
            while (j < metin.Length)
            {
                if ((metin[j]) == '<' && (metin[j + 1] == '/'))
                {

                    // kapama durumu geldiyse acma durumunu ifade eden '>' isaretne
                    // kadar yığıttan cek gecici stringine at(insert metodunu kullanarak)
                    konrol = s.pop();
                    string gecici = "";
                    while ((konrol[0]) != ('>'))
                    {
                        gecici = gecici.Insert(0, konrol[0].ToString());
                        konrol = s.pop();
                    }



                    // islemisec metoduna gerekli  paretmereler olan metinin indis değerini 
                    // saklayan j değiskeni ,metni gösteren metin stringi ve yığıttan cekilen elemanları 
                    //gösteren gecici stringini gönder.dönen bicimlendirilmis gecici stringini
                    //biçimlenmis_gecici ye ata
                    bicimli_gecici = islemiSec(j, metin, gecici);
                    //biciimli_gecici stringinin uzunluğundan hücük olduğu sürece döngüde bicimli_gecicinin 
                    //elemanlarını düzgün sırayla yığıta it
                    for (int i = 0; i < bicimli_gecici.Length; i++)
                    {
                        s.push(bicimli_gecici[i].ToString());
                    }

                    //4 artırki kapama durumunda ki isarteleri atlasın(<,/,> gibi )
                    j = j + 4;
                }

                // kapama durumu değilse buraya gir
                else
                {
                    // ve acma durumu geldiğinde bicimlendirme karekterinin nerde 
                    //olduğunu bilmemiz için '>' isaretini yığıta it. istediğimizi
                    //yığıta itebiliriz('<') 3 artır ki acma durumunda ki işaretlerin 
                    //hepsini atlasın

                    if ((metin[j] == '<') && (metin[j + 2] == '>'))
                    {
                        s.push(metin[j + 2].ToString());
                        j = j + 3;
                    }
                    //acma durumuda değilse normal karekterdir.onu yığıta it
                    else
                    {
                        s.push(metin[j].ToString());
                        j = j + 1;
                    }


                }

            }
            //her yeni cektiğimizi en basa koyacak sekilde ata.ve bicimli yi dön.
            int indeks = 0;
            while (s.isEmpty() == false)
            {
                bicimli = bicimli.Insert(indeks, s.pop());

            }
            return bicimli;
        }


        static string buyult(string ek)
        {
            return ek.ToUpper();
        }


        static string tekrarlıYaz(string ek)
        {
            int z = 0;
            string ikili = "";
            for (int i = 0; i < ek.Length; i++)
            {
                for (int j = 0; j < 2; j++)
                { ikili = ikili.Insert(z++, ek[i].ToString()); }

            }
            return ikili;
        }
        // ek stringinin elemanlarını sil.
        static string gizle(string ek)
        {
            return ek.Remove(0);
        }
        //stringin basına ve sonuna koseli parantez ekle
        static string koseliEkle(string ek)
        {
            return '[' + ek + ']';
        }

        //metin[indis+2] nin yani bicimlendirme etiketinin değeri hangi kosula esitse 
        //o kosula gir.ordaki fonksyonu cağır dönen stringi dizgi stringine ata
        static string islemiSec(int indis, string metin, string dizgi)
        {
            if ((metin[indis + 2]) == 'u')
            { dizgi = buyult(dizgi); }
            if ((metin[indis + 2]) == 'b')
            { dizgi = tekrarlıYaz(dizgi); }
            if ((metin[indis + 2]) == 'h')
            { dizgi = gizle(dizgi); }
            if ((metin[indis + 2]) == 'p')
            { dizgi = koseliEkle(dizgi); }
            return dizgi;


        }




    }
    class Calistirma : Gercekleme
    {
        static void Main(string[] args)
        {
            //oku metodunu cağir.parametre olarak 
            //okunacak dosyanın adresini gönderdik.dönen sonucu da met stringine atadık

            string met = Oku("C:/kaynak.txt");

            Console.WriteLine(hataliMİ(met));
            //eğer konrol metodundan dönen değer "hatali" ya esitse asağıdaki uyarıyı yazdır
            if (hataliMİ(met) == "hatali")
            {
                Console.WriteLine("Kaynak dosyanin bicimlendirme etiketleri hatalidir ,kontrol ediniz.");
            }
            // esit değilse hatasizdir.bicimlendir medonu cağır.dönen bicimlenmis metni yazdir 
            else
            {
                Console.Write(bicimlendir(met));
            }

            Console.ReadLine();
        }
    }
}