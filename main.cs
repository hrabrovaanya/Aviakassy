using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security;

  class Program
    {
        /*Консольное приложение "Авиакассы"
         Выполнила: Храброва А.А., группа 1521501*/
        static void Main(string[] args)
        {
            Console.Write("Добро пожаловать в авиакассу \"Freedom flight\".\n(Мы не отвечаем за реальное существование предлагаемых рейсов, вы всё делаете на свой страх и риск).\n");
          authorization();
            Console.ReadLine();
        }
     
        static void authorization(){         
     Authorizator auth = new Authorizator();
           string username, passwd;
          Console.Write("\nПОЖАЛУЙСТА, ПРОЙДИТЕ АВТОРИЗАЦИЮ: \n");
          Console.Write("Необходим вход для \n\t1)покупателя,\n\t2)администратора: \nВыберите 1 или 2: ");
          int status =  Int32.Parse(Console.ReadLine());
          int dop_status=0;
             if(status==1){
           Console.Write("Вы хотите \n\t1)зайти в существующую учетную запись,\n\t2)создать новую учетную запись: \nВыберите 1 или 2: ");
          dop_status =  Int32.Parse(Console.ReadLine());
        }
          if(dop_status==2){
             Console.Write("Как к вам обращаться: ");
          string name = Console.ReadLine();
            Console.Write("Придумайте логин: ");
          username = Console.ReadLine();
          Console.Write("Придумайте пароль: ");
           passwd = Console.ReadLine();
           auth.register(name,username,passwd);
            Console.Write("Регистрация прошла успешно. Зaйдите в свою учетную запись... \n");
            authorization();
          }
          else{
            Console.Write("Логин: ");
           username = Console.ReadLine();
          Console.Write("Пароль: ");
       passwd = Console.ReadLine();
      if(auth.authorizate(status,username, passwd)){
        if(status==1){
         Console.Write("Здесь вы можете преобрести билеты по направлениям:\n");
            preview_information();
            Console.Write("Введите пункт прибытия: ");
            string place_of_arrival = Console.ReadLine();
            Console.WriteLine("-----------------------------------------------------------------------------------");
            List<Flight_list> flight_list = new List<Flight_list>();//list of searched flights
            flight_list = Build_a_list_of_flights(flight_list, place_of_arrival);
            Displaying_a_list_of_flights(flight_list); 
        }
       else if(status==2)    {
         Console.Write("Здесь будет функция добавления рейса");
       }
      }
          else {
            Console.Write("Не удалось войти в систему. Возможна ошибка логина или пароля.\nПроверьте данные и попробуйте войти еще раз.\n");
             authorization();
          }
          }         
        }
    
        static void preview_information()//displays information about available flights
        {
            string line = "";
            using (StreamReader s = new StreamReader("File//Имеющиеся рейсы.txt"))
            {
                while ((line = s.ReadLine()) != null)
                {
                    string[] l = line.Split('|');
                    Console.Write($"Тула - {l[0]}\n");
                }
            };
        }


        static List<Flight_list> Build_a_list_of_flights(List<Flight_list> flight_list, string place_of_arrival)//Creates a list with all the information about the searched flights
        {
            string line = "";
            bool flight_availability = false;
            using (StreamReader s = new StreamReader("File//Имеющиеся рейсы.txt"))
            {
                while ((line = s.ReadLine()) != null)
                {
                    
                    string[] l = line.Split('|');
                    if (l[0] == place_of_arrival)
                    {
                        flight_list.Add(new Flight_list(l[0], l[1]));
                        flight_availability = true;
                    }
                }
            };
            if (!flight_availability) { Console.Write("У нас не продаются билеты на данный рейс...\n"); }
            return flight_list;
        }


        static void Displaying_a_list_of_flights(List<Flight_list> flight_list)//display of all information about the desired flights
        {
            foreach (var list_1 in flight_list)
            {
                Console.WriteLine($"Рейсы по направлению: Тула - {list_1.Place_of_arrival}");
                Console.WriteLine("-----------------------------------------------------------------------------------");
                foreach (var list_2 in list_1.Flight_information1)
                {
                    Console.WriteLine($"Рейс: {list_2.Flight_number}, \nТип самолета: {list_2.Flight_type}, \nДата и время отлета: {list_2.Takeoff_time}, \nДата и время прилета: {list_2.Landing_time}");
                    foreach (var list_3 in list_2.Crew1)
                    {
                        Console.WriteLine($"Экипаж: \n\tПилот: {list_3.Pilot},  \n\tВторой пилот: {list_3.Co_pilot}, \n\tБортовой инженер: {list_3.Engineer},  \n\tСостав бортпроводников: {list_3.Stewar}");
                    }
                    Console.WriteLine("Билеты в продаже:");
                    foreach (var list_4 in list_2.Tickets)
                    {
                        Console.WriteLine($"\tКласс: {list_4.Flight_class},  \n\tКоличество: {list_4.Quantity},  \n\tЦена: {list_4.Price}\n");
                    }
                    Console.WriteLine("-----------------------------------------------------------------------------------");
                }
            }
        }
    }



