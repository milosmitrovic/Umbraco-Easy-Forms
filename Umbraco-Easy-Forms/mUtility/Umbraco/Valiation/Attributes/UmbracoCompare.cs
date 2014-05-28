using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Umbraco.Web;

namespace mUtility.Umbraco.Validation.Attributes
{
    public class UmbracoCompareAttribute : CompareAttribute
    {
        public UmbracoCompareAttribute(string otherProperty, string errorMessageDictionaryKey) 
            : base(otherProperty)
        {
            if (!string.IsNullOrWhiteSpace(errorMessageDictionaryKey))
            {
                base.ErrorMessage = new UmbracoHelper(UmbracoContext.Current).GetDictionaryValue(errorMessageDictionaryKey);
            }
        }
    }
}
