using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;
using ProtoBuf.SocketRpc.Client;
using ProtoBuf.SocketRpc.Server;
using log4net;

namespace ProtoBuf.SocketRpc.PerfTests {
    [TestFixture]
    public class RountripTests {

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int _port;

        [SetUp]
        public void Setup() {
            _log.Debug("priming logger");
            _port = new Random().Next(1000, 30000);
        }

        [Test]
        public void Can_send_receiveMany() {
            var payloadstring = "";
            using(RpcServerBuilder.CreateSync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), _port))
                .WithHandler("foo", "bar", r => new Response { response_proto = r.request_proto, error = "OK" })
                .Start()
                ) {
                Console.WriteLine("created server");
                EchoServer();
            }
        }

        private void EchoServer() {
            using(var client = new RpcClient("127.0.0.1", _port)) {
                var n = 30000;
                var t = Stopwatch.StartNew();
                for(var i = 0; i < n; i++) {
                    var payload = new StringBuilder();
                    for(var j = 0; j < 10; j++) {
                        payload.Append(Guid.NewGuid().ToString());
                    }
                    var bytes = Encoding.ASCII.GetBytes(payload.ToString());
                    var response = client.Send(new Request { service_name = "foo", method_name = "bar", request_proto = bytes });
                    Assert.AreEqual("OK", response.error);
                    Assert.AreEqual(bytes, response.response_proto);
                }
                t.Stop();
                var rate = n / t.Elapsed.TotalSeconds;
                Console.WriteLine("Executed {0} commands at {1:0}commands/second", n, rate);
            }
        }
    }
}
