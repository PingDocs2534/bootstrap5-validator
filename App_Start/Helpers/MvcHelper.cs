using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace bootstrap5.App_Start.Helpers
{
    public static class MvcHelper
    {
        public static MvcHtmlString ValidateModelStatusBootstrap5(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
                return MvcHtmlString.Create(string.Empty);

            var firsInputId = string.Empty;
            var html = new StringBuilder();
            html.AppendLine("<script>");
            foreach (var item in modelState)
            {
                var id = item.Key;
                var chars = id.ToCharArray();

                if (string.IsNullOrEmpty(firsInputId))
                    firsInputId = id;

                chars[0] = new string(chars[0], 1).ToLower().ToCharArray()[0];
                var varName = new string(chars);
                var errorMsg = item.Value.Errors[0].ErrorMessage;
                html.AppendLine($"\tconst {varName} = document.getElementsByName('{id}')[0];");
                html.AppendLine($"\t{varName}.setCustomValidity(' ');");
                html.AppendLine($"\t{varName}.parentElement.getElementsByClassName('invalid-feedback')[0].innerHTML = '{HttpUtility.HtmlDecode(errorMsg)}';");
            }

            html.AppendLine($"\tconst eform = document.getElementById('{firsInputId}').form;");
            html.AppendLine($"\teform.classList.add('was-validated');");
            html.AppendLine("</script>");

            return MvcHtmlString.Create(html.ToString());
        }

        public static MvcHtmlString ValidateBt5<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            var modelStatus = htmlHelper.ViewData.ModelState;
            if (modelStatus.IsValid)
                return MvcHtmlString.Create(string.Empty);
            var js = @"
                        let forms = document.querySelectorAll('form.need-validate');
                        for (let i = 0; i < forms.length; i++) {
                            let eform = forms.item(i);
                            eform.classList.add('was-validated');
                        }";
            return MvcHtmlString.Create(@"<script>document.addEventListener('DOMContentLoaded', function () { " + js + " });</script>"); ;
        }

        public static MvcHtmlString ValidationMessageBT5For<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string devalidationMessage, object htmlAttributes)
        {
            var id = expression.Body.ToString().Split('.')[1];
            var errorMsg = devalidationMessage;
            var modelStatus = htmlHelper.ViewData.ModelState;
            if (!modelStatus.IsValid)
            {
                foreach (var item in modelStatus)
                {
                    if (item.Key == id)
                    {
                        errorMsg = item.Value.Errors[0].ErrorMessage;
                        break;
                    }
                }
            }
            var routeData = new RouteValueDictionary(htmlAttributes);
            var classBuilder = new StringBuilder(routeData["Class"].ToString());
            classBuilder.Append(" invalid-feedback");
            routeData["Class"] = classBuilder.ToString();

            var mainTag = new TagBuilder("div");
            mainTag.InnerHtml = errorMsg;
            mainTag.MergeAttributes(routeData);
            return MvcHtmlString.Create(mainTag.ToString());
        }
    }
}