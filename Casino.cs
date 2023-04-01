using System;
using System.Collections.Generic;

namespace Cards
{
    class Program
    {
        static void Main(string[] args)
        {
            Casino casino = new Casino();
            casino.StartWork();
        }
    }
}

class Card
{
    public Card(string suit, string meaning)
    {
        Suit = suit;
        Meaning = meaning;
    }

    public string Suit { get; private set; }

    public string Meaning { get; private set; }
}

class Deck
{
    private string[] _suits = { "черви", "пики", "кресте", "бубы" };
    private string[] _meanings = { "6", "7", "8", "9", "10", "валет", "дама", "король", "туз" };
    public List<Card> Cards { private set; get; }

    public Deck()
    {
        Cards = new List<Card>();
        CreateCards();
    }

    public void CreateCards()
    {
        for (int i = 0; i < _suits.Length; i++)
        {
            for (int j = 0; j < _meanings.Length; j++)
            {
                Cards.Add(new Card(_suits[i], _meanings[j]));
            }
        }
    }
}

class Player
{
    private List<Card> _cardsInHand;

    public Player()
    {
        _cardsInHand = new List<Card>();
    }

    public void ShowCards()
    {
        foreach (Card card in _cardsInHand)
            Console.WriteLine(card.Suit + " " + card.Meaning);
    }

    public void AddCard(Card card)
    {
        _cardsInHand.Add(card);
    }
}

class Croupier
{
    private Deck _deck;
    private ShuffleMachine _shuffleMachine = new ShuffleMachine();

    public Croupier()
    {
        _deck = new Deck();
    }

    public void Shuffle()
    {
        _shuffleMachine.MixCards(_deck);
    }

    public Card GiveCard()
    {
        int countCards = _deck.Cards.Count;

        if (countCards > 0)
        {
            Card newCard = _deck.Cards[0];
            _deck.Cards.RemoveAt(0);
            return newCard;
        }
        else
        {
            Console.WriteLine("Больше карт нету!");
            return null;
        }
    }

    public void ShowCards()
    {
        foreach (Card card in _deck.Cards)
            Console.Write(card.Suit + " " + card.Meaning);
    }
}

class ShuffleMachine
{
    public List<Card> MixCards(Deck deck)
    {
        Random random = new Random();
        Card buferCard;
        List<Card> cards = deck.Cards;
        int countCards = cards.Count;

        for (int i = 0; i < countCards; i++)
        {
            int newIndex = random.Next(countCards);
            buferCard = cards[newIndex];
            cards[newIndex] = cards[i];
            cards[i] = buferCard;
        }

        return cards;
    }
}

class Casino
{
    bool isWorking = true;
    Player player = new Player();
    Croupier croupier = new Croupier();
    ShuffleMachine mixingMachine = new ShuffleMachine();

    public void StartWork()
    {
        const int OPTION_GET_CARD = 1;
        const int OPTION_SHOW_CARDS = 2;
        const int OPTION_EXIT = 3;
        const int OPTION_SHOW_REMAINING_CARDS = 4;
        const int OPTION_MIX_CARDS = 5;

        Console.WriteLine("Приветствую!");

        while (isWorking)
        {
            Console.WriteLine("Выберите действие: \n" +
                $"{OPTION_GET_CARD} - взять карту;\n" +
                $"{OPTION_SHOW_CARDS} - вскрыть карты(показать)\n" +
                $"{OPTION_EXIT} - выйти;\n" +
                $"{OPTION_SHOW_REMAINING_CARDS} - показать оставшиеся карты;\n" +
                $"{OPTION_MIX_CARDS} - перемешать колоду;");

            switch (GetNumber())
            {
                case OPTION_GET_CARD:
                    TransferTheCardToThePlayer();
                    break;

                case OPTION_SHOW_CARDS:
                    player.ShowCards();
                    break;

                case OPTION_EXIT:
                    isWorking = false;
                    break;

                case OPTION_SHOW_REMAINING_CARDS:
                    croupier.ShowCards();
                    break;

                case OPTION_MIX_CARDS:
                    croupier.Shuffle();
                    break;

                default:
                    Console.WriteLine("Вы ввели некоректное значение!");
                    break;
            }

            Console.Write("Нажмите любую клавишу для продолжения.");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine();
        }
    }

    private void TransferTheCardToThePlayer()
    {
        var newCard = croupier.GiveCard();
        if (newCard != null)
            player.AddCard(newCard);
    }


    private int GetNumber()
    {
        int result = 0;
        bool isWorking = true;

        while (isWorking)
        {
            if (int.TryParse(Console.ReadLine(), out result))
                isWorking = false;
            else
                Console.WriteLine("Вы ввели некоректное чтсло! Попробуйте еще раз.");
        }

        return result;
    }
}