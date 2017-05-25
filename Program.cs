using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Timers;
using System.Reflection;

namespace ConsoleApplication2
{
    class Program
    {             
        static void Main(string[] args)
        {
//Обьявление списка животных и зоопарка
            List<Animal> la = new List<Animal>();
            Zoo zoo = new Add(la);
//Добавление первого животного
            zoo.BeginMsg();
            zoo.Act();
//Установка таймера
            Timer tmr = new Timer();
            tmr.Interval = 5000;
            tmr.Elapsed += (object sender, ElapsedEventArgs e) =>
                {
                        zoo = new RandChng(la);
                        la = zoo.Act();
                        if (zoo.CheckAllDead(la)) 
                        { 
                                    Console.WriteLine("Доигрался! Все животные мертвы! Через 3 секунды произойдет выход.");
                                    System.Threading.Thread.Sleep(3000);
                                    tmr.Stop(); 
                                    Environment.Exit(0); 
                        }
                };
//Выбор и выполнение функций
            string action;
            while(true)
            {
                action = zoo.GetAction();
                    switch (action)
                    {
                        case "Add": zoo = new Add(la); la = zoo.Act(); break;
                        case "Heal": zoo = new Heal(la); la = zoo.Act(); break;
                        case "Feed": zoo = new Feed(la); la = zoo.Act(); break;
                        case "Delete": zoo = new Delete(la); la = zoo.Act(); break;
                        case "RandChng":
                                        {
                                            if (!tmr.Enabled)
                                                tmr.Start();
                                            else
                                                Console.WriteLine(action+" уже запущен.");
                                        }break;
                    }
            }
        }
    }
    //Описание перечисления и классов
    public enum State {full,hungry,ill,dead};
    //класс параметров видов зверей
    public class Animal
    {
        protected int _helth;
        protected int _maxHelth;
        protected string _name;
        protected State _state=State.full;

