using Systems.Input;
using TnieYuPackage.GlobalExtensions;
using UnityEngine;

namespace Systems.Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speedScale = 5f;
        
        private Camera mainCamera;
        private Vector2 playerDirection;

        void Awake()
        {
            mainCamera = Camera.main;
        }
        
        private void OnEnable()
        {
            PlayerInputReader.Instance.Move += OnPlayerMove;
        }

        void OnPlayerMove(Vector2 direction)
        {
            playerDirection = direction;
        }

        void Update()
        {
            if (playerDirection.magnitude > 0.1f)
            {
                gameObject.transform.Translate(CalculateVector(playerDirection) * speedScale * Time.deltaTime, Space.World);
            }
        }

        Vector3 CalculateVector(Vector2 direction)
        {
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;

            return (cameraRight * direction.x + cameraForward * direction.y).With(y:0).normalized;
        }

        void OnDisable()
        {
            PlayerInputReader.Instance.Move -= OnPlayerMove;
        }
        

    }
}