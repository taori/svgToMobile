using System.Collections.Generic;

namespace mobile.imagetools.shared.Extensions
{
	public static class DictionaryExtensions
	{
		public static TValue GetInitializedValue<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key) where TValue : new()
		{
			if (!source.TryGetValue(key, out var value))
			{
				value = new TValue();
				source.Add(key, value);
				return value;
			}

			return value;
		}
	}
}