using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HtmlAction.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
    }
}