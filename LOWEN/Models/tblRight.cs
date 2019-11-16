using System;
using System.Collections.Generic;

namespace LOWEN.Models
{
    public partial class tblRight
    {
        public int id { get; set; }
        public Nullable<int> idModule { get; set; }
        public Nullable<int> idUser { get; set; }
        public Nullable<int> Role { get; set; }
    }
}
