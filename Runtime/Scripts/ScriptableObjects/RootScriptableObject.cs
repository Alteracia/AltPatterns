// from KasperGameDev/Nested-Scriptable-Objects-Example 
// https://github.com/KasperGameDev/Nested-Scriptable-Objects-Example/blob/main/Assets/Scripts/ContainerDamageType.cs
// 

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Alteracia.Patterns
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