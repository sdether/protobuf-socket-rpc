/*
 * MindTouch.Clacks
 * 
 * Copyright (C) 2011 Arne F. Claassen
 * geekblog [at] claassen [dot] net
 * http://github.com/sdether/MindTouch.Clacks
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Net;
using ProtoBuf.SocketRpc.Server.Sync;

namespace ProtoBuf.SocketRpc.Server {
    public class RpcServerBuilder : ISyncServerBuilder {

        public static ISyncServerBuilder CreateSync(IPEndPoint endPoint) {
            return new RpcServerBuilder(endPoint, false);
        }

        private readonly IPEndPoint _endPoint;
        private readonly SyncCommandDispatcher _dispatcher = new SyncCommandDispatcher();

        private RpcServerBuilder(IPEndPoint endPoint, bool isAsync) {
            _endPoint = endPoint;
        }

        public RpcServer Build() {
            return new RpcServer(_endPoint, new SyncClientHandlerFactory(_dispatcher));
        }

        ISyncServerBuilder ISyncServerBuilder.WithService<TService>(TService handler) {
            throw new NotImplementedException();
        }

        ISyncServerBuilder ISyncServerBuilder.WithHandler(string serviceName, string methodName, Func<Request, Response> handler) {
            _dispatcher.AddHandler(serviceName, methodName, handler);
            return this;
        }

        public ISyncServerBuilder WithDisconnectionHandler(string serviceName, string methodName, Func<Request, Response> handler) {
            throw new NotImplementedException();
        }

        public ISyncServerBuilder WithDisconnectionHandler(string serviceName, string methodName, Response staticResponse) {
            throw new NotImplementedException();
        }
    }
}