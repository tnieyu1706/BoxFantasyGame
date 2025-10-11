using EditorAttributes;
using NaughtyAttributes;
using Systems.Input;
using TnieYuPackage.GlobalExtensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.Player
{
    public class CameraController : MonoBehaviour
    {
        #region PROPERTIES
        
        [HideInInspector] [SerializeField, EditorAttributes.Required] private GameObject bodyPlayer;
        [HideInInspector] public float pitchSensitivity = 1f;
        [HideInInspector] public float smoothSpeed = 10f;
        [HideInInspector] [EditorAttributes.MinMaxSlider(-5, 5)] 
        public Vector2Int pitchMin;

        [FoldoutGroup(
            "Properties",
            nameof(bodyPlayer),
            nameof(pitchSensitivity),
            nameof(smoothSpeed),
            nameof(pitchMin)
        )]
        public Void propertiesGroup;

        [HideInInspector] [SerializeField, EditorAttributes.ReadOnly] 
        private float yaw;
        [HideInInspector] [SerializeField, EditorAttributes.ReadOnly] 
        private Quaternion targetRotation;

        [FoldoutGroup(
            "Read Values",
            nameof(yaw),
            nameof(targetRotation)
        )]
        public Void readValuesGroup;

        #endregion
        
        void OnEnable()
        {
            PlayerInputReader.Instance.Look += Look;
        }

        void Look(Vector2 lookDelta)
        {
            if (lookDelta.x == 0) return;

            if (lookDelta.x > pitchMin.x && lookDelta.x < pitchMin.y)
            {
                return;
            }

            yaw += lookDelta.x * pitchSensitivity * Time.deltaTime;

            targetRotation = Quaternion.Euler(0f, yaw, 0f);

            bodyPlayer.transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                smoothSpeed * Time.deltaTime
            );
        }

        void OnDisable()
        {
            PlayerInputReader.Instance.Look -= Look;
        }
    }
}