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
        /// ����� ������ ������ ������ ������ 5 ���� �� ������
        /// -����� ������� ����� �� ����� �� �������������� ������
        /// -��� ��������� ����� �������������� ��� ���� ����� �� �������������� ������
        /// ������ 5 ���� � ���������������� ������ ������ ��������� � ��������� ������ 
        /// </summary>
        [TestMethod]
        public void TestGetNextHand()
        {
            var player = new Player("��������", new List<Card>());                                 //������� ������ "�����"
            player.GetNextHand(new Deck());                                                        //������� ����� �������� ����� �� ����� ������
            CollectionAssert.AreEqual(new Deck().Take(5).Select(card => card.ToString()).ToList(), //����� �� ����� ������ 5 ����, �������� ������ ����
                                             player.Hand.Select(card => card.ToString()).ToList());//�������� ������ �� 5 ���� ������ ��� ���������
        }

        /// <summary>
        /// ����������� ����� ����� ����� � ���� � ������ ����������� �����, ������ �� �� ���� � ������� ������������ ��������� ����
        /// </summary>
        [TestMethod]
        public void TestDoYouHaveAny()
        {
            IEnumerable<Card> cards = new List<Card>()  //��������� ������ ���� �� 6� ����������� ����
            {
                new Card(Values.�����, Suits.���),
                new Card(Values.������, Suits.������),
                new Card(Values.������, Suits.����),
                new Card(Values.�������, Suits.�����),
                new Card(Values.������, Suits.�����),
                new Card(Values.�����, Suits.������),
            };
            var player = new Player("��������", cards); //������� ������, � ������� ����������� ������� ����
                                                        //�������� �� ���� ������ ����� ��������� ������
            var threes = player.DoYouHaveAny(Values.������, new Deck()).Select(Card => Card.ToString()).ToList();
            CollectionAssert.AreEqual(new List<string>()
            {
                "������ �����",                         //���������� ����� ������ ���������
                "������ ������",                        //� ������� ���������� ������� �����
                "������ ����",
            }, threes);
            Assert.AreEqual(3, player.Hand.Count());    //� ������ ������ �������� ��� �����
                                                        //�������� �� ���� ������ ����� ��������� �����
            var jacks = player.DoYouHaveAny(Values.�����, new Deck()).Select(Card => Card.ToString()).ToList();
            CollectionAssert.AreEqual(new List<string>()
            {
                "����� ������",                          //���������� ����� ������ ���������
                "����� ���",                         //� ������� ����������� ������� �������
            }, jacks);
                                                        //����� ���������� �����
            var hand = player.Hand.Select(Card => Card.ToString()).ToList();
                                                        //���������� ����� ������ ���� ������� �����
            CollectionAssert.AreEqual(new List<string>() { "������� �����" }, hand);
                                                        //������ ������ ������ ��������� � ����������� � ������ ��������� �������
            Assert.AreEqual("�������� ����� 1 ����� � 0 ����", player.Status);
        }

        /// <summary>
        /// ����� ������ ��������� ���������� ����� � ���� ������,
        /// � ����������� ������� ������ �� 4� ���� ����������� �������� � �����
        /// </summary>
        [TestMethod]
        public void TestAddCardsAndPullOutBooks()
        {
            IEnumerable<Card> cards = new List<Card>()  //�������� ������ ���� ��� �������� ������
            {
                new Card(Values.�����, Suits.���),
                new Card(Values.������, Suits.������),
                new Card(Values.�����, Suits.����),
                new Card(Values.������, Suits.����),
                new Card(Values.�������, Suits.�����),
                new Card(Values.�����, Suits.�����),
                new Card(Values.�����, Suits.������),
            };
            var player = new Player("��������", cards); //������� ������ ������ � ������� ����������� ������� ����
            Assert.AreEqual(0, player.Books.Count());   //� ������ ����� ���� 0 ���� ����� ��� ��������
            var cardsToAdd = new List<Card>()           //����� ������� ����� �������� ������ � ����
            {
                new Card(Values.������, Suits.����),
                new Card(Values.������, Suits.���),
            };
            player.AddCardsAndPullOutBooks(cardsToAdd); //�������� ������ ���� � ���� � �������� �����
            var books = player.Books.ToList();          //�������� ������ ���� ������� ����� �������
                                                        //����� ������ ��� �������� ����� � ������
            CollectionAssert.AreEqual(new List<Values>() { Values.������, Values.����� }, books); 
                                                        //������� ��� ����� �� ���� ������
            var hand = player.Hand.Select(Card => Card.ToString()).ToList(); 
                                                        //� ������ ����� �������� ������� �����
            CollectionAssert.AreEqual(new List<string>() { "������� �����" }, hand);
                                                        //������ ������ ������ ��������� � ����������� � ������ ���������
            Assert.AreEqual("�������� ����� 1 ����� � 2 �����", player.Status);
        }
        /// <summary>
        /// ����� ����� ������ ������ ����� � ���� �� ������
        /// </summary>
        [TestMethod]
        public void TestDrawCard()
        {
            var player = new Player("��������", new List<Card>());          //������� ������ � ������ �������
            player.DrawCard(new Deck());                                    //������ ������ ����� �� ����� ������
            Assert.AreEqual(1, player.Hand.Count());                        //� ������ ������ ���� ���� ����� � ����
            Assert.AreEqual("��� �����", player.Hand.First().ToString());   //� ������ � ���� ������ ���� ��� �����
        }
        /// <summary>
        /// ����� ������ ������� �������� ������� ����� �� ���� ������
        /// </summary>
        [TestMethod]
        public void TestRandomValueFromHand()
        {
            var player = new Player("��������", new Deck());                    //������� ������ � ���������������� �������
            Player.Random = new MockRandom() { ValueToReturn = 0 };             //������ ������ �������� �� �������� � ����������� ������
            Assert.AreEqual("���", player.RandomValueFromHand().ToString());    //������-��������� ����� ������ ����� ������� ���
            Player.Random = new MockRandom() { ValueToReturn = 4 };             
            Assert.AreEqual("������", player.RandomValueFromHand().ToString()); //������-��������� ����� ������ ����� ������� ������
            Assert.AreEqual("������", player.RandomValueFromHand().ToString()); //������-��������� ����� ������ ����� ������� ������
            Player.Random = new MockRandom() { ValueToReturn = 8 };
            Assert.AreEqual("������", player.RandomValueFromHand().ToString()); //������-��������� ����� ������ ����� ������� ������
        }
    }

    /// <summary>
    /// �������� ���������� �������� ��� ������������, ������� ������ ���������� ����������� ��������
    /// </summary>
    public class MockRandom : System.Random
    {
        public int ValueToReturn { get; set; } = 0;
        public override int Next() => ValueToReturn;
        public override int Next(int maxValue) => ValueToReturn;
        public override int Next(int minValue, int maxValue) => ValueToReturn;
    }
}