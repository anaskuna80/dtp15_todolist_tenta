using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using static dtp15_todolist.Todo;

namespace dtp15_todolist
{
    class MyIO // Initiering av klassen MyIO
    {
        static public string ReadCommand(string prompt) // Inläsning av kommandon
        {
            Console.Write(prompt);
            return Console.ReadLine(); // Här sker inläsning
        }
        static public bool Equals(string rawCommand, string expected) // Metoden Equals jämför inmatningen och kollar om det är initierat i programmet
        {
            string command = rawCommand.Trim(); // Städa upp kommando
            if (command == "") return false; // Om commando är tom så returnera false
            else
            {
                string[] cwords = command.Split(' '); // Annars dela upp strängen av det inmatna kommandot med mellanslag
                if (cwords[0] == expected) return true; // Om första kommandot är det samma som väntas returnera true
            }
            return false; // ... annars false
        }
        static public bool HasArgument(string rawCommand, string expected) // Denna metod kollar om det finns fler argument än ett i inmatning.
        {
            string command = rawCommand.Trim(); // Ta bort onödiga mellanslag
            if (command == "") return false;
            else
            {
                string[] cwords = command.Split(' '); // Dela upp inmatna sträng till en array med mellanslag
                if (cwords.Length < 2) return false; // Om det bara blir ett ord av inmatningen så returnera false
                if (cwords[1] == expected) return true; // Annars sant
            }
            return false;
        }
    }
    public class Todo // Klass ToDo - Definering av status per uppgift
    {
        public static List<TodoItem> list = new List<TodoItem>(); // Skapande av listan list.

        public const int Active = 1; // Konstanten Active, aktiv
        public const int Waiting = 2; // Konstanten Waiting, väntande
        public const int Ready = 3; // Konstanten Ready, avklarad
        public static string StatusToString(int status)
        {
            switch (status) // Tilldelande av ekvivalenter för repektive status 
            {
                case Active: return "aktiv";
                case Waiting: return "väntande";
                case Ready: return "avklarad";
                default: return "(felaktig)";
            }
        }
        public static void StatusChanger(string command, string changeTask)
        {
            changeTask.Trim();

            if (command == "aktivera")
            {
                foreach (TodoItem item in list)
                {
                    if (item.task == changeTask) { item.status = Active; Console.WriteLine($" {item.task} är nu AKTIVERAD. ({item.status})"); break; }
                }
            }
            else if (command == "vänta")
            {
                    foreach (TodoItem item in list)
                    {
                        if (item.task == changeTask) { item.status = Waiting; Console.WriteLine($" {item.task} är nu VÄNTANDE. ({item.status})"); break; }
                    }
            }
            else if (command == "klar")
            {
                        foreach (TodoItem item in list)
                        {
                            if (item.task == changeTask) { item.status = Ready; Console.WriteLine($" {item.task} är nu AVKLARAD. ({item.status})"); break; }
                        }
            }
            else { Console.WriteLine($" Fel! {command} {changeTask} är ett felaktigt kommando"); }
        }

        public class TodoItem 
        {
            public int status;
            public int priority;
            public string task;
            public string taskDescription;
            public TodoItem(int priority, string task, string taskInfo) 
            {
                this.status = Active;
                this.priority = priority;
                this.task = task;
                this.taskDescription = taskInfo;
            }
            public TodoItem(string todoLine) 
            {
                string[] field = todoLine.Split('|'); 
                status = int.Parse(field[0]); 
                priority = int.Parse(field[1]); 
                task = field[2];
                taskDescription = field[3]; 
            }
            public void Print(bool verbose = false) 
            {
                string statusString = StatusToString(status);
                Console.Write($"|{statusString,-12}|{priority,-6}|{task,-25}|"); 
                if (verbose) 
                    Console.WriteLine($"{taskDescription,-40}|"); 
                else
                    Console.WriteLine();
            }
            public static void SaveFile()
            {
                string saveFile = "";
                Console.Write(" Vad ska filen heta? Filen kommer att få filändelsen .lis > ");
                saveFile = Console.ReadLine();
                saveFile = saveFile + ".lis";
                int lines = 0;
                using (TextWriter swr = new StreamWriter(saveFile))
                {
                    for (int s = 0; s < list.Count; s++)
                    {
                        string line = $"{list[s].status}|{list[s].priority}|{list[s].task}|{list[s].taskDescription}";
                        swr.WriteLine(line);
                        lines++;
                    }
                }             
                Console.Write($" Sparade {lines} rader i ");
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{saveFile}\n"); 
                Console.ResetColor();
            }
        }
        public static void NewTask()
        {
            int nyprio;
            string? nytask, nyinfo;
            Console.Write("\n Vad är det för uppgift du vill lägga till? -> ");
            nytask = Console.ReadLine();
            Console.Write($" Kan ge en kort beskrivning av uppgiften? -> ");
            nyinfo = Console.ReadLine();
            Console.Write($" Vilket prio vill du sätta på {nytask}? (Ange 1-3) > ");
            nyprio = Int32.Parse(Console.ReadLine());
            TodoItem item = new TodoItem(nyprio, nytask, nyinfo);
            list.Add(item);
            Console.WriteLine($" {item.task} tillagd!");
        }
    
