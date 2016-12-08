using UnityEngine;
using System.Collections;
using System.IO;
using ExitGames.Client.Photon;
using System.Runtime.Serialization.Formatters.Binary;

public static class PhotonCustomSerialize{

	public static void RegisterCustomTypes()
	{
		PhotonPeer.RegisterType(typeof(StagePiece), (byte)'S', SerializeObject, DeserializeObject);
		PhotonPeer.RegisterType(typeof(StageSaveData), (byte)'D', SerializeObject, DeserializeObject);
		PhotonPeer.RegisterType(typeof(Character), (byte)'C', SerializeObject, DeserializeObject);
		PhotonPeer.RegisterType(typeof(Statistic), (byte)'T', SerializeObject, DeserializeObject);
		PhotonPeer.RegisterType(typeof(byte[]), (byte)'B', SerializeObject, DeserializeObject);
		//PhotonPeer.RegisterType(typeof(object), (byte)'O', SerializeObject, DeserializeObject);
	}

	public static byte[] SerializeObject(object customObject)
	{
		if(customObject == null)
			return null;

		BinaryFormatter bf = new BinaryFormatter();
		using(MemoryStream ms = new MemoryStream())
		{
			bf.Serialize(ms, customObject);
			return ms.ToArray();
		}
	}

	public static object DeserializeObject(byte[] bytes)
	{
		BinaryFormatter bf = new BinaryFormatter();
		using(MemoryStream ms = new MemoryStream())
		{
			ms.Write(bytes, 0, bytes.Length);
			ms.Seek(0, SeekOrigin.Begin);
			object customObject = bf.Deserialize(ms);
			return customObject;
		}
	}
}
