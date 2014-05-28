using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Umbraco.Web;
using System.Globalization;

namespace mUtility.Umbraco.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class UmbracoDateFieldsCompareAttribute : ValidationAttribute
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
        /// The date field to compare with
        /// </summary>
        private string dateFieldToCompareWith = string.Empty;
        /// <summary>
        /// The comparison type
        /// </summary>
        private DateComparisonType comparisonType;

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoDateFieldsCompareAttribute"/> class.
        /// </summary>
        /// <param name="dateFieldToCompareWith">The date field to compare with.</param>
        /// <param name="dateComparisonType">Type of the date comparison.</param>
        public UmbracoDateFieldsCompareAttribute(string dateFieldToCompareWith, DateComparisonType dateComparisonType, string errorMessageDictionaryKey)
        {
            this.dateFieldToCompareWith = dateFieldToCompareWith;
            this.comparisonType = dateComparisonType;

            if (!string.IsNullOrWhiteSpace(errorMessageDictionaryKey))
                base.ErrorMessage = umbraco.GetDictionaryValue(errorMessageDictionaryKey);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoDateFieldsCompareAttribute"/> class.
        /// </summary>
        /// <param name="dateFormat">The date format.</param>
        /// <param name="dateFieldToCompareWith">The date field to compare with.</param>
        /// <param name="dateComparisonType">Type of the date comparison.</param>
        public UmbracoDateFieldsCompareAttribute(string dateFormat, string dateFieldToCompareWith, DateComparisonType dateComparisonType, string errorMessageDictionaryKey)
        {
            this.dateFormat = dateFormat;
            this.dateFieldToCompareWith = dateFieldToCompareWith;
            this.comparisonType = dateComparisonType;

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
                    {
                        return CompareDates(result, validationContext);
                    }
                }
                else
                {
                    if (DateTime.TryParse(value.ToString(), out result))
                    {
                        return CompareDates(result, validationContext);
                    }
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Compares the dates.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns></returns>
        private ValidationResult CompareDates(DateTime date, ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(dateFieldToCompareWith))
            {
                DateTime result;

                var dateToCompare = validationContext.ObjectType.GetProperty(dateFieldToCompareWith).GetValue(validationContext.ObjectInstance, null);

                if (dateToCompare != null)
                {
                    if (!string.IsNullOrWhiteSpace(dateFormat))
                    {
                        if (DateTime.TryParseExact(dateToCompare.ToString(), dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                        {
                            return SwitchComparisonTypes(date, result, validationContext);
                        }
                    }
                    else
                    {
                        if (DateTime.TryParse(dateToCompare.ToString(), out result))
                        {
                            return SwitchComparisonTypes(date, result, validationContext);
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Switches the comparison types.
        /// </summary>
        /// <param name="fieldDate">The field date.</param>
        /// <param name="fieldToCompareDate">The field to compare date.</param>
        /// <returns></returns>
        private ValidationResult SwitchComparisonTypes(DateTime fieldDate, DateTime fieldToCompareDate, ValidationContext validationContext)
        {
            switch (comparisonType)
            {
                case DateComparisonType.GreaterThan:

                    if (fieldDate > fieldToCompareDate)
                        return ValidationResult.Success;

                    return new ValidationResult(base.ErrorMessageString, new List<string>() { validationContext.MemberName });

                case DateComparisonType.GreaterThanOrEqual:

                    if (fieldDate >= fieldToCompareDate)
                        return ValidationResult.Success;

                    return new ValidationResult(base.ErrorMessageString, new List<string>() { validationContext.MemberName }); 


                case DateComparisonType.LowerThanOrEqual:

                    if (fieldDate <= fieldToCompareDate)
                        return ValidationResult.Success;

                    return new ValidationResult(base.ErrorMessageString, new List<string>() { validationContext.MemberName });

                case DateComparisonType.LowerThan:

                    if (fieldDate < fieldToCompareDate)
                        return ValidationResult.Success;

                    return new ValidationResult(base.ErrorMessageString, new List<string>() { validationContext.MemberName });

                case DateComparisonType.Equal:

                    if (fieldDate == fieldToCompareDate)
                        return ValidationResult.Success;

                    return new ValidationResult(base.ErrorMessageString, new List<string>() { validationContext.MemberName });

                default:
                    return ValidationResult.Success;
            }
        }
    }
}
