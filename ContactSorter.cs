/* 
 * Richard Andersson
 * 100804
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ContactRegistryLibrary
{
    /// <summary>
    /// Jämför objekt av typ Contact
    /// </summary>
    public class ContactSorter: System.Collections.IComparer
    {
        public int Compare(object x, object y) // tillfällig kommentar
        {
            Type typ = x.GetType();
            Type typ2 = y.GetType();
            if(x is Contact && y is Contact)
                if(x is Person && y is Person)
                    return ((Person)x).CompareTo((Person)y);
                else
                    return ((Contact)x).CompareTo((Contact)y);
            else if (x.GetType() == typeof(TreeNode))
            {
                TreeNode tx = x as TreeNode;
                TreeNode ty = y as TreeNode;

                if (tx.Text.Length != ty.Text.Length)
                    return tx.Text.Length - ty.Text.Length;
                return -string.Compare(ty.Text, tx.Text);
            }

            return 0;
        }
    }
}
