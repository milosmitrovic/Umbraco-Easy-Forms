using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Umbraco.Web;


namespace mUtility.Umbraco.Validation.Attributes
{
    /// <summary>
    ///  
    /// </summary>
    public class UmbracoRequredAttribute : RequiredAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoRequredAttribute"/> class.
        /// </summary>
        public UmbracoRequredAttribute(string errorMessageDictionaryKey)
            : base()
        {
            //base.ErrorMessageResourceName = null;

            if (!string.IsNullOrWhiteSpace(errorMessageDictionaryKey))
                base.ErrorMessage = new UmbracoHelper(UmbracoContext.Current).GetDictionaryValue(errorMessageDictionaryKey);
        }
    }
}
