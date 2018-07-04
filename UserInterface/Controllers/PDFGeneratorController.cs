using System.IO;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Net.Mail;
using System.Net;
using System;
using UserInterface.Models;
using iTextSharp.text.pdf.draw;
using Newtonsoft.Json;
using iTextSharp.tool.xml.pipeline;

namespace UserInterface.Controllers
{
    public class PDFGeneratorController : Controller
    {
        // GET: PDFGenerator
        public ActionResult Index()
        {
            //string result= SendPDFDoc("");
            return View();
        }
        [HttpPost]
        public string SendPDFDoc(string MailBody)
        {
            try
            {
                string imageURL = Server.MapPath("~/Content/images/logo.png");
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                //Resize image depend upon your need
                jpg.ScaleToFit(70f, 60f);
                jpg.SpacingBefore = 10f;
                jpg.SpacingAfter = 1f;

                jpg.Alignment = Element.ALIGN_LEFT;
                string mailBody = MailBody.Replace("<br>","<br/>").ToString();
                StringReader reader = new StringReader(mailBody.ToString());
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    pdfDoc.Open();
                    jpg.SetAbsolutePosition(pdfDoc.Left, pdfDoc.Top - 60);
                    pdfDoc.Add(jpg);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    MailMessage mm = new MailMessage("gochurchmail@gmail.com", "tom.a4s.son@gmail.com");
                    mm.Subject = "iTextSharp PDF";
                    mm.Body = "iTextSharp PDF Attachment";
                    mm.Attachments.Add(new Attachment(new MemoryStream(bytes), "iTextSharpPDF.pdf"));
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential();
                    NetworkCred.UserName = "gochurchmail@gmail.com";
                    NetworkCred.Password = "thri@2015";
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    return "Email send successfully";
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        public string PrintPDF(PDFTools pDFToolsObj)
        {
            //string imageURL = Server.MapPath("~/Content/images/logo.png");
            //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
            ////Resize image depend upon your need
            //jpg.ScaleToFit(70f, 60f);
            //jpg.SpacingBefore = 10f;
            //jpg.SpacingAfter = 1f;

            //jpg.Alignment = Element.ALIGN_LEFT;
            string htmlBody = pDFToolsObj.Content.Replace("<br>", "<br/>").ToString();
            StringReader reader = new StringReader(htmlBody.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 85f, 30f);
            byte[] bytes = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                Footer footobj = new Footer();
                footobj.imageURL=Server.MapPath("~/Content/images/header.png");
                footobj.imgURL = Server.MapPath("~/Content/images/footer.jpg");
                footobj.Header = XMLWorkerHelper.ParseToElementList(pDFToolsObj.Headcontent, null);
                footobj.Tableheader = "\n" + "Customer Payment Ledger Report" + "\n";
                writer.PageEvent = footobj;
                // Our custom Header and Footer is done using Event Handler
                //TwoColumnHeaderFooter PageEventHandler = new TwoColumnHeaderFooter();
                //writer.PageEvent = PageEventHandler;
                //// Define the page header
                //PageEventHandler.Title = "Column Header";
                //PageEventHandler.HeaderFont = FontFactory.GetFont(BaseFont.COURIER_BOLD, 10, Font.BOLD);
                //PageEventHandler.HeaderLeft = "Group";
                //PageEventHandler.HeaderRight = "1";
                pdfDoc.Open();
                //jpg.SetAbsolutePosition(pdfDoc.Left, pdfDoc.Top - 60);
                //pdfDoc.Add(jpg);
                //PdfContentByte cb = writer.DirectContent;
                //cb.MoveTo(pdfDoc.Left, pdfDoc.Top-60 );
                //cb.LineTo(pdfDoc.Right, pdfDoc.Top-60);
                //cb.SetLineWidth(1);
                //cb.SetColorStroke(new CMYKColor(0f, 12f, 0f, 7f));
                //cb.Stroke();
                //cb.MoveTo(pdfDoc.Left, pdfDoc.Top+5);
                //cb.LineTo(pdfDoc.Right, pdfDoc.Top+5);
                //cb.SetLineWidth(1);
                //cb.SetColorStroke(new CMYKColor(0f, 12f, 0f, 7f));
                //cb.Stroke();

                //Paragraph welcomeParagraph = new Paragraph("Hello, World!");
                // Our custom Header and Footer is done using Event Handler

                //pdfDoc.Add(welcomeParagraph);

                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                //for (int i = 0; i <= 2; i++)
                //{
                //    // Define the page header
                //    PageEventHandler.HeaderRight = i.ToString();
                //    if (i != 1)
                //    {
                //        pdfDoc.NewPage();
                //    }
                //}
                pdfDoc.Close();
                bytes = memoryStream.ToArray();
                memoryStream.Close();
            }
            string fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), "Customer Payment Ledger.pdf");
            System.IO.File.WriteAllBytes(fname, bytes);
            //File(bytes, "application/pdf", "Report.pdf").sa
            //bytes.SaveAs(fname);
            return JsonConvert.SerializeObject(new { Result = "OK", URL = "../Content/Uploads/Customer Payment Ledger.pdf" });

        }
        public partial class Footer : PdfPageEventHelper

