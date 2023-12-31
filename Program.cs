using OpenMacroBoard.SDK;
using org.dmxc.lumos.Kernel.Misc;
using StreamDeckSharp;
using System;
using System.Linq;
using StreamDeckSDK = StreamDeckSharp.StreamDeck;

namespace org.dmxc.lumos.StreamDeck
{
    partial class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Currently Connected:");
                foreach (IDeviceReference deskRefecence in StreamDeckSDK.EnumerateDevices().ToArray())
                {
                    var sdkStreamDeck = deskRefecence.Open();
                    string sn = sdkStreamDeck.GetSerialNumber();
                    string fw = sdkStreamDeck.GetFirmwareVersion();
                    sdkStreamDeck.KeyStateChanged += SdkStreamDeck_KeyStateChanged;
                    sdkStreamDeck.ConnectionStateChanged += SdkStreamDeck_ConnectionStateChanged;

                    Console.WriteLine($"Discoverd Device Name: {deskRefecence.DeviceName};\tSN: {sn};\tFW: {fw}");
                }
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine("Discovered due to Listening:");
                var deviceDiscoveryContext = DeviceContext.Create().AddListener<StreamDeckListener>();
                var listener = new Listener();
                deviceDiscoveryContext.DeviceStateReports.Subscribe(listener);
                listener.NewDeviceConnected += Listener_NewDeviceConnected;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }

        private static void Listener_NewDeviceConnected(object sender, EventArgs<DeviceStateReport> e)
        {
            IDeviceReference deskRefecence = e.Value.DeviceReference;
            if (e.Value.Connected)
            {
                var sdkStreamDeck = deskRefecence.Open();
                string sn = sdkStreamDeck.GetSerialNumber();
                string fw = sdkStreamDeck.GetFirmwareVersion();
                sdkStreamDeck.KeyStateChanged += SdkStreamDeck_KeyStateChanged;

                Console.WriteLine($"Discoverd Device Name: {deskRefecence.DeviceName};\tSN: {sn};\tFW: {fw}");
            }
            else
            {
                Console.WriteLine($"Disconnected Device Name: {deskRefecence.DeviceName}");
            }
        }

        private static void SdkStreamDeck_KeyStateChanged(object sender, OpenMacroBoard.SDK.KeyEventArgs e)
        {
            IMacroBoard sdkStreamDeck = sender as IMacroBoard;
            string sn = sdkStreamDeck.GetSerialNumber();
            Console.WriteLine($"KeyChanged SN: {sn};\tButtonID: {e.Key}");
        }

        private static void SdkStreamDeck_ConnectionStateChanged(object sender, OpenMacroBoard.SDK.ConnectionEventArgs e)
        {
            IMacroBoard sdkStreamDeck = sender as IMacroBoard;
            string sn = sdkStreamDeck.GetSerialNumber();
            string state = e.NewConnectionState ? "Connected" : "Disconnected";
            Console.WriteLine($"ConnectionChanged SN: {sn};\tState:{state}");
        }
    }
}
