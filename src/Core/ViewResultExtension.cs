using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.tool.xml;

namespace System.Web.Mvc
{
    public class PdfFromHtmlResult : ViewResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (string.IsNullOrEmpty(this.ViewName))
            {
                this.ViewName = context.RouteData.GetRequiredString("action");
            }

            if (this.View == null)
            {
                this.View = this.FindView(context).View;
            }

            // First get the html from the Html view
            using (var htmlView = new StringWriter())
            {
                var vwContext = new ViewContext(context, this.View, this.ViewData, this.TempData, htmlView);
                this.View.Render(vwContext, htmlView);

                // Convert to pdf

                var response = context.HttpContext.Response;

                using (var pdfStream = new MemoryStream())
                {
                    var pdfDoc = new Document();
                    var pdBWriter = PdfWriter.GetInstance(pdfDoc, pdfStream);
                    
                    using (var htmlRdr = new StringReader(htmlView.ToString()))
                    {
                        pdfDoc.Open();

                        XMLWorkerHelper.GetInstance().ParseXHtml(pdBWriter, pdfDoc, htmlRdr);
                        pdfDoc.Close();
                    }                    

                    response.ContentType = "application/pdf";
                    response.AddHeader("Content-Disposition", "inline; " + this.ViewName + ".pdf");
                    byte[] pdfBytes = pdfStream.ToArray();
                    response.OutputStream.Write(pdfBytes, 0, pdfBytes.Length);
                }
            }
        }
    }

    public static class HtmlHelperExtensions
    {
        public static IHtmlString ServerSideInclude(this HtmlHelper helper, string serverPath)
        {
            var filePath = HttpContext.Current.Server.MapPath(serverPath);

            var markup = File.ReadAllText(filePath);
            return new HtmlString(markup);
        }
    }
}
