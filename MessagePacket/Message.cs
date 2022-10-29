using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using BayraktarGame;

namespace Message
{
    public enum MessageType
    {
        Data, Command
    }
    [Serializable]
    public abstract class MessagePacket
    {
        private MessageType _type;

        protected MessagePacket(MessageType type)
        {
            _type = type;
        }

        public static MessagePacket FromBytes(byte[] messageArray)
        {
            using (MemoryStream stream = new MemoryStream(messageArray))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                if (formatter.Deserialize(stream) is MessagePacket message)
                    return message;
                return null;
            }
        }

        public byte[] ToBytes()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                return stream.ToArray();
            }
        }
    }
    [Serializable]
    public class MessageCommand : MessagePacket
    {
        public MessageCommand() : base(MessageType.Command)
        {
        }
        public string Command { get; set; }

    }

    public enum AuthorizeMode
    {
        Registration, Login
    }
    [Serializable]
    public class MessageAuthorize : MessageCommand
    {
        public AuthorizeMode Mode { get; }
        public MessageAuthorize(AuthorizeMode mode)
        {
            Mode = mode;
        }
        public string Login { get; set; }
        public string Password { get; set; }
    }
    [Serializable]
    public class MessageBool : MessageCommand
    {
        public bool Response { get; }

        public MessageBool(bool response)
        {
            Response = response;
        }
    }
    [Serializable]
    public class Coords
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    [Serializable]
    public class MessageCoords : MessagePacket
    {
        public MessageCoords() : base(MessageType.Data)
        {
        }

        public MessageCoords(int x, int y) : this()
        {
            Coords.X = x;
            Coords.Y = y;
        }

        protected MessageCoords(Coords coords) : this()
        {
            Coords = coords;
        }

        public Coords Coords { get; set; }
    }
    [Serializable]
    public class MessageUnit: MessageCoords
    {
        public MessageUnit(Unit unit, int x, int y): base(x,y)
        {
            Unit = unit;
        }

        public MessageUnit(Unit unit, Coords coords) : base(coords)
        {
            Unit = unit;
        }
        public Unit Unit { get; set; } 
    }
}
