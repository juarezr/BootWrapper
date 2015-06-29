using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BootWrapper.BW.Core.Report
{
    public abstract class BasePDFReport
    {
        #region protected members

        protected abstract void DrawReport();

        protected abstract void DrawHeader(PdfWriter writer, Document document);
        protected abstract void DrawFooter(PdfWriter writer, Document document);

        protected Document Document { get; set; }
        protected MemoryStream MemoryStream { get; set; }
        
        #endregion

        protected BasePDFReport(Rectangle pageSize, string title, string description, string authorName)
        {
            this.MemoryStream = new MemoryStream();
            // Create an instance of the document class which represents the PDF document itself.
            this.Document = new Document(pageSize, 25, 25, 30, 30);
            // Create an instance to the PDF file by creating an instance of the PDF
            // Writer class using the document and the filestrem in the constructor.
            // Add meta information to the document
            authorName = !String.IsNullOrWhiteSpace(authorName) ? authorName : "Gerado automaticamente pelo aplicativo.";
            this.Document.AddAuthor(authorName);
            this.Document.AddCreator("Sistema de Automação de Terminal");
            this.Document.AddKeywords("PDF tutorial education");
            this.Document.AddSubject(description);
            this.Document.AddTitle(title);

            Render();
        }

        #region public members

        private void Render()
        {
            PdfWriter writer = PdfWriter.GetInstance(this.Document, MemoryStream);
            writer.PageEvent = new CustomPageHandler();

            // Open the document to enable you to write to the document
            this.Document.Open();

            // Call the virtual method DrawReport to generate the pages
            this.DrawReport();

            // Close the document
            this.Document.Close();
            // Close the writer instance
            writer.Close();
        }

        protected class CustomPageHandler : PdfPageEventHelper
        {
            // Ovewrited for creating page headers footers, etc...
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                // cell height 
                //float cellHeight = document.TopMargin;

                // PDF document size      
                Rectangle page = document.PageSize;

                // create two column table
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.TotalWidth = page.Width;

                // add image; PdfPCell() overload sizes image to fit cell
                //PdfPCell c = new PdfPCell(ImageHeader, true);
                //c.HorizontalAlignment = Element.ALIGN_RIGHT;
                //c.FixedHeight = cellHeight;
                //c.Border = PdfPCell.NO_BORDER;
                //head.AddCell(c);

                // add the header text
                //c = new PdfPCell(new Phrase(
                //  DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " GMT",
                //  new Font(Font.FontFamily.COURIER, 8)
                //));
                //c.Border = PdfPCell.NO_BORDER;
                //c.VerticalAlignment = Element.ALIGN_BOTTOM;
                //c.FixedHeight = cellHeight;
                //head.AddCell(c);

                // since the table header is implemented using a PdfPTable, we call
                // WriteSelectedRows(), which requires absolute positions!
                //head.WriteSelectedRows(
                //  0, -1,  // first/last row; -1 flags all write all rows
                //  0,      // left offset
                //    // ** bottom** yPos of the table
                //  page.Height - cellHeight + head.TotalHeight,
                //  writer.DirectContent
                //);
            }
        }

        #endregion

    }
}