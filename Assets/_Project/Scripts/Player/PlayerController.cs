using Systems.Input;
using TnieYuPackage.GlobalExtensions;
using TnieYuPackage.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speedScale = 5f;

        [SerializeField] private Camera playerCamera;
        private Vector2 playerDirection;

        void Awake()
        {
            if (playerCamera == null)
                playerCamera = Camera.main;
        }

        private void OnEnable()
        {
            PlayerInputReader.Instance.Move += OnPlayerMove;
            LocationGlobalConfig.Instance.playerGameObject = this.gameObject;
        }
        
        void OnDisable()
        {
            PlayerInputReader.Instance.Move -= OnPlayerMove;
            LocationGlobalConfig.Instance.playerGameObject = null;
        }

        void OnPlayerMove(Vector2 direction)
        {
            playerDirection = direction;
        }

        void Update()
        {
            if (playerDirection.magnitude > 0.1f)
            {
                gameObject.transform.Translate(CalculateVector(playerDirection) * speedScale * Time.deltaTime,
                    Space.World);
            }
        }

        Vector3 CalculateVector(Vector2 direction)
        {
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;

            return (cameraRight * direction.x + cameraForward * direction.y).With(y: 0).normalized;
        }
    }
}