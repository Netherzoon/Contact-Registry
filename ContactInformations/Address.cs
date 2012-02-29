/* 
 * Richard Andersson
 * 100804
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContactRegistryLibrary
{
    [Serializable]
    public class Address: ContactInformation
    {
        public Address() : base() { }
        public Address(string cInformation) : base(cInformation, String.Empty) { }
        public Address(string cInformation, string comment) : base(cInformation, comment) { }
    }
}
