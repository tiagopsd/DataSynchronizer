using System;
using System.Collections.Generic;
using System.Text;

namespace DataSynchronizer.Domain.Models
{
    public class ColumnsModel
    {
        public List<string> Names { get; set; }
        public List<object> Values { get; set; }
    }
}