abstract class User
  {
    protected string username;
    protected string passwd;
    public abstract bool ExistenceUser(string username, string passwd);
  }

  class DefaultUser : User
  {
    public DefaultUser(string username, string passwd)
    {
      this.username = username;
      this.passwd = passwd;
    }

public override bool ExistenceUser(string username, string passwd){
        string line = "";
            bool user_existence = false;
            using (StreamReader s = new StreamReader("File//Пользователи.txt"))
            {
                while ((line = s.ReadLine()) != null)
                {
                    
                    string[] l = line.Split('|');
                    if(l[0] ==username && l[1]==passwd)
                    {                   
                        user_existence = true;
                        Console.Write($"Добро пожаловать, {l[2]}!\n");
                    }
                }
            };
    return user_existence;
  }
  }

  class Administrator : User
  {
    public Administrator(string username, string passwd)
    {
      this.username = username;
      this.passwd = passwd;
    }

public override bool ExistenceUser(string username, string passwd){
        string line = "";
            bool user_existence = false;
            using (StreamReader s = new StreamReader("File//Администраторы.txt"))
            {
                while ((line = s.ReadLine()) != null)
                {
                    
                    string[] l = line.Split('|');
                    if(l[0] ==username && l[1]==passwd)
                    {                   
                        user_existence = true;
                        Console.Write($"Добро пожаловать, {l[2]}!\n");
                    }
                }
            };
    return user_existence;
  }
  }

  class Authorizator
  {
    public Authorizator()
    {  
    }
    // Авторизация пользователя 
    public bool authorizate(int status, string username, string passwd)
    {
  if(status==1){
    DefaultUser user = new  DefaultUser(username, passwd);
        if(user.ExistenceUser(username, passwd))return true;
         else return false;
               }
  else if(status==2){
    Administrator user = new  Administrator (username, passwd);
     if(user.ExistenceUser(username, passwd))return true;
         else return false;
                    }
    else return false;
    }
    // Регистация пользователя
    public void register(string name,string username, string passwd){
      System.IO.StreamWriter writer = new System.IO.StreamWriter("File//Пользователи.txt",true);
              writer.WriteLine($"\n{username}|{passwd}|{name}");
              writer.Close();
    }
  }

