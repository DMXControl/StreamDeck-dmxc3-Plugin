using LumosLIB.Tools;
using OpenMacroBoard.SDK;
using org.dmxc.lumos.Kernel.Input.Macroboard;
using System;
using System.Collections.Generic;

namespace org.dmxc.lumos.StreamDeck
{
    public sealed class StreamDeckBoard : AbstractMacroBoard, IMacroBoard_SetBrightness, IMacroBoard_DrawFullScreenBitmap

    {
        internal IMacroBoard sdkStreamDeck;
        public string FirmwareVersion => this.sdkStreamDeck.GetFirmwareVersion();
        public string SerialNumber => this.sdkStreamDeck.GetSerialNumber();
        public override bool IsConnected => this.sdkStreamDeck.IsConnected;

        public StreamDeckBoard(IMacroBoard sdkStreamDeck) : base(sdkStreamDeck.GetSerialNumber())
        {
            this.sdkStreamDeck = sdkStreamDeck;

            this.Width = (uint)this.sdkStreamDeck.Keys.CountX;
            this.Height = (uint)this.sdkStreamDeck.Keys.CountY;

            this.sdkStreamDeck.ConnectionStateChanged += SdkStreamDeck_ConnectionStateChanged;

            base.initialize();
        }
        protected override IKey[] generateKeys()
        {
            List<IKey> keys = new List<IKey>();
            foreach (var sdkKey in this.sdkStreamDeck.Keys)
            {
                uint keyID = (uint)this.sdkStreamDeck.Keys.IndexOf(sdkKey);
                uint x = (uint)(keyID % this.sdkStreamDeck.Keys.CountX);
                uint y = (uint)(keyID / this.sdkStreamDeck.Keys.CountX);
                var key = new StreamDeckKey(this, keyID, new System.Drawing.Rectangle(sdkKey.X, sdkKey.Y, sdkKey.Width, sdkKey.Height), x, y);
                keys.Add(key);
            }
            return keys.ToArray();
        }

        private void SdkStreamDeck_ConnectionStateChanged(object sender, ConnectionEventArgs e)
        {
            if (e.NewConnectionState)
                this.OnConnected(EventArgs.Empty);
            else
                this.OnDisconnected(EventArgs.Empty);
        }

        public override void SetAllKeyBitmaps(byte[] bitmap)
        {
            if (!this.sdkStreamDeck.IsConnected)
                return;

            this.sdkStreamDeck.SetKeyBitmap(KeyBitmap.Create.FromStream(bitmap.ToMS()));
        }
        public override void ClearAllKeys()
        {
            if (!this.sdkStreamDeck.IsConnected)
                return;

            this.sdkStreamDeck.ClearKeys();
        }
        public override void ShowLogo()
        {
            if (!this.sdkStreamDeck.IsConnected)
                return;

            this.sdkStreamDeck.ShowLogo();
        }
        public void SetBrightness(byte brightness)
        {
            if (!this.sdkStreamDeck.IsConnected)
                return;

            this.sdkStreamDeck.SetBrightness(brightness);
        }
        public void DrawFullScreenBitmap(byte[] bitmap)
        {
            if (!this.sdkStreamDeck.IsConnected)
                return;

            this.sdkStreamDeck.SetKeyBitmap(KeyBitmap.Create.FromStream(bitmap.ToMS()));
        }

        public override void Dispose()
        {
            this.sdkStreamDeck.Dispose();
        }
    }
}