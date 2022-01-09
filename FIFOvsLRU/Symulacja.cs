using FIFOalpha;
using System;

/*
 * nazwa: FIFOvsLRU - porównanie dwóch algorytmów wymiany stron na 100 zbiorach po 100 stron
 * autor: Filip Dowhan 259683
 * wersja: 1.0 - 09.01.2022
 */

class Symulacja
{
    Queue<Strona> ramkaFIFO = new();
    List<Strona> ramkaLRU = new();
    Dictionary<int, int> indeksy = new Dictionary<int, int>();

    List<Strona> strony = new();

    /* FUNKCJA SŁUŻĄCA DO WYGENEROWANIA PLIKÓW Z DANYMI PROCESÓW (unused,devtool)
     * 
    private static async Task generujPlikiDanych()
    {
        Random random = new Random();

        for (int i = 1; i < 101; i++)
        {
            using StreamWriter file = new(i + ".txt", append: true);
            for (int j = 0; j < 100; j++)
            {
                await file.WriteLineAsync(random.Next(1, 21)+" ");
            }

        }
    }
     */

    private void dodajStrony(int x)
    {
        strony.Clear();

        string nazwaPliku = "DANE\\"+x+".txt";
        string[] plik = File.ReadAllLines(nazwaPliku);

        foreach (string linia in plik)
        {
            int nr = int.Parse(linia);
            strony.Add(new Strona(nr));
        }
    }

    private void FIFO(int rozmiar, int iter)
    {
        int PF = 0;
        bool check;

        ramkaFIFO.Clear();

        foreach (Strona strona in strony)
        {
            check = false;

            foreach (Strona stronaRamka in ramkaFIFO)
            {
                if (strona.Numer == stronaRamka.Numer)
                {
                    check = true;
                }
            }

            if (check)
            {
                continue;
            }
            else
            {
                if (rozmiar > ramkaFIFO.Count)
                {
                    ramkaFIFO.Enqueue(strona);
                    PF++;
                }
                else
                {
                    ramkaFIFO.Dequeue();
                    ramkaFIFO.Enqueue(strona);
                    PF++;
                }
            }
        }
        Console.WriteLine("[FIFO][" + iter + "][rozmiar ramki: " + rozmiar + "] : " + PF);
    }

    private void LRU(int rozmiar, int iter) //FINALLY WORKING !!!
    {
        int PF = 0;
        bool check;


        ramkaLRU.Clear();

        for (int i = 0; i < strony.Count; i++)
        {
            check = false;

            foreach (Strona stronaRamka in ramkaLRU)
            {
                if (strony[i].Numer == stronaRamka.Numer)
                {
                    check = true;
                }
            }

            if (check)
            {
                if (indeksy.ContainsKey(strony[i].Numer))
                {
                    indeksy[strony[i].Numer] = i;
                }
                continue;
            }
            else
            {
                int lru, war = 0;
                if (rozmiar > ramkaLRU.Count)
                {
                    ramkaLRU.Add(strony[i]);
                    PF++;
                    if (indeksy.ContainsKey(strony[i].Numer))
                    {
                        indeksy[strony[i].Numer] = i;
                    }
                    else
                    {
                        indeksy.Add(strony[i].Numer, i);
                    }
                }
                else
                {
                    lru = int.MaxValue;

                    foreach (Strona stronaRamka2 in ramkaLRU)
                    {
                        int temp = stronaRamka2.Numer;

                        if (indeksy[temp] < lru)
                        {
                            lru = indeksy[temp];
                            war = temp;
                        }
                    }

                    foreach (Strona stronaRamka2 in ramkaLRU)
                    {
                        if (war == stronaRamka2.Numer)
                        {
                            ramkaLRU.Remove(stronaRamka2);
                            indeksy.Remove(stronaRamka2.Numer);
                            ramkaLRU.Add(strony[i]);
                            PF++;
                            break;
                        }
                    }

                    if (indeksy.ContainsKey(strony[i].Numer))
                    {
                        indeksy[strony[i].Numer] = i;
                    }
                    else
                    {
                        indeksy.Add(strony[i].Numer, i);
                    }
                }
            }
        }
        Console.WriteLine("[LRU][" + iter + "][rozmiar ramki: " + rozmiar + "] : " + PF);
    }

    // Main
    static void Main(string[] args)
    {
        Symulacja symulacja = new Symulacja();

        int rozmiarRamki;
        for (int i = 1; i < 101; i++)
        {
            rozmiarRamki = 3;
            symulacja.dodajStrony(i);

            while (rozmiarRamki != 9)
            {
                symulacja.FIFO(rozmiarRamki, i);
                symulacja.LRU(rozmiarRamki, i);
                rozmiarRamki += 2;
            }
            Console.WriteLine("\n");
        }
    }
}