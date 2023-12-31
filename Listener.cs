using OpenMacroBoard.SDK;
using System;

namespace org.dmxc.lumos.StreamDeck
{
    public class EventArgs<T> : EventArgs
    {
        public readonly T Value;

        public EventArgs(T value)
        {
            Value = value;
        }
    }
    public class Listener : IObserver<DeviceStateReport>
    {
        public event EventHandler<EventArgs<DeviceStateReport>> NewDeviceConnected;
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(DeviceStateReport value)
        {
            this.NewDeviceConnected?.Invoke(this, new EventArgs<DeviceStateReport>(value));
        }
    }
}