using TMPro;
using UnityEngine;

namespace TnieYuPackage.DialogueSystem.DialogueGraph.Runtime.Scripts
{
    public class CharacterPanelManager : MonoBehaviour
    {
        public TextMeshProUGUI characterText;

        private void Awake()
        {
            if (characterText == null)
            {
                characterText = GetComponentInChildren<TextMeshProUGUI>();
            }
        }
    }
}