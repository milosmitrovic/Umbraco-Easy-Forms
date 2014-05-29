Umbraco Easy Forms
==================

Allows you to create forms in native mvc manner, with validation messages from umbraco dictionary and full model usage.


###How to use

###Partial View (Macro or Template)

    //Place where you want your form to appear
    @Html.Action("GetContactForm", "Form")
    

###Model

    public class FormModel
    {
        [UmbracoRequred("Text1RequiredErrorKey")]
        public string Text1 { get; set; }
    }


###Controller

        [HttpGet]
        public ActionResult GetContactForm()
    	{
    	    return PartialView("ContactFormPartial", new FormModel());
        }
