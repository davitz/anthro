using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// Simple camera controller
    /// </summary>
    [RequireComponent(typeof (Image))]
    public class CameraRotator : UIBehaviour, IDragHandler
    {
        public Transform cameraTransform;
        public Transform target;
        [Header("Position")]
        public float distanceMin = 10;
        public float distanceMax = 30;
        public float yOffset = 0;
        public float scrollSensitivity = 1000;
        public float scrollSmoothing = 10;
        [Header("Rotation")]
        public float tiltMin = -85;
        public float tiltMax = 85;
        public float rotationSensitivity = 0.5f;
        public float rotationSpeed = 20;

        private float distance;
        private float scrollDistance;
        private float velocity;
        private float lookAngle;
        private float tiltAngle;
        private Quaternion rotation;

        protected override void Awake()
        {
            base.Awake();
            tiltAngle = (tiltMin + tiltMax)/2;
            distance = scrollDistance = (distanceMax + distanceMin)/2;

            if (cameraTransform == null || target == null) return;

            cameraTransform.rotation = rotation = Quaternion.Euler(tiltAngle, lookAngle, 0);
            cameraTransform.position = CalculateCameraPosition();
        }

        private void LateUpdate()
        {
            if (cameraTransform == null || target == null) return;

            if (cameraTransform.rotation != rotation)
            {
                cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, rotation,
                    Time.deltaTime*rotationSpeed);
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            bool rightBumper = Input.GetKey("joystick button 5");
            bool leftBumper = Input.GetKey("joystick button 4");

            if (rightBumper)
            {
                scroll = -0.1f;
            }
            else if (leftBumper)
            {
                scroll = 0.1f;
            }

            if (scroll != 0)
            {
                scrollDistance -= scroll*Time.deltaTime*scrollSensitivity;
                scrollDistance = Mathf.Clamp(scrollDistance, distanceMin, distanceMax);
            }
            

            if (distance != scrollDistance)
            {
                distance = Mathf.SmoothDamp(distance, scrollDistance, ref velocity, Time.deltaTime*scrollSmoothing);
            }

            cameraTransform.position = CalculateCameraPosition();

            float axisX = Input.GetAxis("Controller Right Joystick X");
            float axisY = Input.GetAxis("Controller Right Joystick Y");

            if (axisX != 0 || axisY != 0)
            {
                RotateCameraWithController(axisX, axisY);
            }
        }

        public void OnDrag(PointerEventData eventData)
        { 
            if (cameraTransform == null || target == null) return;

            lookAngle += eventData.delta.x*rotationSensitivity;
            tiltAngle -= eventData.delta.y*rotationSensitivity;
            tiltAngle = Mathf.Clamp(tiltAngle, tiltMin, tiltMax);
            rotation = Quaternion.Euler(tiltAngle, lookAngle, 0);
        }

        private Vector3 CalculateCameraPosition()
        {
            return target.position + cameraTransform.rotation*(Vector3.back*distance) + Vector3.up*yOffset;
        }

        private void RotateCameraWithController(float axisX, float axisY)
        {
            if (cameraTransform == null || target == null) return;

            lookAngle += axisX * rotationSensitivity;
            tiltAngle -= axisY * rotationSensitivity;
            tiltAngle = Mathf.Clamp(tiltAngle, tiltMin, tiltMax);
            rotation = Quaternion.Euler(tiltAngle, lookAngle, 0);
        }
    }
}