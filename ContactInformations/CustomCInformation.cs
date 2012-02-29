/* 
 * Richard Andersson
 * 100804
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ContactRegistryLibrary
{
    /// <summary>
    /// Kontaktinformationstypen som kan användas till vilken sorts kontaktinformation man vill. T.ex Facebook. 
    /// </summary>
    public class CustomCInformation : ContactInformation
    {
        private string customType_;

        public CustomCInformation() { }
        public CustomCInformation(string customType, string cInformation) : this(customType, cInformation, String.Empty) { }
        public CustomCInformation(string customType, string cInformation, string comment)
            : base(cInformation, comment)
        {
            customType_ = customType;
        }

        /// <summary>
        /// Egen implementation av writeXML
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Type", this.CustomType);
            writer.WriteAttributeString("Value", this.CInformation);
            writer.WriteAttributeString("Comment", this.Comment);
        }

        public string CustomType
        {
            get { return customType_; }
            set
            {
                if (value != null && value != String.Empty)
                    customType_ = value;
            }
        }
    }
}