////////////////////////////////////////////////////////////////////////
    class Flight_list //contains all the information about the searched flights
    {
        private string place_of_arrival;
        private string id;
        private List<Flight_information> flight_information1 = new List<Flight_information>();

        public string Place_of_arrival { get => place_of_arrival; set => place_of_arrival = value; }
        public string Id { get => id; set => id = value; }
        internal List<Flight_information> Flight_information1 { get => flight_information1; set => flight_information1 = value; }

        public Flight_list(string place_of_arrival, string id)
        {
            this.Place_of_arrival = place_of_arrival;
            this.Id = id;
            this.Flight_information1 = Flight_information_Add(this.Id, Flight_information1);
        }


        private List<Flight_information> Flight_information_Add(string id, List<Flight_information> flight_information)
        {
            string line = "";
            using (StreamReader s = new StreamReader("File//Информация о рейсе.txt"))
            {
                while ((line = s.ReadLine()) != null)
                {
                    string[] l = line.Split('|');
                    if (id == l[0])
                    {
                        flight_information.Add(new Flight_information(l[1], l[2], l[3], l[4]));
                    }
                }
            };

            return flight_information;
        }


        public class Flight_information //class containing all information about flights
        {
            private string flight_number;
            private string flight_type;
            private string takeoff_time;
            private string landing_time;
            private List<Crew> crew1 = new List<Crew>();
            private List<Ticket> tickets = new List<Ticket>();

            public string Flight_number { get => flight_number; set => flight_number = value; }
            public string Flight_type { get => flight_type; set => flight_type = value; }
            public string Takeoff_time { get => takeoff_time; set => takeoff_time = value; }
            public string Landing_time { get => landing_time; set => landing_time = value; }
            internal List<Crew> Crew1 { get => crew1; set => crew1 = value; }
            internal List<Ticket> Tickets { get => tickets; set => tickets = value; }

            public Flight_information(string flight_number, string flight_type, string takeoff_time, string landing_time)
            {
                this.Flight_number = flight_number;
                this.Flight_type = flight_type;
                this.Takeoff_time = takeoff_time;
                this.Landing_time = landing_time;
                this.Crew1 = Сreate_crew_list(this.Flight_number, Crew1);
                this.Tickets = Сreate_ticket_list(this.Flight_number, Tickets);
            }


            private List<Crew> Сreate_crew_list(string flight_number, List<Crew> crew)
            {
                string line = "";
                using (StreamReader s = new StreamReader("File//Экипаж.txt"))
                {
                    while ((line = s.ReadLine()) != null)
                    {
                        string[] l = line.Split('|');
                        if (flight_number == l[0])
                        {
                            crew.Add(new Crew(l[1], l[2], l[3], l[4]));
                        }
                    }
                };

                return crew;
            }


            private List<Ticket> Сreate_ticket_list(string flight_number, List<Ticket> tickets)
            {
                string line = "";
                using (StreamReader s = new StreamReader("File//Билеты.txt"))
                {
                    while ((line = s.ReadLine()) != null)
                    {
                        string[] l = line.Split('|');
                        if (flight_number == l[0])
                        {
                            tickets.Add(new Ticket(l[1], Int32.Parse(l[2]), Int32.Parse(l[3])));
                        }
                    }
                };

                return tickets;
            }


            public class Crew //a class containing information about the crew of the flight
            {
                private string pilot;
                private string co_pilot;
                private string engineer;
                private string stewar;

                public string Pilot { get => pilot; set => pilot = value; }
                public string Co_pilot { get => co_pilot; set => co_pilot = value; }
                public string Engineer { get => engineer; set => engineer = value; }
                public string Stewar { get => stewar; set => stewar = value; }

                public Crew(string pilot, string co_pilot, string engineer, string stewar)
                {
                    this.Pilot = pilot;
                    this.Co_pilot = co_pilot;
                    this.Engineer = engineer;
                    this.Stewar = stewar;

                }

            }


            public class Ticket //class containing information about flight tickets
            {
                private string flight_class;
                private int quantity;
                private int price;

                public string Flight_class { get => flight_class; set => flight_class = value; }
                public int Quantity { get => quantity; set => quantity = value; }
                public int Price { get => price; set => price = value; }

                public Ticket(string flight_class, int quantity, int price)
                {
                    this.Flight_class = flight_class;
                    this.Quantity = quantity;
                    this.Price = price;
                }
            }

        }
    }

