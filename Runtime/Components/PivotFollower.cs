using Alteracia.Patterns.ScriptableObjects.Events;
using UnityEngine;

namespace Alteracia.Patterns.Components
{
    public class PivotFollower : MonoBehaviour
    {
        [Header("Invokes")]
        [SerializeField] private TransformComponentEvent _transformEvents;
    
        [Header("Subscribes")]
        [SerializeField] private Vector3ObjectEvent _positionEvent;
        [SerializeField] private QuaternionObjectEvent _rotationEvent;
        [SerializeField] private Vector3ObjectEvent _lookAtEvent;
        [SerializeField] private Vector3ObjectEvent _scaleEvent;
        [SerializeField] private FloatObjectEvent _localScaleEvent;

        private Transform _transform;

        private void Start()
        {
            _transform = transform;

            _transformEvents.OnEvent?.Invoke(_transform);

            ChangePosition(_positionEvent.Last);
            LookAt(_lookAtEvent.Last);
            //ChangeRotation(_rotationEvent.Last);
            if (Vector3.zero != _scaleEvent.Last) ChangeScale(_scaleEvent.Last);
            if (_localScaleEvent.Last > float.Epsilon) ChangeLocalScale(_localScaleEvent.Last);
            
            _positionEvent.OnEvent += ChangePosition;
            _rotationEvent.OnEvent += ChangeRotation;
            _lookAtEvent.OnEvent += LookAt;
            _scaleEvent.OnEvent += ChangeScale;
            _localScaleEvent.OnEvent += ChangeLocalScale;
        }

        private void ChangeLocalScale(float scale)
        {
            _transform.localScale = scale *
                                    (_scaleEvent.Last.magnitude < float.Epsilon ? Vector3.one : _scaleEvent.Last);
        }

        private void ChangeScale(Vector3 scale)
        {
            _transform.localScale = scale * 
                                    (_localScaleEvent.Last < float.Epsilon ? 1f : _localScaleEvent.Last);
        }

        private void ChangePosition(Vector3 position)
        {
            _transform.position = position;
        }

        private void ChangeRotation(Quaternion rotation)
        {
            _transform.rotation = rotation;
        }

        private void LookAt(Vector3 point)
        {
            _transform.LookAt(new Vector3(point.x, _transform.position.y, point.z));
        }

        private void OnDestroy()
        {
            _positionEvent.OnEvent -= ChangePosition;
            _rotationEvent.OnEvent -= ChangeRotation;
            _lookAtEvent.OnEvent -= LookAt;
            _scaleEvent.OnEvent -= ChangeScale;
            _localScaleEvent.OnEvent -= ChangeLocalScale;
        }
    }
}
