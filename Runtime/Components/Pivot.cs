using System.Threading.Tasks;
using Alteracia.Patterns.ScriptableObjects.Events;
using UnityEngine;

namespace Alteracia.Patterns.Components
{
    public class Pivot : MonoBehaviour
    {
        public enum UpdateOptions { OnCall, Always }

        public UpdateOptions updateOn = UpdateOptions.OnCall;

        [Header("Invokes")] 
        [SerializeField] private Vector3ObjectEvent positionEvent;
        [SerializeField] private QuaternionObjectEvent rotationEvent;
        [SerializeField] private Vector3ObjectEvent scaleEvent;
        [SerializeField] private FloatObjectEvent localScaleEvent;

        private void Awake()
        {

        }

        // Start is called before the first frame update
        async void Start()
        {
            UpdateTransform();
            if (updateOn == UpdateOptions.OnCall)
            {
                await Task.Yield();
                UpdateTransform(); // Case when scene with Always option was previous
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (updateOn == UpdateOptions.Always) UpdateTransform();
        }

        public void UpdateTransform()
        {
            positionEvent.OnEvent?.Invoke(this.transform.position);
            rotationEvent.OnEvent?.Invoke(this.transform.rotation);
           // scaleEvent.OnEvent?.Invoke(this.transform.lossyScale); // TODO Add scale and options
           // localScaleEvent.OnEvent?.Invoke(this.transform.localScale.x);
        }
    }
}