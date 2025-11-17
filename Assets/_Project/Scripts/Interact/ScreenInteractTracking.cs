using System;
using TnieYuPackage.CustomAttributes;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using UnityEngine;

namespace Systems.Interact
{
    public class ScreenInteractTracking : SceneSingletonBehaviour<ScreenInteractTracking>
    {
        public float maxDistanceTracking = 2f;
        [TnieLayerMaskDropdown] public int layerMaskTracking;

        public bool raycastDebug = false;

        [SerializeField] private Camera playerCamera;
        private Collider lastTrackingCollider;

        private Ray ray;
        private RaycastHit hit;

        public Action<Collider> OnTrackingEnter;
        public Action<Collider> OnTrackingStay;
        public Action<Collider> OnTrackingExit;

        void Start()
        {
            if (playerCamera == null)
                playerCamera = Camera.main;
        }

        #region DebugTest

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

        #endregion

        void Tracking()
        {
            if (playerCamera == null) return;

            ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out hit, 2f, layerMaskTracking))
            {
                if (hit.collider != lastTrackingCollider)
                {
                    if (lastTrackingCollider != null)
                        HandleTrackingExit(hit.collider);

                    HandleTrackingEnter(hit.collider);
                }
                else
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
                    playerCamera.transform.position,
                    playerCamera.transform.position + playerCamera.transform.forward * maxDistanceTracking,
                    Color.cyan
                );
        }

        private void HandleTrackingEnter(Collider target)
        {
            OnTrackingEnter?.Invoke(target);
            lastTrackingCollider = target;
        }

        private void HandleTrackingExit(Collider target)
        {
            OnTrackingExit?.Invoke(target);
            lastTrackingCollider = null;
        }

        void Update()
        {
            Tracking();
        }
    }
}