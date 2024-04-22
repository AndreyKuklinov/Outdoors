using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.Utils
{
    public static class AssetDatabaseExtensions
    {
        public static T[] FindAssets<T>(string filter = null) where T : Object
        {
            var type = typeof(T).Name;
            var typeFilter = string.IsNullOrEmpty(filter) ? $"t: {type}" : $"t: {type} {filter}";
            var guids = AssetDatabase.FindAssets(typeFilter);
            var assets = new T[guids.Length];
            for (int i = 0; i < assets.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
            return assets;
        }

        public static T FindAsset<T>(string filter = null) where T : Object
        {
            return FindAssets<T>(filter).OrderBy(a => a ? a.name : string.Empty).FirstOrDefault();
        }
    }

    public static class AssetDatabaseUtils
    {
        [System.Obsolete("Soon will be removed")]
        public static T[] FindAssets<T>() where T : Object
        {
            return AssetDatabaseExtensions.FindAssets<T>();
        }
    }
}
