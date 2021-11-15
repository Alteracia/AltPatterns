// from KasperGameDev/Nested-Scriptable-Objects-Example 
// https://github.com/KasperGameDev/Nested-Scriptable-Objects-Example/blob/main/Assets/Scripts/ContainerDamageType.cs
// 

using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Alteracia.Patterns.ScriptableObjects
{
    public abstract class RootScriptableObject : ScriptableObject
    {
        [HideInInspector][SerializeField] private List<NestedScriptableObject> nested = new List<NestedScriptableObject>();

        public List<NestedScriptableObject> Nested
        {
            get => nested;
            set => nested = value;
        }

#if UNITY_EDITOR

        protected void AddNested<T>() where T : NestedScriptableObject
        {
            var newNested = ScriptableObject.CreateInstance<T>();
            newNested.name = typeof(T).Name;
            AddNewNested(newNested);
        }
        
        public void AddNested<T>(T toCopy) where T : NestedScriptableObject
        {
            var newNested = ScriptableObject.Instantiate(toCopy);
            newNested.name = toCopy.name;
            AddNewNested(newNested);
        }
        
        private void AddNewNested<T>(T newNested) where T : NestedScriptableObject
        {
            newNested.Initialise(this);
            nested.Add(newNested);

            AssetDatabase.AddObjectToAsset(newNested, this);
            AssetDatabase.SaveAssets();

            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(newNested);
        }

        [ContextMenu("Delete all")]
        protected void DeleteAll()
        {
            for (int i = nested.Count; i-- > 0;)
            {
                NestedScriptableObject tmp = nested[i];

                nested.Remove(tmp);
                Undo.DestroyObjectImmediate(tmp);
            }

            AssetDatabase.SaveAssets();
        }

#endif
    }
}