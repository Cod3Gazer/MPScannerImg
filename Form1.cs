using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using System;
using System.Collections.Generic;
using System.Drawing;
using SdImage = System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WIA;

namespace MPScannerImg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ListScanners();
            Console.SetOut(new TextBoxStreamWriter(rtbConsoleLog));
        }

        private void ListScanners()
        {
            // WIA Device Manager
            var deviceManager = new DeviceManager();
            for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
            {
                // Only consider scanner devices
                if (deviceManager.DeviceInfos[i].Type == WiaDeviceType.ScannerDeviceType)
                {
                    cmbScanners.Items.Add(new ScannerInfo
                    {
                        Name = deviceManager.DeviceInfos[i].Properties["Name"].get_Value().ToString(),
                        DeviceID = deviceManager.DeviceInfos[i].DeviceID
                    });
                }
            }

            if (cmbScanners.Items.Count > 0)
            {
                cmbScanners.SelectedIndex = 0;
            }
        }

        private void btnScanAndSave_Click(object sender, EventArgs e)
        {
            if (cmbScanners.SelectedItem == null)
            {
                MessageBox.Show("Please select a scanner.");
                return;
            }

            var scannerInfo = (ScannerInfo)cmbScanners.SelectedItem;
            var savePath = txtSavePath.Text;

            if (!Directory.Exists(savePath))
            {
                MessageBox.Show("Please enter a valid save path.");
                return;
            }

            var images = ScanDocuments(scannerInfo.DeviceID);
            if (images.Count > 0)
            {
                var pdfPath = Path.Combine(savePath, "ScannedDocument.pdf");
                SaveImagesToPdf(images, pdfPath);
                MessageBox.Show($"PDF saved at {pdfPath}");
            }
            else
            {
                MessageBox.Show("No images scanned.");
            }
        }
        private List<Image> ScanDocuments(string scannerId)
        {
            List<Image> scannedImages = new List<Image>();
            var deviceManager = new DeviceManager() as IDeviceManager; // Use the IDeviceManager interface

            Device scannerDevice = null;
            foreach (DeviceInfo info in deviceManager.DeviceInfos)
            {
                if (info.DeviceID == scannerId && info.Type == WiaDeviceType.ScannerDeviceType)
                {
                    scannerDevice = info.Connect();
                    break;
                }
            }

            if (scannerDevice != null)
            {
                SetScannerToADF(scannerDevice);

                int pageCounter = 1;  // To number the scanned images

                while (true)
                {
                    try
                    {
                        // Directly transfer image without showing UI
                        ImageFile imageFile = (ImageFile)scannerDevice.Items[1].Transfer("{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}");

                        //byte[] imageBytes = (byte[])imageFile.FileData.get_BinaryData();
                        //using (var ms = new MemoryStream(imageBytes))
                        //{
                        //    scannedImages.Add(Image.FromStream(ms));
                        //}

                        // Convert the image file to a System.Drawing.Image object
                        Image scannedImage = ConvertImageFileToImage(imageFile);
                        scannedImages.Add(scannedImage);

                        // Save the image to disk for verification
                        // Use DateTime to generate a unique filename for each image
                        //string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        //var savePath = txtSavePath.Text;
                        //string imagePath = Path.Combine(savePath, $"ScannedImage_{timeStamp}_{pageCounter}.jpeg");
                        //SaveImage(scannedImage, imagePath);
                        pageCounter++;
                        // Introduce a 5-second delay to allow the scanner's status to update
                        int waitTime = int.Parse(txtWaitTime.Text);
                        System.Threading.Thread.Sleep(waitTime * 1000); // Sleep for 5000 milliseconds (5 seconds)
                    }
                    catch (COMException ex)
                    {
                        Console.WriteLine("Caught COMException: " + ex.Message);
                        if (ex.ErrorCode == -2145325046) // WIA_ERROR_PAPER_EMPTY
                        {
                            Console.WriteLine("The document feeder is empty. No more pages to scan.");
                            //hasMorePages = false;  // Prevent further attempts to scan
                        }
                        else
                        {
                            // Handle or log other COM exceptions differently if necessary
                            Console.WriteLine("An unexpected scanner error occurred. Continuing with available images.");
                        }
                        break;
                    }
                }
            }

            return scannedImages;
        }

        private Image ConvertImageFileToImage(ImageFile imageFile)
        {
            byte[] imageBytes = (byte[])imageFile.FileData.get_BinaryData();
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                return Image.FromStream(ms);
            }
        }

        private void SaveImage(Image image, string path)
        {
            try
            {
                using (Bitmap imgCopy = new Bitmap(image))
                {
                    imgCopy.Save(path, SdImage.ImageFormat.Jpeg); // Save the copy
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save image: {ex.Message}");
            }
        }

        private bool HasMorePages(Device device)
        {
            // Introduce a 5-second delay to allow the scanner's status to update
            int waitTime = int.Parse(txtWaitTime.Text);
            System.Threading.Thread.Sleep(waitTime * 1000); // Sleep for 5000 milliseconds (5 seconds)

            const int WIA_DPS_DOCUMENT_HANDLING_STATUS = 3087;
            const int WIA_DPS_DOCUMENT_HANDLING_SELECT = 3088;
            const int FEED_READY = 0x0001;

            Property documentHandlingStatus = null;
            Property documentHandlingSelect = null;

            foreach (Property prop in device.Properties)
            {
                if (prop.PropertyID == WIA_DPS_DOCUMENT_HANDLING_STATUS)
                    documentHandlingStatus = prop;
                if (prop.PropertyID == WIA_DPS_DOCUMENT_HANDLING_SELECT)
                    documentHandlingSelect = prop;
            }

            if (documentHandlingSelect != null && (Convert.ToInt32(documentHandlingSelect.get_Value()) & FEED_READY) != FEED_READY)
            {
                return false;  // Indicates that the feeder is not selected or not ready
            }

            if (documentHandlingStatus != null)
            {
                return (Convert.ToInt32(documentHandlingStatus.get_Value()) & FEED_READY) == FEED_READY;
            }

            return false;
        }

        private void SetScannerToADF(Device scannerDevice)
        {
            const int WIA_DPS_DOCUMENT_HANDLING_SELECT = 3088;
            const int FEEDER = 1;

            foreach (Property prop in scannerDevice.Properties)
            {
                if (prop.PropertyID == WIA_DPS_DOCUMENT_HANDLING_SELECT)
                {
                    prop.set_Value(FEEDER);
                    break;
                }
            }
        }
        public void SaveImagesToPdf(List<System.Drawing.Image> images, string outputPath)
        {
            iText.Layout.Element.Image image = new iText.Layout.Element.Image(ImageDataFactory.Create(images[0], null));
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outputPath));
            Document doc = new Document(pdfDoc, new iText.Kernel.Geom.PageSize(image.GetImageWidth(), image.GetImageHeight()));

            for (int i = 0; i < images.Count; i++)
            {
                image = new iText.Layout.Element.Image(ImageDataFactory.Create(images[i], null));
                pdfDoc.AddNewPage(new iText.Kernel.Geom.PageSize(image.GetImageWidth(), image.GetImageHeight()));
                image.SetFixedPosition(i + 1, 0, 0);
                doc.Add(image);
            }

            doc.Close();

            //using (PdfWriter pdfWriter = new PdfWriter(outputPath))
            //{
            //    using (PdfDocument pdf = new PdfDocument(pdfWriter))
            //    {
            //        Document document = new Document(pdf);

            //        foreach (var img in images)
            //        {
            //            try
            //            {
            //                var imgData = iText.IO.Image.ImageDataFactory.Create(img, null);
            //                var pdfImg = new iText.Layout.Element.Image(imgData).SetAutoScale(true);
            //                document.Add(pdfImg);
            //            }
            //            catch (ExternalException ex)
            //            {
            //                Console.WriteLine("Failed to save image: " + ex.Message);
            //                continue;  // Continue with the next image even if one fails
            //            }


            //        }

            //        document.Close(); // Close the document to finalize the PDF
            //    }
            //}
        }
    }

    public class ScannerInfo
    {
        public string Name { get; set; }
        public string DeviceID { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class TextBoxStreamWriter : TextWriter
    {
        private RichTextBox _output;

        public TextBoxStreamWriter(RichTextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            if (_output.InvokeRequired)
            {
                _output.Invoke(new Action<char>(Write), value);
            }
            else
            {
                _output.AppendText(value.ToString());
            }
        }

        public override void WriteLine(string value)
        {
            if (_output.InvokeRequired)
            {
                _output.Invoke(new Action<string>(WriteLine), value);
            }
            else
            {
                _output.AppendText(value + Environment.NewLine);
            }
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }

}
