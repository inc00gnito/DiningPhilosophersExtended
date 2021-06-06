using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
/// <summary>
/// Problem myślących filozofow z modyfikacjami.
/// Filozof wybiera losowo miejsce, następnie wybiera jedna z mozliwych ksiazek(ale taka której nie wybrał wcześniej)
/// Następnie idzie do stołu, gdzie czyta ksiązke, mysli oraz je
/// Po skonczeniu jedzenia odchodzi od stołu
/// </summary>
public class Philosopher
{
    private readonly int _num;
    public int LeftFork { get; set; }
    public int RightFork { get; set; }

    readonly Random rnd = new Random();
    private readonly Object[] _forks;
    private readonly Object[] _books;
    private readonly Object[] _seats;
    private readonly List<object> _readBooks;

    public Philosopher(int num, Object[] forks, Object[] books, Object[] seats)
    {
        _num = num;
        _forks = forks;
        _books = books;
        _seats = seats;
        //
        _readBooks = new List<object>();
    }

    public void Think()
    {
        Console.WriteLine($"Philosopher {_num} is thinking...");
        Console.WriteLine($"Philosopher {_num} is hungry!");
    }

    public void Eat()
    {
        Console.WriteLine($"Philosopher {_num} starts to eat...");
        Thread.Sleep(rnd.Next(500, 1000));
        Console.WriteLine($"Philosopher {_num} finished eating!");
    }

    public void ReadBook(int num)
    {
        Console.WriteLine($"Philosopher {_num} is reading a book {num}");
        Thread.Sleep(rnd.Next(500, 1000));
    }

    public void PutBackBook(int num)
    {
        Console.WriteLine($"Philosopher {_num} finished book {num}");
    }

    public void PickASeat(int pickedPlace)
    {
        Console.WriteLine($"Philosopher {_num} has picked place {pickedPlace}");
        Thread.Sleep(rnd.Next(500, 1000)); 
    }

    public void LeaveSeat(int pickedPlace)
    {
        Console.WriteLine($"Philosopher {_num} has left place {pickedPlace}");
    }

    public void StartPhilosophing()
    {
        int pickedPlace = 0;


        while (true)
        {
            bool lockTaken = false;
            // end when every book has been read
            if (_readBooks.Count == _books.Length)
                break;
            try
            {
                pickedPlace = rnd.Next(5);
                Monitor.TryEnter(_seats[pickedPlace], 5000, ref lockTaken);
                if (lockTaken)
                {
                    //critical section
                    LeftFork = pickedPlace;
                    RightFork = (pickedPlace + 1) % _forks.Length;
                    PickASeat(pickedPlace);
                    //every Philosopher must individually pick random book number to read now
                    //Only one philosopher can read specific book at time therefore we use Monitor Class to make sure no one else can read it too

                    var bookNumber = 0;
                    while (true)
                    {
                            // pick a book to read until u find one that has not been read
                            bookNumber = rnd.Next(0, 3);
                            if (!_readBooks.Contains(bookNumber))
                            {
                                _readBooks.Add(bookNumber);
                                break;
                            }
                    }
                    
                    Monitor.Enter(_books[bookNumber]);
                    ReadBook(bookNumber);

                    Think();
                    //picking a fork
                    if (_num < _forks.Length - 1)
                    {
                        Monitor.Enter(_forks[LeftFork]);
                        Monitor.Enter(_forks[RightFork]);
                    }
                    else
                    {
                        Monitor.Enter(_forks[RightFork]);
                        Monitor.Enter(_forks[LeftFork]);
                    }

                    Eat();
                    // release forks
                    Monitor.Exit(_forks[RightFork]);
                    Monitor.Exit(_forks[LeftFork]);

                    // release book 
                    PutBackBook(bookNumber);
                    Monitor.Exit(_books[bookNumber]);

                    LeaveSeat(pickedPlace);
                }
                else
                {
                    //keep picking a seat until u find one that is not taken
                    // i.e is not in Monitor.TryEnter
                    while (Monitor.IsEntered(pickedPlace))
                    {
                        pickedPlace = rnd.Next(5);
                    }
                }
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(_seats[pickedPlace]);
            }
        }
    }
}


public class Biesiada
{
    public static void Main()
    {
        int licz_fil = 5;
        Object[] forks = new Object[licz_fil];
        Object[] books = new Object[licz_fil];
        Object[] seats = new Object[licz_fil];
        for (int i = 0; i < 3; i++)
        {
            books[i] = new Object();
        }
        for (int i = 0; i < licz_fil; i++)
        {
            forks[i] = new Object();
            seats[i] = new Object();
        }

        Philosopher[] philosophers = new Philosopher[licz_fil];
        Thread[] watki = new Thread[licz_fil];
        for (int i = 0; i < licz_fil; i++)
        {
            philosophers[i] = new Philosopher(i, forks, books, seats);
            watki[i] = new Thread(philosophers[i].StartPhilosophing);
        }

        foreach (var watek in watki) watek.Start();
        foreach (var watek in watki) watek.Join();

        Console.WriteLine("\n\n End");
        Console.ReadKey();
    }
}