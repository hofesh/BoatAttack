using UnityEngine;

namespace BoatAttack.Boat
{
    /// <summary>
    /// This sends input controls to the boat engine if 'Human'
    /// </summary>
    public class HumanController : MonoBehaviour
    {

        public Engine engine; // the engine script

        public InputControls controls;

        public float throttle;
        public float steering;

        int width;

        void Awake()
        {
            width = Camera.main.pixelWidth;
            controls = new InputControls();

            controls.BoatControls.Trottle.performed += context => throttle = context.ReadValue<float>();
            controls.BoatControls.Trottle.canceled += context => throttle = 0f;

            controls.BoatControls.Steering.performed += context => steering = context.ReadValue<float>();
            controls.BoatControls.Steering.canceled += context => steering = 0f;

            engine = GetComponent<Engine>(); // get the engine script
        }

        private void OnEnable()
        {
            controls.BoatControls.Enable();
        }

        private void OnDisable()
        {
            controls.BoatControls.Disable();
        }

        void Update()
        {
            if (Input.touchSupported)
            {
                if (Input.touchCount > 0)
                {
                    var touch = Input.GetTouch(0);
                    steering = ((float)touch.position.x - width / 2) / (width / 2);
                }
                else
                    steering = 0;
            }
        }

        void FixedUpdate()
        {
            engine.Accel(throttle);
            engine.Turn(steering);
        }
    }
}

