﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Warlock.Networking
{
    public class DefaultNetworker : INetworker
    {
		public void SendOverStream(Stream stream, Notification value)
		{
			byte[] data = ServiceManager.Serializer.Serialize(value);
			byte[] dataLength = BitConverter.GetBytes(data.Length);
			stream.Write(dataLength, 0, dataLength.Length);
			stream.Write(data, 0, data.Length);
			stream.FlushAsync();
		}

		public Notification ReceiveFromStream(Stream stream)
		{
			byte[] dataLength = new byte[4];
			stream.Read(dataLength, 0, dataLength.Length);
			byte[] data = new byte[BitConverter.ToInt32(dataLength, 0)];
			stream.Read(data, 0, data.Length);
			Notification deserialized = ServiceManager.Serializer.Deserialize(data);
			return deserialized;
		}
    }

}
