using UnityEngine;

namespace Afrodita.is4You2 {

    public class ControladorMovimiento : Photon.PunBehaviour {

        public float speed = 6.0F;
        public float jumpSpeed = 8.0F;
        public float gravity = 20.0F;
        public float speedRatation = 1.0F;
        private Vector3 moveDirection = Vector3.zero;

        void Start() {
            CameraWork cameraWork = this.gameObject.GetComponent<CameraWork>();
            Cursor.visible = false;
            if (cameraWork != null) {
                if (photonView.isMine) {
                    cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.Log("Debe asignar una camara al jugador antes de empezar.");
            }
        }

        void Update()
        {
            if (photonView.isMine) {
                Walk();
            }
        }

        void Walk() {
            CharacterController controller = GetComponent<CharacterController>();
            if (controller.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;
                if (Input.GetButton("Jump"))
                    moveDirection.y = jumpSpeed;
                transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speedRatation);
            }
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}