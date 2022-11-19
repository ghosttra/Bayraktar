using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using BayraktarGame;

namespace Message
{
    //public enum MessageType
    //{
    //    Data, Command,
    //    Null,
    //    Text
    //}

    [Serializable]
    public abstract class MessagePacket
    {
        protected MessagePacket()
        {
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
    public class MessageText : MessagePacket
    {
        public string Text { get; set; }
    }

    [Serializable]
    public class MessageNull : MessagePacket
    {
    }

    [Serializable]
    public class MessageGameResult: MessageCommand
    {
        public MessageGameResult()
        {
            Command = "GAME_OVER";
        }
        public int Score { get; set; }
        public User User { get; set; }
        public IPEndPoint Server { get; set; }
    }
    [Serializable]
    public class MessageGameData : MessagePacket
    {
        public IPEndPoint Server { get; set; }
        public GameRole GameRole { get; set; }
        public List<Unit> Units { get; set; }
        
    }

    [Serializable]
    public class MessageConnectionGame : MessagePacket
    {
        public int LocalPort { get; set; }
        public GameMode GameMode { get; set; }

        
    }
    [Serializable]
    public class MessageDataContent : MessagePacket
    {
        

        public MessageDataContent(object content) 
        {
            SetContent(content);
        }
        public string Description { get; set; }
        public byte[] Content { get; set; }

        public void SetContent(object content)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter form = new BinaryFormatter();
                    form.Serialize(stream, content);
                    Content = stream.ToArray();
                }
            }
            catch (Exception e)
            {
                Content = null;
            }
        }
    }

    [Serializable]
    public class MessageCommand : MessagePacket
    {
        
        public string Command { get; set; }
        public object Additional { get; set; }

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
    public class MessageBool : MessagePacket
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
        

        public MessageCoords(int x, int y) 
        {
            Coords = new Coords
            {
                X = x,
                Y = y
            };
        }

        protected MessageCoords(Coords coords) 
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
        public int Id { get; set; }
        public int Speed { get; set; } = 5;
    }
}
