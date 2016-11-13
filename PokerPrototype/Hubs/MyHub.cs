using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using PokerPrototype.Models;

namespace PokerPrototype.Hubs
{
    public class MyHub : Hub
    {
        public void Send(string message)
        {
            string connid = Context.ConnectionId;
            Clients.All.alertMessage(message + " from " + connid);

        }
    }
}