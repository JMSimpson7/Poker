﻿/*

 * TODO:
 * -Rework functions relying on draw returning bool instead of a string
 * -Determine what happens in betting if player does not have money to call/raise
 * -Add IDs for game managers to organize games database
 * -Create GameData class purely to hold data/state. Game Manager shall 
 * run all helper/poker functions json functions related to GameData

 * TODO (see if convert game manager to JSON)
    -store each variable in column database
    -retrieve said columns

 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Web.Helpers;
using Newtonsoft.Json;
using HoldemHand;
using System.Web.Mvc;
using PokerGame;
//using System.Security.Cryptography;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace PokerGame
{
    //http://stackoverflow.com/questions/273313/randomize-a-listt
    //So I honestly how this works
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
        public string draw()
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
                /*
                Console.WriteLine("Drawn: ");
                drawn.printCard();
                */
                return drawn.value;
            }
            return "";
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
        public string ID { get; set; }
        public string name { get; set; }
        public string hand { get; set; }
        //whether or not Player is in the current game (They have not folded)
        public bool folded { get; set; }
    }

    //Holds all data necessary for GameManager
    //Will be serialized to JSON for storage/retrieval of game state
    public class GameData
    {
        //Attributes-------------------------------------------------------------------
        //ID of room itself. Currently not used
        public int roomID { get; set; }
        //maximum size of room, default to nine;
        public int roomCap { get; set; }
        //last bet number. calls must meet this, raises must beat it
        public int callAmt { get; set; }
        //total amount in pot, init to zero
        public int pot { get; set; }
        //potential check against too many cards on board
        public int boardCount { get; set; }
        //string representation of board. Included for internal hand evaluator.
        public string board { get; set; }
        //list representation of cards currently in the board. Included in case needed for graphical representation
        public List<Card> boardCards;
        //list of active players currently in game        
        public List<Player> activePlayers;
        //list of inactive players not in the game. Included for potential integration issues with SignalR
        public List<Player> inactivePlayers;
        public Deck deck;

        public GameData()
        {
            boardCards = new List<Card> { };
            activePlayers = new List<Player> { };
            inactivePlayers = new List<Player> { };
            deck = new Deck();
        }

    }


    //Responsible for: All game logic, processing all player actions
    //(call, raise, fold, check)
    //draw/deal cards,determine winner
    //SignalR Hub handles updating clients/maintaining turn order
    public class GameManager
    {
        GameData data;
        //include parameter default overrides ("int size=9") later
        public GameManager()
        {
            data = new PokerGame.GameData();

        }
        //Functions--------------------------------------------------------------------
        //TODO: \\ check(), blind(char p, );
        //getwinner(){ 
        // list of winners, usually only contains
        // loop through the active players
        // Hand h1 = new Hand("ad kd", board);
        /// evaluate hand for all players and find maximum
        // in the event of a tie, push onto the winners list
        //intializes beginning state of game
        public void init()
        {
            //all inactivePlayers become activePlayers here
            for (int i = 0; i < inactivePlayers.Count; i++)
            {
                activePlayers.Add(inactivePlayers[i]);
            }
            inactivePlayers.Clear();
            //clean and shuffle deck
            deck.cleanup();
            deck.shuffle();
            //reset board count to zero
            boardCount = 0;
            //empty board
            board = "";
            //Cycle through player list twice to deal cards to players, done to aid potential graphics integration
            for (int i = 0; i < activePlayers.Count; i++)
            {
                //Overwrite will empty previous player hand
                activePlayers[i].hand = deck.draw();
                //set all active players as currently participating in round
                activePlayers[i].folded = false;
            }
            for (int i = 0; i < activePlayers.Count; i++)
            {
                //append " " and second card to each players hand
                activePlayers[i].hand += " " + deck.draw();
            }
        }

        //adds card to board
        public void addBoard()
        {
            //if board is empty
            if (data.board.Equals(""))
            {
                data.board = data.deck.draw();
                data.boardCount = 1;
            }
            else
            {
                //if room to add to board, add
                if (data.boardCount < 5)
                {
                    data.board += " " + data.deck.draw();
                    data.boardCount++;
                }
            }
        }

        //marks player as folded.
        public void fold(string ID)
        {
            for (int i = 0; i < data.activePlayers.Count; i++)
            {
                if (data.activePlayers[i].ID.Equals(ID))
                {
                    data.activePlayers[i].folded = true;
                }
            }
        }
        //validates and checks raising
        public void raise(string ID, int amount)
        {
            for (int i = 0; i < data.activePlayers.Count; i++)
            {
                if (data.activePlayers[i].ID.Equals(ID))
                {
                    //amount must be greater than current call amt, and player must actually have the money
                    if ((amount > data.callAmt) && (data.activePlayers[i].currency - amount >= 0))
                    {
                        data.activePlayers[i].currency -= amount;
                        data.pot += amount;
                        data.callAmt = amount;
                    }
                }
            }
        }
        //validates and checks calling
        public void call(string ID)
        {
            for (int i = 0; i < data.activePlayers.Count; i++)
            {
                if (data.activePlayers[i].ID.Equals(ID))
                {
                    if (data.activePlayers[i].currency - data.callAmt >= 0)
                    {
                        data.activePlayers[i].currency -= data.callAmt;
                        data.pot += data.callAmt;
                    }
                }
            }
        }
        //returns list of ID of winning player(s), delinated by space if more than one winner
        public List<string> getWinner()
        {
            List<string> winners = new List<string> { };
            //copy all non folded players into new list of potential winners
            List<Player> finalPlayers = new List<Player> { };
            for (int i = 0; i < data.activePlayers.Count; i++)
            {
                if (data.activePlayers[i].folded == false)
                {
                    finalPlayers.Add(data.activePlayers[i]);
                }
            }
            //start by examining first hand
            Hand h1 = new Hand(finalPlayers[0].hand, data.board);
            //default to first hand being winner (a hand better than no hand)
            winners.Add(finalPlayers[0].ID);
            //comparison hand
            Hand h2;
            for (int i = 1; i < finalPlayers.Count; i++)
            {
                //for every other active player examine hand, compare, replace if necessary
                h2 = new Hand(finalPlayers[i].hand, data.board);

                //new hand is better
                if (h2 > h1)
                {
                    winners.Clear();
                    winners.Add(finalPlayers[0].ID);
                    h1 = h2;
                }
                else if (h1 > h2)
                {
                    //do nothing
                }
                else
                {
                    //tie, push onto winners list
                    winners.Add(finalPlayers[0].ID);
                }
            }
            return winners;
        }
        public void award(List<string> winners)
        {
            //divide pot among all winners
            int award = data.pot / (winners.Count);
            for (int i = 0; i < data.activePlayers.Count; i++)
            {
                for (int x = 0; x < winners.Count; i++)
                {
                    if (data.activePlayers[i].ID.Equals(winners[i]))
                    {
                        data.activePlayers[i].currency += award;
                    }
                }
            }
            //pot is now empty
            data.pot = 0;
        }
        //player joins midgame. Handles adjusting data, NOT actual network join
        public void join(string IDtag, int money, string username)
        {
            Player temp = new Player();
            temp.ID = IDtag;
            temp.currency = money;
            temp.name = username;
            //add to inactive players, to become active next round
            data.inactivePlayers.Add(temp);
        }
        //GET functions for integration with web interface
        public string getBoard()
        {
            return data.board;
        }
        public int getBoardSize()
        {
            return data.boardCount;
        }
        public int getPlayerCurrency(string ID)
        {
            for (int i = 0; i < data.activePlayers.Count; i++)
            {
                if (data.activePlayers[i].ID.Equals(ID))
                {
                    return data.activePlayers[i].currency;
                }
            }

            return 0;

        }
        public string getPlayerHand(string ID)
        {
            for (int i = 0; i < data.activePlayers.Count; i++)
            {
                if (data.activePlayers[i].ID.Equals(ID))
                {
                    return data.activePlayers[i].hand;
                }
            }

            return "";

        }
        public bool getFold(string ID)
        {
            for (int i = 0; i < data.activePlayers.Count; i++)
            {
                if (data.activePlayers[i].ID.Equals(ID))
                {
                    return data.activePlayers[i].folded;
                }
            }

            return false;

        }
        public int getPot()
        {
            return data.pot;

        }
        public void updateState()
        {
            string output = JsonConvert.SerializeObject(data);
            MySqlConnection Conn = new MySqlConnection("server=sql9.freemysqlhosting.net;database=sql9140372;user=sql9140372;password=WSx2C8iRZx;");
            var cmd = new MySql.Data.MySqlClient.MySqlCommand();
            Conn.Open();
            cmd.Connection = Conn;
            cmd.CommandText = "INSERT INTO games VALUES output ";
            cmd.Prepare();
            MySqlDataReader rdr = cmd.ExecuteReader();
            Conn.Close();
        }
        public string getState()
        {
            MySqlConnection Conn = new MySqlConnection("server=sql9.freemysqlhosting.net;database=sql9140372;user=sql9140372;password=WSx2C8iRZx;");
            var cmd = new MySql.Data.MySqlClient.MySqlCommand();
            Conn.Open();
            cmd.Connection = Conn;
            cmd.CommandText = "SELECT jsondata FROM games";
            cmd.Prepare();
            MySqlDataReader rdr = cmd.ExecuteReader();
            string json = (string)rdr["jsondata"];
            //change to point to data class held by this
            data = JsonConvert.DeserializeObject<GameData>(json);

            Conn.Close();
            return json;
        }

        }

}
class Program
{
    static void Main(string[] args)
    {
        /*         Deck testDeck = new Deck();
                 // testDeck.shuffle();
                 testDeck.printDeck();
                 Console.WriteLine(testDeck.checkShuffle());
                 testDeck.checkDraw();
                 testDeck.checkCleanup();
                 Console.ReadLine();
        */
        GameManager manager = new GameManager();
        manager.join("1", 100, "Bob");
        manager.join("2", 150, "Joe");
        manager.init();
        manager.updateState();
    }
}


}