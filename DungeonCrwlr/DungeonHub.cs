using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System.Threading;

namespace DungeonCrwlr
{
    public class Broadcaster
    {
        private readonly static Lazy<Broadcaster> _instance =
            new Lazy<Broadcaster>(() => new Broadcaster());
        // We're going to broadcast to all clients a maximum of 25 times per second
        private readonly TimeSpan BroadcastInterval =
            TimeSpan.FromMilliseconds(40);
        private readonly IHubContext _hubContext;
        private Timer _broadcastLoop;
        private CharactersModel _model;
        private bool _modelUpdated;

        public Broadcaster()
        {
            // Save our hub context so we can easily use it 
            // to send to its connected clients
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<DungeonHub>();

            _model = new CharactersModel();
            _modelUpdated = false;

            // Start the broadcast loop
            _broadcastLoop = new Timer(
                BroadcastShape,
                null,
                BroadcastInterval,
                BroadcastInterval);
        }

        public void BroadcastShape(object state)
        {
            // No need to send anything if our model hasn't changed
            if (_modelUpdated)
            {
                // This is how we can access the Clients property 
                // in a static hub method or outside of the hub entirely
                _hubContext.Clients.AllExcept(_model.LastUpdatedBy).updateShape(_model);
                _modelUpdated = false;
            }
        }

        public void BroadcastMap(object state)
        {

        }

        public void UpdateScreen(CharactersModel clientModel)
        {
            _model = clientModel;
            _modelUpdated = true;
        }

        public static Broadcaster Instance
        {
            get
            {
                return _instance.Value;
            }
        }
    }

    public class DungeonHub : Hub
    {
        public void UpdateScreens(CharactersModel _p_Client)
        {
            _p_Client.LastUpdatedBy = Context.ConnectionId;
            Clients.AllExcept(_p_Client.LastUpdatedBy).updateScreen(_p_Client);
        }
    }

    public class CharactersModel
    {
        public CharactersModel()
        {
            Locations = new List<Location>();
        }

        [JsonProperty("Locations")]
        public List<Location> Locations { get; set; }

        public string Background { get; set; }
        
        [JsonIgnore]
        public string LastUpdatedBy { get; set; }
    }

    public class Location
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("left")]
        public double Left { get; set; }
        [JsonProperty("top")]
        public double Top { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
    }
}