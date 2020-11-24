using System;
using System.Text;
using UnityEngine;

public class Horizon2_OM3 : MonoBehaviour
{
    /// <summary>
    /// Mapbox Access Token for elevations.
    /// Leave blank if you do not want to use elevations.
    /// </summary>
    [Tooltip("Mapbox Access Token for elevations.\nLeave blank if you do not want to use elevations.")]
    public string mapboxKey;

    /// <summary>
    /// Number of tiles horizontally (X axis).
    /// </summary>
    [Tooltip("Number of tiles horizontally (X axis).")]
    public int countX = 3;

    /// <summary>
    /// Number of tiles vertically (Z axis).
    /// </summary>
    [Tooltip("Number of tiles vertically (Z axis).")]
    public int countY = 3;

    /// <summary>
    /// Offset of the horizon mesh along the y-axis relative to the map. Keep it negative.
    /// </summary>
    [Tooltip("Offset of the horizon mesh along the y-axis relative to the map. Keep it negative.")]
    public float positionYOffset = -5;

    /// <summary>
    /// Offset zoom of tiles relative to map.zoom. Keep positive.
    /// </summary>
    [Tooltip("Offset zoom of tiles relative to map.zoom. Keep positive.")]
    public int zoomOffset = 3;

    /// <summary>
    /// Shader of the tiles mesh.
    /// </summary>
    [Tooltip("Shader of the tiles mesh.")]
    public Shader shader;

    /// <summary>
    /// Offset of the render queue relative to render queue of the shader.
    /// </summary>
    [Tooltip("Offset of the render queue relative to render queue of the shader.")]
    public int renderQueueOffset = 100;

    /// <summary>
    /// Tile resolution.
    /// </summary>
    [Tooltip("Tile resolution.")]
    public int resolution = 32;

    private OnlineMapsTile[] tiles;
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;
    private Vector2[] uv;
    private int[] triangles;
    private OnlineMaps map;
    private OnlineMapsTileSetControl control;
    private MeshRenderer meshRenderer;

    private Vector2 ctl;
    private Vector2 cbr;

    private void DownloadElevation(OnlineMapsTile tile)
    {
        if (string.IsNullOrEmpty(mapboxKey)) return;

        string url = new StringBuilder("https://api.mapbox.com/v4/mapbox.terrain-rgb/")
            .Append(tile.zoom).Append("/").Append(tile.x).Append("/").Append(tile.y)
            .Append(".pngraw?access_token=").Append(mapboxKey).ToString();
        OnlineMapsWWW www = new OnlineMapsWWW(url);
        www.OnComplete += delegate
        {
            tile["cdata"] = new CData(www.bytes);
            OnGetElevation(ctl.x, ctl.y, cbr.x, cbr.y);
            UpdateMesh();
        };
    }

    private void InitMesh()
    {
        GameObject go = new GameObject("Horizon");
        go.transform.parent = map.gameObject.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.Euler(Vector3.zero);
        go.transform.localScale = Vector3.one;

        MeshFilter meshFilter = go.AddComponent<MeshFilter>();
        meshRenderer = go.AddComponent<MeshRenderer>();
        int countTiles = countX * countY;

        mesh = new Mesh();
        mesh.name = "Horizon";
        mesh.MarkDynamic();

        meshFilter.sharedMesh = mesh;

        tiles = new OnlineMapsTile[countTiles];

        int countVertices = (countX + 1) * (countY + 1) * resolution * resolution;
        vertices = new Vector3[countVertices];
        normals = new Vector3[countVertices];
        uv = new Vector2[countVertices];
        triangles = new int[6 * resolution * resolution];

        mesh.vertices = vertices;
        mesh.subMeshCount = countTiles;
        float r1 = resolution - 1;

        int index = 0;
        for (int i = 0; i < (countX + 1) * (countY + 1); i++)
        {
            for (int x = 0; x < resolution; x++)
            {
                for (int y = 0; y < resolution; y++)
                {
                    normals[index] = Vector3.up;
                    uv[index++] = new Vector2(x / r1, 1 - y / r1);
                }
            }
        }

        mesh.uv = uv;
        mesh.normals = normals;

        Material[] materials = new Material[countTiles];

        for (int i = 0; i < countTiles; i++)
        {
            int ti = 0;
            for (int x = 0; x < resolution - 1; x++)
            {
                for (int y = 0; y < resolution - 1; y++)
                {
                    int vi = i * resolution * resolution + x * resolution + y;
                    triangles[ti] = vi;
                    triangles[ti + 1] = vi + resolution + 1;
                    triangles[ti + 2] = vi + 1;
                    triangles[ti + 3] = vi;
                    triangles[ti + 4] = vi + resolution;
                    triangles[ti + 5] = vi + resolution + 1;
                    ti += 6;
                }
            }

            mesh.SetTriangles(triangles, i);

            Material material = new Material(shader);
            material.renderQueue = shader.renderQueue + renderQueueOffset;
            materials[i] = material;
        }

        meshRenderer.sharedMaterials = materials;

        UpdateMesh();

        mesh.RecalculateBounds();
    }

