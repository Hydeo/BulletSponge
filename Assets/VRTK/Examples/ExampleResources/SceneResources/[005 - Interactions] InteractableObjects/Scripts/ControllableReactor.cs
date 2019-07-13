namespace VRTK.Examples
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;
    using VRTK.Controllables;

    public class ControllableReactor : MonoBehaviour
    {
        public VRTK_BaseControllable controllable;
        public Text displayText;
        public string outputOnMax = "Maximum Reached";
        public string outputOnMin = "Minimum Reached";
        public float rotationSpeed = 0;
        private float previousSpeed = 0;
        private Quaternion previousRotation;
        public Rail rail;
        private float maxSpeed = 0;


        protected virtual void OnEnable()
        {
            controllable = (controllable == null ? GetComponent<VRTK_BaseControllable>() : controllable);
            controllable.ValueChanged += ValueChanged;
            controllable.MaxLimitReached += MaxLimitReached;
            controllable.MinLimitReached += MinLimitReached;
            
        }

        protected virtual void FixedUpdate()
        {

        }

        protected virtual void ValueChanged(object sender, ControllableEventArgs e)
        {
            if (displayText != null)
            {
                if (Time.frameCount % 10 == 0)
                {
                    rotationSpeed = (float)Math.Round(getAngularVelocity() / 360.0f, 2);
                    maxSpeed = rail.currentSpeed > maxSpeed ? rail.currentSpeed : maxSpeed;
                }

                displayText.text = maxSpeed + " _ " + rotationSpeed.ToString(); ;
            }
        }

        private float getAngularVelocity()
        {
            Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(previousRotation);

            previousRotation = transform.rotation;

            float angle = 0.0f;
            Vector3 axis = Vector3.zero;

            deltaRotation.ToAngleAxis(out angle, out axis);           

            return angle * (1.0f / Time.deltaTime);
        }

        protected virtual void MaxLimitReached(object sender, ControllableEventArgs e)
        {
            if (outputOnMax != "")
            {
                Debug.Log(outputOnMax);
            }
        }

        protected virtual void MinLimitReached(object sender, ControllableEventArgs e)
        {
            if (outputOnMin != "")
            {
                Debug.Log(outputOnMin);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting && previousSpeed != rotationSpeed)
            {
                float photonSpeed = rotationSpeed;
                stream.Serialize(ref photonSpeed);
                previousSpeed = rotationSpeed;
            }
            else
            {
                float photonSpeed = 0.0f;
                stream.Serialize(ref photonSpeed);
                this.rotationSpeed = photonSpeed;
            }
        }
    }
}