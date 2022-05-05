using SolidWorks.Interop.swdocumentmgr;
using System;

namespace BlueByte.SOLIDWORKS.DocumentManager.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ComponentExtension
    {

        /// <summary>
        /// Traverses the component tree and perform an action based on a predicate. This methods opens and closes component documents to read the tree using <see cref="ISwDMComponent11.GetDocument(bool, out SwDmDocumentOpenError)"/> but does not close the top-level document.
        /// </summary>
        /// <param name="swDocument">The SOLIDWORKS assembly document.</param>
        /// <param name="filter">The predicate filter.</param>
        /// <param name="action">The action.</param>
        public static void TraverseComponentTreeAndDo(this ISwDMDocument19 swDocument, Predicate<ISwDMComponent> filter = null, Action<ISwDMComponent11> action = null)
        {

            if (swDocument.FullName.ToLower().EndsWith("sldasm") == false)
                throw new ArgumentException($"{nameof(swDocument)} must be an assembly document.");
            
            var activeConfigurationName = swDocument.ConfigurationManager.GetActiveConfigurationName();
            var activeConfiguration = swDocument.ConfigurationManager.GetConfigurationByName(activeConfigurationName) as ISwDMConfiguration17;
            var components = activeConfiguration.GetComponents() as object[];


            Action<ISwDMComponent11, string> traverse = default(Action<ISwDMComponent11, string>);

            traverse = (ISwDMComponent11 component, string parentSelectionString) => {


                if (filter != null)
                {
                    if (filter.Invoke(component))
                        if (action != null)
                            action.Invoke(component);
                }
                else if (action != null)
                        action.Invoke(component);
                
            
                // break when component is a part
                if (component.PathName.ToLower().EndsWith("sldprt"))
                    return; 

                SwDmDocumentOpenError error;

                var componentDocument = component.GetDocument(true, out error);

                var referencedConfiguration = component.ConfigurationName;

                var configuration = componentDocument.ConfigurationManager.GetConfigurationByName(referencedConfiguration) as ISwDMConfiguration17;

                var children = configuration.GetComponents() as object[];
                if (children != null)
                foreach (var child in children)
                {
                    var swChild = child as ISwDMComponent11;
                    traverse(swChild, swChild.SelectName2);

                        
                 }


                 //close document
                componentDocument.CloseDoc();
            };

            if (components != null)
            {

                foreach (var component in components)
                {
                    var swcomponent = component as ISwDMComponent11;
                    traverse(swcomponent, string.Empty);
                }

            }

        
 
        }


    }
}
