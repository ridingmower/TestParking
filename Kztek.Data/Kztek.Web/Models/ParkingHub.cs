using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Kztek.Web.Models
{
    public class ParkingHub : Hub
    {
        public static ConcurrentDictionary<string, string> ConnectedIds = new ConcurrentDictionary<string, string>();

        public void helloServer(string id)
        {
            var responseMes = DateTime.Now.ToString();

            Clients.All.helloApp(responseMes);
        }

        public void displayQRCodeServer(dynamic data)
        {
            Clients.All.displayQRCodeClient(data);
        }

        public override Task OnConnected()
        {
            Trace.TraceInformation("MapHub started. ID: {0}", Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string garbage; // to be collected by the Garbage Collector
            ConnectedIds.TryRemove(Context.ConnectionId, out garbage);

            return base.OnDisconnected(stopCalled);
        }
    }
}