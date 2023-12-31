using LumosLIB.Tools;
using LumosToolsLIB.Tools;
using OpenMacroBoard.SDK;
using org.dmxc.lumos.Kernel.Input.Macroboard;
using System;
using System.Drawing;

namespace org.dmxc.lumos.StreamDeck
{
    public sealed class StreamDeckKey : IKey
    {
        public uint ID { get; }

        public uint X_Index { get; }
        public uint Y_Index { get; }

        public string Name { get; }

        public uint Width { get; }
        public uint Height { get; }

        public Rectangle Area { get; }

        public event EventHandler<Kernel.Input.Macroboard.KeyEventArgs> KeyStateChanged;

        public IGUIMacroBoard ParentMacroBoard { get; internal set; }
        public IMacroBoard sdkStreamDeck => (this.ParentMacroBoard as StreamDeckBoard).sdkStreamDeck;

        public StreamDeckKey(IGUIMacroBoard parent, uint id, Rectangle area, uint x_Index, uint y_Index)
        {
            this.ParentMacroBoard = parent;

            this.ID = id;
            this.X_Index = x_Index;
            this.Y_Index = y_Index;

            this.Area = area;
            this.Width = (uint)area.Width;
            this.Height = (uint)area.Height;

            this.Name = $"Button {Y_Index + 1}.{X_Index + 1}";

            this.sdkStreamDeck.KeyStateChanged += SdkStreamDeck_KeyStateChanged;
        }

        private void SdkStreamDeck_KeyStateChanged(object sender, OpenMacroBoard.SDK.KeyEventArgs e)
        {
            if (e.Key == this.ID)
                this.KeyStateChanged.InvokeFailSafe(this, new Kernel.Input.Macroboard.KeyEventArgs(this, e.IsDown));
        }

        public void SetKeyBitmap(byte[] bitmap)
        {
            if (!this.sdkStreamDeck.IsConnected)
                return;

            this.sdkStreamDeck.SetKeyBitmap((int)this.ID, KeyBitmap.Create.FromStream(bitmap.ToMS()));
        }
        public void ClearKey()
        {
            if (!this.sdkStreamDeck.IsConnected)
                return;

            this.sdkStreamDeck.ClearKey((int)this.ID);
        }
        public override string ToString()
        {
            return $"{this.ParentMacroBoard.ID}-{this.Name}";
        }
    }
}
