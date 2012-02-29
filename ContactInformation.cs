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
    public class ContactInformation: IXmlSerializable
    {
        protected string comment_;
        protected string cInformation_;

        public ContactInformation() { }
        public ContactInformation(string cInformation) : this(cInformation, String.Empty) { }
        public ContactInformation(string cInformation, string comment)
        {
            cInformation_ = cInformation;
            comment_ = comment;
        }

        /// <summary>
        /// Hämtar all information för smidighets skull. 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cInfo"></param>
        /// <param name="comment"></param>
        public void GetInfo(out Type type, out string cInfo, out string comment)
        {
            type = this.GetType();
            cInfo = CInformation;
            if (Comment != null)
                comment = Comment;
            else
                comment = String.Empty;
        }

        public string Comment
        {
            get { return comment_; }
            set
            {
                if (value != null && value.GetType() == typeof(string))
                    comment_ = value;
            }
        }

        public string CInformation
        {
            get { return cInformation_; }
            set
            {
                if (value != null && value.GetType() == typeof(string) && value != String.Empty)
                    cInformation_ = value;
            }
        }

        //----------------Implementation av IXmlSerializable-----

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            CInformation = reader["Value"];
            Comment = reader["Comment"];
            reader.Read();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Type", this.GetType().Name);
            writer.WriteAttributeString("Value", this.CInformation);
            writer.WriteAttributeString("Comment", this.Comment);
        }
    }
}
