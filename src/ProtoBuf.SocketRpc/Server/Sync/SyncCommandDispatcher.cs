using System;
using System.Collections.Generic;

namespace ProtoBuf.SocketRpc.Server.Sync {
    class SyncCommandDispatcher : ISyncCommandDispatcher {
        private readonly Dictionary<string, Tuple<bool, Func<Request, Response>>> _handlers = new Dictionary<string, Tuple<bool, Func<Request, Response>>>();

        public DispatchResponse Dispatch(Request request) {
            var handler = _handlers[request.service_name + "." + request.method_name];
            var response = handler.Item2(request);
            return new DispatchResponse(response, handler.Item1);
        }

        public void AddHandler(string serviceName, string methodName, Func<Request, Response> handler) {
            _handlers[serviceName + "." + methodName] = new Tuple<bool, Func<Request, Response>>(false, handler);
        }

        public void AddDisconnectionHandler(string serviceName, string methodName, Func<Request, Response> handler) {
            _handlers[serviceName + "." + methodName] = new Tuple<bool, Func<Request, Response>>(true, handler);
        }

        public void AddDisconnectionHandler(string serviceName, string methodName, Response staticResponse) {
            _handlers[serviceName + "." + methodName] = new Tuple<bool, Func<Request, Response>>(true, (request) => staticResponse);
        }
    }
}