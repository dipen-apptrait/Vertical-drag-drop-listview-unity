using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class DraggableCard : MonoBehaviour
    {
        public DraggableCardPanelBase CardPanelBase { get; private set; }

        private RectTransform _rectTransform;
        private Canvas _canvas;
        
        private int _totalSiblingCount;
        private int _defaultSortingOrder;
        
        private bool _isAlreadyHaveCanvas;
        private bool _isLayoutVertical;
        private bool _isDragStarted;
        
        private Vector3 _currentPosition;

        private DraggableCardEventArgs _args;

        private void Awake()
        {
            CardPanelBase = GetComponentInParent<DraggableCardPanelBase>();
            _rectTransform = GetComponent<RectTransform>();
            _isAlreadyHaveCanvas = TryGetComponent(out _canvas);
        }

        private void Start()
        {
            _isLayoutVertical = CardPanelBase.LayoutGroup is VerticalLayoutGroup;
            
            _args = new DraggableCardEventArgs
            {
                StartIndex = _rectTransform.GetSiblingIndex(),
                EndIndex = -1,
                Card = this
            };
        }

        public void StartDragging()
        {
            if (!gameObject.TryGetComponent(out _canvas))
            {
                _canvas = gameObject.AddComponent<Canvas>();
            }
            
            _canvas.overrideSorting = true;
            _canvas.sortingOrder = 2;

            _defaultSortingOrder = _canvas.sortingOrder;
            _currentPosition = _rectTransform.position;
            
            CardPanelBase.OnDragStart(_args);
            _isDragStarted = true;
        }

        private void Update()
        {
            if (_isDragStarted) DragCard();
        }

        public void Drag(Vector3 eventDelta)
        {
            DragCard(eventDelta);
        }

        private void DragCard(Vector3 delta = default)
        {
            var rectTransformPosition = _rectTransform.position;
            
            Vector3 position;
            if (delta == default)
            {
                position = Input.mousePosition;
            }
            else
            {
                position = delta + rectTransformPosition;
            }

            _rectTransform.position = _isLayoutVertical 
                ? new Vector3(rectTransformPosition.x, position.y, rectTransformPosition.z)
                : new Vector3(position.x, rectTransformPosition.y, rectTransformPosition.z);
            
            var currentSiblingIndex = _rectTransform.GetSiblingIndex();
            for (var i = 0; i < CardPanelBase.GetChildCount; i++)
            {
                if (i == currentSiblingIndex) continue;
            
                // TODO: If other transform's pivot is not centered, dragging might not work properly.
                var otherTransform = CardPanelBase.GetChild(i);
            
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

            _args.EndIndex = currentSiblingIndex;
            CardPanelBase.OnDrag(_args);
        }

        public void OnDragEnd()
        {
            if (_isAlreadyHaveCanvas)
            {
                _canvas.sortingOrder = _defaultSortingOrder;
            }
            else
            {
                Destroy(_canvas);
            }
            
            if(_isDragStarted) _rectTransform.position = _currentPosition;
            _args.StartIndex = _rectTransform.GetSiblingIndex();
            
            CardPanelBase.OnDragEnd(_args);
            _isDragStarted = false;
        }
    }
}
