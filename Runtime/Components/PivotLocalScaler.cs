using System.Linq;

using UnityEngine;

using Alteracia.Patterns.ScriptableObjects;
using Alteracia.Patterns.ScriptableObjects.Events;

namespace Alteracia.Patterns.Components
{
    public class PivotLocalScaler : MonoBehaviour
    {
        public float maxTarget = 1.8f; // TODO other dimensions
        [Header("Subscribes")]
        [SerializeField] private NestedScriptableObject[] transformEvents; // TODO Check object
        [Header("Invokes")]
        [SerializeField] private FloatObjectEvent localScaleEvent;

        // Start is called before the first frame update
        void Start()
        {
            foreach (var tse in transformEvents
                         .Where(te => te is TwoStateEvents<Transform>)
                         .Cast<TwoStateEvents<Transform>>())
            {
                tse.OnPrimaryEvent += Scale; // TODO Add options for OnSecondary
                if (tse.LastPrimary != null) Scale(tse.LastPrimary);
            }

            foreach (var oe in transformEvents
                         .Where(te => te is ObjectEvent<Transform>)
                         .Cast<ObjectEvent<Transform>>())
            {
                oe.OnEvent += Scale;
                if (oe.Last != null) Scale(oe.Last);
            }
            
            foreach (var ce in transformEvents
                         .Where(te => te is ComponentEvent<Component>) // TODO FIX THIS
                         .Cast<ComponentEvent<Component>>())
            {
                ce.OnEvent += Scale;
                if (ce.Last != null) Scale(ce.Last);
            }
        }

        private void OnDestroy()
        {
            foreach (var tse in transformEvents
                         .Where(te => te is TwoStateEvents<Transform>)
                         .Cast<TwoStateEvents<Transform>>())
            {
                tse.OnPrimaryEvent -= Scale; // TODO Add options for OnSecondary
            }

            foreach (var oe in transformEvents
                         .Where(te => te is ObjectEvent<Transform>)
                         .Cast<ObjectEvent<Transform>>())
            {
                oe.OnEvent -= Scale;
            }
            
            foreach (var oe in transformEvents
                         .Where(te => te is ComponentEvent<Animator>)
                         .Cast<ComponentEvent<Animator>>())
            {
                oe.OnEvent -= Scale;
            }
        }

        private void Scale(Component obj)
        {
            Scale(obj.transform);
        }

        private void Scale(Transform obj)
        {
            Debug.Log("scale " + obj.name);
            if (!obj.GetComponentInChildren<Renderer>(true)) return;

            Vector3? checkBounds = GetBounds(obj);
            if (checkBounds == null) return;
            var bounds = (Vector3) checkBounds;

            float scale = maxTarget / Mathf.Max(bounds.x, bounds.y, bounds.z);
            Debug.Log("calculated scale = " + scale);
            if (Mathf.Abs(localScaleEvent.Last - scale) < float.Epsilon) return;

            Debug.Log("Send scale = " + scale);
            localScaleEvent.OnEvent?.Invoke(scale);
        }

        private Vector3? GetBounds(Transform obj)
        {
            var mesh = obj.GetComponentInChildren<MeshFilter>(true);
            if (mesh)
            {
                Mesh sharedMesh;
                (sharedMesh = mesh.sharedMesh).RecalculateBounds();
                return sharedMesh.bounds.size;
            }
            var skin = obj.GetComponentInChildren<SkinnedMeshRenderer>(true);
            if (!skin) return null;
            
            Mesh sharedMesh1;
            (sharedMesh1 = skin.sharedMesh).RecalculateBounds();
            return sharedMesh1.bounds.size;
        }
    }
}