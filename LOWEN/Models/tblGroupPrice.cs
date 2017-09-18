using System;
using System.Collections.Generic;

namespace LOWEN.Models
{
    public partial class tblGroupPrice
    {
        public int id { get; set; }
        public string GroupName { get; set; }
        public Nullable<int> Sale { get; set; }
        public Nullable<int> Ord { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
    }
}
