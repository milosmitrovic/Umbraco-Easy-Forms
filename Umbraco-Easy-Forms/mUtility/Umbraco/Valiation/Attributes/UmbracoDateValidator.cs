using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Umbraco.Web;

namespace mUtility.Umbraco.Validation.Attributes 
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class UmbracoDateValidatorAttribute : ValidationAttribute
    {

        /// <summary>
        /// Gets or sets the error message dictionary key.
        /// </summary>
        /// <value>
        /// The error message dictionary key.
        /// </value>
        //public string ErrorMessageDictionaryKey { get; set; }

        /// <summary>
        /// The umbraco
        /// </summary>
        private UmbracoHelper umbraco = new UmbracoHelper(UmbracoContext.Current);

        /// <summary>
        /// The date format
        /// </summary>
        private string dateFormat = string.Empty;

        /// <summary>
        /// The required
        /// </summary>
        private bool required = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoDateValidatorAttribute" /> class.
        /// </summary>
        /// <param name="required">if set to <c>true</c> [required].</param>
        public UmbracoDateValidatorAttribute(string errorMessageDictionaryKey, bool required = false)
        {
            this.required = required;

            if (!string.IsNullOrWhiteSpace(errorMessageDictionaryKey))
                base.ErrorMessage = umbraco.GetDictionaryValue(errorMessageDictionaryKey);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoDateValidatorAttribute" /> class.
        /// </summary>
        /// <param name="dateFormat">The date format.</param>
        /// <param name="required">if set to <c>true</c> [required].</param>
        public UmbracoDateValidatorAttribute(string dateFormat, string errorMessageDictionaryKey, bool required = false)
        {
            this.required = required;
            this.dateFormat = dateFormat;

            if (!string.IsNullOrWhiteSpace(errorMessageDictionaryKey))
                base.ErrorMessage = umbraco.GetDictionaryValue(errorMessageDictionaryKey);
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime result;

                if (!string.IsNullOrWhiteSpace(dateFormat))
                {
                    if (DateTime.TryParseExact(value.ToString(), dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                        return ValidationResult.Success;
                }
                else
                {
                    if (DateTime.TryParse(value.ToString(), out result))
                        return ValidationResult.Success;
                }

                return new ValidationResult(base.ErrorMessageString, new List<string> { validationContext.MemberName });
            }
            else
            {
                if (required)
                {
                    return new ValidationResult(base.ErrorMessageString, new List<string> { validationContext.MemberName });
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
        }
    }
}
