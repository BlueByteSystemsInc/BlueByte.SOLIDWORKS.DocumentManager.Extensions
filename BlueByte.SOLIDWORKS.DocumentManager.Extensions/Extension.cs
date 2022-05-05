using SolidWorks.Interop.swdocumentmgr;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueByte.SOLIDWORKS.DocumentManager.Extensions
{


    public static class Extension {


      

        #region Public Methods

        /// <summary>
        /// Adds the configuration specific property.
        /// </summary>
        /// <param name="swDoc">The sw document.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <param name="propertType">Type of the propert.</param>
        /// <param name="newValue">The new value.</param>
        /// <exception cref="System.ArgumentNullException">configurationName</exception>
        public static void AddConfigurationSpecificProperty(this SwDMDocument19 swDoc, string fieldName, string configurationName, SwDmCustomInfoType propertType, string newValue)
        {

            SwDMConfigurationMgr swCfgMgr = default(SwDMConfigurationMgr);

            SwDMConfiguration14 swCfg = default(SwDMConfiguration14);

            if (string.IsNullOrWhiteSpace(configurationName))
                throw new ArgumentNullException(nameof(configurationName));

            swCfgMgr = swDoc.ConfigurationManager;

            swCfg = (SwDMConfiguration14)swCfgMgr.GetConfigurationByName(configurationName);

            swCfg.AddCustomProperty(fieldName, propertType, newValue);
        }

        /// <summary>
        /// Adds the custom property.
        /// </summary>
        /// <param name="swDoc">The sw document.</param>
        /// <param name="newCustomProperty">The new custom property.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="newCustomPropertyValue">The new custom property value.</param>
        /// <returns></returns>
        public static bool AddCustomProperty(this SwDMDocument19 swDoc, string newCustomProperty, SwDmCustomInfoType fieldType, string newCustomPropertyValue)
        {

            return swDoc.AddCustomProperty(newCustomProperty, fieldType, newCustomPropertyValue);
        }

        /// <summary>
        /// Closes the specified sw document.
        /// </summary>
        /// <param name="swDoc">The sw document.</param>
        /// <remarks>This method releases the com object from memory.</remarks>
        public static void Close(this SwDMDocument19 swDoc)
        {
            if (swDoc != null)
            {
                swDoc.CloseDoc();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(swDoc);
            }
        }


        /// <summary>
        /// Check if the property exists or not (custom tab)
        /// </summary>
        /// <param name="swDmDocument">The sw dm document.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static bool DoesPropertyExist(this SwDMDocument19 swDmDocument, string property)
        {
            var l = new List<String>();

            if (swDmDocument.GetCustomPropertyCount() > 0)
            {
                var customProperties = swDmDocument.GetCustomPropertyNames() as string[];
                if (customProperties != null && customProperties.Length > 0)
                    l.AddRange(customProperties);
            }

            return l.Contains(property);
        }

        /// <summary>
        /// Check if the property exists in configuration.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="property">The property.</param>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <returns></returns>
        public static bool DoesPropertyExistInConfiguration(this SwDMDocument19 document, string property, string configurationName)
        {
            var l = new List<String>();
            var swDmDocument = document as ISwDMDocument10;
            var configuration = swDmDocument.ConfigurationManager.GetConfigurationByName(configurationName);

            if (configuration.GetCustomPropertyCount() > 0)
            {
                var properties = configuration.GetCustomPropertyNames() as string[];
                if (properties != null && properties.Any())
                {
                    l.AddRange(properties);
                }
            }

            return l.Contains(property);
        }

        /// <summary>
        /// Gets all configuration specific properties.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <returns></returns>
        public static string[] GetAllConfigurationSpecificProperties(this SwDMDocument19 document, string configurationName)
        {
            var swDocument = document as ISwDMDocument20;
            var configuration = swDocument.ConfigurationManager.GetConfigurationByName(configurationName) as SwDMConfiguration17;
            var configurationProperties = configuration.GetCustomPropertyNames() as string[];

            if (configurationProperties != null && configurationProperties.Length > 0)
            {
                return configurationProperties as string[];
            }

            return new string[] { };
        }

        /// <summary>
        /// Gets all custom properties.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public static string[] GetAllCustomProperties(this SwDMDocument19 document)
        {
            var swDocument = document as ISwDMDocument20;
            var customProperties = swDocument.GetCustomPropertyNames() as string[];

            if (customProperties != null && customProperties.Length > 0)
            {
                return customProperties as string[];
            }

            return new string[] { };
        }

        /// <summary>
        /// Gets all property names.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public static string[] GetAllProperties(this SwDMDocument19 document)
        {
            var l = new List<string>();


            if (document != null)
            {
                var swDmDocument = document as ISwDMDocument;

                var configurationNames = GetConfigurationNames(document);

                foreach (var configurationName in configurationNames)
                {
                    if (swDmDocument.ConfigurationManager.GetConfigurationByName(configurationName).GetCustomPropertyCount() > 0)
                    {
                        var properties = swDmDocument.ConfigurationManager.GetConfigurationByName(configurationName).GetCustomPropertyNames() as string[];
                        if (properties != null)
                            foreach (var property in properties)
                            {
                                if (l.Contains(property) == false)
                                    l.Add(property);
                            }
                    }
                }

                if (swDmDocument.GetCustomPropertyCount() > 0)
                {
                    var customProperties = swDmDocument.GetCustomPropertyNames() as string[];
                    if (customProperties != null && customProperties.Length > 0)
                    {
                        foreach (var customProperty in customProperties)
                        {
                            if (l.Contains(customProperty) == false)
                                l.Add(customProperty);
                        }
                    }
                }
            }


            return l.ToArray();
        }

        /// <summary>
        /// Gets the configuration names.
        /// </summary>
        /// <param name="swDoc">The sw document.</param>
        /// <returns></returns>
        public static string[] GetConfigurationNames(this SwDMDocument19 swDoc)
        {
            if (swDoc != null)
                return (swDoc.ConfigurationManager.GetConfigurationNames() as object[]).Cast<string>().ToArray();
            else
                return new string[] { };
        }


        /// <summary>
        /// Gets the configuration specific property.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <param name="customPropertyType">Type of the custom property.</param>
        /// <param name="isEvaluatedValue">if set to <c>true</c> [is evaluated value].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">configurationName</exception>
        public static string GetConfigurationSpecificProperty(this SwDMDocument19 document, string fieldName, string configurationName, out SwDmCustomInfoType customPropertyType, bool isEvaluatedValue = false)
        {
            if (string.IsNullOrWhiteSpace(configurationName))
                throw new ArgumentNullException(nameof(configurationName));

            var swdocument = document as SwDMDocument19;
            SwDMConfigurationMgr swCfgMgr = swdocument.ConfigurationManager;
            SwDMConfiguration14 swCfg = (SwDMConfiguration14)swCfgMgr.GetConfigurationByName(configurationName);
            var evaluatedPropertyValue = swCfg.GetCustomPropertyValues(fieldName, out customPropertyType, out string propertyValue);

            if (string.IsNullOrEmpty(propertyValue)) return evaluatedPropertyValue;
            return isEvaluatedValue ? evaluatedPropertyValue : propertyValue;
        }

        /// <summary>
        /// Gets the custom property value.
        /// </summary> v
        /// <param name="document">The document.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="isEvaluatedValue">if set to <c>true</c> [is evaluated value].</param>
        /// <returns></returns>
        public static string GetCustomProperty(this SwDMDocument19 document, string propertyName, out SwDmCustomInfoType propertyType, bool isEvaluatedValue = false)
        {
            var swdocument = document as ISwDMDocument19;
            var evaluatedPropertyValue = swdocument.GetCustomPropertyValues(propertyName, out propertyType, out string propertyValue);

            if (string.IsNullOrEmpty(propertyValue)) return evaluatedPropertyValue;
            return isEvaluatedValue ? evaluatedPropertyValue : propertyValue;
        }

      


        /// <summary>
        /// Saves the document.
        /// </summary>
        /// <param name="swDoc">The sw document.</param>
        public static void SaveDocument(this SwDMDocument19 swDoc)
        {
            if (swDoc != null)
                swDoc.Save();
        }
        /// <summary>
        /// Sets the configuration specific property.
        /// </summary>
        /// <param name="swDoc">The sw document.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <param name="newValue">The new value.</param>
        /// <exception cref="System.ArgumentNullException">configurationName</exception>
        public static void SetConfigurationSpecificProperty(this SwDMDocument19 swDoc, string fieldName, string configurationName, string newValue)
        {

           
            
            SwDMConfigurationMgr swCfgMgr = default(SwDMConfigurationMgr);

            SwDMConfiguration14 swCfg = default(SwDMConfiguration14);

            if (string.IsNullOrWhiteSpace(configurationName))
                throw new ArgumentNullException(nameof(configurationName));

            swCfgMgr = swDoc.ConfigurationManager;

            swCfg = (SwDMConfiguration14)swCfgMgr.GetConfigurationByName(configurationName);

            swCfg.SetCustomProperty(fieldName, newValue);
        }


        /// <summary>
        /// Sets the custom property.
        /// </summary>
        /// <param name="swdocument">The swdocument.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        public static void SetCustomProperty(this SwDMDocument19 swdocument, string propertyName, string value)
        {

            swdocument.SetCustomProperty(propertyName, value);
        }

        /// <summary>
        /// Sets the type of the custom property.
        /// </summary>
        /// <param name="swDocument">The sw document.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="configurationName">Name of the configuration.</param>
        public static void SetCustomPropertyType(this SwDMDocument19 swDocument, string propertyName, SwDmCustomInfoType propertyType, string configurationName = "")
        {
            

            if (configurationName == "")
            {
                var customPropertyValue = swDocument.GetCustomProperty(propertyName, out SwDmCustomInfoType customPropertyType);
                var deleteResult = swDocument.DeleteCustomProperty(propertyName);
                var addResult = swDocument.AddCustomProperty(propertyName, propertyType, customPropertyValue);
            }
            else
            {
                var configuration = swDocument.ConfigurationManager.GetConfigurationByName(configurationName);

                var configPropertyValue = configuration.GetCustomProperty(propertyName, out SwDmCustomInfoType configPropertyType);
                var deleteResult = configuration.DeleteCustomProperty(propertyName);
                var addResult = configuration.AddCustomProperty(propertyName, propertyType, configPropertyValue);
            }
        }

        #endregion

    }
}
