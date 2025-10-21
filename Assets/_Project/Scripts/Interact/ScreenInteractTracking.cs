using System;
using TnieYuPackage.CustomAttributes;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using UnityEngine;

namespace Systems.Interact
{
    public class ScreenInteractTracking : SingletonBehavior<ScreenInteractTracking>
    {
        public float maxDistanceTracking = 2f;
        [TnieLayerMaskDropdown] public int layerMaskTracking;

        public bool raycastDebug = false;

        private Camera mainCamera;
        private Collider lastTrackingCollider;
        
        private Ray ray;
        private RaycastHit hit;

        public Action<Collider> OnTrackingEnter;
        public Action<Collider> OnTrackingStay;
        public Action<Collider> OnTrackingExit;

        protected override void Awake()
        {
            base.Awake();
            mainCamera = Camera.main;
        }

        void OnEnable()
        {
            OnTrackingEnter += TrackingEnterLog;
            OnTrackingExit += TrackingExitLog;
        }

        void OnDisable()
        {
            OnTrackingEnter -= TrackingEnterLog;
            OnTrackingExit -= TrackingExitLog;
        }

        void TrackingEnterLog(Collider collider)
        {
            Debug.Log($"Enter - {collider.gameObject.name}");
        }

        void TrackingExitLog(Collider collider)
        {
            Debug.Log($"Exit - {collider.gameObject.name}");
        }

        void Tracking()
        {
            ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            if (Physics.Raycast(ray, out hit, 2f, layerMaskTracking))
            {
                if (lastTrackingCollider == null)
                {
                    HandleTrackingEnter(hit.collider);
                }
                else if (lastTrackingCollider != hit.collider)
                {
                    HandleTrackingExit(lastTrackingCollider);
                    HandleTrackingEnter(hit.collider);
                }

                OnTrackingStay?.Invoke(hit.collider);
            }
            else
            {
                if (lastTrackingCollider != null)
                {
                    HandleTrackingExit(lastTrackingCollider);
                }
            }

            if (raycastDebug)
                Debug.DrawLine(
                    mainCamera.transform.position,
                    mainCamera.transform.position + mainCamera.transform.forward * maxDistanceTracking,
                    Color.cyan
                );
        }

        private void HandleTrackingEnter(Collider collider)
        {
            OnTrackingEnter?.Invoke(hit.collider);
            lastTrackingCollider = hit.collider;
        }

        private void HandleTrackingExit(Collider collider)
        {
            OnTrackingExit?.Invoke(lastTrackingCollider);
            lastTrackingCollider = null;
        }

        void Update()
        {
            Tracking();
        }
    }
}