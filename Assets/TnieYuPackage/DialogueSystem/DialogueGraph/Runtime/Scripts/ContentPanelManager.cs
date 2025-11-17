using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Scripts
{
    public class ContentPanelManager : MonoBehaviour, IPointerClickHandler
    {
        public TextMeshProUGUI contentText;
        public UnityEvent onPointerClick = new UnityEvent();

        void Awake()
        {
            if (contentText == null)
            {
                contentText = GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onPointerClick?.Invoke();
        }
    }
}