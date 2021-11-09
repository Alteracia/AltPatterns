// from KasperGameDev/Nested-Scriptable-Objects-Example 
// https://github.com/KasperGameDev/Nested-Scriptable-Objects-Example/blob/main/Assets/Scripts/DamageType.cs

using UnityEditor;
using UnityEngine;

namespace Alteracia.Patterns
{
    public abstract class NestedScriptableObject : ScriptableObject
    {
        [HideInInspector][SerializeField] private RootScriptableObject root;

        public RootScriptableObject Root => root;

#if UNITY_EDITOR
        public void Initialise(RootScriptableObject newRoot) => root = newRoot;
        
        /*
        [ContextMenu("Save")]
        private void SaveThis()
        {
            Undo.RecordObject(this, $"Save {this.name}");
            AssetDatabase.SaveAssets();

            EditorUtility.SetDirty(root);
            EditorUtility.SetDirty(this);
        }
        */
        
        [ContextMenu("Delete")]
        private void DeleteThis()
        {
            root.Nested.Remove(this);
            Undo.DestroyObjectImmediate(this);
            AssetDatabase.SaveAssets();
            
            EditorUtility.SetDirty(root);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}