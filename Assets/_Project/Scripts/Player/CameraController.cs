using EditorAttributes;
using Systems.Input;
using UnityEngine;

namespace Systems.Player
{
    public class CameraController : MonoBehaviour
    {
        #region PROPERTIES

        [HideInInspector] [SerializeField, EditorAttributes.Required]
        private GameObject bodyPlayer;

        [HideInInspector] [SerializeField, EditorAttributes.Required]
        private GameObject headPlayer;

        [HideInInspector] public float yawSensitivity = 1f;
        [HideInInspector] public float smoothSpeed = 60f;
        [HideInInspector] public float pitchSensitivity = 1f;

        [HideInInspector] [MinMaxSlider(-120, 120)]
        public Vector2 pitchRange = new Vector2(-70, 70);

        [FoldoutGroup(
            "Properties",
            nameof(bodyPlayer),
            nameof(headPlayer),
            nameof(yawSensitivity),
            nameof(pitchSensitivity),
            nameof(smoothSpeed),
            nameof(pitchRange)
        )]
        public Void propertiesGroup;

        [SerializeField, ReadOnly]
        private float yaw;
        private Quaternion yawTargetRotation;

        [SerializeField, ReadOnly]
        private float pitch;
        private Quaternion pitchTargetRotation;

        #endregion

        void OnEnable()
        {
            PlayerInputReader.Instance.Look += Look;
        }

        void Look(Vector2 lookDelta)
        {
            LookYawDirection(lookDelta.x);
            LookPitchDirection(lookDelta.y);
        }

        private void LookYawDirection(float x)
        {
            if (x == 0) return;

            yaw += x * yawSensitivity * Time.deltaTime;

            yawTargetRotation = Quaternion.Euler(0f, yaw, 0f);
            // bodyPlayer.transform.rotation = Quaternion.Slerp(
            //     bodyPlayer.transform.rotation,
            //     yawTargetRotation,
            //     smoothSpeed * Time.deltaTime
            // );
            
            bodyPlayer.transform.localRotation = yawTargetRotation;
        }

        private void LookPitchDirection(float y)
        {
            if (y == 0) return;

            pitch -= y * pitchSensitivity * Time.deltaTime;
            
            pitch = Mathf.Clamp(pitch, pitchRange.x, pitchRange.y);

            pitchTargetRotation = Quaternion.Euler(pitch, 0f, 0f);
            // headPlayer.transform.rotation = Quaternion.Slerp(
            //     headPlayer.transform.rotation,
            //     pitchTargetRotation,
            //     smoothSpeed * Time.deltaTime
            // );
            
            headPlayer.transform.localRotation = pitchTargetRotation;
        }

        void OnDisable()
        {
            PlayerInputReader.Instance.Look -= Look;
        }
    }
}