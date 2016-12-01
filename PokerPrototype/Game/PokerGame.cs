using System;
using System.Collections.Generic;
using System.Threading;
using HoldemHand;
//using System.Security.Cryptography;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace PokerGame
{
    //http://stackoverflow.com/questions/273313/randomize-a-listt
    //So I honestly have no idea what this does
    //Copied from link above, should improve Random so that our shuffle can handle 
    //threads, and reduce shuffling in predictable ways
    public static class ThreadSafeRandom
    {
        [ThreadStatic]
        private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }
    public class Card
    {
        //future note, treat all 1's as spades. Pseudo "14" and "1"
        //using values 1-13, where 1 is ace, 11 is jack, 12 is queen, and 13 is king
        //represent as string in order for hand scoring using Keith's Rule (Format "As"= ace of spades)
        public string value { get; set; }
        public void printCard()
        {
            Console.WriteLine("{0}", this.value);
        }
    }
    public class Deck
    {
        //General Master List of cards. We're keeping this just in case
        private Card[] cards;
        //List of all cards currently located in the deck
        List<Card> deckCards;
        //List of all cards currently in game. Either on table, or in hand
        List<Card> gameCards;
        //constructor creates array of 52 cards to serve as a deck
        public Deck()
        {
            cards = new Card[52];
            //initialize empty lists
            deckCards = new List<Card> { };
            gameCards = new List<Card> { };
            int i = 0;
            //Adding Spades Cards
            for (i = 0; i < 52; i++)
            {
                cards[i] = new Card();
            }
            cards[0].value = "2s";
            cards[1].value = "2h";
            cards[2].value = "2d";
            cards[3].value = "2c";
            cards[4].value = "3s";
            cards[5].value = "3h";
            cards[6].value = "3d";
            cards[7].value = "3c";
            cards[8].value = "4s";
            cards[9].value = "4h";
            cards[10].value = "4d";
            cards[11].value = "4c";
            cards[12].value = "5s";
            cards[13].value = "5h";
            cards[14].value = "5d";
            cards[15].value = "5c";
            cards[16].value = "6s";
            cards[17].value = "6h";
            cards[18].value = "6d";
            cards[19].value = "6c";
            cards[20].value = "7s";
            cards[21].value = "7h";
            cards[22].value = "7d";
            cards[23].value = "7c";
            cards[24].value = "8s";
            cards[25].value = "8h";
            cards[26].value = "8d";
            cards[27].value = "8c";
            cards[28].value = "9s";
            cards[29].value = "9h";
            cards[30].value = "9d";
            cards[31].value = "9c";
            cards[32].value = "10s";
            cards[33].value = "10h";
            cards[34].value = "10d";
            cards[35].value = "10c";
            cards[36].value = "js";
            cards[37].value = "jh";
            cards[38].value = "jd";
            cards[39].value = "jc";
            cards[40].value = "qs";
            cards[41].value = "qh";
            cards[42].value = "qd";
            cards[43].value = "qc";
            cards[44].value = "ks";
            cards[45].value = "kh";
            cards[46].value = "kd";
            cards[47].value = "kc";
            cards[48].value = "as";
            cards[49].value = "ah";
            cards[50].value = "ad";
            cards[51].value = "ac";
            for (i = 0; i < 52; i++)
            {
                deckCards.Add(cards[i]);
            }
        }
        //CITE:http://stackoverflow.com/questions/273313/randomize-a-listt 
        public void shuffle()
        {
            int n = deckCards.Count;//for amount of cards in deck currently
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                Card temp = deckCards[k];
                deckCards[k] = deckCards[n];
                deckCards[n] = temp;
            }

        }
        //draw continually from index 0, which is the top of the deck
        public bool draw()
        {
            //make sure deck actually has a card to draw
            if (deckCards.Count > 0)
            {
                //retrieve card from deck
                Card drawn = deckCards[0];
                //remove from deck
                deckCards.RemoveAt(0);
                //add to lsit of cards in game
                gameCards.Add(drawn);
                Console.WriteLine("Drawn: ");
                drawn.printCard();
                return true;
            }
            return false;
        }
        //takes all cards in play, readds them to deck, then reshuffles
        public void cleanup()
        {
            //re-add from latest index to 0 for simplicity
            //and to work around removes
            while (gameCards.Count > 0)
            {
                deckCards.Add(gameCards[0]);
                gameCards.RemoveAt(0);
            }
            //deck now contains all cards again
            shuffle();
        }
//TESTING FUNCTIONS-----------------------------------------------------------------------------------
//These functions are prefaced with either "check" or "print", and are used for testing exclusively
        public bool checkShuffle()
        {
            //first shuffle, then print deck for visual comparison
            shuffle();
            printDeck();
            //code chunk checks for duplicate cards
            //for every card currently in the deck
            for (int i = 0; i < deckCards.Count; i++)
            {

                //go through the rest of the deck
                for (int x = 0; x < deckCards.Count; x++)
                {
                    //Note: Probably a more efficient way to do this
                    //Set up equivalency function later in Card class?
                    //duplicate occurs if two cards in different locations hold same value
                    if ((deckCards[x].value.Equals(deckCards[i].value)) && (x != i))
                    {
                        Console.WriteLine("Cards Equivalent were:");
                        deckCards[x].printCard();
                        deckCards[i].printCard();
                        Console.WriteLine("{0}=x and i={1} ", x, i);
                        return false;
                    }
                }
            }
            return true;
        }
        public void checkDraw()
        {
            for (int i = 0; i < 53; i++)
            {
                draw();
            }
        }
        public void checkCleanup()
        {
            for (int i = 0; i < deckCards.Count; i++)
            {
                draw();
            }
            cleanup();
            //clean up already shuffles so printdeck
            printDeck();
            Console.WriteLine(checkShuffle());
        }
        //function to print deck.
        //This is not to display deck, prints to console for error checking
        public void printDeck()
        {
            for (int i = 0; i < deckCards.Count; i++)
            {
                deckCards[i].printCard();
            }
            //Console.WriteLine("Size of Deck={0}", deckCards.Count);
        }
        public void printGameCards()
        {
            for (int i = 0; i < gameCards.Count; i++)
            {
                gameCards[i].printCard();
            }
        }
    }
    //container class for Player information (currency amt, name, hand[represented as string])
    public class Player
    {
        public int currency { get; set; }
        public string name;
        public string hand { get; set; }
    }

    //manages game logic
    public class GameManager
    {
        //Attributes-------------------------------------------------------------------
        //maximum size of room, default to nine;
        int roomCap;
        //total amount in pot, init to zero
        int pot;
        //string representation of board. Included for internal hand evaluator.
        string board;
        //list representation of cards currently in the board. Included in case needed for graphical representation
        List<Card> boardCards;
        //list of active players        
        List<Player> activePlayers;
        //list of inactive players. Included for potential integration issues with SignalR
        List<Player> inactivePlayers;
        Deck deck;
        //Functions--------------------------------------------------------------------
        //include parameter default overrides ("int size=9") later
        public GameManager()
        {
            roomCap = 9;
            pot = 0;
            board = "";
            boardCards = new List<Card> { };
            activePlayers = new List<Player> { };
            inactivePlayers = new List<Player> { };
            deck = new Deck();
        }


    }
    class Program
    {
        static void Main(string[] args)
        {
            Deck testDeck = new Deck();
            // testDeck.shuffle();
            testDeck.printDeck();
            Console.WriteLine(testDeck.checkShuffle());
            testDeck.checkDraw();
            testDeck.checkCleanup();
            Console.ReadLine();

        }
    }


}