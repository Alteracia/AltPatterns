using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

using Alteracia.Web;

namespace Alteracia.Patterns.ScriptableObjects
{
    [CreateAssetMenu(fileName = "WebConfigurationReader", menuName = "ConfigurationReader/WebReader", order = 0)]
    [System.Serializable]
    public class WebConfigurationReader : ConfigurationReader
    {
        //[SerializeField] private bool local;
        [SerializeField] private string urlToHost;
        
        public override async Task ReadConfigFile(ScriptableObject configurable)
        {
            //Debug.Log(GetUrl(urlToHost, configurable.name));
            using (var req = await Requests.Get(GetUrl(urlToHost, configurable.name)))
            {
                if (!req.Success()) return;
                JsonUtility.FromJsonOverwrite(req.downloadHandler.text, configurable);
            }
        }

        private static string GetUrl(string urlToHost, string nameOfFile)
        {
            string entry = "";
            if (!urlToHost.StartsWith("http://") && !urlToHost.StartsWith("file://")) entry = "http://";
            urlToHost = Regex.Replace(urlToHost, @"\{([^}]+)\}", match =>
            {
                var fullFieldName = match.Value.Substring(1, match.Value.Length - 2);
                var split = fullFieldName.Split('.');
                var nameOfField = split[split.Length - 1];
                var nameOfType = fullFieldName.Remove(fullFieldName.LastIndexOf(nameOfField) - 1);
                Type staticPublicType = null;
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    staticPublicType = assembly.GetType(nameOfType);
                    if (staticPublicType != null) break;
                }
                if (staticPublicType == null) return "null";
                var value = staticPublicType
                    .GetField(nameOfField, BindingFlags.Public | BindingFlags.Static)
                    ?.GetValue(staticPublicType);
                return (string)value;
            });
            //Debug.Log(urlToHost);
            return $"{entry}{urlToHost}/{nameOfFile}.json";
            //System.IO.Path.Combine(local ? Application.absoluteURL : urlToHost, configurable.name + ".json")
        }
    }
    
}
