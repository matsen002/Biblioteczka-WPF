using System;
using System.Collections.Generic;

namespace MyLibrary
{
    public enum Categories { Książka, Audiobook, Film}
    public enum Types { Fantasy, ScienceFiction, Powieść, Horror}
    
    interface ViewNews
    {
        int News(int a, int b);
    }

    public abstract class ElementOfLibrary : ViewNews
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public Categories Category { get; set; }
        public Types Type { get; set; }

        public ElementOfLibrary(string title, string author, Categories category, Types type)
        {
            Title = title;
            Author = author;
            Category = category;
            Type = type;
        }

        public virtual int News(int presentValue, int addOrSubtract)
        {
            return 0;
        }
    }

    public class Book : ElementOfLibrary
    {
        public string ISBN { get; set; }
        public int NumberOfPages { get; set; }

        public Book(string title, string author, Categories category, Types type, string nISBN, int numberOfPages) 
            : base (title, author, category, type)
        {
            ISBN = nISBN;
            NumberOfPages = numberOfPages;
        }

        public override int News(int totalToRead, int addOrSubtract)
        {
            if (addOrSubtract == 1)       // 1 to dodawanie, 0 to odejmowanie
            {
                totalToRead += NumberOfPages;
            }
            else
            {
                totalToRead -= NumberOfPages;
            }
            return totalToRead;
        }
    }

    public class Audiobook : ElementOfLibrary
    {
        public int Length { get; set; }
        public int NumberOfActors { get; set; }

        public Audiobook(string title, string author, Categories category, Types type, int length, int numberOfActors)
            : base(title, author, category, type)
        {
            Length = length;
            NumberOfActors = numberOfActors;
        }

        public override int News(int totalToListen, int addOrSubtract)
        {
            if (addOrSubtract == 1)
            {
                totalToListen += Length;
            }
            else
            {
                totalToListen -= Length;
            }
            return totalToListen;
        }
    }

    public class Movie : ElementOfLibrary
    {
        public int Length { get; set; }
        public string ReleaseDate { get; set; }

        public Movie(string title, string author, Categories category, Types type, int length, string releaseDate)
            : base(title, author, category, type)
        {
            Length = length;
            ReleaseDate = releaseDate;
        }

        public override int News(int totalToWatch, int addOrSubtract)
        {
            if (addOrSubtract == 1)
            {
                totalToWatch += Length;
            }
            else
            {
                totalToWatch -= Length;
            }
            return totalToWatch;
        }
    }
}
