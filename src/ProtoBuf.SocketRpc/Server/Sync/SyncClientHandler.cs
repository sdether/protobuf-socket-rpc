/*
 * ProtoBuf.SocketRpc
 * 
 * Copyright (C) 2012 Arne F. Claassen
 * geekblog [at] claassen [dot] net
 * https://github.com/sdether/protobuf-socket-rpc
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
using System.Net.Sockets;
using System.Threading;

namespace ProtoBuf.SocketRpc.Server.Sync {
    public class SyncClientHandler : AClientRequestHandler {

        private const string TERMINATOR = "\r\n";
        private static readonly Logger.ILog _log = Logger.CreateLog();

        private readonly ISyncCommandDispatcher _dispatcher;
        private readonly NetworkStream _stream;
       
        public SyncClientHandler(Socket socket, ISyncCommandDispatcher dispatcher, Action<IClientHandler> removeCallback) : base(socket, removeCallback) {
            _dispatcher = dispatcher;
            _stream = new NetworkStream(socket);
        }

        protected override void Receive(Action<Request> continuation) {
            continuation(Serializer.DeserializeWithLengthPrefix<Request>(_stream, PrefixStyle.Fixed32));
        }

        protected override void Dispatch(Request request, Action<DispatchResponse> continuation) {
            var response = _dispatcher.Dispatch(request);
            continuation(response);
        }

        protected override void SendResponse(Response response, Action continuation) {
            Serializer.SerializeWithLengthPrefix(_stream, response, PrefixStyle.Fixed32);
            ThreadPool.QueueUserWorkItem(_ => continuation());
        }
    }
}
