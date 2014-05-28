using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace mUtility.Umbraco.Valiation
{
    public static class Extensions
    {
        /// <summary>
        /// Models the is valid.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller">The controller.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static bool ModelIsValid<T>(this SurfaceController controller, T model) where T : class
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, serviceProvider: null, items: null);

            return Validator.TryValidateObject(model, validationContext, validationResults, false);
        }

        /// <summary>
        /// Gets the model errors.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller">The controller.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static JsonResult GetModelErrors<T>(this SurfaceController controller, T model) where T : class
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, serviceProvider: null, items: null);

            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
            {
                var errors = validationResults.Where(item => item.MemberNames.Any()).Select(item => new
                {
                    FieldName = item.MemberNames.Where(field => !string.IsNullOrWhiteSpace(field)).First(),
                    ErrorMessage = item.ErrorMessage
                });

                return new JsonResult() { Data = new { Errors = errors } };
            }

            return new JsonResult();
        }

        /// <summary>
        /// Gets the model sent from controller.
        /// </summary>
        /// <typeparam name="T">Model type</typeparam>
        /// <param name="page">The WebViewPage.</param>
        public static void GetModel<T>(this WebViewPage<T> page) where T : class
        {
            var models = page.ViewContext.TempData.Where(item => item.Value is T);
            string modelName = string.Empty;
            if (models.Any())
            { 
                modelName = models.First().Key.ToString();
                var model = (T)models.First().Value;

                page.ViewData.Model = model;
                page.Html.ViewData.Model = model;
                page.ViewContext.ViewData.Model = model;

                page.ViewContext.TempData.Remove(models.First().Key);
            }

            var modelStates = page.ViewContext.TempData.Where(item => item.Key == string.Concat(modelName, "-ModelState")).ToList();

            if (modelStates.Any())
            {
                page.ViewData.ModelState.Clear();
                page.Html.ViewData.ModelState.Clear();
                page.ViewContext.ViewData.ModelState.Clear();

                if (modelStates.FirstOrDefault().Value is ModelStateDictionary)
                    foreach (var item in (ModelStateDictionary)modelStates.First().Value)
                    {
                        page.ViewData.ModelState.Add(item);
                        page.Html.ViewData.ModelState.Add(item);
                        page.ViewContext.ViewData.ModelState.Add(item);
                    }

                page.ViewContext.TempData.Remove(modelStates.First().Key);
            }

            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(T));

            page.ViewData.ModelMetadata = modelMetadata;
            page.Html.ViewData.ModelMetadata = modelMetadata;
            page.ViewContext.ViewData.ModelMetadata = modelMetadata;
        }

        /// <summary>
        /// Sets the model for transfer on view.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller">The controller.</param>
        /// <param name="model">The model.</param>
        public static RedirectToUmbracoPageResult SetModel<T>(this RedirectToUmbracoPageResult actionResult, System.Web.Mvc.Controller controller, T model) where T : class
        {
            var modelId = Guid.NewGuid().ToString();
            controller.TempData.Add(modelId, model);
            controller.TempData.Add(string.Concat(modelId, "-ModelState"), controller.ModelState);

            return actionResult;
        }
    }
}
