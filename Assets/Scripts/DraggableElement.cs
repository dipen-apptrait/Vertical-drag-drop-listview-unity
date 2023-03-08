using UnityEngine;
using UnityEngine.UI;

public class DraggableElement : MonoBehaviour
{
    private DraggableElementsPanel _draggableElementsPanel;
        
    private RectTransform _rectTransform;
    private Canvas _canvas;
        
    private int _totalSiblingCount;
    private int _initialSiblingIndex;
    private int _canvasSortingOrder;
        
    private bool _isAlreadyHaveCanvas;
    private bool _isLayoutVertical;

    private Vector3 _currentPosition;

    private void Awake()
    {
        _draggableElementsPanel = GetComponentInParent<DraggableElementsPanel>();
        _isLayoutVertical = _draggableElementsPanel.GetComponent<LayoutGroup>() is VerticalLayoutGroup;
    }

    public void StartDragging()
    {
        _rectTransform = GetComponent<RectTransform>();
        _isAlreadyHaveCanvas = TryGetComponent(out _canvas);
            
        _canvas = _isAlreadyHaveCanvas ? _canvas : gameObject.AddComponent<Canvas>();
        _canvasSortingOrder = _isAlreadyHaveCanvas ? _canvas.sortingOrder : 0;
            
        _currentPosition = _rectTransform.position;
            
        _totalSiblingCount = _draggableElementsPanel.GetChildCount;
        _initialSiblingIndex = _rectTransform.GetSiblingIndex();

        _canvas.overrideSorting = true;
        _canvas.sortingOrder = 2;
    }

    public void Drag(Vector3 eventPosition)
    {
        var rectTransformPosition = _rectTransform.position;

        _rectTransform.position = _isLayoutVertical 
            ? new Vector3(rectTransformPosition.x, eventPosition.y, rectTransformPosition.z)
            : new Vector3(eventPosition.x, rectTransformPosition.y, rectTransformPosition.z);
            
        for (var i = 0; i < _totalSiblingCount; i++)
        {
            var currentSiblingIndex = _rectTransform.GetSiblingIndex();
            if (i == currentSiblingIndex) continue;
            
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
            _canvas.sortingOrder = _canvasSortingOrder;
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