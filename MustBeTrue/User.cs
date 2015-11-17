using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using AcceptTermsAndConditions.Attributes;

namespace AcceptTermsAndConditions.Models
{
    public class User
    {
        [MustBeTrue(ErrorMessage = "Please accept the Terms & Conditions.")]
        [DisplayName("I accept the Terms & Conditions.")]
        public bool AcceptTerms { get; set; }
    }
}