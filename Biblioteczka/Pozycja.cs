using System;
using System.Collections.Generic;

namespace Biblioteczka
{
    public enum Kategorie { Książka, Audiobook, Film}
    public enum Gatunki { Fantastyka, ScienceFiction, Młodzieżowa, Horror}
    
    interface KoniecznieWsywietlCiekawostke
    {
        int Ciekawostka(int a, int b);
    }

    public abstract class Pozycja : KoniecznieWsywietlCiekawostke
    {
        public string Tytul { get; set; }
        public string Autor { get; set; }
        public Kategorie Kategoria { get; set; }
        public Gatunki Gatunek { get; set; }

        public Pozycja(string tytul, string autor, Kategorie kategoria, Gatunki gatunek)
        {
            Tytul = tytul;
            Autor = autor;
            Kategoria = kategoria;
            Gatunek = gatunek;
        }

        public virtual int Ciekawostka(int biezacaWartosc, int dodawacCzyOdejmowac)
        {
            return 0;
        }
    }

    public class Ksiazka : Pozycja
    {
        public string NumerISBN { get; set; }
        public int IloscStron { get; set; }

        public Ksiazka(string tytul, string autor, Kategorie kategoria, Gatunki gatunek, string numerISBN, int iloscStron) 
            : base (tytul, autor, kategoria, gatunek)
        {
            NumerISBN = numerISBN;
            IloscStron = iloscStron;
        }

        public override int Ciekawostka(int wSumieDoPrzeczytania, int dodawacCzyOdejmowac)
        {
            if (dodawacCzyOdejmowac == 1)       // 1 to dodawanie, 0 to odejmowanie
            {
                wSumieDoPrzeczytania += IloscStron;
            }
            else
            {
                wSumieDoPrzeczytania -= IloscStron;
            }
            return wSumieDoPrzeczytania;
        }
    }

    public class Audiobook : Pozycja
    {
        public int CzasTrwania { get; set; }
        public int LiczbaAktorow { get; set; }

        public Audiobook(string tytul, string autor, Kategorie kategoria, Gatunki gatunek, int czasTrwania, int liczbaAktorow)
            : base(tytul, autor, kategoria, gatunek)
        {
            CzasTrwania = czasTrwania;
            LiczbaAktorow = liczbaAktorow;
        }

        public override int Ciekawostka(int wSumieDoWysluchania, int dodawacCzyOdejmowac)
        {
            if (dodawacCzyOdejmowac == 1)
            {
                wSumieDoWysluchania += CzasTrwania;
            }
            else
            {
                wSumieDoWysluchania -= CzasTrwania;
            }
            return wSumieDoWysluchania;
        }
    }

    public class Film : Pozycja
    {
        public int CzasTrwania { get; set; }
        public string DataPremiery { get; set; }

        public Film(string tytul, string autor, Kategorie kategoria, Gatunki gatunek, int czasTrwania, string dataPremiery)
            : base(tytul, autor, kategoria, gatunek)
        {
            CzasTrwania = czasTrwania;
            DataPremiery = dataPremiery;
        }

        public override int Ciekawostka(int wSumieDoObejrzenia, int dodawacCzyOdejmowac)
        {
            if (dodawacCzyOdejmowac == 1)
            {
                wSumieDoObejrzenia += CzasTrwania;
            }
            else
            {
                wSumieDoObejrzenia -= CzasTrwania;
            }
            return wSumieDoObejrzenia;
        }
    }
}