        public int Helth { get { return _helth; } set { _helth = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public State State { get { return _state; } set { _state = value; } }
        public int MaxHelth { get { return _maxHelth; }  }

    }
    //классы видов зверей в зоопарке
    public class Lion : Animal
    {
        public Lion()
        {
            _maxHelth = _helth = 5;
        }
    }
    public class Tiger : Animal
    {
        public Tiger()
        {
            _maxHelth = _helth = 4;
        }
    }
    public class Elephant : Animal
    {
        public Elephant()
        {
            _maxHelth = _helth = 7;
        }
    }
    public class Bear : Animal
    {
        public Bear()
        {
            _maxHelth = _helth = 6;
        }
    }
    public class Wolf : Animal
    {
        public Wolf()
        {
            _maxHelth = _helth = 4;
        }
    }
    public class Fox : Animal
    {
        public Fox()
        {
            _maxHelth = _helth = 3;
        }
    }
    //Класс зоопарка
    public abstract class Zoo
    {
        protected List<Animal> listA;

        //Вывод начального сообщения
        public void BeginMsg()
        {
            Console.WriteLine("Для начала работы нужно добавить хотя бы одно животное.");
            Console.WriteLine("Включен режим добавления животного.");
        }
        //Считывание действия
        public string GetAction()
        {
            List<string> lstr = new List<string>() { "Add", "Feed", "Heal", "Delete", "RandChng" };
            Console.Write("Введите действие\r\n");
            foreach (var a in lstr)
                Console.Write(" " + a+" ");

            bool res = false;
            string locVar="";
            while (!res)
            {
                locVar = Console.ReadLine();
                foreach (var a in lstr)
                    if (a == locVar)
                    {
                        res = true;
                        break;
                    }
                if (!res)
                    Console.WriteLine("Такого действия в списке доступных нет.");
            }
            return locVar;

        }
        //Считывание имени животного
        protected string GetName()
        {
            Console.WriteLine("Введите имя");
            return Console.ReadLine();
        }
        //Считывание вида животного
        protected string GetBreed()
        {
            List<string> lstr=new List<string>(){"Lion","Tiger","Elephant","Bear","Wolf","Fox"};
            bool res=false;
            string locVar="";
            Console.WriteLine("Введите вид животного\r\n");
            foreach (var a in lstr)
                Console.Write(" " + a + " ");
            while(!res)
            {
                locVar=Console.ReadLine();
                for (int i = 0; i < lstr.Count; i++)
                    if (lstr[i] == locVar)
                    {
                        res = true;
                        break;
                    }
                if (!res)
                Console.WriteLine("Такого вида в зоопарке нет");
            }
            return locVar;
        }
        //Все животные в списке мертвы?
        public bool CheckAllDead(List<Animal> la)
        {
            bool res = true;
            for (int i = 0; i < la.Count; i++)
                if (la[i].State != State.dead)
                    res = false;
            return res;
        }
        
        //Возможные действия зоопарка
        public abstract List<Animal> Act();
    }
    //Классы конкретных действий зоопарка
    public class Add : Zoo
    {
        public Add(List<Animal> la)
        {
            listA = la;
        }
        Animal anm;
           
        public override List<Animal> Act()
        {
            string name = GetName();
            string breed = GetBreed();
//Свич по видам животных
            switch(breed) 
            {
                case "Lion"  :  anm = new Lion();break;
                case "Tiger": anm = new Tiger();break;
                case "Elephant": anm=new Elephant();break;
                case "Bear": anm=new Bear();break;
                case "Wolf": anm=new Wolf();break;
                case "Fox": anm=new Fox();break;
            }
            
            anm.Name = name;
            listA.Add(anm);
            
            Console.WriteLine("Животное {0} добавлено в зоопарк.", name);
            return listA;
        }

    }
    public class Feed : Zoo
    {
        public Feed(List<Animal> la)
        {
            listA = la;
        }
        public override List<Animal> Act()
        {
            string name = GetName();
            string locvar = "Животного нет в списке и/или животное не голодное";
            foreach (var a in listA)
                if (a.Name == name & a.State == State.hungry)
                {
                    a.State = State.full;
                    locvar="Животное " + name + " теперь сытое.";
                }
            Console.WriteLine(locvar);
            return listA;
        }
    }
    public class Heal : Zoo
    {
        public Heal(List<Animal> la)
        {
            listA = la;
        }
        public override List<Animal> Act()
        {
            string local = "Животного нет в списке и/или его здоровье максимально";
            string name = GetName();
            
            foreach (var a in listA)
                if (a.Helth < a.MaxHelth & a.Name == name)
                {
                    local = "Здоровье животного " + name + " увелично на 1.";
                    a.Helth++;
                }
            Console.WriteLine(local);
            return listA;      
        }

    }
    public class Delete : Zoo
    {
        public Delete(List<Animal> la)
       {
             listA = la;
       }
        public override List<Animal> Act()
        {
            string name = GetName();
            string local = "Животного с таким именим и/или нулевым здоровьем нет";

            for (int i = 0; i < listA.Count; i++)
            {
                if (listA[i].Helth == 0 & listA[i].Name == name)
                {
                    listA.RemoveAt(i--);
                    local = "Животное " + listA[i].Name + " удалено";
                }
            }
            Console.WriteLine(local);
            return listA;
        }
    }
    ////Изменение здоровья или состояния рандомного животного 
    public class RandChng : Zoo
    {
        public RandChng(List<Animal> la)
        {
            listA = la;
        }
        public override List<Animal> Act()
        {
            Random rand=new Random();
            int rnd = rand.Next(0, listA.Count);
                switch (listA[rnd].State)
                 {
                   case State.full: listA[rnd].State = State.hungry; Console.WriteLine("\r\nЖивотное {0} изменило состояние на голоден", listA[rnd].Name); break;
                   case State.hungry: listA[rnd].State = State.ill; Console.WriteLine("\r\nЖивотное {0} изменило состояние на болен", listA[rnd].Name); break;
                   case State.ill: listA[rnd].Helth--; Console.WriteLine("\r\nЖивотное {0} изменило здоровье, здоровье = {1}", listA[rnd].Name, listA[rnd].Helth); break;
                 }
                if (listA[rnd].Helth == 0)
                {
                    Console.WriteLine("Животное {0} умерло.", listA[rnd].Name);
                    listA[rnd].State = State.dead;
                }
                                     
            return listA;
        }
    }
   
}
