//using System;
//МАНДАТНЫЕ ПОЛИТИКИ БЕЗОПАСНОСТИ ПОЛИТИКА БЕЗОПАСНОСТИ БЕЛЛА-ЛАПАДУЛЫ



namespace LR2
{
    class Program
    {

        private static List<string> Users = new List<string> { "Admin", "Vasya", "Ilya", "Dima", "Katya", 
                                                                "Petya", "Nastya", "Gosha", "Alex" };
        private static List<string> Objects = new List<string> { "file1", "file2", "file3", "file4" };
        private static List<string> LevelsSecurity = new List<string> { "NONCONFIDENTIAL", "CONFIDENTIAL", 
                                                                        "SECRET", "TOP SECRET" };
        private static int usersCount = Users.Count;
        private static int objectsCount = Objects.Count;

        private static bool userFlag = false;

        private static string command = "";
        private static string user = "";
        
        private static int read = 1;
        private static int write = 0;
        private static int[][] levelObjectConfidentiality = new int[objectsCount][];

        private static int[][] levelAccess = new int[usersCount][];
        private static int[][] savedLevelAccess = new int[usersCount][];
        private static Random rand = new Random();
        /// <summary>
        /// Идентификация пользователей
        /// </summary>
        static void Identification()
        {
            while (!userFlag && command != "exit")
            {
                Console.Write("User: ");
                command = Console.ReadLine();
                for (int i = 0; i < usersCount; i++)
                {
                    if (command == Users[i])
                    {
                        userFlag = true;
                        user = command;
                    }
                }
                if (command == "exit")
                {
                    Console.WriteLine("Выход из системы.");
                }
                else if (!userFlag)
                {
                    Console.WriteLine("Идентификация не прошла, попробуйте снова");
                }
                else
                {
                    Console.WriteLine("Идентификация прошла успешно, добро пожаловать в систему");
                    //PrintTableAccess(user);
                }
            }
        }
        /// <summary>
        /// Установка уровней доступа пользователей
        /// </summary>
        static void FillAccess(ref int[][] levelAccess)
        {
            for (int i = 0; i < usersCount; i++)
            {
                levelAccess[i] = new int[1];
                savedLevelAccess[i] = new int[1];
                if (Users.IndexOf("Admin") == i)
                {
                    levelAccess[i][0] = LevelsSecurity.Count-1; // присваиваем администратору наивысший уровень доступа
                }
                else
                {
                    levelAccess[i][0] = rand.Next(LevelsSecurity.Count - 1);
                }
                savedLevelAccess[i][0] = levelAccess[i][0];
            }
        }
        /// <summary>
        /// Установка уровней конфиденциальности объектов
        /// </summary>
        static void FillObjectConfidentiality(ref int[][] levelObjectConfidentiality)
        {
            for (int i = 0; i < objectsCount; i++)
            {
                levelObjectConfidentiality[i] = new int[1];
                levelObjectConfidentiality[i][0] = rand.Next(LevelsSecurity.Count-1); // присваиваем объекту случайный уровень конфиденциальности
            }
        }
        /// <summary>
        /// Печать сформированной модели БЛМ 
        /// </summary>
        static void PrintModelBLM()
        {
            //печать уровня конфиденциальности объектов
            Console.WriteLine("Objects:");
            for (int i = 0; i < objectsCount; i++)
            {
                Console.WriteLine($"#{i}    {Objects[i]}:       {LevelsSecurity[levelObjectConfidentiality[i][0]]}");
            }
            //печать уровня доступа субъектов(пользователей)
            Console.WriteLine("Subjects:");
            for (int i = 0; i < usersCount; i++)
            {
                Console.WriteLine($"#{i}    {Users[i]}:         {LevelsSecurity[levelAccess[i][0]]}");
            }
        }
        /// <summary>
        /// Поиск файла
        /// </summary>
        static void FindFile(out string file, int mode = 0)
        {
            string com = "";
            file = "";
            bool isNumeric;
            bool fileFlag = false;
            int number;
            while (file == "" && !fileFlag)
            {
                Console.Write("Над каким объектом производится операция? ");
                com = Console.ReadLine();
                isNumeric = int.TryParse(com, out number);
                if (!isNumeric)
                {
                    for (int i = 0; i < objectsCount; i++)
                    {
                        if (com == Objects[i])
                        {
                            file = Objects[i];
                            fileFlag = true;
                            break;
                        }
                    }
                }
                else
                {
                    if (number < objectsCount)
                    {
                        file = Objects[number];
                        fileFlag = true;
                    }
                }
                if (!fileFlag)
                {
                    Console.WriteLine("Данный файл не существует");
                }
            }
        }


