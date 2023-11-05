using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ScrollableCardPanel : DraggableCardPanelBase
    {
        [SerializeField] private bool isVertical;
        
        [SerializeField] private float scrollSpeed;
        
        [SerializeField] private ScrollRect scrollRect;
        
        [SerializeField] public RectTransform downRectTransform;
        [SerializeField] public RectTransform upRectTransform;

        [SerializeField] private Scrollbar verticalScrollbar;
        [SerializeField] private Scrollbar horizontalScrollbar;
        
        private bool _dragStarted;
        private DraggableCard _card;
        private Rect _cardRect;

        private readonly Vector3[] _rectCorners = new Vector3[4];

        private readonly Vector3[] _downRectCornerPos = new Vector3[4];
        private readonly Vector3[] _upRectCornerPos = new Vector3[4];
        
        private void Start()
        {
            if(scrollRect == null) return;

            scrollRect.content.GetWorldCorners(_rectCorners);
            
            downRectTransform.GetLocalCorners(_downRectCornerPos);
            upRectTransform.GetLocalCorners(_upRectCornerPos);
        }

        private void Update()
        {
            if (!_dragStarted) return;
            var mousePoint = Input.mousePosition;
            if (RectTransformUtility.RectangleContainsScreenPoint(downRectTransform, mousePoint)) ScrollDown();
            if (RectTransformUtility.RectangleContainsScreenPoint(upRectTransform, mousePoint)) ScrollUp();
        }

        public override void OnDragStart(DraggableCardEventArgs args)
        {
            base.OnDragStart(args);

            _dragStarted = true;
        }
        
        public override void OnDragEnd(DraggableCardEventArgs args)
        {
            base.OnDragEnd(args);

            _dragStarted = false;

            // Reset layout group
            LayoutGroup.CalculateLayoutInputVertical();
            LayoutGroup.CalculateLayoutInputHorizontal();
            LayoutGroup.SetLayoutVertical();
            LayoutGroup.SetLayoutHorizontal();
        }

        private void ScrollUp()
        {
            var positionChange = Time.deltaTime * scrollSpeed;

            if (isVertical)
            {
                if (verticalScrollbar == null) return;
                
                // Gets faster by going up.
                var localPointerPosition = upRectTransform.InverseTransformPoint(Input.mousePosition);
                positionChange *= Mathf.Abs(_upRectCornerPos[0].y - localPointerPosition.y);
                
                scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition + positionChange);
            }
            else
            {
                if (horizontalScrollbar == null) return;
             
                // Gets faster by going left.
                var localPointerPosition = upRectTransform.InverseTransformPoint(Input.mousePosition);
                positionChange *= Mathf.Abs(_upRectCornerPos[2].x - localPointerPosition.x);

                scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition - positionChange);
            }
        }

        private void ScrollDown()
        {
            var positionChange = Time.deltaTime * scrollSpeed;

            if (isVertical)
            {
                if (verticalScrollbar == null) return;
             
                // Gets faster by going bottom.
                var localPointerPosition = downRectTransform.InverseTransformPoint(Input.mousePosition);
                positionChange *= Mathf.Abs(_downRectCornerPos[1].y - localPointerPosition.y);

                scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition - positionChange);
            }
            else
            {
                if (horizontalScrollbar == null) return;
             
                // Gets faster by going right.
                var localPointerPosition = downRectTransform.InverseTransformPoint(Input.mousePosition);
                positionChange *= Mathf.Abs(_downRectCornerPos[1].x - localPointerPosition.x);

                scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition + positionChange);
            }
        }
    }
}