    private void OnGetElevation(double leftLng, double topLat, double rightLng, double bottomLat)
    {
        ctl = new Vector2((float)leftLng, (float)topLat);
        cbr = new Vector2((float)rightLng, (float)bottomLat);

        double tlx, tly, brx, bry;

        map.projection.CoordinatesToTile(ctl.x, ctl.y, map.zoom, out tlx, out tly);
        map.projection.CoordinatesToTile(cbr.x, cbr.y, map.zoom, out brx, out bry);

        int scale = 1 << zoomOffset;

        int zoom = map.zoom - zoomOffset;

        short[,] heights = new short[32, 32];
        double rx = (brx - tlx) / 31;
        double ry = (bry - tly) / 31;

        for (int x = 0; x < 32; x++)
        {
            double tx = (rx * x + tlx) / scale;

            for (int y = 0; y < 32; y++)
            {
                double ty = (ry * y + tly) / scale;

                OnlineMapsTile tile = OnlineMapsTile.GetTile(zoom, (int) tx, (int) ty);
                if (tile == null)
                {
                    heights[x, y] = 0;
                    continue;
                }
                CData data = tile["cdata"] as CData;
                if (data == null)
                {
                    heights[x, y] = 0;
                    continue;
                }
                heights[x, 31 - y] = data.GetElevation(tx, ty);
            }
        }

        if (OnlineMapsElevationManagerBase.isActive) OnlineMapsBingMapsElevationManager.instance.SetElevationData(heights);
    }

    private void OnTileDownloaded(OnlineMapsTile tile)
    {
        for (int i = 0; i < countX * countY; i++) if (tiles[i] == tile) meshRenderer.sharedMaterials[i].mainTexture = tile.texture;
        DownloadElevation(tile);
    }

    private void Start()
    {
        if (zoomOffset <= 0) throw new Exception("Zoom offset should be positive.");
        if (shader == null) shader = Shader.Find("Diffuse");

        map = OnlineMaps.instance;
        control = OnlineMapsTileSetControl.instance;
        OnlineMapsTile.OnTileDownloaded += OnTileDownloaded;
        if (OnlineMapsCache.instance != null)
        {
            OnlineMapsCache.instance.OnLoadedFromCache += OnTileDownloaded;
        }
        map.OnMapUpdated += UpdateMesh;
        if (OnlineMapsElevationManagerBase.isActive) OnlineMapsElevationManagerBase.instance.OnGetElevation += OnGetElevation;

        InitMesh();
    }

