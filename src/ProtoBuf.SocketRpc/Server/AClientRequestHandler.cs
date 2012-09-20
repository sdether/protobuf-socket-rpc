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
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace ProtoBuf.SocketRpc.Server {
    public abstract class AClientRequestHandler : IDisposable, IClientHandler {

        private static readonly Logger.ILog _log = Logger.CreateLog();

        private readonly Action<IClientHandler> _removeCallback;
        private readonly EndPoint _remote;

        protected readonly Socket _socket;
        private readonly Stopwatch _requestTimer = new Stopwatch();

        private ulong _commandCounter;
        private bool _isDisposed;

        protected AClientRequestHandler(Socket socket, Action<IClientHandler> removeCallback) {
            _socket = socket;
            _remote = _socket.RemoteEndPoint;
            _removeCallback = removeCallback;
        }

        public void ProcessRequests() {
            try {
                StartCommandRequest();
            } catch(Exception e) {
                _log.Warn("starting request processing failed", e);
            }
        }

        protected abstract void Receive(Action<Request> continuation);
        protected abstract void Dispatch(Request request, Action<DispatchResponse> continuation);
        protected abstract void SendResponse(Response response, Action continuation);

        // 1.
        private void StartCommandRequest() {
            _commandCounter++;
            _requestTimer.Start();
            Receive(ProcessRequest);
        }

        protected void EndRequest(Request request, bool disconnect) {
            _requestTimer.Stop();
            _log.DebugFormat("[{0}] Completed request for [{1}.{2}] in {4:0.00}ms",
                _commandCounter,
                request.service_name,
                request.method_name,
                request.request_proto.Length,
                _requestTimer.Elapsed.TotalMilliseconds
            );
            _requestTimer.Reset();
            if(disconnect) {
                Dispose();
            } else {
                StartCommandRequest();
            }
        }

        protected void ProcessRequest(Request request) {
            _log.DebugFormat("[{0}] Received request for [{1}.{2}] with {3} bytes in {4:0.00}ms",
                _commandCounter,
                request.service_name,
                request.method_name,
                request.request_proto.Length,
                _requestTimer.Elapsed.TotalMilliseconds
            );
            Dispatch(request, (response) => HandleResponse(request, response));
        }

        private void HandleResponse(Request request, DispatchResponse response) {
            SendResponse(response.Response, () => EndRequest(request, response.DisconnectOnCompletion));
        }

        public void Dispose() {
            if(_isDisposed) {
                return;
            }
            _log.DebugFormat("Disposing client from {0}", _remote);
            try {
                _socket.Close();
            } catch { }
            _removeCallback(this);
        }
    }
}
