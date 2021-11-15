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
        
        [ContextMenu("Add Vector3 Object Event")]
        private void AddVector3ObjectEvent() => AddNested<Events.Vector3ObjectSubscribableEvent>();
        [ContextMenu("Add Vector3 Two State Event")]
        private void AddVector3TwoStateEvent() => AddNested<Events.Vector3TwoStateSubscribableEvent>();
        
        
        [ContextMenu("Add Quaternion Object Event")]
        private void AddQuaternionObjectEvent() => AddNested<Events.QuaternionObjectSubscribableEvent>();
        [ContextMenu("Add Quaternion Two State Event")]
        private void AddQuaternionTwoStateEvent() => AddNested<Events.QuaternionTwoStateSubscribableEvent>();
        
        
        [ContextMenu("Add Mesh Object Event")]
        private void AddMeshObjectEvent() => AddNested<Events.MeshObjectSubscribableEvent>();
        [ContextMenu("Add Mesh Two State Event")]
        private void AddMeshTwoStateEvent() => AddNested<Events.MeshTwoStateSubscribableEvent>();

        
        [ContextMenu("Add Material Object Event")]
        private void AddMaterialObjectEvent() => AddNested<Events.MaterialObjectSubscribableEvent>();
        [ContextMenu("Add Material Two State Event")]
        private void AddMaterialTwoStateEvent() => AddNested<Events.MaterialTwoStateSubscribableEvent>();
        
        
        [ContextMenu("Add Transform Object Event")]
        private void AddTransformObjectEvent() => AddNested<Events.TransformObjectSubscribableEvent>();
        [ContextMenu("Add Transform Component Event")]
        private void AddTransformComponentEvent() => AddNested<Events.TransformComponentSubscribableEvent>();
        [ContextMenu("Add Transform Two State Event")]
        private void AddTransformTwoStateEvent() => AddNested<Events.TransformTwoStateSubscribableEvent>();
        
        [ContextMenu("Add Renderer Object Event")]
        private void AddRendererObjectEvent() => AddNested<Events.MeshRendererObjectSubscribableEvent>();
        [ContextMenu("Add Renderer Component Event")]
        private void AddRendererComponentEvent() => AddNested<Events.MeshRendererComponentSubscribableEvent>();
        [ContextMenu("Add Renderer Two State Event")]
        private void AddRendererTwoStateEvent() => AddNested<Events.MeshRendererTwoStateSubscribableEvent>();

#endif
        
    }
}