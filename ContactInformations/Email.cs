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
    public class Email: ContactInformation
    {
        public Email() : base() { }
        public Email(string cInformation) : base(cInformation, String.Empty) { }
        public Email(string cInformation, string comment) : base(cInformation, comment) { }
    }
}
