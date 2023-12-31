using OpenMacroBoard.SDK;
using org.dmxc.lumos.Kernel.Input.Macroboard;
using StreamDeckSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using StreamDeckSDK = StreamDeckSharp.StreamDeck;

namespace org.dmxc.lumos.StreamDeck
{
    public class StreamDeckProvider : AbstractMacroBoardProvider
    {
        public override string Name => "StreamDeck";
        private Dictionary<string, StreamDeckBoard> connectedBoards = new Dictionary<string, StreamDeckBoard>();

        private Listener listener;

        public StreamDeckProvider() : base()
        {
        }

        protected override void initialize()
        {
            try
            {
                var deviceDiscoveryContext = DeviceContext.Create().AddListener<StreamDeckListener>();
                var listener = new Listener();
                deviceDiscoveryContext.DeviceStateReports.Subscribe(listener);
                listener.NewDeviceConnected += Listener_NewDeviceConnected;
            }
            catch (Exception e)
            {
                log.ErrorOrDebug(e);
            }
            this.getAvailableMacroBoards(true);
        }

        private void Listener_NewDeviceConnected(object sender, EventArgs<DeviceStateReport> e)
        {
            try
            {
                this.getAvailableMacroBoards(true);
            }
            catch (Exception ex)
            {
                log.ErrorOrDebug(ex);
            }
        }

        private AbstractMacroBoard[] getAvailableMacroBoards(bool fireEvents = false)
        {
            List<AbstractMacroBoard> availableBoards = new List<AbstractMacroBoard>();
            foreach (IDeviceReference deskRefecence in StreamDeckSDK.EnumerateDevices().ToArray())
            {
                try
                {
                    var sdkStreamDeck = deskRefecence.Open();
                    string sn = sdkStreamDeck.GetSerialNumber();
                    if (this.connectedBoards.ContainsKey(sn))
                    {
                        //sdkStreamDeck.Dispose(); //Not Dispose, this trigges a teh ScreenSaver!!! pgrote 18.10.2021
                        availableBoards.Add(this.connectedBoards[sn]);
                        continue;
                    }
                    var streamDeck = new StreamDeckBoard(sdkStreamDeck);
                    streamDeck.Disconnected += StreamDeck_Disconnected;
                    this.connectedBoards[sn] = streamDeck;
                    log.Info($"Detected {deskRefecence.DeviceName} with SN: {streamDeck.SerialNumber} FW: {streamDeck.FirmwareVersion}");
                    availableBoards.Add(streamDeck);

                    if (fireEvents)
                        this.OnMacroBoardChanged(new MacroBoardEventArgs(LumosProtobuf.EChangeType.Added, streamDeck));
                }
                catch (Exception e)
                {
                    log.Error(e);
                }
            }

            var missing = this.connectedBoards.Select(d => d.Key).Except(availableBoards.Select(d => d.ID)).ToArray();
            foreach (var id in missing)
            {
                var macroBoard = this.connectedBoards[id];
                this.connectedBoards.Remove(id);
                macroBoard.Dispose();
                if (fireEvents)
                    this.OnMacroBoardChanged(new MacroBoardEventArgs(LumosProtobuf.EChangeType.Removed, macroBoard));
            }

            return availableBoards.ToArray();
        }

        private void StreamDeck_Disconnected(object sender, EventArgs e)
        {
            StreamDeckBoard macroBoard = sender as StreamDeckBoard;
            this.connectedBoards.Remove(macroBoard.ID);
            this.OnMacroBoardChanged(new MacroBoardEventArgs(LumosProtobuf.EChangeType.Removed, macroBoard));
        }

        public override AbstractMacroBoard[] GetAvailableMacroBoards()
        {
            return this.connectedBoards.Select(d => d.Value).Cast<AbstractMacroBoard>().ToArray();
        }

        public override void Dispose()
        {
            foreach (var board in this.connectedBoards.Select(m => m.Value).ToArray())
            {
                board.Dispose();
            }
            this.connectedBoards.Clear();
        }
    }
}
