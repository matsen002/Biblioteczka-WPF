using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace Biblioteczka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Ksiazka> KsiazkaList { get; set; }
        public ObservableCollection<Audiobook> AudiobookList { get; set; }
        public ObservableCollection<Film> FilmList { get; set; }
        
        int wSumieDoPrzeczytania = 0;
        int wSumieDoWysluchania = 0;
        int wSumieDoObejrzenia = 0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            KsiazkaList = new ObservableCollection<Ksiazka>();
            AudiobookList = new ObservableCollection<Audiobook>();
            FilmList = new ObservableCollection<Film>();

            ComboBoxy();
        }
        
        public void ComboBoxy()     //Ustawienie domyslnych wartości w ComboBoxach
        {
            KategoriaComboBox.ItemsSource = Enum.GetValues(typeof(Kategorie));
            KategoriaComboBox.SelectedIndex = 0;

            GatunekComboBox.ItemsSource = Enum.GetValues(typeof(Gatunki));
            GatunekComboBox.SelectedIndex = 0;

            NazwaPlikuTXT.Text = "Podaj nazwę pliku";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)      
        {
            string autor = AutorTextBox.Text;
            string tytul = TytulTextBox.Text;

            int iloscStron, czasTrwania, liczbaAktorow;
            string dataPremiery, numerISBN;

            Kategorie kategoria = (Kategorie)Enum.Parse(typeof(Kategorie), KategoriaComboBox.Text);
            Gatunki gatunek = (Gatunki)Enum.Parse(typeof(Gatunki), GatunekComboBox.Text);

            int wybranaKategoria = KategoriaComboBox.SelectedIndex;
            switch (wybranaKategoria)
            {
                case 0:
                    try {
                        numerISBN = PierwszeDodatkoweInfoTextBox.Text;
                        iloscStron = int.Parse(DrugieDodatkoweInfoTextBox.Text);

                        Ksiazka ksiazka = new Ksiazka(tytul, autor, kategoria, gatunek, numerISBN, iloscStron);
                        KsiazkaList.Add(ksiazka);
                        
                        wSumieDoPrzeczytania = ksiazka.Ciekawostka(wSumieDoPrzeczytania, 1);
                        CiekawostkaTextBlock2.Text = wSumieDoPrzeczytania + " stron";
                    }
                    catch
                    {
                        MessageBox.Show("Ilość stron powinna być podana jako liczba", "Zła wartość");
                    }
                    break;
                case 1:
                    try {
                        czasTrwania = int.Parse(DrugieDodatkoweInfoTextBox.Text);
                        liczbaAktorow = int.Parse(PierwszeDodatkoweInfoTextBox.Text);

                        Audiobook audiobook = new Audiobook(tytul, autor, kategoria, gatunek, czasTrwania, liczbaAktorow);
                        AudiobookList.Add(audiobook);

                        wSumieDoWysluchania = audiobook.Ciekawostka(wSumieDoWysluchania, 1);
                        CiekawostkaTextBlock2.Text = wSumieDoWysluchania + " minut nagrań";
                    }
                    catch
                    {
                        MessageBox.Show("Czas trwania oraz liczba aktorów powinny być podane jako liczby", "Zła wartość");
                    }
                    break;
                case 2:
                    try
                    {
                        dataPremiery = PierwszeDodatkoweInfoTextBox.Text;
                        czasTrwania = int.Parse(DrugieDodatkoweInfoTextBox.Text);

                        Film film = new Film(tytul, autor, kategoria, gatunek, czasTrwania, dataPremiery);
                        FilmList.Add(film);
                        
                        wSumieDoObejrzenia = film.Ciekawostka(wSumieDoObejrzenia, 1);
                        CiekawostkaTextBlock2.Text = wSumieDoObejrzenia + " minut filmów";
                    }
                    catch
                    {
                        MessageBox.Show("Czas trwania powinien być podany jako liczba", "Zła wartość");
                    }
                    break;
            }
        }

        private void KategoriaComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)     //Wybór dodatkowych informacji o pozycjach w zależności od wybranej kategorii
        {
            int wybranaKategoria = KategoriaComboBox.SelectedIndex;
            switch (wybranaKategoria)
            {
                case 0:
                    PierwszeDodatkoweInfoTextBlock.Text = "Numer ISBN:";
                    DrugieDodatkoweInfoTextBlock.Text = "Ilość stron:";
                    CiekawostkaTextBlock.Text = "Do przeczytania jest w sumie ";
                    CiekawostkaTextBlock2.Text = wSumieDoPrzeczytania + " stron";
                    break;
                case 1:
                    DrugieDodatkoweInfoTextBlock.Text = "Czas trwania:";
                    PierwszeDodatkoweInfoTextBlock.Text = "Liczba aktorów:";
                    CiekawostkaTextBlock.Text = "Do wysłuchania jest w sumie ";
                    CiekawostkaTextBlock2.Text = wSumieDoWysluchania + " minut nagrań";
                    break;
                case 2:
                    DrugieDodatkoweInfoTextBlock.Text = "Czas trwania:";
                    PierwszeDodatkoweInfoTextBlock.Text = "Data premiery:";
                    CiekawostkaTextBlock.Text = "Do obejrzenia jest w sumie ";
                    CiekawostkaTextBlock2.Text = wSumieDoObejrzenia + " minut filmów";
                    break;
            }
        }

        private void DelateButton_Click(object sender, RoutedEventArgs e)   //Usuwanie zaznaczonej pozycji w wybranej kategorii
        {
            int wybranaKategoria = KategoriaComboBox.SelectedIndex;
            switch (wybranaKategoria)
            {
                case 0:
                    try
                    {
                        wSumieDoPrzeczytania = KsiazkaList[KsiazkaListView.SelectedIndex].Ciekawostka(wSumieDoPrzeczytania, 0);
                        CiekawostkaTextBlock2.Text = wSumieDoPrzeczytania + " stron";
                        KsiazkaList.RemoveAt(KsiazkaListView.SelectedIndex);
                    }
                    catch
                    {
                        MessageBox.Show("Zaznacz którą książkę chcesz usunąć lub zmień kategorię.", "Usuń pozycję");
                    }
                    break;
                case 1:
                    try
                    {
                        wSumieDoWysluchania = AudiobookList[AudiobookListView.SelectedIndex].Ciekawostka(wSumieDoWysluchania, 0);
                        CiekawostkaTextBlock2.Text = wSumieDoWysluchania + " minut nagrań";
                        AudiobookList.RemoveAt(AudiobookListView.SelectedIndex);
                    }
                    catch
                    {
                        MessageBox.Show("Zaznacz którego audiobooka chcesz usunąć lub zmień kategorię.", "Usuń pozycję");
                    }
                    break;
                case 2:
                    try
                    {
                        wSumieDoObejrzenia = FilmList[FilmListView.SelectedIndex].Ciekawostka(wSumieDoObejrzenia, 0);
                        CiekawostkaTextBlock2.Text = wSumieDoObejrzenia + " minut filmów";
                        FilmList.RemoveAt(FilmListView.SelectedIndex);
                    }
                    catch
                    {
                        MessageBox.Show("Zaznacz który film chcesz usunąć lub zmień kategorię.", "Usuń pozycję");
                    }
                    break;
            }
        }
            
        private void WczytajPlikTXT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nazwaPliku = "D:\\" + NazwaPlikuTXT.Text;

                FileStream fs = new FileStream(nazwaPliku,
                FileMode.Open, FileAccess.Read);

                try
                {
                    StreamReader sr = new StreamReader(fs);
                    //string[] linie = File.ReadAllLines(nazwaPliku);

                    while (sr.EndOfStream == false)          //Wykonuj dopoki odczyt nie dojdzie do konca pliku
                    {
                        string linia = sr.ReadLine();
                        string[] tablica = linia.Split(' ');

                        if (tablica[0] == "Książka")
                        {
                            Kategorie kategoria = (Kategorie)Enum.Parse(typeof(Kategorie), "Książka");
                            string tytul = tablica[1];
                            string autor = tablica[2];
                            string numerISBN = tablica[3];
                            int iloscStron = int.Parse(tablica[4]);
                            Gatunki gatunek = (Gatunki)Enum.Parse(typeof(Gatunki), tablica[5]);
                            Ksiazka ksiazka = new Ksiazka(tytul, autor, kategoria, gatunek, numerISBN, iloscStron);
                            KsiazkaList.Add(ksiazka);
                            wSumieDoPrzeczytania = ksiazka.Ciekawostka(wSumieDoPrzeczytania, 1);
                        }
                        else if (tablica[0] == "Audiobook")
                        {
                            Kategorie kategoria = (Kategorie)Enum.Parse(typeof(Kategorie), "Audiobook");
                            string tytul = tablica[1];
                            string autor = tablica[2];
                            int czasTrwania = int.Parse(tablica[3]);
                            int liczbaAktorow = int.Parse(tablica[4]);
                            Gatunki gatunek = (Gatunki)Enum.Parse(typeof(Gatunki), tablica[5]);
                            Audiobook audiobook = new Audiobook(tytul, autor, kategoria, gatunek, czasTrwania, liczbaAktorow);
                            AudiobookList.Add(audiobook);
                            wSumieDoWysluchania = audiobook.Ciekawostka(wSumieDoWysluchania, 1);
                        }
                        else if (tablica[0] == "Film")
                        {
                            Kategorie kategoria = (Kategorie)Enum.Parse(typeof(Kategorie), "Film");
                            string tytul = tablica[1];
                            string autor = tablica[2];
                            int czasTrwania = int.Parse(tablica[3]);
                            string dataPremiery = tablica[4];
                            Gatunki gatunek = (Gatunki)Enum.Parse(typeof(Gatunki), tablica[5]);
                            Film film = new Film(tytul, autor, kategoria, gatunek, czasTrwania, dataPremiery);
                            FilmList.Add(film);
                            wSumieDoObejrzenia = film.Ciekawostka(wSumieDoObejrzenia, 1);
                        }
                        else MessageBox.Show("Coś jest nie tak");
                    }

                    sr.Close();

                    MessageBox.Show("Wczytano pomyslnie");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ZapiszPlikTXT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nazwaPliku = "D:\\" + NazwaPlikuTXT.Text;

                FileStream fs = new FileStream(nazwaPliku,
                FileMode.CreateNew, FileAccess.ReadWrite);

                try
                {
                    StreamWriter sw = new StreamWriter(fs);

                    for (int i = 0; i < KsiazkaList.Count; i++)
                    {
                        sw.Write(KsiazkaList[i].Kategoria + " ");
                        sw.Write(KsiazkaList[i].Tytul + " ");
                        sw.Write(KsiazkaList[i].Autor + " ");
                        sw.Write(KsiazkaList[i].NumerISBN + " ");
                        sw.Write(KsiazkaList[i].IloscStron + " ");
                        sw.WriteLine(KsiazkaList[i].Gatunek + " ");
                    }

                    for (int i = 0; i < AudiobookList.Count; i++)
                    {
                        sw.Write(AudiobookList[i].Kategoria + " ");
                        sw.Write(AudiobookList[i].Tytul + " ");
                        sw.Write(AudiobookList[i].Autor + " ");
                        sw.Write(AudiobookList[i].CzasTrwania + " ");
                        sw.Write(AudiobookList[i].LiczbaAktorow + " ");
                        sw.WriteLine(AudiobookList[i].Gatunek + " ");
                    }

                    for (int i = 0; i < FilmList.Count; i++)
                    {
                        sw.Write(FilmList[i].Kategoria + " ");
                        sw.Write(FilmList[i].Tytul + " ");
                        sw.Write(FilmList[i].Autor + " ");
                        sw.Write(FilmList[i].CzasTrwania + " ");
                        sw.Write(FilmList[i].DataPremiery + " ");
                        sw.WriteLine(FilmList[i].Gatunek + " ");
                    }
                    sw.Close();

                    MessageBox.Show("Zapisano pomyslnie");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SortowanieWgTytuluButton_Click(object sender, RoutedEventArgs e)
        {
            int wybranaKategoria = KategoriaComboBox.SelectedIndex;
            switch (wybranaKategoria)
            {
                case 0:
                    ICollectionView KsiazkaCollectionView = CollectionViewSource.GetDefaultView(KsiazkaList); //Pobranie domyślnego widoku kolekcji
                    if (KsiazkaCollectionView.CanSort)
                    {
                        using (KsiazkaCollectionView.DeferRefresh())
                        {
                            KsiazkaCollectionView.SortDescriptions.Clear();
                            KsiazkaCollectionView.SortDescriptions.Add(new SortDescription("Tytul",
                                ListSortDirection.Ascending));
                        }
                    }
                    break;
                case 1:
                    ICollectionView AudiobookCollectionView = CollectionViewSource.GetDefaultView(AudiobookList);
                    if (AudiobookCollectionView.CanSort)
                    {
                        using (AudiobookCollectionView.DeferRefresh())
                        {
                            AudiobookCollectionView.SortDescriptions.Clear();
                            AudiobookCollectionView.SortDescriptions.Add(new SortDescription("Tytul",
                                ListSortDirection.Ascending));
                        }
                    }
                    break;
                case 2:
                    ICollectionView FilmCollectionView = CollectionViewSource.GetDefaultView(FilmList);
                    if (FilmCollectionView.CanSort)
                    {
                        using (FilmCollectionView.DeferRefresh())
                        {
                            FilmCollectionView.SortDescriptions.Clear();
                            FilmCollectionView.SortDescriptions.Add(new SortDescription("Tytul",
                                ListSortDirection.Ascending));
                        }
                    }
                    break;
            }
        }

        private void SortowanieWgAutoraButton_Click(object sender, RoutedEventArgs e)
        {
            int wybranaKategoria = KategoriaComboBox.SelectedIndex;
            switch (wybranaKategoria)
            {
                case 0:
                    ICollectionView KsiazkaCollectionView = CollectionViewSource.GetDefaultView(KsiazkaList);
                    if (KsiazkaCollectionView.CanSort)
                    {
                        using (KsiazkaCollectionView.DeferRefresh())
                        {
                            KsiazkaCollectionView.SortDescriptions.Clear();
                            KsiazkaCollectionView.SortDescriptions.Add(new SortDescription("Autor",
                                ListSortDirection.Ascending));
                        }
                    }
                    break;
                case 1:
                    ICollectionView AudiobookCollectionView = CollectionViewSource.GetDefaultView(AudiobookList);
                    if (AudiobookCollectionView.CanSort)
                    {
                        using (AudiobookCollectionView.DeferRefresh())
                        {
                            AudiobookCollectionView.SortDescriptions.Clear();
                            AudiobookCollectionView.SortDescriptions.Add(new SortDescription("Autor",
                                ListSortDirection.Ascending));
                        }
                    }
                    break;
                case 2:
                    ICollectionView FilmCollectionView = CollectionViewSource.GetDefaultView(FilmList);
                    if (FilmCollectionView.CanSort)
                    {
                        using (FilmCollectionView.DeferRefresh())
                        {
                            FilmCollectionView.SortDescriptions.Clear();
                            FilmCollectionView.SortDescriptions.Add(new SortDescription("Autor",
                                ListSortDirection.Ascending));
                        }
                    }
                    break;
            }
        }

        private void CzyszczenieBiblioteki_Click(object sender, RoutedEventArgs e)
        {
            KsiazkaList.Clear();
            AudiobookList.Clear();
            FilmList.Clear();
            wSumieDoPrzeczytania = 0;
            wSumieDoWysluchania = 0;
            wSumieDoObejrzenia = 0;
        }
    }
}
