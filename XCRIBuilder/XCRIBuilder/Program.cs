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
            xmloutput = xmloutput.Replace("&lt;", "<");
            xmloutput = xmloutput.Replace("&gt;", ">");
            xmloutput = xmloutput.Replace("li value=" + "\"0\"", "li");
            xmloutput = xmloutput.Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");

            string outputfile = @"c:\temp\xmloutput.xml";
            if (args.Length > 0)
            {
                outputfile = args[0];
            }

            File.WriteAllText(outputfile,xmloutput.ToString());         

        }
    }
}
