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
        public class TodoItem // Klass för att dela upp varje rad i [filnamn] i 4 delar med hjälp av "|".
        {
            public int status; // Först initiering av två int (status och priority) och två strängar (task och taskDesctiption)
            public int priority;
            public string task;
            public string taskDescription;
            public TodoItem(int priority, string task, string taskInfo) // Konstruktor för skapande av nya poster
            {
                this.status = Active;
                this.priority = priority;
                this.task = task;
                this.taskDescription = taskInfo;
            }
            public TodoItem(string todoLine) // Konstruktor för skapande av array för uppdelning i fyra "fält" av inläst fil.
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
            Console.WriteLine($" Läste {numRead} rader.\n"); // Talar om hur många rader filen bestod av.
        }
        // Nu följer tre privata metoder för utskrift av data från inläst fil:
        private static void PrintHeadOrFoot(bool head, bool verbose) // Första privata metoden initierar printhuvud och fot för mall till utskrift.
        {
            if (head)
            {
               Console.Write("|status      |prio  |namn                     |");
               if (verbose) Console.WriteLine("beskrivning                             |");
               else Console.WriteLine();
            }
            Console.Write("|------------|------|-------------------------|");
            if (verbose) Console.WriteLine("----------------------------------------|");
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
        public static void PrintHelp() // Hjälplista NYI -- Lägg till flera valmöjligheter
        {
            Console.WriteLine(" +----------------------------------------------------------------------------+");
            Console.WriteLine(" | Kommandon:                                                                 |\n +----------------------------------------------------------------------------+");
            Console.WriteLine(" |       \"hjälp\"        :  Visa denna hjälp.                            YEAH! |");
            Console.WriteLine(" |       \"lista\"        :  Lista att-göra-listan.                         OK! |");
            Console.WriteLine(" |     \"lista allt\"     :  Lista allt aktivt i att-göra-listan.          TBD! |");
            Console.WriteLine(" |       \"beskriv\"      :  Lista precis allt i att-göra-listan.          NYI! |");
            Console.WriteLine(" |         \"ny\"         :  Skapa ny uppgift i att-göra-listan.           NYI! |");
            Console.WriteLine(" | \"aktivera /uppgift/\" :  Göra vald uppgift aktiv i att-göra-listan.    NYI! |");
            Console.WriteLine(" |     \"klar /uppgift/\" :  Göra vald uppgift avklarad i att-göra-listan. NYI! |");
            Console.WriteLine(" |    \"vänta /uppgift/\" :  Göra vald uppgift väntande i att-göra-listan. NYI! |");
            Console.WriteLine(" |        \"sluta\"       :  Spara att-göra-listan och avsluta programmet. TBD! |");
            Console.WriteLine(" +----------------------------------------------------------------------------+");
        }
    }
    class MainClass
    {
        public static void Main(string[] args) // Huvudprogram.
        {
            Console.WriteLine(" Välkommen till att-göra-listan!"); // Hälsar användaren välkommen.
            Console.WriteLine(" -------------------------------\n");
            Todo.ReadListFromFile(); // Inläsning av fil, kallar på den funktionen i klassen Todo.
            Todo.PrintHelp(); // Utskrift av meny/hjälp, kallar på den funktionen i klassen Todo.
            string command; // initiering av en sträng som "behållare" för användarens inmatning
            do // Startar Loop
            {
                command = MyIO.ReadCommand("> "); // Kallar på metoden ReadCommand i klassen MyIO för att be användaren om inmatning.
                if (MyIO.Equals(command, "hjälp")) // Start av If sats... Om command är hjälp så skriv ut menyn en gång till. NYI... Fler else if.
                {
                    Todo.PrintHelp(); // Utskrift av meny/hjälp.
                }
                else if (MyIO.Equals(command, "sluta")) // Annars om command är sluta så skriv ut ett hejdå dör att hoppa ur loop.
                {
                    Console.WriteLine("Hej då!");
                    break;
                }
                else if (MyIO.Equals(command, "ny"))
                {
                    int status = 1;
                    string nyprio, nytask, nyinfo;
                    List<string> newtask = new List<string>();
                    Console.Write("\n Vad är det för uppgift du vill lägga till? -> ");
                    nytask = Console.ReadLine();
                    if (nytask.Length <= 2) { Console.WriteLine($"{nytask} tillagd"); }
                    else { Console.WriteLine("Uppgift för kort!"); }
                    Console.WriteLine("Kan ge en kort beskrivning av uppgiften? -> ");
                    nyinfo = Console.ReadLine();
                    Console.Write($"Vilket prio vill du sätta på {nytask}? (Ange 1-3) >");
                    nyprio = Console.ReadLine();
                    if (nyprio == "0" || nyprio == "4") { Console.WriteLine("Denna prio finns inte!"); }
                    else if (nyprio == "1") { newtask.Add(nyprio); }
                    else if (nyprio == "2") { newtask.Add(nyprio); }
                    else if (nyprio == "3") { newtask.Add(nyprio); }
                    else throw new OutOfMemoryException();
                    newtask.Add(nytask);
                    newtask.Add(nyinfo);
                    Console.WriteLine(newtask);
                    break;
                }
                else if (MyIO.Equals(command, "exit") || MyIO.Equals(command, "quit") || MyIO.Equals(command, "avsluta"))
                {
                    Console.WriteLine("För att avsluta detta lilla program skriv \"sluta\"");
                }
                else if (MyIO.Equals(command, "authors") || MyIO.Equals(command, "programmerad") || MyIO.Equals(command, "cred"))
                {
                    Console.WriteLine("Programmet är skriven av TomKi the teacher och Andreas Kuylenstierna");
                }
                else if (MyIO.Equals(command, "lista")) // Annars om command är lista så... en ny if sats startar:
                {
                    if (MyIO.HasArgument(command, "allt")) // Om command innehåller "lista" + allt så ska beskrivning inkluderas i utskrift
                        Todo.PrintTodoList(verbose: true);
                    else
                        Todo.PrintTodoList(verbose: false); // Annars ska inte beskrivning bli utskriven men bara det nödvändigaste.
                }
                else // Annars meddelas användaren att han/hon har skrivit in ett felaktigt kommando.
                {
                    Console.WriteLine($"Okänt/ogiltigt kommando: \"{command}\"");
                }
            }
            while (true); // Kör loopen medans sant
        }
    }  
}
