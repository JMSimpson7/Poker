using System;
using System.Collections.Generic;
using System.Threading;
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
            public int value { get; set; }
            //letter representing suite. s for spades, d for diamonds, h for hearts, c for clubs
            public char suite { get; set; }
            //True if card is currently in play. False if card is currently in Deck
            public bool inPlay { get; set; }
            public void printCard()
            {
                Console.WriteLine("{0} of {1}", this.value, this.suite);
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
            int counter = 1;
            //Adding Spades Cards
            for (i=0; i < 13; i++)
            {
                cards[i] = new Card();
                cards[i].value = counter;
                cards[i].suite = 's';
                deckCards.Add(cards[i]);
                counter++;
            }
            counter = 1;
            //Adding Diamonds Cards
            for (i=13; i < 26; i++)
            {
                cards[i] = new Card();
                cards[i].value = counter;
                cards[i].suite = 'd';
                deckCards.Add(cards[i]);
                counter++;
            }
            counter = 1;
            //Adding Hearts Cards
            for (i=26; i < 39; i++)
            {
                cards[i] = new Card();
                cards[i].value = counter;
                cards[i].suite = 'h';
                deckCards.Add(cards[i]);
                counter++;
            }
            counter = 1;
            //Adding Clubs cards
            for (i=39; i < 52; i++)
            {
                cards[i] = new Card();
                cards[i].value = counter;
                cards[i].suite='c';
                deckCards.Add(cards[i]);
                counter++;
            }
            counter = 1;
        }

        //CITE:http://stackoverflow.com/questions/273313/randomize-a-listt 
        public void shuffle()
        {
            int n = deckCards.Count;//for amount of cards in deck currently
            while(n>1)
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
            while(gameCards.Count>0)
            {
                deckCards.Add(gameCards[0]);
                gameCards.RemoveAt(0);
            }
            //deck now contains all cards again
            shuffle();
        }
        public bool checkShuffle()
        {
            //first shuffle, then print deck for visual comparison
            shuffle();
            printDeck();
            //code chunk checks for duplicate cards
            Card[] temp;
            temp = new Card[52];
            for (int i = 0; i < deckCards.Count; i++)
            {
                temp[i] = new Card();
            }
            for (int i=0; i<deckCards.Count;i++)
            {
                temp[i] = cards[i];
                for(int x=0; x<temp.Length;x++)
                {
                    //Note: Probably a more efficient way to do this
                    //Set up equivalency function later in Card class? 
                    if ((temp[x].value == temp[i].value) && (x != i) && (temp[x].suite==temp[i].suite))
                    {
                        Console.WriteLine("Cards Equivalent were:");
                        temp[x].printCard();
                        temp[i].printCard();
                        Console.WriteLine("{0}=x and i={1} ",x,i);
                        return false;
                    }
                }
            }
            return true;
        }
        public void checkDraw()
        {
            for(int i=0;i<53;i++)
            {
                draw();
            }
        }
        public void checkCleanup()
        {
            for(int i=0; i<deckCards.Count;i++)
            {
                draw();
            }
            cleanup();
            //clean up already shuffles so printdeck
            printDeck();
            //code chunk ensures each card only occurs once
            Card[] temp;
            temp = new Card[52];
            for (int i = 0; i < deckCards.Count; i++)
            {
                temp[i] = new Card();
            }
            for (int i = 0; i < deckCards.Count; i++)
            {
                temp[i] = cards[i];
                for (int x = 0; x < temp.Length; x++)
                {
                    //Note: Probably a more efficient way to do this
                    //Set up equivalency function later in Card class? 
                    if ((temp[x].value == temp[i].value) && (x != i) && (temp[x].suite == temp[i].suite))
                    {
                        Console.WriteLine("Cards Equivalent were:");
                        temp[x].printCard();
                        temp[i].printCard();
                        Console.WriteLine("{0}=x and i={1} ", x, i);
                    }
                }
            }
        }
        //function to print deck.
        //This is not to display deck, prints to console for error checking
        public void printDeck()
        {
            for(int i = 0; i < deckCards.Count; i++)
            {
                deckCards[i].printCard();
            }
            //Console.WriteLine("Size of Deck={0}", deckCards.Count);
        }
        public void printGameCards()
        {
            for(int i = 0; i < gameCards.Count; i++)
            {
                gameCards[i].printCard();
            }
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
