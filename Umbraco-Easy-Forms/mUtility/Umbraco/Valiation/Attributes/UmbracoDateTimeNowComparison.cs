using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Umbraco.Web;
using System.Globalization;

namespace mUtility.Umbraco.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class UmbracoDateTimeNowComparisonAttribute : ValidationAttribute
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
        /// The comparison type
        /// </summary>
        private DateComparisonType comparisonType;
        /// <summary>
        /// The only date
        /// </summary>
        private bool onlyDate = false;


        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoDateTimeNowComparisonAttribute"/> class.
        /// </summary>
        /// <param name="dateComparisonType">Type of the date comparison.</param>
        /// <param name="onlyDate">if set to <c>true</c> [only date].</param>
        public UmbracoDateTimeNowComparisonAttribute(DateComparisonType dateComparisonType, string errorMessageDictionaryKey, bool onlyDate = false)
        {
            this.comparisonType = dateComparisonType;
            this.onlyDate = onlyDate;

            if (!string.IsNullOrWhiteSpace(errorMessageDictionaryKey))
                base.ErrorMessage = umbraco.GetDictionaryValue(errorMessageDictionaryKey);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoDateTimeNowComparisonAttribute"/> class.
        /// </summary>
        /// <param name="dateFormat">The date format.</param>
        /// <param name="dateComparisonType">Type of the date comparison.</param>
        /// <param name="onlyDate">if set to <c>true</c> [only date].</param>
        public UmbracoDateTimeNowComparisonAttribute(string dateFormat, DateComparisonType dateComparisonType, string errorMessageDictionaryKey, bool onlyDate = false)
        {
            this.dateFormat = dateFormat;
            this.comparisonType = dateComparisonType;
            this.onlyDate = onlyDate;

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
                        return DateComparison(result, validationContext);
                }
                else
                {
                    if (DateTime.TryParse(value.ToString(), out result))
                        return DateComparison(result, validationContext);
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Dates the comparison.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        private ValidationResult DateComparison(DateTime date, ValidationContext validationContext)
        {
            var compareDate = onlyDate ? date.Date : date;
            var currentDate = onlyDate ? DateTime.Now.Date : DateTime.Now;

            switch (comparisonType)
            {
                case DateComparisonType.GreaterThan:

                    if (compareDate > currentDate)
                        return ValidationResult.Success;

                    return new ValidationResult(base.ErrorMessageString, new List<string> { validationContext.MemberName });

                case DateComparisonType.GreaterThanOrEqual:

                    if (compareDate >= currentDate)
                        return ValidationResult.Success;

                    return new ValidationResult(base.ErrorMessageString, new List<string> { validationContext.MemberName });


                case DateComparisonType.LowerThanOrEqual:

                    if (compareDate <= currentDate)
                        return ValidationResult.Success;

                    return new ValidationResult(base.ErrorMessageString, new List<string> { validationContext.MemberName });

                case DateComparisonType.LowerThan:

                    if (compareDate < currentDate)
                        return ValidationResult.Success;

                    return new ValidationResult(base.ErrorMessageString, new List<string> { validationContext.MemberName });


                case DateComparisonType.Equal:

                    if (compareDate == currentDate)
                        return ValidationResult.Success;

                    return new ValidationResult(base.ErrorMessageString, new List<string> { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
