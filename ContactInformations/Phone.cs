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
    public class Phone: ContactInformation
    {
        public Phone() : base() { }
        public Phone(string cInformation) : base(cInformation, String.Empty) { }
        public Phone(string cInformation, string comment) : base(cInformation, comment) { }
    }
}