        public static void ReadListFromFile() // Metod för inläsning av fil användande av StreamReader.
        {
            string todoFileName = "todo.lis"; // Hårdkodning av att använda filen todo.lis -- TBD
            Console.Write($" Läser från fil ");
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(todoFileName); // Talar om för användaren att filen är inläst.
            Console.ResetColor();
            Console.Write("."); Thread.Sleep(500);
            Console.Write("."); Thread.Sleep(500);
            Console.Write("."); Thread.Sleep(500);
            Console.Write("."); Thread.Sleep(500);
            Console.Write("."); Thread.Sleep(500);
            StreamReader sr = new StreamReader(todoFileName); 
            int numRead = 0;

            string line;
            while ((line = sr.ReadLine()) != null) 
            {
                TodoItem item = new TodoItem(line); 
                list.Add(item); 
                numRead++;
            }
            sr.Close(); 
            Console.WriteLine($" Läste {numRead} rader.\n"); 
        }

        public static void ReadNewListFromFile(string command) 
        {           
            string todoFileName = "";
            Console.Write(" Vad heter filen? > ");
            todoFileName = Console.ReadLine();
            try
            {
                if (todoFileName != null)
                {
                    list.Clear();
                    StreamReader sr = new StreamReader(todoFileName);
                    Console.WriteLine($" Läser från fil ");
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(todoFileName);
                    Console.ResetColor();
                    Console.Write("."); Thread.Sleep(500);
                    Console.Write("."); Thread.Sleep(500);
                    Console.Write("."); Thread.Sleep(500);
                    Console.Write("."); Thread.Sleep(500);
                    Console.Write("."); Thread.Sleep(500);                   
                    int numRead = 0;

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        TodoItem item = new TodoItem(line);
                        list.Add(item);
                        numRead++;
                    }
                    sr.Close();
                    Console.WriteLine($" Läste {numRead} rader.\n");
                }   
                else { Console.WriteLine(" Eh va?"); }
            }
            catch (FileNotFoundException ex)
            { Console.WriteLine($" Filen { todoFileName} finns inte: " + ex.Message); }
            
        }

        private static void PrintHeadOrFoot(bool head, bool verbose) 
        {
            if (head)
            {
                Console.Write("|status      |prio  |namn                     |");
                if (verbose) Console.WriteLine("beskrivning                             |");
                else Console.WriteLine();
            }
            Console.Write("|------------|------|-------------------------|");
            if (verbose) Console.WriteLine("----------------------------------------+");
            else Console.WriteLine();
        }
        private static void PrintHead(bool verbose) // Metod två kallar på ovanstående metod och sätter head som sann
        {
            PrintHeadOrFoot(head: true, verbose);
        }
        private static void PrintFoot(bool verbose) // Metod tre kallar på metod 1 för att sätta head till false
        {
            PrintHeadOrFoot(head: false, verbose);
        }
        public static void PrintTodoList(bool verbose = false) // utskrift av varje rad med en foreach utan description
        {
            PrintHead(verbose);
            foreach (TodoItem item in list)
            {
                item.Print(verbose);
            }
            PrintFoot(verbose);
        }

