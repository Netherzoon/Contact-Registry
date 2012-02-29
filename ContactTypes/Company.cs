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
    [Serializable]
    public class Company: Contact, IComparable<Company>, IXmlSerializable
    {
        private string companyName_;

        public Company() 
        {
            ImageIndex = 3;
        }
        public Company(string companyName, Phone phoneNr) : this(companyName, phoneNr, 3) { }
        public Company(string companyName, Phone phoneNr, int imageIndex) : base(phoneNr, imageIndex) 
        {
            companyName_ = companyName;
            InitializeText();
        }

        public string CompanyName
        {
            get { return companyName_; }
            set
            {
                if (value != null && value != String.Empty)
                {
                    companyName_ = value;
                }
                InitializeText();
            }
        }

        public override string FullName
        {
            get { return companyName_; }
        }

        public int CompareTo(Company other)
        {
            return this.FullName.CompareTo(other.FullName);
        }

        public override XmlSchema GetSchema()
        {
            return null;
        }

        public override void ReadXml(XmlReader reader)
        {
            string s = reader.LocalName;
            if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "Company")
            {
                string s2 = reader["CompanyName"];
                CompanyName = s2;
                string s3 = FullName;
                base.ReadXml(reader);
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("CompanyName", this.FullName);
            base.WriteXml(writer);
        }
    }
}
