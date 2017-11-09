using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_UWP
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;
#endif 

public class Beacon
{
    public Guid Uuid;
    public ushort MajorId;
    public ushort MinorId;
}

public class BeaconReceiver : MonoBehaviour {

#if UNITY_UWP
    BluetoothLEAdvertisementWatcher watcher;
    public static ushort BEACON_ID = 0x004C;
#endif

    public Action<Beacon> BeaconReceived = delegate { };

    void Awake()
    {
#if UNITY_UWP
        watcher = new BluetoothLEAdvertisementWatcher();
        var manufacturerData = new BluetoothLEManufacturerData
        {
        CompanyId = BEACON_ID,
        };
        watcher.AdvertisementFilter.Advertisement.ManufacturerData.Add(manufacturerData);
        watcher.Received += Watcher_Received;
        watcher.Start();
#endif
    }

#if UNITY_UWP
    private void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
    {
        foreach(BluetoothLEManufacturerData md in args.Advertisement.ManufacturerData)
        {
            ushort identifier = md.CompanyId;
            if(identifier != BEACON_ID)
            {
                continue;
            }
            DataReader reader = DataReader.FromBuffer(md.Data);
            byte advertisementType = reader.ReadByte();
            byte len = reader.ReadByte();
            if(advertisementType == 0x02 && len == 0x15)
            {
                int a = reader.ReadInt32();
                short b = reader.ReadInt16();
                short c = reader.ReadInt16();
                byte[] d = new byte[8];
                reader.ReadBytes(d);
                Guid uuid = new Guid(a,b,c,d);
                ushort major = reader.ReadUInt16();
                ushort minor = reader.ReadUInt16();
                Debug.Log(uuid + " " + major + " " + minor + " " + args.RawSignalStrengthInDBm);

                var beacon = new Beacon
                {
                    Uuid = uuid,
                    MajorId = major,
                    MinorId = minor
                };

                UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                {
                    BeaconReceived(beacon);
                }, true);
            }
        }
    }
#endif
}
