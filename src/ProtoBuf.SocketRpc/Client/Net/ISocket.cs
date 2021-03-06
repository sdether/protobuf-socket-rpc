﻿/*
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

namespace ProtoBuf.SocketRpc.Client.Net {
    public interface ISocket : IDisposable {
        bool Connected { get; }
        //int Send(byte[] buffer, int offset, int size);
        //int Receive(byte[] buffer, int offset, int size);
        Stream Stream { get; }
    }
}