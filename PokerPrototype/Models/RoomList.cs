using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokerPrototype.Models
{
    public class RoomList : List<RoomModel>
    {
        public RoomList()
        {
            Add(new RoomModel(1, "noobs welcome", 2, 8, 200));
            Add(new RoomModel(2, "Josh's room", 3, 5, 400));
            Add(new RoomModel(3, "top kek", 4, 4, 300));
        }
    }
}