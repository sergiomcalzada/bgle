

using System;
using System.Collections.Generic;

namespace bgle.Graph.Rexpro.Core.protocol.msg
{
    /// <summary>
    /// A basic RexProMessage containing the basic components of every message that Rexster can process.
    /// </summary>
    public class RexProMessage
    {
        public const int MESSAGE_HEADER_SIZE = 11;

        public const int MESSAGE_HEADER_PROTOCOL = 0;

        public const int MESSAGE_HEADER_SERIALIZER = 1;

        public const int MESSAGE_HEADER_MESSAGE_TYPE = 6;

        public Guid Session { get; set; }
        public Guid Request { get; set; }

        public RexProMessageMeta Meta = new RexProMessageMeta();



        protected virtual RexProMessageMetaField[] GetMetaFields()
        {
            RexProMessageMetaField[] fields = { };
            return fields;
        }

        public void ValidateMetaData()
        {
            foreach (var f in this.GetMetaFields())
            {
                f.ValidateMeta(this.Meta);
            }
        }

        public byte[] ToByteArray()
        {
            throw new NotImplementedException();
        }

        public MessageType MessageType { get { throw new NotImplementedException(); } }
    }

    public class ErrorResponseMessage : RexProMessage
    {
        public String ErrorMessage;

        public const int INVALID_MESSAGE_ERROR = 0;
        public const int INVALID_SESSION_ERROR = 1;
        public const int SCRIPT_FAILURE_ERROR = 2;
        public const int AUTH_FAILURE_ERROR = 3;
        public const int GRAPH_CONFIG_ERROR = 4;
        public const int CHANNEL_CONFIG_ERROR = 5;
        public const int RESULT_SERIALIZATION_ERROR = 6;
        public const int UNKNOWN_ERROR = 7;

        protected const string FLAG_META_KEY = "flag";

        protected override RexProMessageMetaField[] GetMetaFields()
        {
            RexProMessageMetaField[] fields = {
                    //indicates this session should be destroyed
                   new RexProMessageMetaField(FLAG_META_KEY, true, null, typeof(int))
            };
            return fields;
        }

        public int MetaGetFlag()
        {
            var value = Meta[FLAG_META_KEY];
            if (value is int)
            {
                return (int)value;
            }
            if (value is long)
            {
                return (int)((long)value);
            }

            return default(int);
        }

        public void MetaSetFlag(int val)
        {
            Meta.Add(FLAG_META_KEY, val);
        }
    }

    public class SessionRequestMessage : RexProMessage
    {
        protected const String KILL_SESSION_META_KEY = "killSession";
        protected const String GRAPH_NAME_META_KEY = "graphName";
        protected const String GRAPH_OBJECT_NAME_META_KEY = "graphObjName";
        protected override RexProMessageMetaField[] GetMetaFields()
        {
            RexProMessageMetaField[] fields = {
            //indicates this session should be destroyed
            new RexProMessageMetaField(KILL_SESSION_META_KEY, false, false, typeof(Boolean)),

            //sets the graph and graph variable name for this session, optional
            new RexProMessageMetaField(GRAPH_NAME_META_KEY, false, null, typeof(string)),
            new RexProMessageMetaField(GRAPH_OBJECT_NAME_META_KEY, false, "g", typeof(string))
        };
            return fields;
        }

        public String Username;
        public String Password;

        public void MetaSetKillSession(Boolean val)
        {
            Meta.Add(KILL_SESSION_META_KEY, val);
        }

        public Boolean MetaGetKillSession()
        {
            return Meta.Get<bool>(KILL_SESSION_META_KEY);
        }

        public void MetaSetGraphName(String val)
        {
            Meta.Add(GRAPH_NAME_META_KEY, val);
        }

        public string MetaGetGraphName()
        {
            return this.Meta.Get<string>(GRAPH_NAME_META_KEY);
        }

        public void MetaSetGraphObjName(String val)
        {
            Meta.Add(GRAPH_OBJECT_NAME_META_KEY, val);
        }

        public string MetaGetGraphObjName()
        {
            return this.Meta.Get<string>(GRAPH_OBJECT_NAME_META_KEY);
        }
    }

