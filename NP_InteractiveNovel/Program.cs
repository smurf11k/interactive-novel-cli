using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace NP_InteractiveNovel
{
    internal class Program
    {
        /// <summary>
        /// типи користувачів: admin, user, guest
        /// </summary>
        static String currentRole = "guest";
        static String currentUser = "";

        /// <summary>
        /// Шляхи до основних файлів
        /// </summary>
        static String PATH_TO_DATA = AppContext.BaseDirectory + "\\Data\\";
        static String PATH_TO_USER_DATA = PATH_TO_DATA + "UserData\\";
        static String PATH_TO_USERS = PATH_TO_USER_DATA + "users.txt";
        static String PATH_TO_USER_ROLES = PATH_TO_USER_DATA + "roles.txt";
        static String PATH_TO_SAVES = PATH_TO_USER_DATA + "userSaves.txt";
        static String PATH_TO_ABOUT = PATH_TO_DATA + "about.txt";
        static String PATH_TO_HELP = PATH_TO_DATA + "help.txt";
        static String PATH_TO_NOVEL = "";

        /// <summary>
        /// Обрана новела для гри
        /// </summary>
        static String novelName = "";

        /// <summary>
        /// Роздільник
        /// </summary>
        static char separator = '\t';

        /// <summary>
        /// Списки 
        /// </summary>
        static ArrayList userList = new ArrayList();
        static ArrayList userSaveList = new ArrayList();
        static List<UserSave> allUserSaves = new List<UserSave>();

        /// <summary>
        /// Структура даних користувача
        /// </summary>
        struct User
        {
            public string login;
            public string password;
            public string role;
            public string email;

            public string DataToString()
            {
                return login + "\t" + password + "\t" + role + "\t" + email;
            }

            public void Print()
            {
                Console.WriteLine("\t" +
                                    login + "\t" +
                                    role + "\t" +
                                    email);
            }
        }

        /// <summary>
        /// Структура даних збережених ігор користувачів
        /// </summary>
        struct UserSave
        {
            public string name;
            public string id;
            public string novel;

            public string SavesDataToString()
            {
                return name + "\t" + id + "\t" + novel;
            }
        }

        /// <summary>
        /// Точка входу
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            StartConfig();
            Console.Clear();
            Intro();
            MainMenu(currentRole);
        }

        /// <summary>
        /// Встановлення кольорів
        /// </summary>
        static void StartConfig()
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        /// <summary>
        /// Початкове вітання коритсувача
        /// </summary>
        static void Intro()
        {
            Console.Clear();
            Console.WriteLine(@"


                ███████╗████████╗ ██████╗ ██████╗ ██╗   ██╗     ██████╗  █████╗ ███╗   ███╗███████╗
                ██╔════╝╚══██╔══╝██╔═══██╗██╔══██╗╚██╗ ██╔╝    ██╔════╝ ██╔══██╗████╗ ████║██╔════╝
                ███████╗   ██║   ██║   ██║██████╔╝ ╚████╔╝     ██║  ███╗███████║██╔████╔██║█████╗  
                ╚════██║   ██║   ██║   ██║██╔══██╗  ╚██╔╝      ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝  
                ███████║   ██║   ╚██████╔╝██║  ██║   ██║       ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗
                ╚══════╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝   ╚═╝        ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝                                                                   
            
            ");
            Console.WriteLine("Натисніть довільну клавішу для продовження...");
            Console.ReadKey();
        }

        /// <summary>
        /// Пояснення управління та правил гри
        /// </summary>
        static void Help()
        {
            Console.Clear();
            TitleMessage("            Help");
            Console.WriteLine();
            if (ReadDataFromFile(PATH_TO_HELP) != null)
            {
                foreach (string line in ReadDataFromFile(PATH_TO_HELP))
                {
                    Console.WriteLine(line);
                }
            }
            else
            {
                AlertMessage("Файл не знайдено.");
            }
        }

        /// <summary>
        /// Довідка про програму
        /// </summary>
        static void About()
        {
            Console.Clear();
            if (ReadDataFromFile(PATH_TO_ABOUT) != null)
            {
                foreach (string line in ReadDataFromFile(PATH_TO_ABOUT))
                {
                    Console.WriteLine(line);
                }
            }
            else
            {
                AlertMessage("\nФайл не знайдено.");
            }
        }

        //MENU

        /// <summary>
        /// Головне меню
        /// </summary>
        /// <param name="role"> поточна роль користувача </param>
        static void MainMenu(string role)
        {
            Console.Clear();
            TitleMessage("        Головне меню");
            Console.WriteLine("[1] Авторизація");
            Console.WriteLine("[2] Довідка");
            if (role == "admin" || role == "user")
            {
                Console.WriteLine("[3] Грати");
                Console.WriteLine("[4] Help");
            }
            if (role == "admin")
            {
                Console.WriteLine("[5] Редагувати");
                Console.WriteLine("[6] Пошук");
            }
            Console.WriteLine("[ESC] Вихід\n");

            int userChoice = (int)Console.ReadKey().KeyChar;
            if (role == "guest" && (userChoice == 51 || userChoice == 52))
            {
                AlertMessage("\n\nУ вас недостатньо прав на використання цієї функції.");
            }
            else if (role != "admin" && (userChoice == 53 || userChoice == 54))
            {
                AlertMessage("\n\nУ вас недостатньо прав на використання цієї функції.");
            }
            else
            {
                switch (userChoice)
                {
                    case 49:
                        Authorisation();
                        break;
                    case 50:
                        About();
                        break;
                    case 27:
                        Environment.Exit(0);
                        break;
                    case 51:
                        Play();
                        break;
                    case 52:
                        Help();
                        break;
                    case 53:
                        EditingMenu();
                        break;
                    case 54:
                        SearchData();
                        break;
                    default:
                        AlertMessage("\n\nВиберіть один з вищевказаних варіантів.");
                        break;
                }
            }
            Console.WriteLine("\nНатисніть довільну клавішу для продовження...");
            Console.ReadKey();
            MainMenu(currentRole);
        }

        /// <summary>
        /// Меня для редагування даних 
        /// </summary>
        static void EditingMenu()
        {
            Console.Clear();
            TitleMessage("       Меню Редагування");
            Console.WriteLine("[1] Змінити роль користувача");
            Console.WriteLine("[2] Додати користувача");
            Console.WriteLine("[3] Видалити користувача");
            Console.WriteLine("[4] Подивитись список користувачів");
            Console.WriteLine("[5] Подивитись список збережених ігор користувачів");
            Console.WriteLine("[6] Видалити збережені ігри користувачів");
            Console.WriteLine("[7] Повернутися до Головного Меню");
            Console.WriteLine("[ESC] Вихід\n");

            int userChoice = (int)Console.ReadKey().KeyChar;
            switch (userChoice)
            {
                case 49:
                    ChangeRoles();
                    break;
                case 50:
                    AddUser();
                    break;
                case 51:
                    DeleteUser();
                    break;
                case 52:
                    Console.Clear();
                    ReadUsers();
                    ViewUsers();
                    break;
                case 53:
                    Console.Clear();
                    ViewUserSaves();
                    break;
                case 54:
                    DeleteUserSaves();
                    break;
                case 55:
                    MainMenu(currentRole);
                    break;
                case 27:
                    Environment.Exit(0);
                    break;
                default:
                    AlertMessage("\n\nВиберіть один з вищевказаних варіантів.");
                    break;
            }
            Console.WriteLine("\nНатисніть довільну клавішу для продовження...");
            Console.ReadKey();
            EditingMenu();
        }

        //*******************************************
        //USER LOGIN/REGISTER

        /// <summary>
        /// Авторизація
        /// </summary>
        static void Authorisation()
        {
            Console.Clear();
            TitleMessage("        Авторизація");
            if (!File.Exists(PATH_TO_USERS))
            {
                AlertMessage("\nФайл з користувачами не знайдено. Створення файлу");
                using (File.Create(PATH_TO_USERS)) { }
                Console.ReadKey();
                Registration();
                return;
            }
            SystemMessage("Вже маєте акаунт?(+/-). Натисніть будь-яку іншу клавішу для скасування.");
            char userChoice = Console.ReadKey().KeyChar;
            if (userChoice == '-')
            {
                Registration();
            }
            else if (userChoice == '+')
            {
                SystemMessage("\nЛогін:");
                string tmpLogin = Console.ReadLine();
                SystemMessage("Пароль:");
                string tmpPassword = Console.ReadLine();
                if (VerificationUser(tmpLogin, tmpPassword))
                {
                    currentUser = tmpLogin;
                    Console.WriteLine("\nВи авторизовані як " + currentUser);
                    Console.WriteLine("Ваша роль " + currentRole);
                }
                else
                {
                    AlertMessage("\nАвторизацію не здійснено.");
                }
            }
            else
            {
                AlertMessage("\n\nАвторизацію скасовано.");
            }
        }

        /// <summary>
        /// Реєстрація
        /// </summary>
        static void Registration()
        {
            Console.Clear();
            TitleMessage("         Реєстрація");
            SystemMessage("Логін:");
            string tmpLogin = Console.ReadLine();
            if (UserExists(tmpLogin))
            {
                AlertMessage("\nКористувач з таким логіном вже існує.");
                AlertMessage("\nРеєстрацію не здійснено.");
                return;
            }
            SystemMessage("Пароль:");
            string tmpPassword = Console.ReadLine();
            SystemMessage("E-mail:");
            string tmpEmail = Console.ReadLine();
            if (VerificationEmail(tmpEmail))
            {
                User user = new User();
                user.login = tmpLogin;
                user.password = tmpPassword;
                user.email = tmpEmail;
                user.role = "user";
                AppendDataToFile(PATH_TO_USERS, user.DataToString());
                SystemMessage("\nУспішно створено акаунт " + tmpLogin);
            }
            else
            {
                AlertMessage("\nРеєстрацію не здійснено.");
            }
        }

        /// <summary>
        /// Перевірка на існування користувача з таким самим логіном
        /// </summary>
        /// <param name="login"> введений логін </param>
        /// <returns></returns>
        static bool UserExists(string login)
        {
            if (ReadDataFromFile(PATH_TO_USERS) != null)
            {
                foreach (string strOneLine in ReadDataFromFile(PATH_TO_USERS))
                {
                    if (strOneLine.Split(separator)[0] == login)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Перевірка на існування користувача в базі
        /// </summary>
        /// <param name="log"> введений логін </param>
        /// <param name="pass"> введений пароль </param>
        /// <returns></returns>
        static bool VerificationUser(string log, string pass)
        {
            if (ReadDataFromFile(PATH_TO_USERS) != null)
            {
                foreach (string strOneLine in ReadDataFromFile(PATH_TO_USERS))
                {
                    if (strOneLine.Split(separator)[0] == log && strOneLine.Split(separator)[1] == pass)
                    {
                        currentRole = strOneLine.Split(separator)[2];
                        return true;
                    }
                    else if (strOneLine.Split(separator)[0] == log && strOneLine.Split(separator)[1] != pass)
                    {
                        AlertMessage("\nНеправильний пароль!");
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Перевірка на корректність email
        /// </summary>
        /// <param name="mail"> введений email </param>
        static bool VerificationEmail(string mail)
        {
            if (mail.Contains("@"))
            {
                return true;
            }
            AlertMessage("\nНеправильний email!");
            return false;
        }

        /// <summary>
        /// Перевірка на корректність ролі
        /// </summary>
        /// <param name="role"> введена роль </param>
        /// <returns></returns>
        static bool VerificationRole(string role)
        {
            if (File.Exists(PATH_TO_USER_ROLES))
            {
                if (File.ReadAllText(PATH_TO_USER_ROLES).IndexOf(role) > -1)
                {
                    return true;
                }
                else
                {
                    AlertMessage("\nНеправильна роль!");
                }
            }
            else
            {
                AlertMessage("\nФайл не знайдено.");
            }
            return false;
        }

        //*******************************************
        //EDITING FILES

        /// <summary>
        /// Зчитування файлів в масив рядків
        /// </summary>
        /// <param name="path"> шлях до файлу </param>
        /// <returns></returns>
        static Array ReadDataFromFile(string path)
        {
            Array tmpArrayString = null;
            if (!File.Exists(path))
            {
                return tmpArrayString;
            }
            return File.ReadAllLines(path);
        }

        /// <summary>
        /// Додавання даних у кінець файлу
        /// </summary>
        /// <param name="path"> шлях </param>
        /// <param name="line"> дані </param>
        static void AppendDataToFile(string path, string line)
        {
            if (!File.Exists(path))
            {
                AlertMessage("\nФайл не знайдено.");
            }
            else
            {
                File.AppendAllText(path, line + '\n');
            }
        }

        //*******************************************
        //EDITING MENU
        //ADMIN ONLY

        /// <summary>
        /// Пошук
        /// </summary>
        static void SearchData()
        {
            Console.Clear();
            TitleMessage("            Пошук");
            ChooseNovel();
            Console.Clear();
            TitleMessage("            Пошук");
            AlertMessage("Пошук сцен в новелі «" + novelName + "»");
            SystemMessage("\nВведіть ID сцени для пошуку:");
            string chosenId = Console.ReadLine().ToLower();
            Console.WriteLine();
            if (!File.Exists(PATH_TO_NOVEL + chosenId + ".txt"))
            {
                AlertMessage("\nСцен з таким ID не знайдено.");
            }
            else
            {
                FindId(chosenId);
            }
        }

        /// <summary>
        /// Пошук сцени за вибраним ID
        /// </summary>
        /// <param name="chosenId"> вибране ID сцени </param>
        static void FindId(string chosenId)
        {
            string[] separators = { "title:", "body:", "choice:" };
            string[] sections = File.ReadAllText(PATH_TO_NOVEL + chosenId + ".txt").Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string title = sections[0].Trim();
            string body = sections[1].Trim();

            SystemMessage("Заголовок:");
            Console.WriteLine(title + "\n");
            SystemMessage("Текст:");
            Console.WriteLine(body);
            if (sections.Length >= 3)
            {
                string choicesSection = sections[2].Trim();
                string[] choices = choicesSection.Split(';');
                SystemMessage("\nМожливі варіанти переходу:");
                if (choices.Length == 1)
                {
                    Console.WriteLine("(Автоматичний перехід) " + choices[0]);
                }
                else if (choices.Length > 1)
                {
                    for (int i = 0; i < choices.Length; i++)
                    {
                        Console.WriteLine(i + 1 + ") " + choices[i]);
                    }
                }
            }
            else
            {
                SystemMessage("\nНема переходу до інших сцен.");
            }
        }

        /// <summary>
        /// Зчитування користувачів
        /// </summary>
        static void ReadUsers()
        {
            userList.Clear();
            if (ReadDataFromFile(PATH_TO_USERS) != null)
            {
                foreach (string line in ReadDataFromFile(PATH_TO_USERS))
                {
                    User tmpUser = new User();
                    tmpUser.login = line.Split(separator)[0];
                    tmpUser.password = line.Split(separator)[1];
                    tmpUser.role = line.Split(separator)[2];
                    tmpUser.email = line.Split(separator)[3];
                    userList.Add(tmpUser);
                }
            }
        }

        /// <summary>
        /// Перегляд користувачів
        /// </summary>
        static void ViewUsers()
        {
            TitleMessage("         Користувачі");
            if (!File.Exists(PATH_TO_USERS))
            {
                AlertMessage("\nФайл з користувачами не знайдено.");
                return;
            }
            int i = 1;
            foreach (User user in userList)
            {
                Console.Write(i + ") ");
                user.Print();
                i++;
            }
        }

        /// <summary>
        /// Зміна ролі користувача
        /// </summary>
        static void ChangeRoles()
        {
            Console.Clear();
            TitleMessage("  Зміна ролей користувачів");
            if (!File.Exists(PATH_TO_USERS))
            {
                AlertMessage("\nФайл з користувачами не знайдено.");
                return;
            }
            ReadUsers();
            ViewUsers();
            bool validChoice = false;
            SystemMessage("Бажаєте змінити роль користувача? (+/-)");
            while (!validChoice)
            {
                char userChoice = Console.ReadKey().KeyChar;
                if (userChoice == '-')
                {
                    SystemMessage("\nОперацію скасовано.");
                    return;
                }
                else if (userChoice == '+')
                {
                    validChoice = true;
                    SystemMessage("\nВиберіть номер користувача для зміни ролі:");
                    int userIndex;
                    if (int.TryParse(Console.ReadLine(), out userIndex) && userIndex > 0 && userIndex < userList.Count + 1)
                    {
                        SystemMessage("Нова роль:");
                        string newRole = Console.ReadLine();
                        if (VerificationRole(newRole))
                        {
                            User tmpUser = (User)userList[userIndex - 1];
                            tmpUser.role = newRole;
                            userList[userIndex - 1] = tmpUser;
                            SystemMessage("Успішно змінено роль користувача " + tmpUser.login + "\n");
                            File.WriteAllText(PATH_TO_USERS, "");
                            foreach (User user in userList)
                            {
                                AppendDataToFile(PATH_TO_USERS, user.DataToString());
                            }
                            ViewUsers();
                        }
                    }
                    else
                    {
                        AlertMessage("\nНеправильний номер користувача!");
                    }
                }
                else
                {
                    AlertMessage("\nВиберіть щось з вищевказаних варіантів.");
                }
            }
        }

        /// <summary>
        /// Додавання користувача
        /// </summary>
        static void AddUser()
        {
            Console.Clear();
            TitleMessage("    Додавання користувачів");
            if (!File.Exists(PATH_TO_USERS))
            {
                AlertMessage("\nФайл з користувачами не знайдено.");
                return;
            }
            ReadUsers();
            ViewUsers();
            SystemMessage("Бажаєте додати нового користувача? (+/-)");
            bool validChoice = false;
            while (!validChoice)
            {
                char userChoice = Console.ReadKey().KeyChar;
                if (userChoice == '-')
                {
                    SystemMessage("\nОперацію скасовано.");
                    return;
                }
                else if (userChoice == '+')
                {
                    validChoice = true;
                    SystemMessage("\nЛогін:");
                    string tmpLogin = Console.ReadLine();
                    if (UserExists(tmpLogin))
                    {
                        AlertMessage("\nКористувач з таким логіном вже існує.");
                        return;
                    }
                    SystemMessage("Пароль:");
                    string tmpPassword = Console.ReadLine();
                    SystemMessage("Роль:");
                    string tmpRole = Console.ReadLine();
                    if (VerificationRole(tmpRole))
                    {
                        SystemMessage("E-mail:");
                        string tmpEmail = Console.ReadLine();
                        if (VerificationEmail(tmpEmail))
                        {
                            User user = new User();
                            user.login = tmpLogin;
                            user.password = tmpPassword;
                            user.email = tmpEmail;
                            user.role = tmpRole;
                            AppendDataToFile(PATH_TO_USERS, user.DataToString());
                            SystemMessage("\nУспішно додано користувача " + tmpLogin + "\n");
                            ReadUsers();
                            ViewUsers();
                        }
                        else
                        {
                            AlertMessage("\nНе вдалося додати новго користувача.");
                        }
                    }
                    else
                    {
                        AlertMessage("\nНе вдалося додати новго користувача.");
                    }
                }
                else
                {
                    AlertMessage("\nВиберіть щось з вищевказаних варіантів.");
                }
            }
        }

        /// <summary>
        /// Видалення користувача
        /// </summary>
        static void DeleteUser()
        {
            Console.Clear();
            TitleMessage("   Видалення користувачів");
            if (!File.Exists(PATH_TO_USERS))
            {
                AlertMessage("\nФайл з користувачами не знайдено.");
                return;
            }
            ReadUsers();
            ViewUsers();
            bool validChoice = false;
            SystemMessage("Бажаєте видалити користувача? (+/-)");
            while (!validChoice)
            {
                char userChoice = Console.ReadKey().KeyChar;
                if (userChoice == '-')
                {
                    SystemMessage("\nОперацію скасовано.");
                    return;
                }
                else if (userChoice == '+')
                {
                    validChoice = true;
                    SystemMessage("\nВиберіть номер користувача для видалення:");
                    int userIndex;
                    if (int.TryParse(Console.ReadLine(), out userIndex) && userIndex > 0 && userIndex < userList.Count + 1)
                    {
                        User deletedUser = (User)userList[userIndex - 1];
                        userList.RemoveAt(userIndex - 1);
                        SystemMessage("\nУспішно видалено користувача " + deletedUser.login + "\n");
                        File.WriteAllText(PATH_TO_USERS, "");
                        foreach (User user in userList)
                        {
                            AppendDataToFile(PATH_TO_USERS, user.DataToString());
                        }
                        ViewUsers();
                    }
                    else
                    {
                        AlertMessage("\nНеправильний номер користувача!");
                    }
                }
                else
                {
                    AlertMessage("\nВиберіть один з вищевказаних варіантів.");
                }
            }
        }

        /// <summary>
        /// Зчитування збережених ігор користувачів
        /// </summary>
        static void ReadUserSaves()
        {
            userSaveList.Clear();
            if (ReadDataFromFile(PATH_TO_SAVES) != null)
            {
                foreach (string line in ReadDataFromFile(PATH_TO_SAVES))
                {
                    UserSave tmpUserSave = new UserSave();
                    tmpUserSave.name = line.Split(separator)[0];
                    tmpUserSave.id = line.Split(separator)[1];
                    tmpUserSave.novel = line.Split(separator)[2];
                    if (tmpUserSave.novel == novelName)
                    {
                        userSaveList.Add(tmpUserSave);
                    }
                }
            }
        }

        /// <summary>
        /// Перегляд збережених ігор користувачів
        /// </summary>
        static void ViewUserSaves()
        {
            allUserSaves.Clear();
            TitleMessage("       Збережені ігри");
            if (ReadDataFromFile(PATH_TO_SAVES) != null)
            {
                int i = 1;
                foreach (string line in ReadDataFromFile(PATH_TO_SAVES))
                {
                    UserSave tmpUserSave = new UserSave();
                    tmpUserSave.name = line.Split(separator)[0];
                    tmpUserSave.id = line.Split(separator)[1];
                    tmpUserSave.novel = line.Split(separator)[2];
                    allUserSaves.Add(tmpUserSave);
                }
                foreach (UserSave userSave in allUserSaves)
                {
                    Console.WriteLine(i + ") " + userSave.name + "\t" + userSave.novel);
                    i++;
                }
            }
            else
            {
                AlertMessage("\nФайл із збереженими іграми користувачів не знайдено.");
            }
        }

        /// <summary>
        /// Видалення збережених ігор користувачів
        /// </summary>
        static void DeleteUserSaves()
        {
            Console.Clear();
            TitleMessage("  Видалення збережених ігор");
            if (!File.Exists(PATH_TO_SAVES))
            {
                AlertMessage("\nФайл із збереженими іграми користувачів не знайдено.");
                return;
            }
            ReadUserSaves();
            ViewUserSaves();
            bool validChoice = false;
            SystemMessage("Бажаєте видалити прогрес користувача? (+/-)");
            while (!validChoice)
            {
                char userChoice = Console.ReadKey().KeyChar;
                if (userChoice == '-')
                {
                    SystemMessage("\nОперацію скасовано.");
                    return;
                }
                else if (userChoice == '+')
                {
                    validChoice = true;
                    SystemMessage("\nВиберіть номер користувача для видалення прогресу:");
                    int userIndex;
                    if (int.TryParse(Console.ReadLine(), out userIndex) && userIndex > 0 && userIndex < allUserSaves.Count + 1)
                    {
                        UserSave deletedSave = (UserSave)allUserSaves[userIndex - 1];
                        allUserSaves.RemoveAt(userIndex - 1);
                        SystemMessage("\nУспішно видалено прогрес користувача " + deletedSave.name + "\n");
                        File.WriteAllText(PATH_TO_SAVES, "");
                        foreach (UserSave del in allUserSaves)
                        {
                            AppendDataToFile(PATH_TO_SAVES, del.SavesDataToString());
                        }
                        ViewUserSaves();
                    }
                    else
                    {
                        AlertMessage("\nНеправильний номер користувача!");
                    }
                }
                else
                {
                    AlertMessage("\nВиберіть один з вищевказаних варіантів.");
                }
            }
        }

        //*******************************************
        //GAME

        /// <summary>
        /// Грати
        /// </summary>
        static void Play()
        {
            Console.Clear();
            ChooseNovel();
            ReadUserSaves();
            if (CurrentUserScene() != "" && CurrentUserNovel() != "")
            {
                SystemMessage("Бажаєте продовжити збережену гру? (+/-). Натисніть будь-яку іншу клавішу для повернення до Головного Меню.");
                char userChoice = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (userChoice == '+')
                {
                    PATH_TO_NOVEL = PATH_TO_DATA + "Novel\\" + CurrentUserNovel() + "\\";
                    RunGame(CurrentUserScene());
                }
                else if (userChoice == '-')
                {
                    DefaultStart();
                }
                else
                {
                    MainMenu(currentRole);
                }
            }
            else
            {
                DefaultStart();
            }
        }

        /// <summary>
        /// Старт нової гри
        /// </summary>
        static void DefaultStart()
        {
            SystemMessage("Нова гра.");
            Console.ReadKey();
            RunGame("start");
        }

        /// <summary>
        /// Зчитування тексту сцен з файлів
        /// </summary>
        /// <param name="idScene"> ID файлу з зчитується текст історії </param>
        static void RunGame(string idScene)
        {
            Console.Clear();
            if (!File.Exists(PATH_TO_NOVEL + idScene + ".txt"))
            {
                AlertMessage("\nФайл не знайдено.");
            }
            else
            {
                string[] separators = { "title:", "body:", "choice:" };
                string[] sections = File.ReadAllText(PATH_TO_NOVEL + idScene + ".txt").Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (sections.Length < 2)
                {
                    AlertMessage("\nНеправильна структура файлу!");
                }
                else
                {
                    string title = sections[0].Trim();
                    string body = sections[1].Trim();
                    Console.WriteLine(title + "\n");
                    Console.WriteLine(body);
                    ProcessChoice(sections);
                }
            }
        }

        /// <summary>
        /// Опрацювання вибору користувача
        /// </summary>
        /// <param name="sections"> секції (title, body та choice) </param>
        static void ProcessChoice(string[] sections)
        {
            if (sections.Length >= 3)
            {
                string choicesSection = sections[2].Trim();
                string[] choices = choicesSection.Split(';');
                if (choices.Length == 1)
                {
                    OneChoice(choices[0]);
                }
                else if (choices.Length > 1)
                {
                    MultipleChoices(choices);
                }
            }
            else
            {
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Автоматичний перехід до наступної сцени
        /// Можливе збереження прогресу користувача або поверненя до Головного Меню
        /// </summary>
        /// <param name="sceneId"> id сцени </param>
        static void OneChoice(string sceneId)
        {
            bool validChoice = false;
            SystemMessage("\nБажаєте зберегти свій прогрес? (+/-) або натисніть 0 для повернення до Головного Меню.");
            while (!validChoice)
            {
                char userChoice = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (userChoice)
                {
                    case '+':
                        validChoice = true;
                        SaveProgress(sceneId);
                        Console.ReadKey();
                        RunGame(sceneId);
                        break;
                    case '-':
                        validChoice = true;
                        RunGame(sceneId);
                        break;
                    case '0':
                        validChoice = true;
                        MainMenu(currentRole);
                        break;
                    default:
                        AlertMessage("\nВиберіть щось із вищевказаних варіантів.");
                        break;
                }
            }
        }

        /// <summary>
        /// Перехід до наступної сцени за вибором користувача
        /// </summary>
        /// <param name="choices"> варіанти вибору (id сцен) </param>
        static void MultipleChoices(string[] choices)
        {
            int userChoice;
            while (!int.TryParse(Console.ReadLine(), out userChoice) || userChoice < 1 || userChoice > choices.Length)
            {
                AlertMessage("\nВиберіть щось із вищевказаних варіантів.");
            }
            RunGame(choices[userChoice - 1]);
        }

        /// <summary>
        /// Вибір новели
        /// </summary>
        static void ChooseNovel()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(PATH_TO_DATA + "Novel\\");
            DirectoryInfo[] directories = dirInfo.GetDirectories();
            TitleMessage("       Доступні новели");
            for (int i = 0; i < directories.Length; i++)
            {
                Console.WriteLine(i + 1 + ") " + directories[i].Name);
            }
            Console.WriteLine("\nВиберіть номер новели:\n");
            int userChoice;
            bool validChoice = false;
            while (!validChoice)
            {
                if (int.TryParse(Console.ReadLine(), out userChoice) && userChoice > 0 && userChoice < directories.Length + 1)
                {
                    validChoice = true;
                    PATH_TO_NOVEL = PATH_TO_DATA + "Novel\\" + directories[userChoice - 1].Name + "\\";
                    novelName = directories[userChoice - 1].Name;
                }
                else
                {
                    AlertMessage("\nВиберіть щось із вищевказаних варіантів.");
                }
            }
        }

        /// <summary>
        /// Поточна сцена
        /// </summary>
        /// <returns> id сцени </returns>
        static string CurrentUserScene()
        {
            ReadUserSaves();
            foreach (UserSave us in userSaveList)
            {
                if (us.name == currentUser)
                {
                    return us.id;
                }
            }
            return "";
        }

        /// <summary>
        /// Поточна новела
        /// </summary>
        /// <returns> назву новели </returns>
        static string CurrentUserNovel()
        {
            ReadUserSaves();
            foreach (UserSave us in userSaveList)
            {
                if (us.name == currentUser)
                {
                    return us.novel;
                }
            }
            return "";
        }

        /// <summary>
        /// Збереження прогресу користувача
        /// </summary>
        static void SaveProgress(string chapterId)
        {
            UserSave save = new UserSave();
            save.name = currentUser;
            save.id = chapterId;
            save.novel = novelName;
            if (!File.Exists(PATH_TO_SAVES))
            {
                AlertMessage("\nФайл не знайдено. Створення файлу...\n");
                using (File.Create(PATH_TO_SAVES)) { }
                Console.ReadKey();
            }
            string[] lines = File.ReadAllLines(PATH_TO_SAVES);
            bool userSaveExists = false;
            foreach (string line in lines)
            {
                string[] parts = line.Split(separator);
                if (parts[0] == currentUser && parts[2] == novelName)
                {
                    userSaveExists = true;
                    break;
                }
            }
            if (!userSaveExists)
            {
                AppendDataToFile(PATH_TO_SAVES, save.SavesDataToString());
            }
            else
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(separator);
                    if (parts[0] == currentUser && parts[2] == novelName)
                    {
                        lines[i] = save.SavesDataToString();
                        break;
                    }
                }
                File.WriteAllText(PATH_TO_SAVES, "");
                foreach (string line in lines)
                {
                    AppendDataToFile(PATH_TO_SAVES, line);
                }
            }
            SystemMessage("Гру збережено.");
        }

        //*******************************************
        //MESSAGES

        /// <summary>
        /// Тривожне повідомлення
        /// </summary>
        /// <param name="message"> текст повідомлення </param>
        static void AlertMessage(string message)
        {
            ConsoleColor backgroundColor = ConsoleColor.DarkRed;
            ConsoleColor foregroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
        }

        /// <summary>
        /// Системне повідомлення
        /// </summary>
        /// <param name="message"> текст повідомлення </param>
        static void SystemMessage(string message)
        {
            ConsoleColor backgroundColor = ConsoleColor.DarkRed;
            ConsoleColor foregroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(message);
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
        }

        /// <summary>
        /// Заголовок
        /// </summary>
        /// <param name="message"> текст заголовку </param>
        static void TitleMessage(string message)
        {
            Console.WriteLine("┏━━━━━━━━━━━━━▲━━━━━━━━━━━━━┓");
            Console.WriteLine(message);
            Console.WriteLine("┗━━━━━━━━━━━━━▼━━━━━━━━━━━━━┛");
        }
    }
}
