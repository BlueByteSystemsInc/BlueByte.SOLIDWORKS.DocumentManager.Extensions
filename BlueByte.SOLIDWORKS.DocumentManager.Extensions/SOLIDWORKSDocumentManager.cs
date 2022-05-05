using SolidWorks.Interop.swdocumentmgr;
using System;

namespace BlueByte.SOLIDWORKS.DocumentManager.Extensions
{
    public class SOLIDWORKSDocumentManager  : IDisposable
    {
 
        #region Private Fields

        private SwDMApplication swDocMgr;

        #endregion

        
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SOLIDWORKSDocumentManager"/> class.
        /// </summary>
        /// <param name="licenseKey">The license key.</param>
        public SOLIDWORKSDocumentManager(string licenseKey)
        {    
            Init(licenseKey);
        }

        void Init(string licenseKey)
        {
            SwDMClassFactory swClassFact = default(SwDMClassFactory);

            swClassFact = new SwDMClassFactory();

            swDocMgr = swClassFact.GetApplication(licenseKey);
        }

        /// <summary>
        /// Opens the document.
        /// </summary>
        /// <param name="sDocFileName">Name of the  document file.</param>
        /// <param name="openFileEvenIfReadyOnly">if set to <c>true</c> [open file even if ready only].</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Cannot open non-SOLIDWORKS files.
        /// or
        /// File may be readonly or open in another application. File cannot be processed.
        /// </exception>
        public SwDMDocument19 OpenDocument(string sDocFileName, bool openFileEvenIfReadyOnly = false)
        {
            SwDMDocument19 swDoc = default(SwDMDocument19);
            SwDmDocumentType nDocType = 0;
            SwDmDocumentOpenError nRetVal = 0;

            if (sDocFileName.ToLower().EndsWith("sldprt"))
            {
                nDocType = SwDmDocumentType.swDmDocumentPart;
            }
            else if (sDocFileName.ToLower().EndsWith("sldasm"))
            {
                nDocType = SwDmDocumentType.swDmDocumentAssembly;
            }
            else if (sDocFileName.ToLower().EndsWith("slddrw"))
            {
                nDocType = SwDmDocumentType.swDmDocumentDrawing;
            }
            else
            {
                // Not a SOLIDWORKS file
                nDocType = SwDmDocumentType.swDmDocumentUnknown;

                // So cannot open
                throw new Exception("Cannot open non-SOLIDWORKS files.");
            }

            swDoc = (SwDMDocument19)swDocMgr.GetDocument(sDocFileName, nDocType, openFileEvenIfReadyOnly, out nRetVal);

            if (swDoc == null)
                throw new Exception("File may be readonly or open in another application. File cannot be processed.");

            return swDoc;
        }

        public void Dispose()
        {
            if (swDocMgr != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(swDocMgr);
            }
        }



        #endregion


    }

}
