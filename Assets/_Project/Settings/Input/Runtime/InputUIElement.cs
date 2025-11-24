using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Input
{
    public class InputUIElement : MonoBehaviour
    {
        public TextMeshProUGUI inputText;
        public Image inputImage;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}