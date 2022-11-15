namespace dtp15_todolist
{
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
        public class TodoItem // Klass för att dela upp varje rad i [filnamn] i 4 delar med hjälp av "|".
        {
            public int status; // Först initiering av två int (status och priority) och två strängar (task och taskDesctiption)
            public int priority;
            public string task;
            public string taskDescription;
            public TodoItem(int priority, string task) // Lista alla aktiva poster
            {
                this.status = Active;
                this.priority = priority;
                this.task = task;
                this.taskDescription = "";
            }
            public TodoItem(string todoLine) // Skapande av array för uppdelning i fyra "fält"
            {
                string[] field = todoLine.Split('|'); // Dela allt i array/strängen "field" vid varje pipe
                status = Int32.Parse(field[0]); // status har index 0
                priority = Int32.Parse(field[1]); // priority har index 1
                task = field[2]; // task har index 2
                taskDescription = field[3]; // taskDescription har index 3 av max 4 (0-3)
            }
            public void Print(bool verbose = false) // Metoden Print som har variabeln verbose(=mångordig) satt till false som default.
            {
                string statusString = StatusToString(status);
                Console.Write($"|{statusString,-12}|{priority,-6}|{task,-25}|"); // utskrift av task
                if (verbose) // om verbose är sann så...
                    Console.WriteLine($"{taskDescription,-40}|"); //...skriv ut hela uppgiftens beskriving.
                else
                    Console.WriteLine(); // eller skriv en tom rad.
            }
        }
        public static void ReadListFromFile() // Metod för inläsning av fil användande av StreamReader.
        {
            string todoFileName = "todo.lis"; // Hårdkodning av att använda filen todo.lis -- TBD
            Console.Write($"Läser från fil {todoFileName} ... "); // Talar om för användaren att filen är inläst.
            StreamReader sr = new StreamReader(todoFileName); // Men det är inte gjort förrän här.
            int numRead = 0;

            string line;
            while ((line = sr.ReadLine()) != null) // Sålänge inläsning sr inte är null så...
            {
                TodoItem item = new TodoItem(line); // ...skapa en ny rad av inläst fil
                list.Add(item); // Lägg till detta i listan list.
                numRead++;
            }
            sr.Close(); // Stäng fil
            Console.WriteLine($"Läste {numRead} rader."); // Talar om hur många rader filen bestod av.
        }
        private static void PrintHeadOrFoot(bool head, bool verbose) // Nu följer tre privata metoder för utskrift av data från inläst fil.
        {
            if (head)
            {
                Console.Write("|status      |prio  |namn                     |");
                if (verbose) Console.WriteLine("beskrivning                             |");
                else Console.WriteLine();
            }
            Console.Write("|------------|------|-------------------------|");
            if (verbose) Console.WriteLine("---------------------------------------------|");
            else Console.WriteLine();
        }
        private static void PrintHead(bool verbose)
        {
            PrintHeadOrFoot(head: true, verbose);
        }
        private static void PrintFoot(bool verbose)
        {
            PrintHeadOrFoot(head: false, verbose);
        }
        public static void PrintTodoList(bool verbose = false)
        {
            PrintHead(verbose);
            foreach (TodoItem item in list)
            {
                item.Print(verbose);
            }
            PrintFoot(verbose);
        }
        public static void PrintHelp()
        {
            Console.WriteLine("Kommandon:");
            Console.WriteLine("hjälp    lista denna hjälp");
            Console.WriteLine("lista    lista att-göra-listan");
            Console.WriteLine("sluta    spara att-göra-listan och sluta");
        }
    }
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Välkommen till att-göra-listan!");
            Todo.ReadListFromFile();
            Todo.PrintHelp();
            string command;
            do
            {
                command = MyIO.ReadCommand("> ");
                if (MyIO.Equals(command, "hjälp"))
                {
                    Todo.PrintHelp();
                }
                else if (MyIO.Equals(command, "sluta"))
                {
                    Console.WriteLine("Hej då!");
                    break;
                }
                else if (MyIO.Equals(command, "lista"))
                {
                    if (MyIO.HasArgument(command, "allt"))
                        Todo.PrintTodoList(verbose: true);
                    else
                        Todo.PrintTodoList(verbose: false);
                }
                else
                {
                    Console.WriteLine($"Okänt kommando: {command}");
                }
            }
            while (true);
        }
    }
    class MyIO
    {
        static public string ReadCommand(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
        static public bool Equals(string rawCommand, string expected)
        {
            string command = rawCommand.Trim();
            if (command == "") return false;
            else
            {
                string[] cwords = command.Split(' ');
                if (cwords[0] == expected) return true;
            }
            return false;
        }
        static public bool HasArgument(string rawCommand, string expected)
        {
            string command = rawCommand.Trim();
            if (command == "") return false;
            else
            {
                string[] cwords = command.Split(' ');
                if (cwords.Length < 2) return false;
                if (cwords[1] == expected) return true;
            }
            return false;
        }
    }
}
