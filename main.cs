using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

    class Program
    {
        /*Консольное приложение "Авиакассы"
         Выполнила: Храброва А.А., группа 1521501*/
        static void Main(string[] args)
        {
            Console.Write("Добро пожаловать в авиакассу \"Freedom flight\".\n(Мы не отвечаем за реальное существование предлагаемых рейсов, вы всё делаете на свой страх и риск).\n");
            Console.Write("Здесь вы можете преобрести билеты по направлениям:\n");

            string line = "";
            using (StreamReader s = new StreamReader("File//Имеющиеся рейсы.txt"))
            {
                while ((line = s.ReadLine()) != null)
                {
                    string[] l = line.Split('|');
                    Console.Write($"Тула - {l[0]}\n");
                }
            };
            Console.Write("Введите пункт прибытия: ");
            string where = Console.ReadLine();
            Console.WriteLine("-----------------------------------------------------------------------------------");
            List<Avia> av = new List<Avia>();
            av = BuildAviaList(av, where);
            Vivod(av);
            Console.ReadLine();
        }
        static List<Avia> BuildAviaList(List<Avia> av, string where)
        {
            string line = "";
            using (StreamReader s = new StreamReader("File//Имеющиеся рейсы.txt"))
            {
                while ((line = s.ReadLine()) != null)
                {
                    string[] l = line.Split('|');
                    if (l[0] == where) {
                        av.Add(new Avia(l[0], l[1]));
                    }
                }
            };
            return av;
        }
        static void Vivod(List<Avia> av)
        {
            foreach (var list_1 in av)
            {
                Console.WriteLine($"Рейсы по направлению: Тула - {list_1.where}");
                Console.WriteLine("-----------------------------------------------------------------------------------");
                foreach (var list_2 in list_1.p_t)
                {
                    Console.WriteLine($"Рейс: {list_2.flight}, \nТип самолета: {list_2.type}, \nДата и время отлета: {list_2.takeoff_time}, \nДата и время прилета: {list_2.landing_time}");
                    foreach (var list_3 in list_2.crew)
                      {
                        Console.WriteLine($"Экипаж: \n\tПилот: {list_3.pilot},  \n\tВторой пилот: {list_3.co_pilot}, \n\tБортовой инженер: {list_3.engineer},  \n\tСостав бортпроводников: {list_3.stewar}");
                    }
                    Console.WriteLine("Билеты в продаже:");
                    foreach (var list_4 in list_2.tickets)
                    {
                        Console.WriteLine($"\tКласс: {list_4.clas},  \n\tКоличество: {list_4.quantity},  \n\tЦена: {list_4.price}\n");
                    }
                    Console.WriteLine("-----------------------------------------------------------------------------------");
                }
            }
        }
    }
    class Avia
    {
        public string where;
        public string id;
        public List<Place_Time> p_t= new List<Place_Time>();
        public Avia(string where, string id)//Страна прибытия
        {
            this.where = where;
            this.id = id;
            this.p_t = Place_Time_Add(this.id,p_t);
        }
        private List<Place_Time> Place_Time_Add(string id, List<Place_Time> p_t)
        {
          string line = "";
            using (StreamReader s = new StreamReader("File//Информация о рейсе.txt"))
            {
                while ((line = s.ReadLine()) != null)
                {
                    string[] l = line.Split('|');
                    if (id == l[0]) {
                        p_t.Add(new Place_Time(l[1], l[2], l[3], l[4]));
                    }
                }
            };

            return p_t;
        }
        public class Place_Time //Информация по рейсу
        {
            public string flight;
            public string type;
            public string takeoff_time;
            public string landing_time;
           public List<Crew> crew = new List<Crew>();
           public List<Ticket> tickets = new List<Ticket>();
 
            public Place_Time(string flight, string type, string takeoff_time, string landing_time)
            {
                this.flight = flight;
                this.type = type;
                this.takeoff_time = takeoff_time;
                this.landing_time = landing_time;
                this.crew = Crew_Add(this.flight, crew);
                this.tickets = Ticket_Add(this.flight, tickets);
            }
            private List<Crew> Crew_Add(string flight, List<Crew> crew)
            {
                string line = "";
                using (StreamReader s = new StreamReader("File//Экипаж.txt"))
                {
                    while ((line = s.ReadLine()) != null)
                    {
                        string[] l = line.Split('|');
                        if (flight == l[0])
                        {
                            crew.Add(new Crew(l[1], l[2], l[3], l[4]));
                        }
                    }
                };

                return crew;
            }
            private List<Ticket> Ticket_Add(string flight, List<Ticket> tickets)
            {
                string line = "";
                using (StreamReader s = new StreamReader("File//Билеты.txt"))
                {
                    while ((line = s.ReadLine()) != null)
                    {
                        string[] l = line.Split('|');
                        if (flight == l[0])
                        {
                           tickets.Add(new Ticket( l[1], Int32.Parse(l[2]), Int32.Parse(l[3])));
                        }
                    }
                };

                return tickets;
            }
           public class Crew //Экипаж
            {
                public string pilot;
                public string co_pilot;
                public string engineer;
                public string stewar;

                public Crew(string pilot, string co_pilot, string engineer, string stewar)
                {
                    this.pilot = pilot;
                    this.co_pilot = co_pilot;
                    this.engineer = engineer;
                    this.stewar = stewar;

                }
             
            }
            public class Ticket //Билеты
            {
                public string clas;
                public int quantity;
                public int price;

                public Ticket(string clas, int quantity, int price)
                {
                    this.clas = clas;
                    this.quantity = quantity;
                    this.price = price;
                }
            }
      
        }
    }
   
