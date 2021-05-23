using System;
using System.Collections.Generic;

// Token: 0x0200013E RID: 318
public class ListUtils
{
	// Token: 0x0600077C RID: 1916 RVA: 0x00037BC4 File Offset: 0x00035DC4
	public static void Shuffle<T>(List<T> list)
	{
		Random random = new Random();
		int i = list.Count;
		while (i > 1)
		{
			i--;
			int index = random.Next(i + 1);
			T value = list[index];
			list[index] = list[i];
			list[i] = value;
		}
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x00037C18 File Offset: 0x00035E18
	public static List<T> Copy<T>(List<T> list)
	{
		List<T> list2 = new List<T>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(list[i]);
		}
		return list2;
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x00037C50 File Offset: 0x00035E50
	public static Dictionary<K, V> CopyDictionary<K, V>(Dictionary<K, V> dictionary)
	{
		Dictionary<K, V> dictionary2 = new Dictionary<K, V>();
		foreach (K key in dictionary.Keys)
		{
			dictionary2.Add(key, dictionary[key]);
		}
		return dictionary2;
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x00037CB8 File Offset: 0x00035EB8
	public static T[] CopyArray<T>(T[] array)
	{
		T[] array2 = new T[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = array[i];
		}
		return array2;
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x00037CF4 File Offset: 0x00035EF4
	public static List<KT> DictionaryKeysToList<KT, VT>(Dictionary<KT, VT> dictionary)
	{
		List<KT> list = new List<KT>();
		foreach (KT item in dictionary.Keys)
		{
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x00037D58 File Offset: 0x00035F58
	public static List<VT> DictionaryValuesToList<KT, VT>(Dictionary<KT, VT> dictionary)
	{
		List<VT> list = new List<VT>();
		foreach (VT item in dictionary.Values)
		{
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x00037DBC File Offset: 0x00035FBC
	public static List<int> GetIndexList<T>(List<T> list)
	{
		List<int> list2 = new List<int>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(i);
		}
		return list2;
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x00037DF0 File Offset: 0x00035FF0
	public static List<T> RemoveListElementsFromList<T>(List<T> haystackList, List<T> needleList)
	{
		List<T> list = ListUtils.Copy<T>(haystackList);
		for (int i = 0; i < needleList.Count; i++)
		{
			while (list.Contains(needleList[i]))
			{
				list.RemoveAt(list.IndexOf(needleList[i]));
			}
		}
		return list;
	}
}
