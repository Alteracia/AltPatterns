using System;
using UnityEngine;

namespace Alteracia.Patterns.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ScriptableEventsRegistry", menuName = "AltEvents/ScriptableEventsRegistry", order = 1)]
    public class ScriptableEventsRegistry : RootScriptableObject
    {
       
#if UNITY_EDITOR
        
        void Awake()
        {
            ScriptableEventsRegistryBuss.Instance?.AddRegistry(this);
        }

        private void OnValidate()
        {
           // Debug.Log("On Validation");
        }

        protected override void OnAdded()
        {
            ScriptableEventsRegistryBuss.Instance?.AddRegistry(this);
        }

        [ContextMenu("Add Vector3 Object Event")]
        private void AddVector3ObjectEvent() => AddNested<Events.Vector3ObjectEvent>();
        [ContextMenu("Add Vector3 Two State Event")]
        private void AddVector3TwoStateEvent() => AddNested<Events.Vector3TwoStateEvent>();
        
        
        [ContextMenu("Add Quaternion Object Event")]
        private void AddQuaternionObjectEvent() => AddNested<Events.QuaternionObjectEvent>();
        [ContextMenu("Add Quaternion Two State Event")]
        private void AddQuaternionTwoStateEvent() => AddNested<Events.QuaternionTwoStateEvent>();
        
        
        [ContextMenu("Add Mesh Object Event")]
        private void AddMeshObjectEvent() => AddNested<Events.MeshObjectEvent>();
        [ContextMenu("Add Mesh Two State Event")]
        private void AddMeshTwoStateEvent() => AddNested<Events.MeshTwoStateEvent>();

        
        [ContextMenu("Add Material Object Event")]
        private void AddMaterialObjectEvent() => AddNested<Events.MaterialObjectEvent>();
        [ContextMenu("Add Material Two State Event")]
        private void AddMaterialTwoStateEvent() => AddNested<Events.MaterialTwoStateEvent>();
        
        
        [ContextMenu("Add Transform Object Event")]
        private void AddTransformObjectEvent() => AddNested<Events.TransformObjectEvent>();
        [ContextMenu("Add Transform Component Event")]
        private void AddTransformComponentEvent() => AddNested<Events.TransformComponentEvent>();
        [ContextMenu("Add Transform Two State Event")]
        private void AddTransformTwoStateEvent() => AddNested<Events.TransformTwoStateEvent>();
        
        [ContextMenu("Add Renderer Object Event")]
        private void AddRendererObjectEvent() => AddNested<Events.MeshRendererObjectEvent>();
        [ContextMenu("Add Renderer Component Event")]
        private void AddRendererComponentEvent() => AddNested<Events.MeshRendererComponentEvent>();
        [ContextMenu("Add Renderer Two State Event")]
        private void AddRendererTwoStateEvent() => AddNested<Events.MeshRendererTwoStateEvent>();

#endif
        
    }
}