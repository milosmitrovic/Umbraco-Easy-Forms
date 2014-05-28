using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Umbraco.Web;



namespace mUtility.Umbraco.Validation.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public class UmbracoEmailAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoEmailAttribute"/> class.
        /// </summary>
        public UmbracoEmailAttribute(string errorMessageDictionaryKey)
            : base(@"^$|^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
        {
            if (!string.IsNullOrWhiteSpace(errorMessageDictionaryKey))
                base.ErrorMessage = new UmbracoHelper(UmbracoContext.Current).GetDictionaryValue(errorMessageDictionaryKey);

        
        }
    }
}
