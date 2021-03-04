using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealWorldTilesetSize : MonoBehaviour
{
    private void Start()
        {
            // Initial resize
            UpdateSize();

            // Subscribe to change zoom
            OnlineMaps.instance.OnChangeZoom += OnChangeZoom;
        }

        private void OnChangeZoom()
        {
            UpdateSize();
        }

        private void UpdateSize()
        {
            // Get distance (km) between corners of map
            Vector2 distance = OnlineMapsUtils.DistanceBetweenPoints(OnlineMaps.instance.topLeftPosition,
                OnlineMaps.instance.bottomRightPosition);

            // Set tileset size
            OnlineMapsControlBaseDynamicMesh.instance.sizeInScene = distance * 1000;

            // Redraw map
            OnlineMaps.instance.Redraw();
        }
}
