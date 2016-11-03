using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokerPrototype.Models
{
    public class RoomModel
    {
        public int roomID { get; set; }
        public string roomName { get; set; }
        public int currentNumberPlayers { get; set; }
        public int maxPlayerCount { get; set; }
        public int blinds { get; set; }
        public RoomModel(int roomID, string roomName, int currentNumberPlayers, int maxPlayerCount, int blinds)
        {
            this.roomID = roomID;
            this.roomName = roomName;
            this.currentNumberPlayers = currentNumberPlayers;
            this.maxPlayerCount = maxPlayerCount;
            this.blinds = blinds;
        }
    }
}