    private void UpdateMesh()
    {
        int zoom = map.zoom - zoomOffset;
        if (zoom < 3) zoom = 3;

        for (int i = 0; i < countX * countY; i++) if (tiles[i] != null) tiles[i].Unblock(this);

        double tx, ty;
        map.GetTilePosition(out tx, out ty, zoom);

        int itx = Mathf.RoundToInt((float)(tx - countX / 2f));
        int ity = Mathf.RoundToInt((float)(ty - countY / 2f));

        Vector3 offset = new Vector3(0, positionYOffset, 0) - transform.position;

        int max = 1 << zoom;
        Material[] materials = meshRenderer.sharedMaterials;

        float r1 = resolution - 1;
        int vi = 0;

        double tlx, tly, brx, bry;
        map.GetCorners(out tlx, out tly, out brx, out bry);
        float elevationScale = OnlineMapsElevationManagerBase.GetBestElevationYScale(tlx, tly, brx, bry);

        int ox = countY * resolution * resolution - (resolution - 1) * resolution;
        int oz = resolution * (resolution - 1) + 1;

        for (int x = 0; x < countX; x++)
        {
            int tileX = itx + x;
            int nextTileX = tileX + 1;
            if (tileX >= max) tileX -= max;
            if (nextTileX >= max) nextTileX -= max;

            for (int y = 0; y < countY; y++)
            {
                int tileY = ity + y;
                int nextTileY = tileY + 1;

                if (tileY >= max) tileY -= max;
                if (nextTileY >= max) nextTileY -= max;

                OnlineMapsTile tile = OnlineMapsTile.GetTile(zoom, tileX, tileY);
                if (tile == null)
                {
                    OnlineMapsTile parentTile = OnlineMapsTile.GetTile(zoom - 1, tileX / 2, tileY / 2);
                    tile = new OnlineMapsRasterTile(tileX, tileY, zoom, map);
                    tile.parent = parentTile;
                }
                int tileIndex = x * countY + y;
                tiles[tileIndex] = tile;
                tile.Block(this);

                CData data = tile["cdata"] as CData;
                bool hasElevation = data != null;

                double px, py;

                map.projection.TileToCoordinates(tileX, tileY, zoom, out px, out py);
                Vector3 v1 = control.GetWorldPosition(px, py) + offset;

                map.projection.TileToCoordinates(nextTileX, nextTileY, zoom, out px, out py);
                Vector3 v2 = control.GetWorldPosition(px, py) + offset;
                Vector3 ov = (v2 - v1) / r1;

                for (int vx = 0; vx < resolution; vx++)
                {
                    for (int vz = 0; vz < resolution; vz++)
                    {
                        Vector3 v = new Vector3(ov.x * vx + v1.x, 0, ov.z * vz + v1.z);
                        if (hasElevation)
                        {
                            if (vz == 0 && y > 0) v.y = vertices[vi - oz].y;
                            else if (vx == 0 && x > 0) v.y = vertices[vi - ox].y;
                            else
                            {
                                double evx = vx / r1;
                                double evz = vz / r1;
                                if (evx >= 1) evx = 0.999;
                                if (evz >= 1) evz = 0.999;
                                if (OnlineMapsElevationManagerBase.isActive) v.y = data.GetElevation(evx, evz) * elevationScale + offset.y;
                                else v.y = positionYOffset;
                            }
                        }
                        else v.y = positionYOffset;
                        vertices[vi++] = v;
                    }
                }

                materials[tileIndex].mainTexture = tile.texture;
                materials[tileIndex].color = new Color(1, 1, 1, tile.texture != null ? 1 : 0);
            }
        }

        mesh.vertices = vertices;
    }

    internal class CData
    {
        private short[,] heights;

        public CData(byte[] bytes)
        {
            Texture2D texture = new Texture2D(256, 256, TextureFormat.RGB24, false);
            texture.LoadImage(bytes);

            const int res = 256;

            if (texture.width != res || texture.height != res) return;

            Color[] colors = texture.GetPixels();

            heights = new short[res, res];

            for (int y = 0; y < res; y++)
            {
                int py = (255 - y) * res;

                for (int x = 0; x < res; x++)
                {
                    Color c = colors[py + x];

                    double height = -10000 + (c.r * 255 * 256 * 256 + c.g * 255 * 256 + c.b * 255) * 0.1;
                    heights[x, y] = (short)Math.Round(height);
                }
            }
        }

        public short GetElevation(double tx, double ty)
        {
            if (heights == null) return 0;

            double rx = tx - Math.Floor(tx);
            double ry = ty - Math.Floor(ty);
            int x = (int)Math.Round(rx * 256);
            int y = (int)Math.Round(ry * 256);
            if (x > 255) x = 255;
            if (y > 255) y = 255;
            return heights[x, y];
        }
    }
}