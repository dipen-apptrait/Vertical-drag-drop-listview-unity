using UnityEngine;
using UnityEngine.UI;

public class DraggableElement : MonoBehaviour
{
    private DraggableElementsPanel _draggableElementsPanel;
        
    private RectTransform _rectTransform;
    private Canvas _canvas;
        
    private int _totalSiblingCount;
    private int _initialSiblingIndex;
    private int _defaultCanvasSortingOrder;
        
    private bool _isAlreadyHaveCanvas;
    private bool _isLayoutVertical;

    private Vector3 _currentPosition;

    private void Awake()
    {
        _draggableElementsPanel = GetComponentInParent<DraggableElementsPanel>();
        _isLayoutVertical = _draggableElementsPanel.GetComponent<LayoutGroup>() is VerticalLayoutGroup;
        
        _rectTransform = GetComponent<RectTransform>();
        _isAlreadyHaveCanvas = TryGetComponent(out _canvas);

        _totalSiblingCount = _draggableElementsPanel.GetChildCount;
        _initialSiblingIndex = _rectTransform.GetSiblingIndex();
    }

    public void StartDragging()
    {
        _canvas = _isAlreadyHaveCanvas ? _canvas : gameObject.AddComponent<Canvas>();
        _defaultCanvasSortingOrder = _isAlreadyHaveCanvas ? _canvas.sortingOrder : 0;
            
        _currentPosition = _rectTransform.position;

        _canvas.overrideSorting = true;
        _canvas.sortingOrder = 2;
    }

    public void Drag(Vector3 eventDelta)
    {
        var rectTransformPosition = _rectTransform.position;

        _rectTransform.position = _isLayoutVertical 
            ? new Vector3(rectTransformPosition.x, rectTransformPosition.y + eventDelta.y, rectTransformPosition.z)
            : new Vector3(rectTransformPosition.x + eventDelta.x, rectTransformPosition.y, rectTransformPosition.z);

        for (var i = 0; i < _totalSiblingCount; i++)
        {
            var currentSiblingIndex = _rectTransform.GetSiblingIndex();
            if (i == currentSiblingIndex) continue;
            
            // TODO: If other transform's pivot is not centered, dragging might not work properly.
            var otherTransform = _draggableElementsPanel.GetChild(i);
            
            var distance = (int)Vector3.Distance(_rectTransform.position, otherTransform.position);
            if (distance > 10) continue;

            var otherOldPosition = otherTransform.position;
            var rectPosition = _rectTransform.position;

            if (_isLayoutVertical)
            {
                otherTransform.position = new Vector3(otherOldPosition.x, _currentPosition.y, otherOldPosition.z);
                rectPosition = new Vector3(rectPosition.x, otherOldPosition.y, rectPosition.z);
            }
            else
            {
                otherTransform.position = new Vector3(_currentPosition.x, otherOldPosition.y, otherOldPosition.z);
                rectPosition = new Vector3(otherOldPosition.x, rectPosition.y, rectPosition.z);
            }
                
            _rectTransform.position = rectPosition;
            _currentPosition = rectPosition;

            _rectTransform.SetSiblingIndex(otherTransform.GetSiblingIndex());
        }            
    }

    public void OnDragEnd()
    {
        if (_isAlreadyHaveCanvas)
        {
            _canvas.sortingOrder = _defaultCanvasSortingOrder;
        }
        else
        {
            DestroyImmediate(_canvas);
        }
            
        _rectTransform.position = _currentPosition;
            
        var eventArgs = new OnDragEventArgs
        {
            StartIndex = _initialSiblingIndex,
            EndIndex = _rectTransform.GetSiblingIndex(),
            Transform = _rectTransform
        };
        _draggableElementsPanel.OnDrag(eventArgs);
    }
}