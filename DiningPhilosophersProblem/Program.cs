using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class Philosopher
{
    private int _num, _leftFork, _rightFork;
    Random rnd = new Random();
    private Object[] _forks;
    private Object[] _books;
    private List<object> _readBooks;


    private readonly object _locker = new object();

    //
    List<int> pickedBookNumbers = new List<int>();
    private List<int> availableBookNumbers = new List<int> {0, 1};

    public Philosopher(int num, Object[] forks, Object[] books)
    {
        _num = num;
        _forks = forks;
        _books = books;
        //
        _leftFork = num;
        _rightFork = (num + 1) % _forks.Length;
        _readBooks = new List<object>();
    }

    public void Think()
    {
        Console.WriteLine($"Philosopher {_num} myśli...");
        Console.WriteLine($"Philosopher {_num} zgłodniał!");
    }

    public void Eat()
    {
        Console.WriteLine($"Philosopher {_num} zaczyna jeść...");
        Thread.Sleep(rnd.Next(500, 1000));
        Console.WriteLine($"Philosopher {_num} skończył jeść!");
    }

    public void ReadBook(int num)
    {
        Console.WriteLine($"Philosopher {_num} is reading a book {num}");
        Thread.Sleep(rnd.Next(500, 1000));
        Console.WriteLine($"Philosopher {_num} finished book {num}");
    }

    public void StartPhilosophing()
    {
        int bookNumber = 0;
        while (true)
        {
            if(_readBooks.Count == _books.Length)
                break;
            //every Philosopher must individually pick random book number to read now
            //sort of works???? lol
            bool flag = false;
            do
            {
                bookNumber = rnd.Next(0, 2);
                if (!_readBooks.Contains(bookNumber))
                {
                    _readBooks.Add(bookNumber);
                    flag = true;
                }

            } while (flag == false);
            
            

            //Only one philosopher can read specific book at time therefore we use Monitor Class to make sure no one else can read it too
            Monitor.Enter(_books[bookNumber]);
            ReadBook(bookNumber);
            Monitor.Exit(_books[bookNumber]);

            //thinking
            Think();
            //picking a fork
            if (_num < _forks.Length - 1)
            {
                Monitor.Enter(_forks[_leftFork]);
                Monitor.Enter(_forks[_rightFork]);
            }
            else
            {
                Monitor.Enter(_forks[_rightFork]);
                Monitor.Enter(_forks[_leftFork]);
            }

            Eat();
            Monitor.Exit(_forks[_rightFork]);
            Monitor.Exit(_forks[_leftFork]);
        }
    }
}

public class Biesiada
{
    public static void Main()
    {
        int licz_fil = 5;
        Object[] _forks = new Object[licz_fil];
        Object[] books = new Object[licz_fil];
        for (int i = 0; i < 2; i++)
        {
            books[i] = new Object();
        }

        for (int i = 0; i < licz_fil; i++)
        {
            _forks[i] = new Object();
        }

        Philosopher[] philosophers = new Philosopher[licz_fil];
        Thread[] watki = new Thread[licz_fil];
        for (int i = 0; i < licz_fil; i++)
        {
            philosophers[i] = new Philosopher(i, _forks, books);
            watki[i] = new Thread(philosophers[i].StartPhilosophing);
        }

        foreach (var watek in watki) watek.Start();
        foreach (var watek in watki) watek.Join();

        Console.ReadKey();
    }
}