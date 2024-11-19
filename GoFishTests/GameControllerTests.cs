using ChotoProCard;
using GoFish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFishTests
{
    [TestClass]
    public class GameControllerTests
    {
        /// <summary>
        /// Инициализатор тестового класса
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            Player.Random = new MockRandom() { ValueToReturn = 0 }; //Объект рандо заменяется заглушком с заранее определённым значением
        }

        /// <summary>
        /// Тестируемый конструктор должен создать объект с определёнными параметрами и свойствами
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            //Создать объект Котроллер Игры с игроком человеком и тремя компьютерными игроками
            var gameController = new GameController("Человек", new List<string>() { "Игрок1", "Игрок2", "Игрок3" });
            //Статус объекта должен иметь следующий вид:
            Assert.AreEqual("Начинается новая игра с игроками: Человек, Игрок1, Игрок2, Игрок3",gameController.Status);
        }

        /// <summary>
        /// Тестируемый метод должен обновить статус игры после того как все игроки сделают свой ход
        /// </summary>
        [TestMethod]
        public void TestNextRound()
        {
            //Конструктор тасует колоду, но MockRandom следит за тем, чтобы она оставалась в порядке
            //итак, у Оуэна должно быть от туза до 5 бубен, у Бритни должно быть от 6 до 10 бубен
            var gameController = new GameController("Оуэн", new List<string>() { "Бриттни" }); //Создать новый игровой контроллер с двумя игроками
            gameController.NextRound(gameController.Opponents.First(), Values.Шестёрка);       //Сыграть раунд: Человек запрашивает Шестёрку
            Assert.AreEqual("Оуэн спрашивает: Бриттни есть ли у тебя Шестёрка?" +              //Статус должен содержать информацию об этом, а так же о том 
            Environment.NewLine + "Бриттни имеет 1 такую карту" +                                //Есть ли у игрока такая карта
            Environment.NewLine + "Бриттни спрашивает: Оуэн есть ли у тебя Семёрка?" +          //Иформацию о ходе компьютерного игрока
            Environment.NewLine + "Бриттни берёт карту" +                                      //Результат хода
            Environment.NewLine + "Оуэн имеет 6 карт и 0 книг" +                               //Информацию о игроке человеке
            Environment.NewLine + "Бриттни имеет 5 карт и 0 книг" +                            //Информацию о игроке компьютере
            Environment.NewLine + "Колода содержит 41 карту" +                                        //Информацию о состоянии колоды
            Environment.NewLine, gameController.Status);                                       //Всё это должно быть в статусе
        }
        /// <summary>
        /// Тестируемый метод, должен начать новую игру с теми же параметрами с которыми был создан Контроллер Игры
        /// </summary>
        [TestMethod]
        public void TestNewGame()
        {
            Player.Random = new MockRandom() { ValueToReturn = 0 };                             //Изменить значение заглушки
            var gameController = new GameController("Оуэн", new List<string>() { "Бриттни" });  //Создать объект с двумя игроками
            gameController.NextRound(gameController.Opponents.First(), Values.Шестёрка);        //Сыграть один раунд
            gameController.NewGame();                                                           //Начать новую игру
            Assert.AreEqual("Оуэн", gameController.HumanPlayer.Name);                           //Имя игрока человека должно сохраниться
            Assert.AreEqual("Бриттни", gameController.Opponents.First().Name);                  //Имя игрока компьютера должно сохраниться
            Assert.AreEqual("Начинается новая игра", gameController.Status);                    //Статус должен содержать сообщение о начале новой игры
        }
    }
}
