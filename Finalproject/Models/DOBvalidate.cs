using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace Finalproject.Models
{
    public class DOBvalidate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dt = Convert.ToDateTime(value);
            if (dt > DateTime.Now)
                return false;
            else
                return true;
            //return base.IsValid(value);
        }
    }
}