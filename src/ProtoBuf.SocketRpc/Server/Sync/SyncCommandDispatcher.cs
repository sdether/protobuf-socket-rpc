using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProtoBuf.SocketRpc.Server.Sync {
    class SyncCommandDispatcher : ISyncCommandDispatcher {

        private static readonly Type _requestType = typeof(Request);
        private static readonly Type _responseType = typeof(Response);

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

        public void AddService<TService>(TService handler) {
            var t = typeof(TService);
            var rpcMethods = from m in t.GetMethods(BindingFlags.Public)
                             let p = m.GetParameters()
                             where m.ReturnType == _responseType && p.Length == 1 && p[0].ParameterType == _requestType
                             select m;
            foreach(var methodInfo in rpcMethods) {
                var m = methodInfo;
                AddHandler(t.Name, m.Name, r => (Response)m.Invoke(handler, new object[] { r }));
            }
        }
    }
}