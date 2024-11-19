using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardComparer;
using ChotoProCard;
using GoFish;
using TwoDecks;

namespace GoFishTests
{
    [TestClass]
    public class GameStateTests
    {
        /// <summary>
        /// Метод тестирует конструктор. После создания объект Состояния игры должен содержать всех игроков с картами на руках
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            var computerPlayerNames = new List<string>()                              //Списаок с именами игроков компьютеров
            {
                "Комп1",
                "Комп2",
                "Комп3",
            };
            var gameState = new GameState("Человек", computerPlayerNames, new Deck());//Создать объект Состояния игры с игроком человеком и тремя компьютерами
                                                                                      //Сравнить списки игроков
            CollectionAssert.AreEqual(new List<string> { "Человек", "Комп1", "Комп2", "Комп3" }, gameState.Players.Select(player => player.Name).ToList());
            Assert.AreEqual(5, gameState.HumanPlayer.Hand.Count());                   //Сравнить количество карт у игрока
        }

        /// <summary>
        /// Тестируемый метод, должен выбирать случайных игроков
        /// </summary>
        [TestMethod]
        public void TestRandomPlayer()
        {
            var computerPlayerNames = new List<string>()                               //Списаок с именами игроков компьютеров
            {
                "Комп1",
                "Комп2",
                "Комп3",
            };
            var gameState = new GameState("Человек", computerPlayerNames, new Deck());//Создать объект Состояния игры с игроком человеком и тремя компьютерами
            Player.Random = new MockRandom() { ValueToReturn = 1 };                   //Заменить обьект рандом, заглушкой с определённым значением
            Assert.AreEqual("Комп2", gameState.RandomPlayer(gameState.Players.ToList()[0]).Name);  //Метод должен вернуть Комп2
            Player.Random = new MockRandom() { ValueToReturn = 0 };                                //Изменить значение заглушки
            Assert.AreEqual("Человек", gameState.RandomPlayer(gameState.Players.ToList()[1]).Name);//Метод должен вернуть человека
            Assert.AreEqual("Комп1", gameState.RandomPlayer(gameState.Players.ToList()[0]).Name);  //Метод должен вернуть Комп1
        }

        /// <summary>
        /// тестируемы метод должен провести раунд игры с заранее задаными значениями
        /// </summary>
        [TestMethod]
        public void TestPlayRound()
        {
            var deck = new Deck();                       //Новая колода для игры
            deck.Clear();                                //Очистить колоду
            var cardsToAdd = new List<Card>()            //Список определённых карт для игры
            {
                                                         //карты которые игра раздаст Owen
                new Card(Values.Валет, Suits.Пик),
                new Card(Values.Валет, Suits.Черв),
                new Card(Values.Шестёрка, Suits.Пик),
                new Card(Values.Валет, Suits.Бубен),
                new Card(Values.Шестёрка, Suits.Черв),
                                                         //карты которые раздаст Бритни
                new Card(Values.Шестёрка, Suits.Бубен),
                new Card(Values.Шестёрка, Suits.Крести),
                new Card(Values.Семёрка, Suits.Пик),
                new Card(Values.Валет, Suits.Пик),
                new Card(Values.Девятка, Suits.Пик),
                                                        //Еще две карты в колоде для Оуэна, которые он возьмёт когда в колоде закончатся карты
                new Card(Values.Дама, Suits.Черв),
                new Card(Values.Король, Suits.Пик),
            };

            foreach (var card in cardsToAdd)            //Добавить карты в колоду
            {
                deck.Add(card);
            }

            var gameState = new GameState("Оуэн", new List<string>() { "Бриттни" }, deck); //Создать объект с двумя игроками и специальной колодой
            var owen = gameState.HumanPlayer;           //Сохранить Оуэна
            var brittney = gameState.Opponents.First(); //Сохранить бритни
            Assert.AreEqual("Оуэн", owen.Name);         //Проверить совпадает ли имя игрока человека
            Assert.AreEqual(5, owen.Hand.Count());      //Проверить получил человек все карты
            Assert.AreEqual("Бриттни", brittney.Name);  //Проверить имя игрока компьютера
            Assert.AreEqual(5, brittney.Hand.Count());  //Проверить получили ли компьютер все карты
            var message = gameState.PlayRound(owen, brittney, Values.Валет, deck); //Сыграть раунд и сохранить сообщение о состоянии игры
                                                                                   //Сообщение должно иметь следующий вид:
            Assert.AreEqual("Оуэн спрашивает: Бриттни есть ли у тебя Валет?" + Environment.NewLine + "Бриттни имеет 1 такую карту", message);
            Assert.AreEqual(1, owen.Books.Count());                                //Игрок человек собрал 1 книгу
            Assert.AreEqual(2, owen.Hand.Count());                                 //У игрока человека осталось 2 карты
            Assert.AreEqual(0, brittney.Books.Count());                            //Игрок компьютер не собрал книг
            Assert.AreEqual(4, brittney.Hand.Count());                             //У игрока копьютера осталось 4 карты
            message = gameState.PlayRound(brittney, owen, Values.Шестёрка, deck);  //Сыграть следующий раунд и сохранить новое сообщение о сотоянии игры
                                                                                   //Новое сообщение должно содержать:
            Assert.AreEqual("Бриттни спрашивает: Оуэн есть ли у тебя Шестёрка?" + Environment.NewLine + "Оуэн имеет 2 такие карты", message);
            Assert.AreEqual(1, owen.Books.Count());                                //Игрок человек всё еще имеет 1 книгу
            Assert.AreEqual(2, owen.Hand.Count());                                 //У игрока человека осталось 2 карты
            Assert.AreEqual(1, brittney.Books.Count());                            //Игрок компьютер собрал 1 книгу
            Assert.AreEqual(2, brittney.Hand.Count());                             //У игрока компьютера осталось 2 карты
            message = gameState.PlayRound(owen, brittney, Values.Дама, deck);      //Создать объект Состояния игры с игроком человеком и тремя компьютерами
                                                                                   //Сообщение должно иметь следующий вид:
            Assert.AreEqual("Оуэн спрашивает: Бриттни есть ли у тебя Дама?" + Environment.NewLine + "В колоде закончились карты", message);
            Assert.AreEqual(1, owen.Books.Count());                                //Игрок человек имеет 1 книгу
            Assert.AreEqual(2, owen.Hand.Count());                                 //У игрока человека осталось 2 карты
        }

        /// <summary>
        /// Тестируемый метод должен определить есть ли победители в игре
        /// </summary>
        [TestMethod]
        public void TestCheckForAWinner()
        {
            var computerPlayerNames = new List<string>() //Списаок с именами игроков компьютеров
            {
                "Компьютер1",
                "Компьютер2",
                "Компьютер3",
            };
            var emptyDeck = new Deck();                  //Пустая колода для игры
            emptyDeck.Clear();                           //Очистить колоду
            var gameState = new GameState("Человек", computerPlayerNames, emptyDeck);                                   
            Assert.AreEqual("Победители: Человек и Компьютер1 и Компьютер2 и Компьютер3", gameState.CheckForWinner());  //Все должны победить
        }
    }
}
