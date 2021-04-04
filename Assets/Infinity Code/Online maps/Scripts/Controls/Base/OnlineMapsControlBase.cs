/*     INFINITY CODE 2013-2019      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class implements the basic functionality control of the map.
/// </summary>
[Serializable]
[OnlineMapsWizardControlHelper(OnlineMapsTarget.texture)]
[RequireComponent(typeof(OnlineMapsMarkerManager))]
public abstract class OnlineMapsControlBase : MonoBehaviour, IOnlineMapsSavableComponent
{
    #region Variables

    #region Static Fields

    /// <summary>
    /// Delay before invoking event OnMapLongPress.
    /// </summary>
    public static float longPressDelay = 1;

    /// <summary>
    /// Distance (pixels) after which will start drag the map.
    /// </summary>
    public static float startDragDistance = 4;

    /// <summary>
    /// Singleton of control
    /// </summary>
    protected static OnlineMapsControlBase _instance;

    #endregion

    #region Actions

    public Action OnDrawMarkers;

    /// <summary>
    /// Event intercepts getting current cursor position.
    /// </summary>
    public Func<Vector2> OnGetInputPosition;
    public GameObject markerParent;
    public MarkerCollided collisionObj;
    public bool lockXAxis;
    public bool lockYAxis;
    private Waypoints waypoints;
    private ContentListDisplay display;

    /// <summary>
    /// Event intercepts getting number of touches.
    /// </summary>
    public Func<int> OnGetTouchCount;

    /// <summary>
    /// Event that occurs when you click on the map.
    /// </summary>
    public Action OnMapClick;

    /// <summary>
    /// Event that occurs when you double-click on the map.
    /// </summary>
    public Action OnMapDoubleClick;

    /// <summary>
    /// Event that occurs when you drag the map.
    /// </summary>
    public Action OnMapDrag;

    /// <summary>
    /// Event that occurs when you long press the map.
    /// </summary>
    public Action OnMapLongPress;

    /// <summary>
    /// Event that occurs when you press on the map.
    /// </summary>
    public Action OnMapPress;

    /// <summary>
    /// Event that occurs when you release the map.
    /// </summary>
    public Action OnMapRelease;

    /// <summary>
    /// Event that occurs when you zoom the map.
    /// </summary>
    public Action OnMapZoom;

    /// <summary>
    /// Event, which occurs when the smooth zoom is started.
    /// </summary>
    public Action OnSmoothZoomBegin;

    /// <summary>
    /// Event, which occurs when the smooth zoom is finish.
    /// </summary>
    public Action OnSmoothZoomFinish;

    /// <summary>
    /// Event, which occurs when the smooth zoom is starts init.
    /// </summary>
    public Action OnSmoothZoomInit;

    /// <summary>
    /// Event, which occurs when the smooth zoom is process.
    /// </summary>
    public Action OnSmoothZoomProcess;

    /// <summary>
    /// Event that occurs at end Update.
    /// </summary>
    public Action OnUpdateAfter;

    /// <summary>
    /// Event that occurs at start Update.
    /// </summary>
    public Action OnUpdateBefore;

    /// <summary>
    /// Event validating that current zoom event is allowed.\n
    /// True - zoom is allowed, false - forbidden.
    /// </summary>
    public Func<OnlineMapsZoomEvent, float, bool> OnValidateZoom;

    #endregion

    #region Public Fields

    /// <summary>
    /// Texture, which will draw the map. \n
    /// To change the texture use OnlineMapsControlBase.SetTexture.
    /// </summary>
    [HideInInspector]
    public Texture2D activeTexture;

    /// <summary>
    /// Specifies whether the user can change zoom of the map.
    /// </summary>
    public bool allowZoom = true;

    /// <summary>
    /// Specifies whether the user can manipulate the map.
    /// </summary>
    public bool allowUserControl = true;

    public OnlineMapsDrawingElementManager drawingElementManager;

    /// <summary>
    /// Specifies whether to move map.
    /// </summary>
    [HideInInspector]
    public bool isMapDrag;

    /// <summary>
    /// Inverts the map touch zoom for mobile devices.
    /// </summary>
    public bool invertTouchZoom = false;

    public OnlineMapsMarkerManager markerManager;

    /// <summary>
    /// Specifies whether to use a smooth touch zoom.
    /// </summary>
    public bool smoothZoom = true;

    /// <summary>
    /// Allows you to zoom the map when double-clicked.
    /// </summary>
    public bool zoomInOnDoubleClick = true;

    /// <summary>
    /// Mode of zoom.
    /// </summary>
    public OnlineMapsZoomMode zoomMode = OnlineMapsZoomMode.target;

    /// <summary>
    /// Sensitivity of the zoom
    /// </summary>
    public float zoomSensitivity = 1;

    #endregion

    #region Protected Fields

    protected Rect _screenRect;
    protected float lastGestureDistance;
    protected Vector2 lastGestureCenter = Vector2.zero;
    protected Vector2 lastInputPosition;
    protected int lastTouchCount;
    protected bool lockClick;
    protected bool waitZeroTouches = false;

    #endregion

    #region Private Fields

#if UNITY_EDITOR
    private Vector2 initialGestureCenter;
#endif

    private OnlineMapsMarkerBase _dragMarker;
    private OnlineMaps _map;
    private OnlineMapsMarker2DDrawer _markerDrawer;
    private IOnlineMapsInteractiveElement activeElement;
    private bool isMapPress;
    private float[] lastClickTimes = { 0, 0 };
    private double lastPositionLat;
    private double lastPositionLng;
    private double lastPositionTX;
    private double lastPositionTY;
    private IEnumerator longPressEnumerator;
    private bool mapDragStarted;
    private Vector2 pressPoint;
    private OnlineMapsSavableItem[] savableItems;
    private Vector2[] touchPositions;
    private bool smoothZoomStarted;
    private int lastGestureTouchCount = 0;

    #endregion

    #endregion

    #region Properties

    /// <summary>
    /// Singleton instance of map control.
    /// </summary>
    public static OnlineMapsControlBase instance
    {
        get { return _instance; }
    }

    /// <summary>
    /// Indicates whether it is possible to get the screen coordinates store. True - for 2D map, false - for the 3D map.
    /// </summary>
    public virtual bool allowMarkerScreenRect
    {
        get { return false; }
    }

    protected virtual bool allowTouchZoom
    {
        get { return true; }
    }

    /// <summary>
    /// Marker that dragged at the moment.
    /// </summary>
    public OnlineMapsMarkerBase dragMarker
    {
        get { return _dragMarker; }
        set
        {
            _dragMarker = value;
            if (_dragMarker != null)
            {
                //UpdateLastPosition();
            }
        }
    }

    public OnlineMapsMarker2DDrawer markerDrawer
    {
        get { return _markerDrawer; }
        set
        {
            if (_markerDrawer != null) _markerDrawer.Dispose();
            _markerDrawer = value;
        }
    }

    /// <summary>
    /// Reference to map instance.
    /// </summary>
    public OnlineMaps map
    {
        get
        {
            if (_map == null) _map = GetComponent<OnlineMaps>();
            return _map;
        }
    }

    /// <summary>
    /// Mipmap for tiles.
    /// </summary>
    public virtual bool mipmapForTiles
    {
        get { return false; }
        set { throw new Exception("This control does not support mipmap for tiles."); }
    }

    /// <summary>
    /// Screen area occupied by the map.
    /// </summary>
    public virtual Rect screenRect
    {
        get { return _screenRect; }
    }

    public bool resultIsTexture
    {
        get { return resultType == OnlineMapsTarget.texture; }
    }

    public virtual OnlineMapsTarget resultType
    {
        get { return OnlineMapsTarget.texture; }
    }

    public virtual bool useRasterTiles
    {
        get { return true; }
    }

    /// <summary>
    /// UV rectangle used by the texture of the map.
    /// NGUI: uiTexture.uvRect.
    /// Other: new Rect(0, 0, 1, 1);
    /// </summary>
    public virtual Rect uvRect
    {
        get { return new Rect(0, 0, 1, 1); }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Function, which is executed after map updating.
    /// </summary>
    protected virtual void AfterUpdate()
    {

    }

    /// <summary>
    /// Function, which is executed before map updating.
    /// </summary>
    protected virtual void BeforeUpdate()
    {

    }

    public virtual OnlineMapsTile CreateTile(int x, int y, int zoom, bool isMapTile = true)
    {
        return new OnlineMapsRasterTile(x, y, zoom, map, isMapTile);
    }

    /// <summary>
    /// Moves the marker to the location of the cursor.
    /// </summary>
    protected void DragMarker()
    {
        double lat, lng;
        display = (ContentListDisplay)FindObjectOfType<ContentListDisplay>();
        waypoints = (Waypoints)FindObjectOfType<Waypoints>();
        collisionObj = (MarkerCollided)FindObjectOfType<MarkerCollided>();

        bool hit = GetCoords(out lng, out lat);

        if (!hit) return;

        double offsetX = lng - lastPositionLng;
        double offsetY = lat - lastPositionLat;

        if (Math.Abs(offsetX) < double.Epsilon && Math.Abs(offsetY) < double.Epsilon) return;

        double px, py;
        dragMarker.GetPosition(out px, out py);
        lastPositionLng = lng;
        lastPositionLat = lat;

        if (lockXAxis)
        {
            px = px + offsetX;
        }
        else if (lockYAxis)
        {
            py = py + offsetY;
        }
        else
        {
            px = px + offsetX;
            py = py + offsetY;
        }

        dragMarker.SetPosition(px, py);

        if (collisionObj.collision)
        {
            dragMarker.SetPosition(px - offsetX * 2.5, py - offsetY * 2.5);
        }

        if (dragMarker.OnDrag != null) dragMarker.OnDrag(dragMarker);
        if (dragMarker is OnlineMapsMarker) map.Redraw();
        display.Prime(waypoints.points);
    }

    /// <summary>
    /// Returns the geographical coordinates of the location where the cursor is.
    /// </summary>
    /// <returns>Geographical coordinates</returns>
    public Vector2 GetCoords()
    {
        return GetCoords(GetInputPosition());
    }

    /// <summary>
    /// Returns the geographical coordinates at the specified coordinates of the screen.
    /// </summary>
    /// <param name="position">Screen coordinates</param>
    /// <returns>Geographical coordinates</returns>
    public Vector2 GetCoords(Vector2 position)
    {
        double lng, lat;
        GetCoords(position, out lng, out lat);
        return new Vector2((float)lng, (float)lat);
    }

    /// <summary>
    /// Returns the geographical coordinates of the location where the cursor is.
    /// </summary>
    /// <param name="lng">Longitude</param>
    /// <param name="lat">Latitude</param>
    /// <returns>True - success, False - otherwise.</returns>
    public bool GetCoords(out double lng, out double lat)
    {
        return GetCoords(GetInputPosition(), out lng, out lat);
    }

    public abstract bool GetCoords(Vector2 position, out double lng, out double lat);

    protected virtual bool GetCoordsInternal(out double lng, out double lat)
    {
        return GetCoords(GetInputPosition(), out lng, out lat);
    }

    /// <summary>
    /// Returns the current cursor position.
    /// </summary>
    /// <returns>Current cursor position</returns>
    public Vector2 GetInputPosition()
    {
        if (OnGetInputPosition != null) return OnGetInputPosition();

        return Input.mousePosition;
    }

    public virtual IOnlineMapsInteractiveElement GetInteractiveElement(Vector2 screenPosition)
    {
        if (IsCursorOnUIElement(screenPosition)) return null;

        OnlineMapsMarker marker = markerDrawer.GetMarkerFromScreen(screenPosition);
        if (marker != null) return marker;

        OnlineMapsDrawingElement drawingElement = map.GetDrawingElement(screenPosition);
        return drawingElement;
    }

    /// <summary>
    /// Converts geographical coordinate to position in the scene relative to the top-left corner of the map in map space.
    /// </summary>
    /// <param name="coords">Geographical coordinate (X - Longitude, Y - Latitude)</param>
    /// <returns>Scene position (in map space)</returns>
    public Vector2 GetPosition(Vector2 coords)
    {
        double px, py;
        GetPosition(coords.x, coords.y, out px, out py);
        return new Vector2((float)px, (float)py);
    }

    /// <summary>
    /// Converts geographical coordinate to position in the scene relative to the top-left corner of the map in map space.
    /// </summary>
    /// <param name="lng">Longitude</param>
    /// <param name="lat">Latitude</param>
    /// <param name="px">Relative position X</param>
    /// <param name="py">Relative position Y</param>
    public virtual void GetPosition(double lng, double lat, out double px, out double py)
    {
        double dx, dy, dtx, dty;
        OnlineMapsBuffer.StateProps lastState = map.buffer.lastState;
        map.projection.CoordinatesToTile(lng, lat, lastState.zoom, out dx, out dy);
        map.projection.CoordinatesToTile(lastState.leftLongitude, lastState.topLatitude, lastState.zoom, out dtx, out dty);
        dx -= dtx;
        dy -= dty;
        int maxX = 1 << (lastState.zoom - 1);
        if (dx < -maxX) dx += maxX << 1;
        if (dx < 0 && map.width == (1 << lastState.zoom) * OnlineMapsUtils.tileSize) dx += map.width / OnlineMapsUtils.tileSize;
        px = dx * OnlineMapsUtils.tileSize / lastState.zoomCoof;
        py = dy * OnlineMapsUtils.tileSize / lastState.zoomCoof;
    }

    /// <summary>
    /// Screen area occupied by the map.
    /// </summary>
    /// <returns>Screen rectangle</returns>
    public virtual Rect GetRect()
    {
        return new Rect();
    }

    public OnlineMapsSavableItem[] GetSavableItems()
    {
        if (savableItems != null) return savableItems;

        savableItems = new[]
        {
            new OnlineMapsSavableItem("control", "Control", SaveSettings)
            {
                loadCallback = LoadSettings
            }
        };

        return savableItems;
    }

    /// <summary>
    /// Converts geographical coordinate to position in screen space.
    /// </summary>
    /// <param name="coords">Geographical coordinate (X - longitude, Y - latitude)</param>
    /// <returns>Screen space position</returns>
    public Vector2 GetScreenPosition(Vector2 coords)
    {
        return GetScreenPosition(coords.x, coords.y);
    }

    /// <summary>
    /// Converts geographical coordinate to position in screen space.
    /// </summary>
    /// <param name="lng">Longitude</param>
    /// <param name="lat">Latitude</param>
    /// <returns>Screen space position</returns>
    public virtual Vector2 GetScreenPosition(double lng, double lat)
    {
        double mx, my;
        GetPosition(lng, lat, out mx, out my);
        OnlineMapsBuffer.StateProps lastState = map.buffer.lastState;
        mx /= lastState.width;
        my /= lastState.height;
        Rect mapRect = GetRect();
        mx = mapRect.x + mapRect.width * mx;
        my = mapRect.y + mapRect.height - mapRect.height * my;
        return new Vector2((float)mx, (float)my);
    }

    public abstract bool GetTile(Vector2 position, out double tx, out double ty);

    protected virtual bool GetTileInternal(Vector2 position, out double tx, out double ty)
    {
        return GetTile(position, out tx, out ty);
    }

    /// <summary>
    /// Returns the current number of touches.
    /// </summary>
    /// <returns>Number of touches</returns>
    public int GetTouchCount()
    {
        if (OnGetTouchCount != null) return OnGetTouchCount();

#if UNITY_WEBGL && !UNITY_EDITOR
        return Input.GetMouseButton(0) ? 1 : 0;
#else
        if (Input.touchSupported)
        {
            if (Input.touchCount > 0) return Input.touchCount;
        }
        return Input.GetMouseButton(0) ? 1 : 0;
#endif
    }

    /// <summary>
    /// Checks whether the cursor over the map.
    /// </summary>
    /// <returns>True - if the cursor over the map, false - if not.</returns>
    protected bool HitTest()
    {
        return HitTest(GetInputPosition());
    }

    protected virtual bool HitTest(Vector2 position)
    {
        return true;
    }

    /// <summary>
    /// Invokes OnMapBasePress.
    /// </summary>
    public void InvokeBasePress()
    {
        OnMapBasePress();
    }

    /// <summary>
    /// Invokes OnMapBaseRelease.
    /// </summary>
    public void InvokeBaseRelease()
    {
        OnMapBaseRelease();
    }

    public bool IsCursorOnUIElement(Vector2 position)
    {
        if (!map.notInteractUnderGUI) return false;
#if !IGUI && ((!UNITY_ANDROID && !UNITY_IOS) || UNITY_EDITOR)
        if (GUIUtility.hotControl != 0) return true;
#endif
        if (EventSystem.current == null) return false;

        PointerEventData pe = new PointerEventData(EventSystem.current);
        pe.position = position;

        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pe, hits);
        if (hits.Count == 0) return false;

        GameObject go = hits[0].gameObject;
        if (go == gameObject) return false;

        return go.GetComponent<OnlineMapsMarkerInstanceBase>() == null;
    }

    protected virtual void LoadSettings(OnlineMapsJSONObject json)
    {
        json.DeserializeObject(this);
    }

    /// <summary>
    /// Event that occurs before Awake.
    /// </summary>
    public virtual void OnAwakeBefore()
    {
        _instance = this;
    }

    private void OnDestroy()
    {
        OnMapClick = null;
        OnMapDoubleClick = null;
        OnMapDrag = null;
        OnMapLongPress = null;
        OnMapPress = null;
        OnMapRelease = null;
        OnMapZoom = null;
        lastClickTimes = null;
        _map = null;
        _dragMarker = null;
        activeTexture = null;
        longPressEnumerator = null;
        _instance = null;
        markerDrawer = null;
        drawingElementManager = null;

        OnDestroyLate();
    }

    /// <summary>
    /// Event is called after the control has been disposed.
    /// </summary>
    protected virtual void OnDestroyLate()
    {

    }

    private void OnEnable()
    {
        _instance = this;
        dragMarker = null;

        if (map == null)
        {
            Debug.LogError("Can not find a script OnlineMaps.");
            OnlineMapsUtils.Destroy(this);
            return;
        }

        drawingElementManager = GetComponent<OnlineMapsDrawingElementManager>();
        if (drawingElementManager == null) drawingElementManager = gameObject.AddComponent<OnlineMapsDrawingElementManager>();

        markerManager = GetComponent<OnlineMapsMarkerManager>();

        activeTexture = map.texture;
        OnlineMapsMarkerManager.Init();
        OnlineMapsDrawingElementManager.Init();
        if (resultIsTexture) markerDrawer = new OnlineMapsMarkerBufferDrawer(this);

        OnEnableLate();
    }

    /// <summary>
    /// Function that is called after control of the map enabled.
    /// </summary>
    protected virtual void OnEnableLate()
    {

    }

    /// <summary>
    /// Called when a gesture zoom.
    /// </summary>
    /// <param name="p1">Screen coordinates of touch point 1</param>
    /// <param name="p2">Screen coordinates of touch point 2</param>
    protected virtual void OnGestureZoom(Vector2 p1, Vector2 p2)
    {

    }

    /// <summary>
    /// Method that is called when you press the map.
    /// </summary>
    protected virtual void OnMapBasePress()
    {
        isMapPress = false;

        if (waitZeroTouches)
        {
            if (GetTouchCount() <= 1) waitZeroTouches = false;
            else return;
        }

        dragMarker = null;
        Vector2 inputPosition = GetInputPosition();
        if (!HitTest(inputPosition)) return;

        if (IsCursorOnUIElement(inputPosition)) return;

        if (OnMapPress != null) OnMapPress();

        lastClickTimes[0] = lastClickTimes[1];
        lastClickTimes[1] = Time.realtimeSinceStartup;

        double tx, ty;
        lastInputPosition = pressPoint = inputPosition;
        if (!GetTile(inputPosition, out tx, out ty)) return;

        isMapPress = true;

        OnlineMapsMarkerBase marker = null;
        OnlineMapsDrawingElement drawingElement = null;

        IOnlineMapsInteractiveElement interactiveElement = GetInteractiveElement(inputPosition);

        if (interactiveElement != null)
        {
            if (interactiveElement is OnlineMapsMarkerBase) marker = interactiveElement as OnlineMapsMarkerBase;
            else if (interactiveElement is OnlineMapsDrawingElement) drawingElement = interactiveElement as OnlineMapsDrawingElement;
        }

        if (marker != null)
        {
            if (marker.OnPress != null) marker.OnPress(marker);
            if (map.showMarkerTooltip == OnlineMapsShowMarkerTooltip.onPress)
            {
                OnlineMapsTooltipDrawerBase.tooltipMarker = marker;
                OnlineMapsTooltipDrawerBase.tooltip = marker.label;
            }
            if (Input.GetKey(KeyCode.LeftControl)) dragMarker = marker;
        }
        else if (drawingElement != null)
        {
            if (drawingElement.OnPress != null) drawingElement.OnPress(drawingElement);
            if (map.showMarkerTooltip == OnlineMapsShowMarkerTooltip.onPress)
            {
                OnlineMapsTooltipDrawerBase.tooltipDrawingElement = drawingElement;
                OnlineMapsTooltipDrawerBase.tooltip = drawingElement.tooltip;
            }
        }

        if (dragMarker == null) isMapDrag = true;

        activeElement = interactiveElement;

        longPressEnumerator = WaitLongPress();
        StartCoroutine(longPressEnumerator);

        if (allowUserControl) OnlineMaps.isUserControl = true;
    }

    /// <summary>
    /// Method that is called when you release the map.
    /// </summary>
    protected virtual void OnMapBaseRelease()
    {
        if (waitZeroTouches && GetTouchCount() == 0) waitZeroTouches = false;
        if (GUIUtility.hotControl != 0) return;

        Vector2 inputPosition = GetInputPosition();
        bool isClick = (pressPoint - inputPosition).sqrMagnitude < 400 && !lockClick;
        lockClick = false;
        isMapDrag = false;
        mapDragStarted = false;
        dragMarker = null;

        if (longPressEnumerator != null)
        {
            StopCoroutine(longPressEnumerator);
            longPressEnumerator = null;
        }

        lastInputPosition = Vector2.zero;
        OnlineMaps.isUserControl = false;

        if (!isMapPress) return;
        isMapPress = false;
        if (OnMapRelease != null) OnMapRelease();

        OnlineMapsMarkerBase marker = null;
        OnlineMapsDrawingElement drawingElement = null;

        IOnlineMapsInteractiveElement interactiveElement = GetInteractiveElement(inputPosition);

        if (interactiveElement != null)
        {
            if (interactiveElement is OnlineMapsMarkerBase) marker = interactiveElement as OnlineMapsMarkerBase;
            else if (interactiveElement is OnlineMapsDrawingElement) drawingElement = interactiveElement as OnlineMapsDrawingElement;
        }

        if (map.showMarkerTooltip == OnlineMapsShowMarkerTooltip.onPress && (OnlineMapsTooltipDrawerBase.tooltipMarker != null || OnlineMapsTooltipDrawerBase.tooltipDrawingElement != null))
        {
            OnlineMapsTooltipDrawerBase.tooltipMarker = null;
            OnlineMapsTooltipDrawerBase.tooltipDrawingElement = null;
            OnlineMapsTooltipDrawerBase.tooltip = null;
        }

        bool isClicked = false;

        if (marker != null)
        {
            if (marker.OnRelease != null) marker.OnRelease(marker);
            if (isClick && marker.OnClick != null)
            {
                marker.OnClick(marker);
                isClicked = true;
            }
        }
        else if (drawingElement != null)
        {
            if (drawingElement.OnRelease != null) drawingElement.OnRelease(drawingElement);
        }

        if (activeElement != null && activeElement != interactiveElement)
        {
            if (activeElement is OnlineMapsMarkerBase)
            {
                OnlineMapsMarkerBase m = activeElement as OnlineMapsMarkerBase;
                if (m.OnRelease != null) m.OnRelease(m);
            }
            else if (activeElement is OnlineMapsDrawingElement)
            {
                OnlineMapsDrawingElement d = activeElement as OnlineMapsDrawingElement;
                if (d.OnRelease != null) d.OnRelease(d);
            }
            activeElement = null;
        }

        if (isClick && Time.realtimeSinceStartup - lastClickTimes[0] < 0.5f)
        {
            if (marker != null && marker.OnDoubleClick != null) marker.OnDoubleClick(marker);
            else if (drawingElement != null && drawingElement.OnDoubleClick != null) drawingElement.OnDoubleClick(drawingElement);
            else
            {
                if (OnMapDoubleClick != null) OnMapDoubleClick();

                if (allowZoom && zoomInOnDoubleClick)
                {
                    if (!((marker != null && marker.OnClick != null) || (drawingElement != null && drawingElement.OnClick != null)))
                    {
                        if (OnValidateZoom == null || OnValidateZoom(OnlineMapsZoomEvent.doubleClick, map.floatZoom + 1))
                        {
                            if (zoomMode == OnlineMapsZoomMode.target) ZoomOnPoint(1, inputPosition);
                            else map.floatZoom += 1;
                        }
                    }
                }
            }

            lastClickTimes[0] = 0;
            lastClickTimes[1] = 0;
        }
        else if (isClick && !isClicked)
        {
            if (drawingElement != null && drawingElement.OnClick != null) drawingElement.OnClick(drawingElement);
            else if (OnMapClick != null) OnMapClick();
        }

        if (map.bufferStatus == OnlineMapsBufferStatus.wait) map.needRedraw = true;
    }

    protected virtual OnlineMapsJSONItem SaveSettings()
    {
        return OnlineMapsJSON.Serialize(new
        {
            allowZoom,
            allowUserControl,
            zoomInOnDoubleClick,
            smoothZoom
        });
    }

    /// <summary>
    /// Specifies the texture, which will draw the map. In texture must be enabled "Read / Write Enabled".
    /// </summary>
    /// <param name="texture">Texture</param>
    public virtual void SetTexture(Texture2D texture)
    {
        activeTexture = texture;
    }

    private void StartGestureZoom()
    {
        if (OnSmoothZoomInit != null) OnSmoothZoomInit();
        smoothZoomStarted = true;
        lastGestureDistance = 0;
        waitZeroTouches = true;
        lockClick = true;
        if (OnSmoothZoomBegin != null) OnSmoothZoomBegin();
    }

    private void StopGestureZoom()
    {
        smoothZoomStarted = false;
        lastGestureCenter = Vector2.zero;

#if UNITY_EDITOR
        initialGestureCenter = Vector2.zero;
#endif 
        lastGestureDistance = 0;
        if (OnSmoothZoomFinish != null) OnSmoothZoomFinish();
    }

    protected void Update()
    {
        if (OnUpdateBefore != null) OnUpdateBefore();

        BeforeUpdate();
        _screenRect = GetRect();

        int touchCount = GetTouchCount();

        if (touchCount != lastTouchCount)
        {
            if (allowTouchZoom)
            {
                if (touchCount == 1) OnMapBasePress();
                else if (touchCount == 0) OnMapBaseRelease();
            }

            if (lastTouchCount == 0) UpdateLastPosition();
        }

        if (isMapDrag && !smoothZoomStarted) UpdatePosition();

        if (allowZoom)
        {
            UpdateZoom();
            UpdateGestureZoom(touchCount);
        }

        lastTouchCount = touchCount;

        if (dragMarker != null) DragMarker();
        else if (HitTest())
        {
            map.tooltipDrawer.ShowMarkersTooltip(GetInputPosition());
        }
        else
        {
            OnlineMapsTooltipDrawerBase.tooltip = string.Empty;
            OnlineMapsTooltipDrawerBase.tooltipMarker = null;
        }
        AfterUpdate();

        if (OnUpdateAfter != null) OnUpdateAfter();

        UpdateLastPosition();
    }

    private void UpdateGestureZoom(int touchCount)
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(0))
        {
            touchCount = 2;

            if (lastGestureCenter == Vector2.zero)
            {
                initialGestureCenter = lastGestureCenter = Input.mousePosition;
            }

            touchPositions = new Vector2[2];
            touchPositions[0] = Input.mousePosition;
            touchPositions[1] = initialGestureCenter * 2 - (Vector2)Input.mousePosition;
        }
#else 
        touchPositions = Input.touches.Select(t => t.position).ToArray();
#endif

        if (touchCount != lastGestureTouchCount)
        {
            lastGestureTouchCount = touchCount;
            if (touchCount == 2) StartGestureZoom();
            else if (smoothZoomStarted) StopGestureZoom();
        }

        if (smoothZoomStarted) UpdateGestureZoomValue();
    }

    private void UpdateGestureZoomValue()
    {
#if UNITY_EDITOR
        float delta = (touchPositions[0].x - lastGestureCenter.x) / 100;

        if (Mathf.Abs(delta) < 0.01f) return;

        lastGestureCenter = touchPositions[0];
#else
        float distance = (touchPositions[0] - touchPositions[1]).magnitude;

        if (Math.Abs(lastGestureDistance) < float.Epsilon)
        {
            lastGestureDistance = distance;
            return;
        }
        float delta = distance / lastGestureDistance - 1;

        if (Mathf.Abs(delta) < 0.01f) return;
        lastGestureDistance = distance;

#endif

        Vector2 screenPosition = Vector2.zero;
        foreach (Vector2 touchPosition in touchPositions) screenPosition += touchPosition;
        screenPosition /= touchPositions.Length;

        if (OnValidateZoom == null || OnValidateZoom(OnlineMapsZoomEvent.gesture, map.floatZoom + delta))
        {
            if (zoomMode == OnlineMapsZoomMode.target) ZoomOnPoint(delta * zoomSensitivity, screenPosition);
            else map.floatZoom += delta * zoomSensitivity;
        }


        if (OnSmoothZoomProcess != null) OnSmoothZoomProcess();
    }


    /// <summary>
    /// Force updates the latest coordinates of cursor.
    /// </summary>
    public void UpdateLastPosition()
    {
        double tx, ty;
        lastInputPosition = GetInputPosition();
        if (GetTile(lastInputPosition, out tx, out ty))
        {
            lastPositionTX = tx;
            lastPositionTY = ty;
            map.projection.TileToCoordinates(lastPositionTX, lastPositionTY, map.zoom, out lastPositionLng, out lastPositionLat);
        }
    }

    /// <summary>
    /// Updates the map coordinates for the actions of the user.
    /// </summary>
    protected void UpdatePosition()
    {
        if (!allowUserControl || GetTouchCount() > 1) return;

        double lat, lng;
        bool hit = GetCoordsInternal(out lng, out lat);

        Vector2 inputPosition = GetInputPosition();

        if (!mapDragStarted && (lastInputPosition - inputPosition).magnitude < startDragDistance) return;
        mapDragStarted = true;

        if (!hit || lastInputPosition == inputPosition) return;

        double offsetX = lng - lastPositionLng;
        double offsetY = lat - lastPositionLat;

        if (offsetX > 270) offsetX -= 360;
        else if (offsetX < -270) offsetX += 360;

        if (Math.Abs(offsetX) > double.Epsilon || Math.Abs(offsetY) > double.Epsilon)
        {
            double px, py;
            map.GetPosition(out px, out py);
            px -= offsetX;
            py -= offsetY;
            map.SetPosition(px, py);

            map.needRedraw = true;

            if (longPressEnumerator != null)
            {
                StopCoroutine(longPressEnumerator);
                longPressEnumerator = null;
            }

            if (OnMapDrag != null) OnMapDrag();
        }

        GetCoordsInternal(out lastPositionLng, out lastPositionLat);
    }

    /// <summary>
    /// Updates the map zoom for mouse wheel.
    /// </summary>
    protected void UpdateZoom()
    {
#if UNITY_IOS && !UNITY_EDITOR
        return;
#endif

        if (!allowUserControl) return;
        if (!HitTest()) return;

        Vector2 inputPosition = GetInputPosition();

        if (IsCursorOnUIElement(inputPosition)) return;

        if (inputPosition.x <= 0 || inputPosition.x >= Screen.width || inputPosition.y <= 0 || inputPosition.y >= Screen.height) return;

        float wheel = Input.GetAxis("Mouse ScrollWheel");
        if (Math.Abs(wheel) < float.Epsilon) return;

#if NETFX_CORE
        wheel = -wheel;
#endif

        int delta = wheel > 0 ? 1 : -1;
        if (OnValidateZoom == null || OnValidateZoom(OnlineMapsZoomEvent.wheel, map.floatZoom + delta))
        {
            if (zoomMode == OnlineMapsZoomMode.target) ZoomOnPoint(delta, inputPosition);
            else map.floatZoom += delta;
        }
    }

    private IEnumerator WaitLongPress()
    {
        yield return new WaitForSeconds(longPressDelay);

        OnlineMapsMarkerBase marker = null;
        OnlineMapsDrawingElement drawingElement = null;
        Vector2 inputPosition = GetInputPosition();

        IOnlineMapsInteractiveElement interactiveElement = GetInteractiveElement(inputPosition);

        if (interactiveElement != null)
        {
            if (interactiveElement is OnlineMapsMarkerBase) marker = interactiveElement as OnlineMapsMarkerBase;
            else if (interactiveElement is OnlineMapsDrawingElement) drawingElement = interactiveElement as OnlineMapsDrawingElement;
        }

        if (marker != null && marker.OnLongPress != null) marker.OnLongPress(marker);
        else if (drawingElement != null && drawingElement.OnLongPress != null) drawingElement.OnLongPress(drawingElement);
        else if (OnMapLongPress != null)
        {
            OnMapLongPress();
            isMapDrag = false;
        }

        longPressEnumerator = null;
    }

    /// <summary>
    /// Changes the zoom keeping a specified point on same place.
    /// </summary>
    /// <param name="zoomOffset">Positive - zoom in, Negative - zoom out</param>
    /// <param name="screenPosition">Screen position</param>
    /// <returns>True - if zoom changed, False - if zoom not changed</returns>
    public bool ZoomOnPoint(float zoomOffset, Vector2 screenPosition)
    {
        float newZoom = Mathf.Clamp(map.floatZoom + zoomOffset, OnlineMaps.MINZOOM, OnlineMaps.MAXZOOM);
        if (Math.Abs(newZoom - map.floatZoom) < float.Epsilon) return false;

        double mx, my;
        bool hit = GetTile(screenPosition, out mx, out my);
        if (!hit) return false;

        map.dispatchEvents = false;

        int zoom = map.zoom;

        double tx, ty, tmx2, tmy2;
        map.GetTilePosition(out tx, out ty);

        double ox = tx - mx;
        double oy = ty - my;

        map.floatZoom = newZoom;

        GetCoords(screenPosition, out mx, out my);
        map.projection.CoordinatesToTile(mx, my, zoom, out tmx2, out tmy2);

        double ox2 = tx - tmx2;
        double oy2 = ty - tmy2;

        tx -= ox - ox2;
        ty -= oy - oy2;

        map.projection.TileToCoordinates(tx, ty, zoom, out tx, out ty);
        map.SetPosition(tx, ty);

        map.dispatchEvents = true;
        map.DispatchEvent(OnlineMapsEvents.changedPosition, OnlineMapsEvents.changedZoom);

        if (OnMapZoom != null) OnMapZoom();
        return true;
    }

    #endregion
}