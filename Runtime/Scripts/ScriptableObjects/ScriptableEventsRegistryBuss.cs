using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Alteracia.Patterns.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ScriptableEventsRegistryBuss", menuName = "AltEvents/ScriptableEventsRegistryBuss", order = 0)]
    public class ScriptableEventsRegistryBuss : ScriptableEventsRegistry
    {
        [SerializeField] private List<ScriptableEventsRegistry> registries = new List<ScriptableEventsRegistry>();
        private static ScriptableEventsRegistryBuss _instance;

        public static ScriptableEventsRegistryBuss Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogWarning("There is no Scriptable Events Registry Buss asset in project");
                }

                return _instance;
            }
        }
        
        // Awake called after application starts - for editor start editor!
        private void Awake()
        {
            Debug.Log("Awake");
        }
        
        // Called after runtime starts
        public void OnEnable()
        {
            Debug.Log("OnEnable");
            BindEventsRegistryAndBuss();
        }
        
        // Called after play pressed in editor
        public void OnDisable()
        {
            Debug.Log("OnDisable");
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(this);
        }

        // Subscribe all on Start
        private void BindEventsRegistryAndBuss()
        {
            foreach (var soEvent in registries.SelectMany(registry => registry.Nested.OfType<ISubscribableEvent>()))
            {
                foreach (var cur in this.Nested.OfType<ISubscribableEvent>())
                {
                    if (!cur.Equals(soEvent)) continue;
                    //Debug.Log("Subscribe");
                    cur.SubscribeTo(soEvent);
                    soEvent.SubscribeTo(cur);
                    break;
                }
            }
        }

        
#if UNITY_EDITOR
        
        private ScriptableEventsRegistryBuss()
        {
            if (_instance == null) _instance = this;
        }
        
        public void AddRegistry(ScriptableEventsRegistry registry)
        {
            if (!registries.Contains(registry))
                registries.Add(registry);

            foreach (var soEvent in registry.Nested.OfType<ISubscribableEvent>())
            {
                bool equal = false;
                foreach (var cur in this.Nested.OfType<ISubscribableEvent>())
                {
                    if (cur.Equals(soEvent))
                    {
                        equal = true;
                        break;
                    }
                    
                }

                if (equal) continue;
                AddNested((NestedScriptableObject)soEvent);
                //Debug.Log("Added");

            }
        }
        
        [ContextMenu("Save")]
        private void SaveThis()
        {
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(this);
        }
        
#endif
    }

}