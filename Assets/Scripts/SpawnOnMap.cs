using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;

public class SpawnOnMap : MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;

    [SerializeField]
    [Geocode]
    List<string> _locationStrings;
    Vector2d[] _locations;

    [SerializeField]
    float _spawnScale = 100f;

    [SerializeField]
    GameObject _markerPrefab;

    public List<GameObject> _spawnedObjects = new List<GameObject>();

    public void SpawnWaypointsOnMapFromString(string location, float altitude) {
        _locationStrings.Clear();
		_locationStrings.Add(location);
        _locations = new Vector2d[_locationStrings.Count];
        for (int i = 0; i < _locationStrings.Count; i++)
        {
            var locationString = _locationStrings[i];
            _locations[i] = Conversions.StringToLatLon(locationString);
            var instance = Instantiate(_markerPrefab);
            instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
            instance.transform.localPosition = new Vector3(instance.transform.localPosition.x, altitude, instance.transform.localPosition.z);
            instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            _spawnedObjects.Add(instance);
        }

		for (int i = 0; i < _spawnedObjects.Count - 1; i++) {
			GameObject renderObject = new GameObject();
			LineRenderer line = renderObject.AddComponent<LineRenderer>();
			line.startWidth = .5f;
			line.endWidth = .1f;
			line.enabled = true;
			line.SetPosition(0, _spawnedObjects[i].transform.localPosition);
			line.SetPosition(1, _spawnedObjects[i + 1].transform.localPosition);
		}
    }

    public void SpawnWaypointsOnMap(string location, Transform playerTransform, Transform playerForwardTransform)
    {
		_locationStrings.Clear();
		_locationStrings.Add(location);
        _locations = new Vector2d[_locationStrings.Count];
        for (int i = 0; i < _locationStrings.Count; i++)
        {
            var locationString = _locationStrings[i];
            _locations[i] = Conversions.StringToLatLon(locationString);
            var instance = Instantiate(_markerPrefab);
            instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
			instance.transform.localPosition = new Vector3(instance.transform.localPosition.x, 0, instance.transform.localPosition.z) + new Vector3(playerForwardTransform.forward.x * 3, playerForwardTransform.forward.y * 3, playerForwardTransform.forward.z * 3);
			instance.transform.localPosition += new Vector3(0, playerTransform.localPosition.y, 0);
            instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            _spawnedObjects.Add(instance);
        }

		for (int i = 0; i < _spawnedObjects.Count - 1; i++) {
			GameObject renderObject = new GameObject();
			LineRenderer line = renderObject.AddComponent<LineRenderer>();
			line.startWidth = .5f;
			line.endWidth = .1f;
			line.enabled = true;
			line.SetPosition(0, _spawnedObjects[i].transform.position);
			line.SetPosition(1, _spawnedObjects[i + 1].transform.position);
		}
    }
}