using System.Web.Mvc;

namespace CRUD.Controllers
{
    internal class HttpStatusCodeResultWithBody : ViewResult
    {
        private int _statusCode;
        private string _description;

        public HttpStatusCodeResultWithBody(int statusCode,
                string description = null)
            : this(null, statusCode, description)
        {

        }
        public HttpStatusCodeResultWithBody(string viewName, int statusCode,
                string description = null)
        {
            _statusCode = statusCode;
            _description = description;
            ViewName = viewName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var httpContext = context.HttpContext;
            var response = httpContext.Response;

            response.StatusCode = _statusCode;
            response.StatusDescription = _description;

            base.ExecuteResult(context);
        }
    }
}