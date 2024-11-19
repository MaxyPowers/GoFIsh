using ChotoProCard;

namespace GoFish
{
    class Program
    {
        /// <summary>
        /// Игра Go Fish!
        /// </summary>
        static void Main(string[] args)
        {
            Console.Write("Введите ваше имя: ");
            var humanName = Console.ReadLine();                               //Считать введённое пользователем имя
            Console.Write("Введите количество компьютерных противников: ");
            int opponentCount;                                                //Переменная для количества компьютерных противников.
            while (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out opponentCount) || opponentCount < 1 || opponentCount > 4)
            {
                Console.WriteLine("Пожалуйста введите количество от 1 до 4"); //Если не удалось спарсить или количество не соответствует
            } 
            Console.WriteLine($"{Environment.NewLine}Добро пожаловать в игру, {humanName}");
            gameController = new GameController(humanName, Enumerable.Range(1, opponentCount).Select(i => $"Игрок #{i}"));
            Console.WriteLine(gameController.Status);                         //Вывести статус игры на консоль
            while (!gameController.GameOver)                                  //Выполнять пока GameOver не true
            {
                while (!gameController.GameOver)                              //Выполнять пока GameOver не true
                {
                    Console.WriteLine($"У вас в руке:");                      //Вывести на консоль карты игрока человека отсортированные по масти и номиналу
                    foreach (var card in gameController.HumanPlayer.Hand.OrderBy(card => card.Suit).OrderBy(card => card.Value))
                        Console.WriteLine(card);
                    var value = PromptForAValue();                            //Получить номинал карты который игрок человек хочет спросить
                    var player = PromptForAnOpponent();                       //Получить номер игрока у которого игрок хочет спросить карту
                    gameController.NextRound(player, value);                  //Сыграть раунд с полученными значениями
                    Console.WriteLine(gameController.Status);
                }
                Console.WriteLine("Нажмите Й для выхода, любую другую клавишу для новой игры.");
                if (Console.ReadKey(true).KeyChar.ToString().ToUpper() == "Т")
                    gameController.NewGame();
            }
        }

        /// <summary>
        /// Игровой контроллер для управления игрой
        /// </summary>
        static GameController gameController;

        /// <summary>
        /// Попросить игрока человека назвать номинал карты которая находится у него в руке
        /// </summary>
        /// <returns>Значение которое запросил игрок</returns>
        static Values PromptForAValue()
        {
            var handValues = gameController.HumanPlayer.Hand.Select(card => card.Value).ToList(); //Получить все карты из руки игрока
            Console.Write("Какой номинал карты вы хотите спросить? ");

            while (true) //Цикл будет повторяться пока пользователь не введёт корректное значение
            {
                //Если полученная с консоли строка преобразуется в Энам Values и в руке игрока содержится полученный номинал
                if (Enum.TryParse(typeof(Values), Console.ReadLine(), out var value) && handValues.Contains((Values)value))
                    return (Values)value; //Вернуть номинал запрашиваемый пользователем
                else
                    Console.WriteLine("Пожалуйста введите номинал карты которая есть у вас в руке.");
            }
        }
        /// <summary>
        /// Запрашивает у игрока противника у которого он хочет попросить карту
        /// </summary>
        /// <returns>Противник у оторого спрашивается карта</returns>
        static Player PromptForAnOpponent()
        {
            var opponents = gameController.Opponents.ToList();  //Выбрать всех противников
            for (int i = 1; i <= opponents.Count(); i++)        
                Console.WriteLine($"{i}. {opponents[i - 1]}");  //Вывусти на консоль всех противников
            Console.Write("У кого вы хотите спросить карту? "); 

            while (true) //Цикл будет выполняться пока пользователь не введёт корректное число
            {
                //Если введённое пользователем число в консоль преобразуется в int и находится в диапазоне от 1 до количества игроков противников
                if (int.TryParse(Console.ReadLine(), out int selection) && selection >= 1 && selection <= opponents.Count())
                    return opponents[selection - 1];            //Вернуть игрока противника по полученному индексу
                else
                    Console.Write($"Пожалуйста введите число от 1 до {opponents.Count()}: ");
            }
        }
    } 
}