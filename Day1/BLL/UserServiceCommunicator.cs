using BLL.Models;
using NetworkConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL
{
    [Serializable]
    public class UserServiceCommunicator : MarshalByRefObject, IDisposable
    {
        public event EventHandler<UserDataApdatedEventArgs> UserAdded;
        public event EventHandler<UserDataApdatedEventArgs> UserDeleted;
        private Sender<BllUser> _sender;
        private Task recieverTask;
        private CancellationTokenSource tokenSource;
        private Receiver<BllUser> _receiver;

        public UserServiceCommunicator(Sender<BllUser> sender, Receiver<BllUser> receiver)
        {
            _sender = sender;
            _receiver = receiver;
        }

        public UserServiceCommunicator(Sender<BllUser> sender) : this(sender, null) { }
        public UserServiceCommunicator(Receiver<BllUser> receiver) : this(null, receiver) { }

        public void RunReceiver()
        {
            if (_receiver == null) return;
            tokenSource = new CancellationTokenSource();
            recieverTask = Task.Run((Action)ReceiveMessages, tokenSource.Token);
        }

        public void Connect(IEnumerable<IPEndPoint> endPoints)
        {
            _sender.Connect(endPoints);
        }

        public void StopReceiver()
        {
            if (tokenSource.Token.CanBeCanceled)
            {
                tokenSource.Cancel();
            }
        }

        private void ReceiveMessages()
        {
            while (true)
            {
                if (tokenSource.IsCancellationRequested) return;
                var message = _receiver.Receive();
                var args = new UserDataApdatedEventArgs
                {
                    User = message.Entity
                };
                switch (message.MessageType)
                {
                    case MessageType.Add: OnUserAdded(this, args); break;
                    case MessageType.Delete: OnUserDeleted(this, args); break;
                }
            }
        }

        public void SendAdd(UserDataApdatedEventArgs args)
        {
            if (_sender == null) return;

            Send(new NetworkMessage<BllUser>
            {
                Entity = args.User,
                MessageType = MessageType.Add
            });
        }
        public void SendDelete(UserDataApdatedEventArgs args)
        {
            if (_sender == null) return;

            Send(new NetworkMessage<BllUser>
            {
                Entity = args.User,
                MessageType = MessageType.Delete
            });
        }

        private void Send(NetworkMessage<BllUser> message)
        {
            _sender.Send(message);
        }

        protected virtual void OnUserDeleted(object sender, UserDataApdatedEventArgs args)
        {
            UserDeleted?.Invoke(sender, args);
        }

        protected virtual void OnUserAdded(object sender, UserDataApdatedEventArgs args)
        {
            UserAdded?.Invoke(sender, args);
        }

        public void Dispose()
        {
            _receiver?.Dispose();
            _sender?.Dispose();
        }
    }
}
