using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public static class Utilities {

	public static List<byte[]> SplitFileUp(byte[] bytes, int lengthToSplitAt)
	{
		List<byte[]> parts = new List<byte[]>();

		for (int i = 0; i < bytes.Length; i += lengthToSplitAt)
		{
			if(i + lengthToSplitAt > bytes.Length)
				lengthToSplitAt = bytes.Length - i;

			byte[] buffer = new byte[lengthToSplitAt];
			Buffer.BlockCopy(bytes, i, buffer, 0, lengthToSplitAt);
			parts.Add(buffer);
		}
		return parts;
	}

	public static byte[] CombineFile(List<byte[]> arrays)
	{
		int offset = 0;
		byte[] fullArray = new byte[arrays.Sum(a => a.Length)];
		foreach (byte[] array in arrays)
		{
			Buffer.BlockCopy(array, 0, fullArray, offset, array.Length);
			offset += array.Length;
		}
		return fullArray;
	}

	public static string GetKeyFromStringArray(string[] array)
	{
		string key = "";
		
		foreach(string str in array)
			key += str;

		return key;
	}

	public static void SetRenderersForGameObject(GameObject go, bool hidden)
	{
		Renderer[] comps = go.GetComponentsInChildren<Renderer>();

		foreach(Renderer comp in comps)
		{
			comp.enabled = !hidden;
		}

		CanvasRenderer[] cans = go.GetComponentsInChildren<CanvasRenderer>();

		foreach(CanvasRenderer can in cans)
		{
			can.gameObject.SetActive(!hidden);
		}
	}

	public static Texture2D ScaleTexture(Texture2D source,int targetWidth,int targetHeight) {
		Texture2D result=new Texture2D(targetWidth,targetHeight,source.format,true);
		Color[] rpixels=result.GetPixels(0);
		float incX=((float)1/source.width)*((float)source.width/targetWidth);
		float incY=((float)1/source.height)*((float)source.height/targetHeight);
		for(int px=0; px<rpixels.Length; px++) {
			rpixels[px] = source.GetPixelBilinear(incX*((float)px%targetWidth),
			                                      incY*((float)Mathf.Floor(px/targetWidth)));
		}
		result.SetPixels(rpixels,0);
		result.Apply();
		return result;
	}

	public static class UI
	{
		public static void UpdateContentBoxSize(GameObject contentBox, int objectCount, float objectScale)
		{
			RectTransform rt = contentBox.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2(rt.sizeDelta.x, objectCount * objectScale);
		}

		public static void ChangeImageTexture(Image img, byte[] newImgData)
		{
			Texture2D OldTexture = img.sprite.texture;
			Texture2D newTexture = new Texture2D(OldTexture.width, OldTexture.height);

			newTexture.LoadImage(newImgData);
			
			img.sprite = Sprite.Create(newTexture, 
			                                new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f,0.5f), img.sprite.pixelsPerUnit);

		}
	}

}
