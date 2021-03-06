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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ProtoBuf.SocketRpc.Client.Net.Helper {
    public class SocketAdapter : ISocket {

        public static ISocket Open(string host, int port, TimeSpan connectTimeout) {
            var timeout = new ManualResetEvent(false);
            Exception connectFailure = null;
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ar = socket.BeginConnect(host, port, r => {
                try {
                    socket.EndConnect(r);
                } catch(Exception e) {
                    connectFailure = e;
                } finally {
                    timeout.Set();
                }
            }, null);

            if(!timeout.WaitOne(connectTimeout)) {
                socket.EndConnect(ar);
                throw new TimeoutException();
            }
            if(connectFailure != null) {
                throw new ConnectException(connectFailure);
            }
            return new SocketAdapter(socket);
        }

        public static ISocket Open(IPEndPoint endPoint, TimeSpan connectTimeout) {
            var timeout = new ManualResetEvent(false);
            Exception connectFailure = null;
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ar = socket.BeginConnect(endPoint, r => {
                try {
                    socket.EndConnect(r);
                } catch(Exception e) {
                    connectFailure = e;
                } finally {
                    timeout.Set();
                }
            }, null);

            if(!timeout.WaitOne(connectTimeout)) {
                socket.EndConnect(ar);
                throw new TimeoutException();
            }
            if(connectFailure != null) {
                throw new ConnectException(connectFailure);
            }
            return new SocketAdapter(socket);
        }

        private readonly Socket _socket;
        private readonly NetworkStream _stream;

        public SocketAdapter(Socket socket) {
            _socket = socket;
            _stream = new NetworkStream(_socket);
        }

        public void Dispose() {
            _socket.Close();
        }

        public bool Connected {
            get {
                try {
                    if(!_socket.Connected) {
                        return false;
                    }
                    var part1 = _socket.Poll(100, SelectMode.SelectRead);
                    var part2 = (_socket.Available == 0);
                    return !(part1 && part2);
                } catch {
                    return false;
                }
            }
        }

        public Stream Stream { get { return _stream; } }

        //public int Send(byte[] buffer, int offset, int size) {
        //    return _socket.Send(buffer, offset, size, SocketFlags.None);
        //}

        //public int Receive(byte[] buffer, int offset, int size) {
        //    return _socket.Receive(buffer, offset, size, SocketFlags.None);
        //}
    }
}