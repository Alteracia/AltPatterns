using System.Threading.Tasks;
using Alteracia.Patterns.ScriptableObjects.Events;
using UnityEngine;

namespace Alteracia.Patterns.Components
{
    public class PivotLossyScaler : MonoBehaviour
    {
        public Pivot.UpdateOptions updateOn = Pivot.UpdateOptions.OnCall;
        [Header("Invokes")]
        [SerializeField] private Vector3ObjectEvent scaleEvent;

        // Start is called before the first frame update
        async void Start()
        {
            UpdateTransform();
            if (updateOn == Pivot.UpdateOptions.OnCall)
            {
                await Task.Yield();
                UpdateTransform(); // Case when scene with Always option was previous
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (updateOn == Pivot.UpdateOptions.Always) UpdateTransform();
        }
        
        public void UpdateTransform()
        {
            scaleEvent.OnEvent?.Invoke(this.transform.lossyScale);
        }
    }
}