    public class SessionResponseMessage : RexProMessage
    {
        public String[] Languages;
    }

    public class ScriptRequestMessage : RexProMessage
    {
        protected const string META_KEY_IN_SESSION = "inSession";
        protected const string META_KEY_GRAPH_NAME = "graphName";
        protected const string META_KEY_GRAPH_OBJECT_NAME = "graphObjName";
        protected const string META_KEY_ISOLATE_REQUEST = "isolate";
        protected const string META_KEY_TRANSACTION = "transaction";
        protected const string META_KEY_CONSOLE = "console";

        protected override RexProMessageMetaField[] GetMetaFields()
        {
            RexProMessageMetaField[] fields = {
            //indicates this requests should be executed in the supplied session
            new RexProMessageMetaField(META_KEY_IN_SESSION, false, false, typeof(bool)),

            //sets the graph and graph variable name for this session, optional
            new RexProMessageMetaField(META_KEY_GRAPH_NAME, false, null, typeof(string)),
            new RexProMessageMetaField(META_KEY_GRAPH_OBJECT_NAME, false, "g", typeof(string)),

            //indicates variables defined in this request will not be available in the next
            new RexProMessageMetaField(META_KEY_ISOLATE_REQUEST, false, true, typeof(bool)),

            //indicates this request should be wrapped in a transaction
            new RexProMessageMetaField(META_KEY_TRANSACTION, false, true, typeof(bool)),

            // indicates the response should be toString'd
            new RexProMessageMetaField(META_KEY_CONSOLE, false, false, typeof(bool))
        };
            return fields;
        }


        public String LanguageName { get; private set; }
        public String Script { get; private set; }
        public RexProBindings Bindings { get; private set; }

        public ScriptRequestMessage(string script, IDictionary<string, object> bindings)
        {
            this.Script = script;
            this.Bindings = new RexProBindings(bindings);
        }

        public RexProBindings GetBindings()
        {
            return this.Bindings;
        }



        /**
         * Sets the inSession meta val
         */
        public void MetaSetInSession(Boolean val)
        {
            Meta.Add("inSession", val);
        }

        /**
         * Gets the inSession meta val, or the default if not set
         */
        public Boolean MetaGetInSession()
        {
            if (!Meta.ContainsKey(META_KEY_IN_SESSION))
            {
                return false;
            }
            else
            {
                return (Boolean)Meta.Get(META_KEY_IN_SESSION);
            }
        }

        public void MetaSetGraphName(String val)
        {
            Meta.Add(META_KEY_GRAPH_NAME, val);
        }

        public String MetaGetGraphName()
        {
            return (String)Meta.Get(META_KEY_GRAPH_NAME);
        }

        public void MetaSetGraphObjName(String val)
        {
            Meta.Add(META_KEY_GRAPH_OBJECT_NAME, val);
        }

        public String MetaGetGraphObjName()
        {
            return (String)Meta.Get(META_KEY_GRAPH_OBJECT_NAME);
        }

        public void MetaSetIsolate(bool val)
        {
            Meta.Add(META_KEY_ISOLATE_REQUEST, val);
        }

        public Boolean MetaGetIsolate()
        {
            return (Boolean)Meta.Get(META_KEY_ISOLATE_REQUEST);
        }

        public void MetaSetTransaction(bool val)
        {
            Meta.Add(META_KEY_TRANSACTION, val);
        }

        public Boolean MetaGetTransaction()
        {
            return (Boolean)Meta.Get(META_KEY_TRANSACTION);
        }

        public void MetaSetConsole(bool v)
        {
            Meta.Add(META_KEY_CONSOLE, v);
        }

        public Boolean MetaGetConsole()
        {
            return (Boolean)Meta.Get(META_KEY_CONSOLE);
        }
    }

    public class ScriptResponseMessage : RexProMessage
    {
        private static readonly MsgPackResultConverter Converter = new MsgPackResultConverter();

        public RexProScriptResult Result = new RexProScriptResult();
        public RexProBindings Bindings = new RexProBindings();

        public static byte[] ConvertResultToBytes(Object result)
        {
            return Converter.Convert(result);
        }
    }
}
