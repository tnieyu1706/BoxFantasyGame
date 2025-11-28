using TnieYuPackage.SOAP.Data;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Test
{
    public class TestSliderAutoLoad : MonoBehaviour
    {
        [SerializeField] private Image sliderImage;
        public FloatSoapDataSo floatData;
        
        private void OnEnable()
        {
            UpdateSlider(floatData.data.Value);
            floatData.data.OnValueChange += UpdateSlider;
        }
        
        private void OnDisable()
        {
            floatData.data.OnValueChange -= UpdateSlider;
        }

        void UpdateSlider(float value)
        {
            sliderImage.fillAmount = value;
        }
    }
}