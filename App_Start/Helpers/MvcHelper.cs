using System.Text;
using System.Web;
using System.Web.Mvc;

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
    }
}