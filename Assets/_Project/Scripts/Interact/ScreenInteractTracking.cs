using TnieYuPackage.CustomAttributes;
using UnityEngine;

namespace Systems.Interact
{
    public class ScreenInteractTracking : MonoBehaviour
    {
        public float maxDistanceTracking = 2f;
        
        [TnieLayerMaskDropdown]
        public int layerMaskTracking;
        
        private Camera mainCamera;

        void Awake()
        {
            mainCamera = Camera.main;
        }

        private Ray ray;
        private RaycastHit hit;
        
        void DisplayRaycast()
        {
            ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            if (Physics.Raycast(ray,out hit, 2f, layerMaskTracking))
            {
                Debug.Log($"Hit {hit.collider.gameObject.name}");
            }
            Debug.DrawLine(mainCamera.transform.position, mainCamera.transform.position + mainCamera.transform.forward * maxDistanceTracking, Color.cyan);
        }
        
        void Update()
        {
            DisplayRaycast();
        }
    }
}