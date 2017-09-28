using System;
using System.Collections.Generic;

namespace LOWEN.Models
{
    public partial class tblProductTag
    {
        public int id { get; set; }
        public Nullable<int> idp { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
    }
}
