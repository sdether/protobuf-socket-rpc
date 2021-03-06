//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: rpc.proto
namespace ProtoBuf.SocketRpc {
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"Request")]
    public partial class Request : global::ProtoBuf.IExtensible {
        public Request() { }

        private string _service_name;
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"service_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public string service_name {
            get { return _service_name; }
            set { _service_name = value; }
        }
        private string _method_name;
        [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name = @"method_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public string method_name {
            get { return _method_name; }
            set { _method_name = value; }
        }
        private byte[] _request_proto;
        [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name = @"request_proto", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public byte[] request_proto {
            get { return _request_proto; }
            set { _request_proto = value; }
        }
        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing) { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"Response")]
    public partial class Response : global::ProtoBuf.IExtensible {
        public Response() { }


        private byte[] _response_proto = null;
        [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name = @"response_proto", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue(null)]
        public byte[] response_proto {
            get { return _response_proto; }
            set { _response_proto = value; }
        }

        private string _error = "";
        [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name = @"error", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string error {
            get { return _error; }
            set { _error = value; }
        }

        private bool _callback = (bool)false;
        [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name = @"callback", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue((bool)false)]
        public bool callback {
            get { return _callback; }
            set { _callback = value; }
        }

        private ErrorReason _error_reason = ErrorReason.BAD_REQUEST_DATA;
        [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name = @"error_reason", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(ErrorReason.BAD_REQUEST_DATA)]
        public ErrorReason error_reason {
            get { return _error_reason; }
            set { _error_reason = value; }
        }
        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing) { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    [global::ProtoBuf.ProtoContract(Name = @"ErrorReason")]
    public enum ErrorReason {

        [global::ProtoBuf.ProtoEnum(Name = @"BAD_REQUEST_DATA", Value = 0)]
        BAD_REQUEST_DATA = 0,

        [global::ProtoBuf.ProtoEnum(Name = @"BAD_REQUEST_PROTO", Value = 1)]
        BAD_REQUEST_PROTO = 1,

        [global::ProtoBuf.ProtoEnum(Name = @"SERVICE_NOT_FOUND", Value = 2)]
        SERVICE_NOT_FOUND = 2,

        [global::ProtoBuf.ProtoEnum(Name = @"METHOD_NOT_FOUND", Value = 3)]
        METHOD_NOT_FOUND = 3,

        [global::ProtoBuf.ProtoEnum(Name = @"RPC_ERROR", Value = 4)]
        RPC_ERROR = 4,

        [global::ProtoBuf.ProtoEnum(Name = @"RPC_FAILED", Value = 5)]
        RPC_FAILED = 5,

        [global::ProtoBuf.ProtoEnum(Name = @"INVALID_REQUEST_PROTO", Value = 6)]
        INVALID_REQUEST_PROTO = 6,

        [global::ProtoBuf.ProtoEnum(Name = @"BAD_RESPONSE_PROTO", Value = 7)]
        BAD_RESPONSE_PROTO = 7,

        [global::ProtoBuf.ProtoEnum(Name = @"UNKNOWN_HOST", Value = 8)]
        UNKNOWN_HOST = 8,

        [global::ProtoBuf.ProtoEnum(Name = @"IO_ERROR", Value = 9)]
        IO_ERROR = 9
    }

}