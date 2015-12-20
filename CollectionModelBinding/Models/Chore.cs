using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelBinding.Models
{
    public class Chore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}