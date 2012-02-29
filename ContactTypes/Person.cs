/* 
 * Richard Andersson
 * 100804
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ContactRegistryLibrary
{
    public class Person : Contact, IComparable<Person>, IXmlSerializable
    {
        public Person() 
        {
            ImageIndex = 2;
        }
        public Person(string fName, string lName, Phone phoneNr) : this(fName, lName, phoneNr, 2) { }
        public Person(string fName, string lName, Phone phoneNr, int imageIndex)
            : base(phoneNr, imageIndex)
        {
            fName_ = fName;
            lName_ = lName;
            InitializeText();
        }

        public override string ToString()
        {
            return LName + ";" + FName;
        }

        private string fName_;
        private string lName_;


        public string FName
        {
            get { return fName_; }
            set
            {
                if (value != null && value != String.Empty)
                    fName_ = value;
                InitializeText();
            }
        }

        public string LName
        {
            get { return lName_; }
            set
            {
                if (value != null && value != String.Empty)
                    lName_ = value;
                InitializeText();
            }
        }

        public override string FullName
        {
            get { return FName + " " + LName; }
        }

        public int CompareTo(Person other)
        {
            if (LName.CompareTo(other.LName) != 0)
                return LName.CompareTo(other.LName);
            else if (FName.CompareTo(other.FName) != 0)
                return FName.CompareTo(other.FName);

            return 0;
        }

        public override XmlSchema GetSchema()
        {
            return null;
        }

        public override void ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "Person")
            {
                FName = reader["FName"];
                LName = reader["LName"];

                base.ReadXml(reader);
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("FName", this.FName);
            writer.WriteAttributeString("LName", this.LName);
            base.WriteXml(writer);

        }
    }
}
