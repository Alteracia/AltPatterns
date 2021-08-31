using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Alteracia.Patterns
{
   [Serializable]
   public abstract class ConfigurationReader : ScriptableObject
   {
      public abstract string GetRelativeUrlToFile(string absolutUrl);

      public abstract Task ReadConfigFile(string relativeUrl, ScriptableObject configurable);
   }

   [Serializable]
   public class ConfigEvent : UnityEvent<ScriptableObject> {}
   
   public abstract class ConfigurableController<T0, T1> : Controller<T0> where T0 : Controller<T0> where T1 : ScriptableObject
   {
      [SerializeField] 
      private bool initOnStart;
      [SerializeField]
      protected ConfigurationReader reader;
      [SerializeField]
      protected string configRelativeUrl;
      [SerializeField]
      protected T1 configuration;
      
      [SerializeField]
      public ConfigEvent configurationReady = new ConfigEvent();

      private void Start()
      {
         if (initOnStart) ReadConfiguration();
      }

      public void SetConfiguration(T1 config, bool read = true)
      {
         this.configuration = config;
         if (read) ReadConfiguration();
      }

      public async void SetConfiguration(string json, bool read = true)
      {
         JsonUtility.FromJsonOverwrite(json, configuration);
         if (read) ReadConfiguration();
      }

      public async void ReadConfiguration()
      {
         if (reader)
         {
            // Try load config
            await reader.ReadConfigFile(configRelativeUrl, configuration);
         }
         
         if (!configuration) return;
         
         this.OnConfigurationRead();
         
         configurationReady?.Invoke(configuration);
      }

      protected abstract void OnConfigurationRead();

      public async System.Threading.Tasks.Task ReadConfigurationFromFile()
      {
         if (reader == null)
         {
            Debug.LogWarning($"No ConfigurationReader for {typeof(T0)} Component of {this.gameObject.name}. Configuration {typeof(T1)} will not be overwritten");
            return;
         }
         await reader.ReadConfigFile(configRelativeUrl, configuration);
      }

      public string ReadConfigurationFromJson()
      {
#if UNITY_EDITOR
         
         var path = UnityEditor.EditorUtility.OpenFilePanel(
            "Open configuration",
            "",
            "json");
         
         if (string.IsNullOrEmpty(path)) return null;

         StreamReader freader = new StreamReader(path);
         string json = freader.ReadToEnd();
         
         JsonUtility.FromJsonOverwrite(json, configuration);
         
         if (reader != null)
            configRelativeUrl = reader.GetRelativeUrlToFile(path);
         else
         {
            Debug.LogWarning($"No ConfigurationReader for {typeof(T0)} Component of {this.gameObject.name}. Path to configuration file will not be provided");
            return null;
         }
         return configRelativeUrl;
            
#else
         return null;
#endif
      }

      public string SaveConfigurationToJson()
      {
         
#if UNITY_EDITOR

         var path = UnityEditor.EditorUtility.SaveFilePanel(
            "Save configuration as json",
            "",
            typeof(T1) + ".json",
            "json");

         if (string.IsNullOrEmpty(path)) return null;
         
         var json = JsonUtility.ToJson(configuration);
         
         // Get only parameters, no reference to object
         Regex regex = new Regex(@"\s*""([^""]*?)""\s*:\s*\{([^\{\}]*?)\}(,|\s|)");
         json = regex.Replace(json, "");
         // Do not save path
         regex = new Regex(@"\s*""(configRelativeUrl)"" *: *(""(.*?)""(,|\s|)|\s*\{(.*?)\}(,|\s|))");
         json = regex.Replace(json, "");
         // Clear "," in the end
         regex = new Regex(@",\s*\}");
         json = regex.Replace(json, "}");
   
         if (string.IsNullOrEmpty(json)) return null;

         StreamWriter writer = new StreamWriter(path, false);
         writer.WriteLine(json);
         writer.Close();
         
         if (reader != null)
            configRelativeUrl = reader.GetRelativeUrlToFile(path);
         else
         {
            Debug.LogWarning($"No ConfigurationReader for {typeof(T0)} Component of {this.gameObject.name}. Path to configuration file will not be provided");
            return null;
         }

         return configRelativeUrl;
         
#else
         return null;
#endif
      }
   }
}
