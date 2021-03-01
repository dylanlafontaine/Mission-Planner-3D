﻿/*     INFINITY CODE 2013-2019      */
/*   http://www.infinity-code.com   */

using UnityEditor;

[CustomEditor(typeof(OnlineMapsBingMapsElevationManager), true)]
public class OnlineMapsBingMapsElevationManagerEditor : OnlineMapsSinglePartElevationManagerEditor
{
    public SerializedProperty bingAPI;

    protected override void CacheSerializedFields()
    {
        base.CacheSerializedFields();

        bingAPI = serializedObject.FindProperty("bingAPI");
    }

    protected override void GenerateLayoutItems()
    {
        base.GenerateLayoutItems();

        rootLayoutItem.Create(bingAPI);
    }
}