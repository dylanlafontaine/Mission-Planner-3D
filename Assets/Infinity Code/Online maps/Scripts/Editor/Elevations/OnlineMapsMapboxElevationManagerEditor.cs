﻿/*     INFINITY CODE 2013-2019      */
/*   http://www.infinity-code.com   */

using UnityEditor;

[CustomEditor(typeof(OnlineMapsMapboxElevationManager), true)]
public class OnlineMapsMapboxElevationManagerEditor : OnlineMapsTiledElevationManagerEditor
{
    public SerializedProperty accessToken;

    protected override void CacheSerializedFields()
    {
        base.CacheSerializedFields();

        accessToken = serializedObject.FindProperty("accessToken");
    }

    protected override void GenerateLayoutItems()
    {
        base.GenerateLayoutItems();

        rootLayoutItem.Create(accessToken);
    }
}