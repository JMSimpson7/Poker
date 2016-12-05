using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using PokerPrototype.Models;
using PokerGame;

namespace PokerPrototype.Hubs
{
    public class MyHub : Hub
    {
        public void Send(string message)
        {
            string connid = Context.ConnectionId;
            Clients.All.alertMessage(message + " from " + connid);
            //hello
        }

        //user is the string ID of the player
        public void fold(string user)
        {
            GameManager manager = new GameManager();
            if (manager.getCurrentPlayer().ID == user)
            {
                manager.getState();
                manager.fold(user);
                if (manager.gameOver())
                {
                    List<String> winners = manager.getWinner();
                    manager.award(winners);
                    //restart game
                    //any post game stuff should be done or called here
                    manager.init();
                }
                else
                {
                    //cycle to next player turn
                    manager.cycle();
                }
            }
            else
            {
                //not player's turn
            }

        }


    }
}