using Systems.Input;
using TnieYuPackage.GlobalExtensions;
using UnityEngine;

namespace Systems.Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speedScale = 5f;
        
        private Camera mainCamera;
        private Vector3 playerDirection;

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
            playerDirection = direction.ConvertToVector3();
        }

        void Update()
        {
            if (playerDirection.magnitude > 0.1f)
            {
                gameObject.transform.Translate(CalculateVector(playerDirection) * speedScale * Time.deltaTime);
            }
        }

        Vector3 CalculateVector(Vector3 direction)
        {
            Vector3 cameraZ = mainCamera.transform.forward;
            Vector3 cameraX = mainCamera.transform.right;

            return (cameraX * direction.x + cameraZ * direction.z).With(y: 0).normalized;
        }

        void OnDisable()
        {
            PlayerInputReader.Instance.Move -= OnPlayerMove;
        }
        

    }
}