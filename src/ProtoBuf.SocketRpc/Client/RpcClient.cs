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
using System.Net;
using ProtoBuf.SocketRpc.Client.Net;
using ProtoBuf.SocketRpc;

namespace ProtoBuf.SocketRpc.Client {

    // TODO: need way to control Receive and Send timeouts
    // Note: RpcClient is not threadsafe. It's assumed that whatever code incorporates manages access to it
    // in a threadsafe manner
    public class RpcClient : IDisposable {
        public const string DEFAULT_TUBE = "default";
        private readonly ISocket _socket;
        private bool _disposed;

        public RpcClient(IPEndPoint endPoint)
            : this(ConnectionPool.GetPool(endPoint)) {
        }

        public RpcClient(string host, int port)
            : this(ConnectionPool.GetPool(host, port)) {
        }

        public RpcClient(IConnectionPool pool)
            : this(pool.GetSocket()) {
        }

        public RpcClient(ISocket socket) {
            if(socket == null) {
                throw new ArgumentNullException("socket");
            }
            _socket = socket;
        }

        public bool Disposed {
            get {
                if(_disposed) {
                    return true;
                }
                if(!_socket.Connected) {
                    _socket.Dispose();
                    _disposed = true;
                }
                return _disposed;
            }
        }

        public Response Send(Request request) {
            ThrowIfDisposed();
            Serializer.SerializeWithLengthPrefix(_socket.Stream,request,PrefixStyle.Fixed32);
            return Serializer.DeserializeWithLengthPrefix<Response>(_socket.Stream, PrefixStyle.Fixed32);
        }

        private void ThrowIfDisposed() {
            if(_disposed) {
                throw new ObjectDisposedException(GetType().ToString());
            }
        }

        public void Dispose() {
            Dispose(true);
        }

        protected virtual void Dispose(bool suppressFinalizer) {
            if(_disposed) {
                return;
            }
            if(suppressFinalizer) {
                GC.SuppressFinalize(this);
            }
            if(_socket != null) {
                _socket.Dispose();
            }
            _disposed = true;
        }

        ~RpcClient() {
            Dispose(false);
        }
    }
}

