using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Alteracia.Patterns.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AddressablesConfigurationReader", menuName = "ConfigurationReader/AddressablesReader", order = 0)]
    [System.Serializable]
    public class AddressablesConfigurationReader : ConfigurationReader
    {
        [SerializeField] private string pathToCatalog;
        public override async Task ReadConfigFile(ScriptableObject configurable)
        {
            AsyncOperationHandle<IResourceLocator> catalog = Addressables.LoadContentCatalogAsync(pathToCatalog);
            await catalog.Task;
            
            IResourceLocator resourceLocator = catalog.Result;
            resourceLocator.Locate(configurable.name, typeof(ScriptableObject), out IList<IResourceLocation> locations);
            IResourceLocation resourceLocation = locations[0];
            
            AsyncOperationHandle<ScriptableObject> loadAssetAsync = Addressables.LoadAssetAsync<ScriptableObject>(resourceLocation);
            await loadAssetAsync.Task;
            
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(loadAssetAsync.Result), configurable);
        }
    }
}