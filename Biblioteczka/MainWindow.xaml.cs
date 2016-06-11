using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace MyLibrary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Book> BookList { get; set; }
        public ObservableCollection<Audiobook> AudiobookList { get; set; }
        public ObservableCollection<Movie> MovieList { get; set; }

        int totalToRead = 0;
        int totalToListen = 0;
        int totalToWatch = 0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            BookList = new ObservableCollection<Book>();
            AudiobookList = new ObservableCollection<Audiobook>();
            MovieList = new ObservableCollection<Movie>();

            SetComboBox();
        }

        public void SetComboBox()
        {
            CategoryComboBox.ItemsSource = Enum.GetValues(typeof(Categories));
            CategoryComboBox.SelectedIndex = 0;

            TypeComboBox.ItemsSource = Enum.GetValues(typeof(Types));
            TypeComboBox.SelectedIndex = 0;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string author = AuthorTextBox.Text;
            string title = TitleTextBox.Text;
            string releaseDate, nISBN;

            Categories category = (Categories)Enum.Parse(typeof(Categories), CategoryComboBox.Text);
            Types type = (Types)Enum.Parse(typeof(Types), TypeComboBox.Text);

            int numberOfPages, length, numberOfActors;
            int selectedCategory = CategoryComboBox.SelectedIndex;
            
            switch (selectedCategory)
            {
                case 0:
                    try
                    {
                        nISBN = FirstAdditionalInformationInfoTextBox.Text;
                        numberOfPages = int.Parse(SecondAdditionalInformationInfoTextBox.Text);

                        Book book = new Book(title, author, category, type, nISBN, numberOfPages);
                        BookList.Add(book);

                        totalToRead = book.News(totalToRead, "add");
                    }
                    catch
                    {
                        MessageBox.Show("Ilość stron powinna być podana jako liczba", "Zła wartość");
                    }
                    break;
                case 1:
                    try
                    {
                        length = int.Parse(SecondAdditionalInformationInfoTextBox.Text);
                        numberOfActors = int.Parse(FirstAdditionalInformationInfoTextBox.Text);

                        Audiobook audiobook = new Audiobook(title, author, category, type, length, numberOfActors);
                        AudiobookList.Add(audiobook);

                        totalToListen = audiobook.News(totalToListen, "add");
                    }
                    catch
                    {
                        MessageBox.Show("Czas trwania oraz liczba aktorów powinny być podane jako liczby", "Zła wartość");
                    }
                    break;
                case 2:
                    try
                    {
                        releaseDate = FirstAdditionalInformationInfoTextBox.Text;
                        length = int.Parse(SecondAdditionalInformationInfoTextBox.Text);

                        Movie movie = new Movie(title, author, category, type, length, releaseDate);
                        MovieList.Add(movie);

                        totalToWatch = movie.News(totalToWatch, "add");
                    }
                    catch
                    {
                        MessageBox.Show("Czas trwania powinien być podany jako liczba", "Zła wartość");
                    }
                    break;
            }
            SetNewsTextBlocks(selectedCategory);
        }

        private void CategoryComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int selectedCategory = CategoryComboBox.SelectedIndex;
            switch (selectedCategory)
            {
                case 0:
                    FirstAdditionalInformationInfoTextBlock.Text = "Numer ISBN:";
                    SecondAdditionalInformationInfoTextBlock.Text = "Ilość stron:";
                    NewsTextBlock.Text = "Do przeczytania jest w sumie ";
                    break;
                case 1:
                    SecondAdditionalInformationInfoTextBlock.Text = "Czas trwania:";
                    FirstAdditionalInformationInfoTextBlock.Text = "Liczba aktorów:";
                    NewsTextBlock.Text = "Do wysłuchania jest w sumie ";
                    break;
                case 2:
                    SecondAdditionalInformationInfoTextBlock.Text = "Czas trwania:";
                    FirstAdditionalInformationInfoTextBlock.Text = "Data premiery:";
                    NewsTextBlock.Text = "Do obejrzenia jest w sumie ";
                    break;
            }
            SetNewsTextBlocks(selectedCategory);
        }

        private void DelateButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedCategory = CategoryComboBox.SelectedIndex;
            switch (selectedCategory)
            {
                case 0:
                    try
                    {
                        totalToRead = BookList[KsiazkaListView.SelectedIndex].News(totalToRead, "subtract");
                        BookList.RemoveAt(KsiazkaListView.SelectedIndex);
                    }
                    catch
                    {
                        MessageBox.Show("Zaznacz którą książkę chcesz usunąć lub zmień kategorię.", "Usuń pozycję");
                    }
                    break;
                case 1:
                    try
                    {
                        totalToListen = AudiobookList[AudiobookListView.SelectedIndex].News(totalToListen, "subtract");
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
                        totalToWatch = MovieList[FilmListView.SelectedIndex].News(totalToWatch, "subtract");
                        MovieList.RemoveAt(FilmListView.SelectedIndex);
                    }
                    catch
                    {
                        MessageBox.Show("Zaznacz który film chcesz usunąć lub zmień kategorię.", "Usuń pozycję");
                    }
                    break;
            }
            SetNewsTextBlocks(selectedCategory);
        }

        private void ReadTXTFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fileName = TXTfilePath.Text + TXTfileName.Text + ".txt";

                FileStream fileStream = new FileStream(fileName,
                FileMode.Open, FileAccess.Read);

                try
                {
                    StreamReader streamReader = new StreamReader(fileStream);

                    while (streamReader.EndOfStream == false)
                    {
                        string line = streamReader.ReadLine();
                        string[] table = line.Split('_');

                        if (table[0] == "Książka")
                        {
                            Categories category = (Categories)Enum.Parse(typeof(Categories), "Książka");
                            string title = table[1];
                            string author = table[2];
                            string nISBN = table[3];
                            int numberOfPages = int.Parse(table[4]);
                            Types type = (Types)Enum.Parse(typeof(Types), table[5]);
                            Book book = new Book(title, author, category, type, nISBN, numberOfPages);
                            BookList.Add(book);
                            totalToRead = book.News(totalToRead, "add");
                        }
                        else if (table[0] == "Audiobook")
                        {
                            Categories category = (Categories)Enum.Parse(typeof(Categories), "Audiobook");
                            string title = table[1];
                            string author = table[2];
                            int length = int.Parse(table[3]);
                            int numberOfActors = int.Parse(table[4]);
                            Types type = (Types)Enum.Parse(typeof(Types), table[5]);
                            Audiobook audiobook = new Audiobook(title, author, category, type, length, numberOfActors);
                            AudiobookList.Add(audiobook);
                            totalToListen = audiobook.News(totalToListen, "add");
                        }
                        else if (table[0] == "Film")
                        {
                            Categories category = (Categories)Enum.Parse(typeof(Categories), "Film");
                            string title = table[1];
                            string author = table[2];
                            int length = int.Parse(table[3]);
                            string releaseDate = table[4];
                            Types type = (Types)Enum.Parse(typeof(Types), table[5]);
                            Movie movie = new Movie(title, author, category, type, length, releaseDate);
                            MovieList.Add(movie);
                            totalToWatch = movie.News(totalToWatch, "add");
                        }
                        else MessageBox.Show("Wystąpił błąd podczas odczytywana danych z pliku");
                    }

                    streamReader.Close();

                    MessageBox.Show("Wczytano pomyslnie");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nie można wczytać pliku.");
                }
                int selectedCategory = CategoryComboBox.SelectedIndex;
                SetNewsTextBlocks(selectedCategory);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie można otworzyć pliku.");
            }
        }

        private void WriteToTXTFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fileName = TXTfilePath.Text + TXTfileName.Text + ".txt";

                FileStream fileStream = new FileStream(fileName,
                FileMode.CreateNew, FileAccess.ReadWrite);

                try
                {
                    StreamWriter streamWriter = new StreamWriter(fileStream);

                    for (int i = 0; i < BookList.Count; i++)
                    {
                        streamWriter.Write(BookList[i].Category + "_");
                        streamWriter.Write(BookList[i].Title + "_");
                        streamWriter.Write(BookList[i].Author + "_");
                        streamWriter.Write(BookList[i].ISBN + "_");
                        streamWriter.Write(BookList[i].NumberOfPages + "_");
                        streamWriter.WriteLine(BookList[i].Type + "_");
                    }

                    for (int i = 0; i < AudiobookList.Count; i++)
                    {
                        streamWriter.Write(AudiobookList[i].Category + "_");
                        streamWriter.Write(AudiobookList[i].Title + "_");
                        streamWriter.Write(AudiobookList[i].Author + "_");
                        streamWriter.Write(AudiobookList[i].Length + "_");
                        streamWriter.Write(AudiobookList[i].NumberOfActors + "_");
                        streamWriter.WriteLine(AudiobookList[i].Type + "_");
                    }

                    for (int i = 0; i < MovieList.Count; i++)
                    {
                        streamWriter.Write(MovieList[i].Category + "_");
                        streamWriter.Write(MovieList[i].Title + "_");
                        streamWriter.Write(MovieList[i].Author + "_");
                        streamWriter.Write(MovieList[i].Length + "_");
                        streamWriter.Write(MovieList[i].ReleaseDate + "_");
                        streamWriter.WriteLine(MovieList[i].Type + "_");
                    }
                    streamWriter.Close();

                    MessageBox.Show("Zapisano pomyślnie");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nie można zapisać do pliku.");
                }
                int selectedCategory = CategoryComboBox.SelectedIndex;
                SetNewsTextBlocks(selectedCategory);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Plik o podanej nazwie już istnieje");
            }
        }

        private void SortByTitleButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedCategory = CategoryComboBox.SelectedIndex;
            switch (selectedCategory)
            {
                case 0:
                    ICollectionView KsiazkaCollectionView = CollectionViewSource.GetDefaultView(BookList);
                    if (KsiazkaCollectionView.CanSort)
                    {
                        using (KsiazkaCollectionView.DeferRefresh())
                        {
                            KsiazkaCollectionView.SortDescriptions.Clear();
                            KsiazkaCollectionView.SortDescriptions.Add(new SortDescription("Title",
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
                            AudiobookCollectionView.SortDescriptions.Add(new SortDescription("Title",
                                ListSortDirection.Ascending));
                        }
                    }
                    break;
                case 2:
                    ICollectionView FilmCollectionView = CollectionViewSource.GetDefaultView(MovieList);
                    if (FilmCollectionView.CanSort)
                    {
                        using (FilmCollectionView.DeferRefresh())
                        {
                            FilmCollectionView.SortDescriptions.Clear();
                            FilmCollectionView.SortDescriptions.Add(new SortDescription("Title",
                                ListSortDirection.Ascending));
                        }
                    }
                    break;
            }
        }

        private void SortByAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedCategory = CategoryComboBox.SelectedIndex;
            switch (selectedCategory)
            {
                case 0:
                    ICollectionView KsiazkaCollectionView = CollectionViewSource.GetDefaultView(BookList);
                    if (KsiazkaCollectionView.CanSort)
                    {
                        using (KsiazkaCollectionView.DeferRefresh())
                        {
                            KsiazkaCollectionView.SortDescriptions.Clear();
                            KsiazkaCollectionView.SortDescriptions.Add(new SortDescription("Author",
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
                            AudiobookCollectionView.SortDescriptions.Add(new SortDescription("Author",
                                ListSortDirection.Ascending));
                        }
                    }
                    break;
                case 2:
                    ICollectionView FilmCollectionView = CollectionViewSource.GetDefaultView(MovieList);
                    if (FilmCollectionView.CanSort)
                    {
                        using (FilmCollectionView.DeferRefresh())
                        {
                            FilmCollectionView.SortDescriptions.Clear();
                            FilmCollectionView.SortDescriptions.Add(new SortDescription("Author",
                                ListSortDirection.Ascending));
                        }
                    }
                    break;
            }
        }

        private void ClearLibrary_Click(object sender, RoutedEventArgs e)
        {
            BookList.Clear();
            AudiobookList.Clear();
            MovieList.Clear();
            totalToRead = 0;
            totalToListen = 0;
            totalToWatch = 0;
            int selectedCategory = CategoryComboBox.SelectedIndex;
            SetNewsTextBlocks(selectedCategory);
        }

        private void ViewHelp_Click(object sender, RoutedEventArgs e)
        {
            string text1 = "Kilka przydatnych informacji o działaniu programu:",
                text2 = "\n1. Zapisywanie do pliku: \nAby zapisać plik należy podać pełną jego nazwę i lokalizację.\nPliki bez podanej ścieżki domyśnie zapisywane są wewnątrz projektu w folderze (...)/bin/Debug",
                text3 = "\n2. Wczytywany plik musi posiadać rozszerzenie .txt. Zapis również następuje do pliku tego typu. Członu .txt nie należy wpisywać w nazwie.",
                text4 = "\n3. Sortowanie dotyczy zaznaczonej kategorii. W celu usunięcia konkretnej pozycji również należy zaznaczyć odpowiednią dla niej kategorię. ";
                
            MessageBox.Show(text1 + text2 + text3 +text4, "Pomoc");
        }

        private void SetNewsTextBlocks(int selectedCategory)
        {
            switch (selectedCategory)
            {
                case 0:
                    NewsTextBlock2.Text = totalToRead + " stron";
                    break;
                case 1:
                    NewsTextBlock2.Text = totalToListen + " minut nagrań";
                    break;
                case 2:
                    NewsTextBlock2.Text = totalToWatch + " minut filmów";
                    break;
            }
        }
    }
}