        public static void PrintActiveList(bool verbose = false) // utskrift av varje rad med en foreach utan description
        {
            PrintHead(verbose=false);
            foreach (TodoItem item in list)
            {
                if (item.status == Active)
                {
                    string statusString = StatusToString(item.status);
                    Console.WriteLine($"|{statusString,-12}|{item.priority,-6}|{item.task,-25}|");
                }
            }
            PrintFoot(verbose=false);
        }
        public static void PrintHelp() 
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" +----------------------------------------------------------------------------+");
            Console.WriteLine(" | Kommandon:                                                                 |\n +----------------------------------------------------------------------------+");
            Console.WriteLine(" |       \"hjälp\"        :  Visa denna hjälp.                            YEAH! |");
            Console.WriteLine(" |       \"lista\"        :  Lista att-göra-listan.                         OK! |");
            Console.WriteLine(" |     \"lista allt\"     :  Lista allt aktivt i att-göra-listan.           OK! |");
            Console.WriteLine(" |       \"beskriv\"      :  Lista precis allt i att-göra-listan.           OK! |");
            Console.WriteLine(" |         \"ny\"         :  Skapa ny uppgift i att-göra-listan.            OK! |");
            Console.WriteLine(" | \"aktivera /uppgift/\" :  Göra vald uppgift aktiv i att-göra-listan.     OK! |");
            Console.WriteLine(" |     \"klar /uppgift/\" :  Göra vald uppgift avklarad i att-göra-listan.  OK! |");
            Console.WriteLine(" |    \"vänta /uppgift/\" :  Göra vald uppgift väntande i att-göra-listan.  OK! |");
            Console.WriteLine(" |        \"ladda\"       :  Ladda en ny att-göra-lista                     OK! |");
            Console.WriteLine(" |        \"spara\"       :  Spara att-göra-listan i ny fil.                OK! |");
            Console.WriteLine(" |        \"sluta\"       :  Avsluta programmet.                            OK! |");
            Console.WriteLine(" +----------------------------------------------------------------------------+");
            Console.ResetColor();
        } 
    }
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(" Välkommen till att-göra-listan!");
            Console.WriteLine(" -------------------------------\n");
            ReadListFromFile();
            PrintHelp();
            string command;
            do
            {
                command = MyIO.ReadCommand("> ");

                if (MyIO.Equals(command, "hjälp")) { Console.Clear(); PrintHelp(); Console.ResetColor(); }

                else if (MyIO.Equals(command, "lista"))
                {
                    if (command.Contains("allt")) { PrintTodoList(verbose: false); }
                    else if (command.Contains("")) { PrintActiveList(verbose: false); }
                    else { Console.WriteLine("Något gick tyvärr fel..."); }
                }

                else if (MyIO.Equals(command, "beskriv")) { PrintTodoList(verbose: true); }

                else if (MyIO.Equals(command, "ny")) { NewTask(); }

                else if (command.StartsWith("aktivera"))
                {
                    string[] commands = command.Split(" ");
                    string check = command[9..];
                    StatusChanger(commands[0], check);
                }

                else if (command.StartsWith("klar"))
                {
                    string[] commands = command.Split(" ");
                    string check = command[5..];
                    StatusChanger(commands[0], check);
                }

                else if (command.StartsWith("vänta"))
                {
                    string[] commands = command.Split(" ");
                    string check = command[6..];
                    StatusChanger(commands[0], check);
                }

                else if (MyIO.Equals(command, "sluta")) { Console.WriteLine(" Hej då!"); Console.ResetColor(); break; }

                // La till detta för att retas lite med användaren :-)
                else if (MyIO.Equals(command, "exit") || MyIO.Equals(command, "quit") || MyIO.Equals(command, "avsluta") || MyIO.Equals(command, "finito") || MyIO.Equals(command, "end"))
                { Console.WriteLine(" För att avsluta detta lilla program skriv: \"sluta\""); }

                 else if (MyIO.Equals(command, "ladda")) { ReadNewListFromFile(command); }

                 else if (MyIO.Equals(command, "spara")) { TodoItem.SaveFile(); }

                 else 
                    Console.WriteLine($" Okänt/ogiltigt kommando: \"{command}\"");
            }
            while (command != "sluta");
        }
    }  
}
