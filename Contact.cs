/* 
 * Richard Andersson
 * 100804
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;


namespace ContactRegistryLibrary
{
    /// <summary>
    /// Implementerar TreeNode så att man kan ha kontaktobjekten i själva Trädvyn istället för att ha en separat lista. 
    /// </summary>
    public class Contact: System.Windows.Forms.TreeNode, IComparable<Contact>, IXmlSerializable
    {
        protected List<ContactInformation> contactInfoList = new List<ContactInformation>();
        protected string profileImagePath;

        public Contact() 
        {
            ImageIndex = 0;
        }
        public Contact(Phone phoneNr) : this(phoneNr, 0) { }
        public Contact(Phone phoneNr, int imageIndex)
        {
            SetNewContactInfo(phoneNr);
            ImageIndex = imageIndex;
        }

        /// <summary>
        /// Sätter texten på trädnoden till vad fullname är. 
        /// </summary>
        public void InitializeText()
        {
            Text = FullName;
        }

        /// <summary>
        /// Lägger dit ny kontaktinfo
        /// </summary>
        /// <param name="cInfo"></param>
        /// <returns></returns>
        public bool SetNewContactInfo(ContactInformation cInfo)
        {
            try
            {
                contactInfoList.Add(cInfo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int GetContactInfoCount()
        {
            return contactInfoList.Count;
        }

        public ContactInformation GetContactInfo(int index)
        {
            try
            {
                return contactInfoList.ElementAt(index);
            }
            catch
            {
                return null;
            };
        }

        /// <summary>
        /// Ändrar kontaktinfo m.h.a 3 strängar som får fungera som "primärnyckel" om man ska se på det som en databas. 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="value"></param>
        /// <param name="comment"></param>
        /// <param name="cInfo"></param>
        /// <returns></returns>
        public bool ChangeContactInfo(string typeName, string value, string comment, ContactInformation cInfo)
        {
            if (!Enum.IsDefined(typeof(ContactInfoTypes), typeName))
            {
                typeName = typeof(CustomCInformation).Name;
            }
            int index = GetInfoIndex(typeName, value, comment);
            try
            {
                if (index != -1)
                {
                    contactInfoList[index] = cInfo;
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Anropar den tidigare versionen av funktionen, är bara till för att förenkla vid anrop. 
        /// </summary>
        /// <param name="cInfoStrings"></param>
        /// <param name="cInfo"></param>
        /// <returns></returns>
        public bool ChangeContactInfo(string[] cInfoStrings, ContactInformation cInfo)
        {
            if (cInfoStrings.Count() == 3)
                return ChangeContactInfo(cInfoStrings[0], cInfoStrings[1], cInfoStrings[2], cInfo);
            else
                return false;
        }

        /// <summary>
        /// Hittar kontaktinfo index m.h.a 3 strängar. 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="value"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        private int GetInfoIndex(string typeName, string value, string comment)
        {
            for (int i = 0; i < contactInfoList.Count; ++i)
                if (contactInfoList[i].CInformation == value && contactInfoList[i].Comment == comment
                    && contactInfoList[i].GetType().Name == typeName)
                    return i;
            return -1;
        }

        /// <summary>
        /// Tar bort kontaktinfo m.h.a GetInfoIndex.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="value"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public bool DeleteContactInfo(string typeName, string value, string comment)
        {
            if (!Enum.IsDefined(typeof(ContactInfoTypes), typeName))
                typeName = typeof(CustomCInformation).Name;
            int index = GetInfoIndex(typeName, value, comment);
            try
            {
                contactInfoList.RemoveAt(index);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteContactInfo(string[] cInfoStrings)
        {
            if(cInfoStrings.Count() == 3)
                return DeleteContactInfo(cInfoStrings[0], cInfoStrings[1], cInfoStrings[2]);
            else 
                return false;
        }

        public override string ToString()
        {
            return FullName;
        }

        private List<ContactInformation> ContactInfoList
        {
            get { return contactInfoList; }
            set
            {
                contactInfoList = value;
            }
        }

        public virtual string FullName
        {
            get { return ""; }
        }

        public string ProfileImagePath
        {
            get { return profileImagePath; }
            set
            {
                if (value != null && value != String.Empty)
                    profileImagePath = value;
            }
        }

        public int CompareTo(Contact other)
        {
            return this.ToString().CompareTo(other.ToString());
        }

        //-------------Implementation av IXmlSerializable--
        // Person och Company har egna implementationer av namn på kontakt, sedan anropar de basklassens funktion 
        // för att utföra resten. 
        //-------------------------------------------------

        /// <summary>
        /// Ska inte göra något speciellt om man inte använder schemas
        /// </summary>
        /// <returns></returns>
        public virtual XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Hämtar kontaktens värden, sedan loopar igenom all kontaktinformation och lägger in det i listan med dens egna readxml. 
        /// </summary>
        /// <param name="reader"></param>
        public virtual void ReadXml(XmlReader reader)
        {
            ProfileImagePath = reader["ProfileImagePath"];

            if (reader.ReadToDescendant("Contact_Information"))
            {
                string typeString = "";
                Type cInfoType = null;
                ContactInformation cInfo = null;
                while (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "Contact_Information")
                {
                    typeString = reader["Type"];
                    if (Enum.IsDefined(typeof(ContactInfoTypes), typeString))
                    {
                        cInfoType = Type.GetType("ContactRegistryLibrary." + typeString + ", ContactRegistryLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                        cInfo = (ContactInformation)Activator.CreateInstance(cInfoType);
                        cInfo.ReadXml(reader);
                        ContactInfoList.Add(cInfo);
                    }
                    else
                    {
                        cInfo = new CustomCInformation();
                        ((CustomCInformation)cInfo).CustomType = typeString;
                        ((CustomCInformation)cInfo).ReadXml(reader);
                        ContactInfoList.Add((CustomCInformation)cInfo);
                    }
                }
            }
        }

        /// <summary>
        /// Skriver kontaktens värden, sedan loopar igenom kontaktlistan och anropar dess värden med deras egna writexml
        /// </summary>
        /// <param name="writer"></param>
        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("ProfileImagePath", this.ProfileImagePath);

            foreach (var cInfo in ContactInfoList)
            {
                writer.WriteStartElement("Contact_Information");
                if(cInfo is CustomCInformation)
                    ((CustomCInformation)cInfo).WriteXml(writer);
                else
                    cInfo.WriteXml(writer);
                writer.WriteEndElement();
            }
        }
    }
}
