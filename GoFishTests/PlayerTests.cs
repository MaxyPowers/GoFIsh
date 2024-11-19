using GoFish;
using CardComparer;
using ChotoProCard;
using TwoDecks;

namespace GoFishTests
{
    [TestClass]
    public class PlayerTests
    {
        /// <summary>
        /// Метод должен выдать игроку первые 5 карт из колоды
        /// -Игрок получит карты из новой не перетасованной колоды
        /// -Для сравнения будет использоваться еще одна новая не перетасованная колода
        /// Первые 5 карт в неперетасованной колоде должны совпадать с выданными игроку 
        /// </summary>
        [TestMethod]
        public void TestGetNextHand()
        {
            var player = new Player("Владелец", new List<Card>());                                 //Создать объект "Игрок"
            player.GetNextHand(new Deck());                                                        //Вызвать метод получить карты из новой колоды
            CollectionAssert.AreEqual(new Deck().Take(5).Select(card => card.ToString()).ToList(), //Взять из новой колоды 5 карт, получить список карт
                                             player.Hand.Select(card => card.ToString()).ToList());//Получить список из 5 карт игрока для сравнения
        }

        /// <summary>
        /// Тестируемый метод дожен найти в руке у игрока релевантные карты, убрать их из руки и вернуть перечисление найденных карт
        /// </summary>
        [TestMethod]
        public void TestDoYouHaveAny()
        {
            IEnumerable<Card> cards = new List<Card>()  //Эммитация колоды карт из 6и определённых карт
            {
                new Card(Values.Валет, Suits.Пик),
                new Card(Values.Тройка, Suits.Крести),
                new Card(Values.Тройка, Suits.Черв),
                new Card(Values.Четвёрка, Suits.Бубен),
                new Card(Values.Тройка, Suits.Бубен),
                new Card(Values.Валет, Suits.Крести),
            };
            var player = new Player("Владелец", cards); //Создать игрока, с заранее определённым набором карт
                                                        //Отобрать из руки игрока карты номиналом Тройка
            var threes = player.DoYouHaveAny(Values.Тройка, new Deck()).Select(Card => Card.ToString()).ToList();
            CollectionAssert.AreEqual(new List<string>()
            {
                "Тройка Бубен",                         //Отобранные карты должны совпадать
                "Тройка Крести",                        //с заранее опредлённым списком троек
                "Тройка Черв",
            }, threes);
            Assert.AreEqual(3, player.Hand.Count());    //У игрока должно остаться три карты
                                                        //Отобрать из руки игрока карты номиналом Валет
            var jacks = player.DoYouHaveAny(Values.Валет, new Deck()).Select(Card => Card.ToString()).ToList();
            CollectionAssert.AreEqual(new List<string>()
            {
                "Валет Крести",                          //Отобранные карты должны совпадать
                "Валет Пик",                         //с заранее определённым списком Вальтов
            }, jacks);
                                                        //Найти оставшиеся карты
            var hand = player.Hand.Select(Card => Card.ToString()).ToList();
                                                        //Оставшаяся карты должна быть Четвёрка Бубен
            CollectionAssert.AreEqual(new List<string>() { "Четвёрка Бубен" }, hand);
                                                        //Статус игрока должен совпадать с определённым в первом параметре текстом
            Assert.AreEqual("Владелец имеет 1 карту и 0 книг", player.Status);
        }

        /// <summary>
        /// Метод должен добавлять полученные карты в руку игрока,
        /// и откладывать готовые наборы из 4х карт одинакового номинала в книги
        /// </summary>
        [TestMethod]
        public void TestAddCardsAndPullOutBooks()
        {
            IEnumerable<Card> cards = new List<Card>()  //Эмитация колоды карт для проверки метода
            {
                new Card(Values.Валет, Suits.Пик),
                new Card(Values.Тройка, Suits.Крести),
                new Card(Values.Валет, Suits.Черв),
                new Card(Values.Тройка, Suits.Черв),
                new Card(Values.Четвёрка, Suits.Бубен),
                new Card(Values.Валет, Suits.Бубен),
                new Card(Values.Валет, Suits.Крести),
            };
            var player = new Player("Владелец", cards); //Создать объект игрока с заранее определённым набором карт
            Assert.AreEqual(0, player.Books.Count());   //У игрока долно быть 0 книг после его создания
            var cardsToAdd = new List<Card>()           //Карты которые нужно добавить игроку в руку
            {
                new Card(Values.Тройка, Suits.Черв),
                new Card(Values.Тройка, Suits.Пик),
            };
            player.AddCardsAndPullOutBooks(cardsToAdd); //Добавить список карт в руку и выложить книги
            var books = player.Books.ToList();          //Получить список книг которые игрок отложил
                                                        //Игрок должен был отложить троки и вальты
            CollectionAssert.AreEqual(new List<Values>() { Values.Тройка, Values.Валет }, books); 
                                                        //Выбрать все карты из руки игрока
            var hand = player.Hand.Select(Card => Card.ToString()).ToList(); 
                                                        //У игрока дожны остаться Четвёрка Бубен
            CollectionAssert.AreEqual(new List<string>() { "Четвёрка Бубен" }, hand);
                                                        //Статус игрока должен совпадать с определённым в первом параметре
            Assert.AreEqual("Владелец имеет 1 карту и 2 книги", player.Status);
        }
        /// <summary>
        /// Метод дожен выдать игроку карту в руку из колоды
        /// </summary>
        [TestMethod]
        public void TestDrawCard()
        {
            var player = new Player("Владелец", new List<Card>());          //Создать игрока с пустой колодой
            player.DrawCard(new Deck());                                    //Выдать игроку карту из новой колоды
            Assert.AreEqual(1, player.Hand.Count());                        //У игрока должна быть одна карта в руке
            Assert.AreEqual("Туз Бубен", player.Hand.First().ToString());   //У игрока в руке должен быть Туз Бубен
        }
        /// <summary>
        /// Метод должен выбрать случайны номинал карты из руки игрока
        /// </summary>
        [TestMethod]
        public void TestRandomValueFromHand()
        {
            var player = new Player("Владелец", new Deck());                    //Создать игрока с неперетасованной колодой
            Player.Random = new MockRandom() { ValueToReturn = 0 };             //Объект рандом заменить на заглушку с определённым числом
            Assert.AreEqual("Туз", player.RandomValueFromHand().ToString());    //Псевдо-случайная карта должна иметь номинал Туз
            Player.Random = new MockRandom() { ValueToReturn = 4 };             
            Assert.AreEqual("Двойка", player.RandomValueFromHand().ToString()); //Псевдо-случайная карта должна иметь номинал Двойка
            Assert.AreEqual("Двойка", player.RandomValueFromHand().ToString()); //Псевдо-случайная карта должна иметь номинал Двойка
            Player.Random = new MockRandom() { ValueToReturn = 8 };
            Assert.AreEqual("Тройка", player.RandomValueFromHand().ToString()); //Псевдо-случайная карта должна иметь номинал Тройка
        }
    }

    /// <summary>
    /// Имитация случайного значения для тестирования, которая всегда возвращает определённое значение
    /// </summary>
    public class MockRandom : System.Random
    {
        public int ValueToReturn { get; set; } = 0;
        public override int Next() => ValueToReturn;
        public override int Next(int maxValue) => ValueToReturn;
        public override int Next(int minValue, int maxValue) => ValueToReturn;
    }
}