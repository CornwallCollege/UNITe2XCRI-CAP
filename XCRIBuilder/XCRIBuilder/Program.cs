using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XCRI;
using System.IO;
using System.Net;
namespace XCRIBuilder
{
    class Program
    {
        static void Main(string[] args)
        {

            StringBuilder xmloutput = new StringBuilder();
            XCRI.XmlGeneration.Interfaces.IXmlGenerator gen =
                XCRI.XmlGeneration.XmlGeneratorFactory.GetXmlGenerator(XCRI.XCRIProfiles.XCRI_v1_2);

            XCRI.Catalog catalog = new XCRI.Catalog();
            catalog.Descriptions.Add(new Description(){Value = "Catalog for Cornwall College"});
            catalog.Providers.Add(new XCRIBuilder.CornwallCollegeProvider());

            gen.RootElement = catalog;
            
            gen.Generate(xmloutput);
            //String output = xmloutput.ToString();
            xmloutput = xmloutput.Replace("&lt;", "<");
            xmloutput = xmloutput.Replace("&gt;", ">");
            xmloutput = xmloutput.Replace("li value=" + "\"0\"", "li");
            //output = WebUtility.HtmlDecode(output);

            File.WriteAllText(@"c:\temp\xmloutput.xml",xmloutput.ToString());
            //Console.Write(xmloutput);

           

        }
    }
}
