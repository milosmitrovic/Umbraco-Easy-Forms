using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Umbraco.Web;

namespace mUtility.Umbraco.Validation.Attributes
{
    public class UmbracoRegExAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoRegExAttribute"/> class.
        /// </summary>
        /// <param name="regularExpressionDictionaryKey">The regular expression dictionary key.</param>
        public UmbracoRegExAttribute(string regularExpressionDictionaryKey, string errorMessageDictionaryKey)
            : base(GetRegex(regularExpressionDictionaryKey))
        {
            if (!string.IsNullOrWhiteSpace(errorMessageDictionaryKey))
                ErrorMessage = new UmbracoHelper(UmbracoContext.Current).GetDictionaryValue(errorMessageDictionaryKey);
        }

        /// <summary>
        /// Gets the regex.
        /// </summary>
        /// <param name="regularExpressionDictionaryKey">The regular expression dictionary key.</param>
        /// <returns></returns>
        private static string GetRegex(string regularExpressionDictionaryKey)
        {
            var regex = new UmbracoHelper(UmbracoContext.Current).GetDictionaryValue(regularExpressionDictionaryKey);

            return regex;
        }
    }
}
