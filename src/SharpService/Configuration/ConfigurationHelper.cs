using System.IO;
using System.Xml;

namespace SharpService.Configuration
{
    public class ConfigurationHelper
    {    
        public static XmlDocument CreateXmlDoc(string innerXml)
        {
            var doc = new XmlDocument();
            var settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            using (var strRdr = new StringReader(innerXml))
            {
                using (var reader = XmlReader.Create(strRdr, settings))
                {
                    doc.Load(reader);
                }
            }
            return doc;
        }
    }
}
