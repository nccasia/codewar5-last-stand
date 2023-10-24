#if UNITY_EDITOR
using System.Reflection;
using UnityEditorInternal;

namespace Anthill.Utils
{
	public class AntUtils
	{
		/// <summary>
		/// Returns an array of the names of the sorting layers available in the editor.
		/// </summary>
		/// <returns>An array of sorting layer names.</returns>
		public static string[] GetSortingLayerNames()
		{
			System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
			PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
			return (string[])sortingLayersProperty.GetValue(null, new object[0]);
		}

		/// <summary>
		/// Recurs the array of indexes of the sorting layers available in the editor.
		/// </summary>
		/// <returns>Array of indexes of sorting layers.</returns>
		public static int[] GetSortingLayerUniqueIDs()
		{
			System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
			PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
			return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
		}
	}
}
#endif