        /// <summary>
        /// operation: read or write
        /// </summary>
        static void CheckAccess(string user, string file, int operation)
        {
            if ((operation == read) && (levelAccess[Users.IndexOf(user)][0] >= levelObjectConfidentiality[Objects.IndexOf(file)][0]))
            {
                Console.WriteLine("Операция прошла успешно!");
            }
            else if ((operation == write) && (levelAccess[Users.IndexOf(user)][0] <= levelObjectConfidentiality[Objects.IndexOf(file)][0])) 
            {
                Console.WriteLine("Операция прошла успешно!");
            }
            else
            {
                Console.WriteLine("Отказ в выполнении операции. Не соответствие уровня доступа");
            }
        }
        /// <summary>
        /// Чтение
        /// </summary>
        static void Read(string user)
        {
            FindFile(out string file);
            CheckAccess(user, file, read);
        }
        /// <summary>
        /// Запись
        /// </summary>
        /// <param name="user"></param>
        static void Write(string user)
        {
            FindFile(out string file);
            CheckAccess(user, file, write);
        }
        /// <summary>
        /// Изменение уровня доступа
        /// </summary>
        /// <param name="user"></param>
        static void Change(string user)
        {
            string com = "";
            Console.Write("Введите свой новый уровень доступа: ");
            com = Console.ReadLine();
            while (true || com =="no") 
            {
                if (LevelsSecurity.Contains(com))
                {
                    if (savedLevelAccess[Users.IndexOf(user)][0] >= LevelsSecurity.IndexOf(com))
                    {
                        Console.WriteLine($"Уровень доступа {user} изменен на {com}");
                        levelAccess[Users.IndexOf(user)][0] = LevelsSecurity.IndexOf(com);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Вы не можете установить себе данный уровень доступа. Если хотите выйти из режима изменения уровня доступа, напишете 'no' или Введите заново новый уровень доступа: ");
                        com = Console.ReadLine();
                        continue;
                    }

                }
                else
                {
                    Console.Write("Неверный уровень доступа. Если хотите выйти из режима изменения уровня доступа, напишете 'no' или Введите заново новый уровень доступа: ");

                    com = Console.ReadLine();
                }
            }
        }


        static void Main()
        {
            Console.WriteLine("Костылев И.А., гр. 4401, вариант 7"); // 9 субъектов доступа и 4 объекта доступа
            FillAccess(ref levelAccess);
            FillObjectConfidentiality(ref levelObjectConfidentiality);
            PrintModelBLM();

            while (command != "exit")
            {
                Identification();

                while ((command != "quit") && (command != "exit"))
                {
                    Console.Write("Жду ваших указаний > ");
                    command = Console.ReadLine();
                    if (command == "quit")
                    {
                        userFlag = false;
                    }
                    switch (command)
                    {
                        case "read":
                            Read(user);
                            break;
                        case "write":
                            Write(user);
                            break;
                        case "change":
                            Change(user);
                            break;
                        case "print":
                            PrintModelBLM();
                            break;
                        case "quit":
                            break;
                        case "exit":
                            Console.WriteLine("Вы точно хотите выйти? Если да, то напишите 'yes'");
                            command = Console.ReadLine();
                            if (command == "yes") { Console.WriteLine("Выход из системы."); command = "exit"; }
                            break;
                        case "":
                            break;
                        default:
                            Console.WriteLine("Неверная команда!");
                            break;
                    }
                }

            }
        }

    }
}