        {
            public string imageURL { get; set; }
            public string Tableheader { get; set; }
            public string CustomerName { get; set; }
            public ElementList Header;
            public string imgURL { get; set; }
            public override void OnEndPage(PdfWriter writer, Document doc)

            {
                var FontColour = new BaseColor(106,60,102);
                string footer = ("Report Generated on: " + DateTime.Now.ToString("dd-MMM-yyyy h:mm tt"));            
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imgURL);
                 jpg.ScaleToFit(800F, 900F);
                Font myFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.ITALIC, BaseColor.WHITE);
                Paragraph foot = new Paragraph();
                Phrase ph = new Phrase(footer, myFont);
                foot.Add(ph);
                PdfPCell cellImage = new PdfPCell(jpg);
                cellImage.Border = 0;
                cellImage.PaddingLeft = 0;
                PdfPCell cell1 = new PdfPCell(foot);
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1.Border = 0;
                cell1.PaddingRight = -15;
               
                cell1.PaddingTop =-25;
     
                PdfPTable footerTbl = new PdfPTable(1);
               footerTbl.TotalWidth = 1000;

               
                footerTbl.AddCell(cellImage);
                footerTbl.AddCell(cell1);

              
                footerTbl.WriteSelectedRows(0, -1,0,45, writer.DirectContent);
            
            }
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                jpg.ScaleToFit(800F, 900F);

                var FontColour = new BaseColor(149, 76, 132);
                var FontColour1 = new BaseColor(0, 100, 3);

                PdfPTable headerSectionTbl = new PdfPTable(2);
                headerSectionTbl.DefaultCell.Border = Rectangle.NO_BORDER;
                float[] ColumnWidths = new float[] { 119.055F, document.PageSize.Width - 119.055F };
                headerSectionTbl.SetWidths(ColumnWidths);
                headerSectionTbl.TotalWidth = document.PageSize.Width;

                PdfPTable headerSection = new PdfPTable(2);
                headerSection.DefaultCell.Border = Rectangle.NO_BORDER;
                float[] ColumnWidth = new float[] { 119.055F, document.PageSize.Width - 119.055F };
                headerSection.SetWidths(ColumnWidths);
                headerSection.TotalWidth = document.PageSize.Width;

              
                Font myFont = FontFactory.GetFont("OpenSans", 14, iTextSharp.text.Font.BOLD, BaseColor.WHITE);             
                Font myFont2 = FontFactory.GetFont("OpenSans", 9, iTextSharp.text.Font.NORMAL, BaseColor.WHITE);
               
                string customer;
                if (CustomerName == "Customer : ")
                {
                    customer = null;
                }
                else
                {
                    customer = "\n" + CustomerName + "\n";
                }


                string documentName = "\n" + Tableheader + "\n";
              
                Paragraph header = new Paragraph();
                Paragraph header1 = new Paragraph();
                Phrase ph1 = new Phrase(documentName, myFont);
                Phrase ph2 = new Phrase(customer, myFont2);
                header.Add(ph1);
                header.Alignment = Element.ALIGN_LEFT;
                header.Add(ph2);
                header1.Alignment = Element.ALIGN_RIGHT;
              
                PdfPCell cellImage = new PdfPCell(jpg);
                cellImage.Border = 0;
                cellImage.PaddingLeft = 0;              
                headerSectionTbl.AddCell(cellImage);
                PdfPCell cell1 = new PdfPCell(header);
                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                 cell1.Border = 0;
                cell1.PaddingRight = 8.8425F;
                 cell1.PaddingTop = -6;
                headerSectionTbl.AddCell(cell1);               
                headerSectionTbl.WriteSelectedRows(0, -1, 0, document.PageSize.Height, writer.DirectContent);
                PdfContentByte contentByte = writer.DirectContent;

             
            }
        }

        public FileResult Download(PDFTools pDFToolsObj)
        {
            //jpg.Alignment = Element.ALIGN_LEFT;
            string htmlBody = pDFToolsObj.Content.Replace("<br>", "<br/>").ToString();
            StringReader reader = new StringReader(htmlBody.ToString());            
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 85f, 30f);
            byte[] bytes = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

                Footer footobj = new Footer();
                footobj.imageURL = Server.MapPath("~/Content/images/header.png");
                footobj.imgURL = Server.MapPath("~/Content/images/footer.jpg");
                footobj.Header = XMLWorkerHelper.ParseToElementList(pDFToolsObj.Headcontent, null);
                footobj.CustomerName = pDFToolsObj.CustomerName;
                footobj.Tableheader = pDFToolsObj.HeaderText;
                writer.PageEvent = footobj;

                // Our custom Header and Footer is done using Event Handler
                //TwoColumnHeaderFooter PageEventHandler = new TwoColumnHeaderFooter();
                //writer.PageEvent = PageEventHandler;
                //// Define the page header
                //PageEventHandler.Title = "Column Header";
                //PageEventHandler.HeaderFont = FontFactory.GetFont(BaseFont.COURIER_BOLD, 10, Font.BOLD);
                //PageEventHandler.HeaderLeft = "Group";
                //PageEventHandler.HeaderRight = "1";
                pdfDoc.Open();
                //jpg.SetAbsolutePosition(pdfDoc.Left, pdfDoc.Top - 60);
                //pdfDoc.Add(jpg);
                //PdfContentByte cb = writer.DirectContent;
                //cb.MoveTo(pdfDoc.Left, pdfDoc.Top-60 );
                //cb.LineTo(pdfDoc.Right, pdfDoc.Top-60);
                //cb.SetLineWidth(1);
                //cb.SetColorStroke(new CMYKColor(0f, 12f, 0f, 7f));
                //cb.Stroke();
                //cb.MoveTo(pdfDoc.Left, pdfDoc.Top+5);
                //cb.LineTo(pdfDoc.Right, pdfDoc.Top+5);
                //cb.SetLineWidth(1);
                //cb.SetColorStroke(new CMYKColor(0f, 12f, 0f, 7f));
                //cb.Stroke();

                //Paragraph welcomeParagraph = new Paragraph("Hello, World!");
                // Our custom Header and Footer is done using Event Handler

                //pdfDoc.Add(welcomeParagraph);

                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                //for (int i = 0; i <= 2; i++)
                //{
                //    // Define the page header
                //    PageEventHandler.HeaderRight = i.ToString();
                //    if (i != 1)
                //    {
                //        pdfDoc.NewPage();
                //    }
                //}
                pdfDoc.Close();
                bytes = memoryStream.ToArray();
                memoryStream.Close();
            }
            string fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), "Customer Payment Ledger.pdf");
            System.IO.File.WriteAllBytes(fname, bytes);
            string contentType = "application/pdf";
            //Parameters to file are
            //1. The File Path on the File Server
            //2. The content type MIME type
            //3. The parameter for the file save by the browser
            return File(fname, contentType, "Customer Payment Ledger.pdf");
        }
       
    }

}

