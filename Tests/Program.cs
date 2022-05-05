using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueByte.SOLIDWORKS.DocumentManager.Extensions;
using SolidWorks.Interop.swdocumentmgr;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            string licenseKey = "SOLIDWORKS DOCUMENT MANAGER API KEY";
            string fileName = @"C:\SOLIDWORKSPDM\Bluebyte\API\Knapheide\bodies\KDB\80017120.SLDASM";

            var docMan = new SOLIDWORKSDocumentManager(licenseKey);
            var document = docMan.OpenDocument(fileName,true);

             document.TraverseComponentTreeAndDo(null, (ISwDMComponent11 x)=> {

                 Console.WriteLine(x.PathName);

            });


            document.Close();

            

            Console.ReadLine();
        }
    }
}
