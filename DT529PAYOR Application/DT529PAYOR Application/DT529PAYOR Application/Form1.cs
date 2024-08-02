using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Payor_Recipient_Information
{
    public partial class Payor_Recipient_Information : Form
    {
        // Used with browseButton and processButton functionality
        private string selectedFile = "";

        // String for saving the custom save location from the user
        private string customSaveLocation = "";

        // Variable to store file name of documentation file that is opened with the "Help" button
        private const string helpDocumentName = "DT529PAYORProcessingInstructions.PDF";


        // Create gradient for background
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle,
                                                                       System.Drawing.Color.LightBlue,  // Start color
                                                                       System.Drawing.Color.White,   // End color
                                                                       90F))             // Angle in degrees
            {
                ColorBlend colorBlend = new ColorBlend();
                colorBlend.Positions = new[] { 0f, 0.5f, 1f };
                colorBlend.Colors = new[] { System.Drawing.Color.LightBlue, System.Drawing.Color.White, System.Drawing.Color.White };
                brush.InterpolationColors = colorBlend;
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        public Payor_Recipient_Information()
        {
            InitializeComponent();

            // Redraws gradient when window is resized
            this.ResizeRedraw = true;
        }

        private void Payor_Recipient_Information_Load(object sender, EventArgs e)
        {

            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form_DragEnter);
            this.DragDrop += new DragEventHandler(Form_DragDrop);

        }

        // Checks if dragged data is a file, sets drag effect to copy
        private void Form_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        // Gets dropped file, checks if it's a LIS, then handles
        private void Form_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                string filePath = files[0]; // Get the first file if multiple files are dropped
                if (System.IO.Path.GetExtension(filePath).Equals(".lis", StringComparison.OrdinalIgnoreCase))
                {
                    // Handle the LIS file
                    selectedFile = filePath;
                    // Update UI to show the selected file, if needed
                    textBox1.Text = selectedFile;
                }
                else
                {
                    MessageBox.Show("Please drop an LIS file.", "Invalid File Type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // browseButton_Click opens the file explorer for selecting a txt document.
        // The selected document's path is then assigned as a string and displayed in the textbox.
        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Filter = "LIS files (*.lis)|*.lis|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    selectedFile = openFileDialog1.FileName;
                    textBox1.Text = selectedFile;

                    // The following two lines force the textbox to show the end of the
                    // file path and not the beginning (better for readability).
                    textBox1.SelectionStart = textBox1.Text.Length;
                    textBox1.ScrollToCaret();
                }
            }
        }

        // Define a custom event args class for message boxes after completing processing
        public class ProcessingCompletedEventArgs : EventArgs
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }

        private event EventHandler<ProcessingCompletedEventArgs> ProcessingCompleted;

        // Trigger events when Process button clicked, also perform operations
        private async void ProcessButton_Click(object sender, EventArgs e)
        {
            // Disable the button and change its color
            processButton.Enabled = false;
            processButton.BackColor = System.Drawing.Color.Gray;

            // Show the loading bar
            loadingCircle.Visible = true;

            // Subscribe to the ProcessingCompleted event
            ProcessingCompleted += OnProcessingCompleted;

            try
            {
                // Perform the processing operation asynchronously
                await Task.Run(() => PerformProcessing());
            }
            finally
            {
                // Unsubscribe from the event
                ProcessingCompleted -= OnProcessingCompleted;

                // Re-enable the button and restore its color
                processButton.Enabled = true;
                processButton.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        // Main method for processing data, method is called inside processButton_Click() above
        private void PerformProcessing()
        {
            if (string.IsNullOrEmpty(selectedFile))
            {
                RaiseProcessingCompleted(false, "Please select a file first.");
                return;
            }

            try
            {
                // Read and process the LIS file
                List<Dictionary<string, string>> extractedData = ReadLisFile(selectedFile);

                string excelFilePath;
                if (!string.IsNullOrEmpty(customSaveLocation))
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(selectedFile) + ".xlsx";
                    excelFilePath = System.IO.Path.Combine(customSaveLocation, fileName);
                }
                else
                {
                    excelFilePath = System.IO.Path.ChangeExtension(selectedFile, ".xlsx");
                }
                CreateCsvFile(extractedData, excelFilePath);

                RaiseProcessingCompleted(true, $"LIS File Processed Successfully! \n\nExcel file created at: {excelFilePath}");
            }
            catch (Exception ex)
            {
                RaiseProcessingCompleted(false, $"An Error Occurred: \n\n{ex.Message}");
            }
        }

        private void RaiseProcessingCompleted(bool success, string message)
        {
            ProcessingCompleted?.Invoke(this, new ProcessingCompletedEventArgs
            {
                Success = success,
                Message = message
            });
        }

        private void OnProcessingCompleted(object sender, ProcessingCompletedEventArgs e)
        {
            // Ensure we're on the UI thread
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => OnProcessingCompleted(sender, e)));
                return;
            }

            // Hide the loading circle
            loadingCircle.Visible = false;

            // Show the appropriate message box
            if (e.Success)
            {
                MessageBox.Show(e.Message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<Dictionary<string, string>> ReadLisFile(string filePath)
        {
            List<Dictionary<string, string>> extractedData = new List<Dictionary<string, string>>();
            string[] lines = File.ReadAllLines(filePath);

            // Regex pattern for Address2
            Regex address2Pattern = new Regex(@"(?i)\b(P\.?\s?O\.?\s*BOX)\.?\s*#?\s*\w+");
            // Regex pattern for State (two uppercase letters with a space before and after)
            Regex statePattern = new Regex(@"\s[A-Z]{2}\s");

            foreach (string line in lines)
            {
                Dictionary<string, string> entry = new Dictionary<string, string>
                {
                    ["Recipient First"] = SafeSubstring(line, 169, 40),
                    ["Recipient Last"] = "",  // Always empty
                    ["RecipientCompany"] = SafeSubstring(line, 209, 40),
                    ["Recipient Address1"] = SafeSubstring(line, 249, 130),
                    ["Recipient Address2"] = SafeSubstring(line, 379, 130),
                    ["Internal Reference"] = ""  // Always empty
                };

                // Process City, State, and Zip
                string cityStateZip = SafeSubstring(line, 509, 130).Trim();

                Match stateMatch = statePattern.Match(cityStateZip);
                if (stateMatch.Success)
                {
                    int stateIndex = stateMatch.Index;

                    entry["Recipient City"] = cityStateZip.Substring(0, stateIndex).Trim();
                    entry["Recipient State"] = stateMatch.Value.Trim();
                    entry["Recipient Zip"] = cityStateZip.Substring(stateIndex + 3).Trim();
                }
                else
                {
                    // If no state is found, put everything in City and leave State and Zip empty
                    entry["Recipient City"] = cityStateZip;
                    entry["Recipient State"] = "";
                    entry["Recipient Zip"] = "";
                }

                // Get Reference number
                entry["Reference"] = SafeSubstring(line, 139, 30);

                extractedData.Add(entry);
            }

            return extractedData;
        }

        // Helper method to safely extract substrings
        private string SafeSubstring(string input, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            if (startIndex >= input.Length) return string.Empty;

            int availableLength = Math.Min(length, input.Length - startIndex);
            return input.Substring(startIndex, availableLength).Trim();
        }


        /*
         * 
        private void WriteDebugTextFile(List<Dictionary<string, string>> extractedData, string filePath)
        {
            string textFilePath = System.IO.Path.ChangeExtension(filePath, ".txt");
            using (StreamWriter writer = new StreamWriter(textFilePath))
            {
                for (int i = 0; i < extractedData.Count; i++)
                {
                    writer.WriteLine($"--- Page {i + 1} ---");
                    foreach (var kvp in extractedData[i])
                    {
                        writer.WriteLine($"{kvp.Key}: {kvp.Value}");
                    }
                    writer.WriteLine();
                }
            }
        }
        *
        */

        private void CreateCsvFile(List<Dictionary<string, string>> data, string filePath)
        {
            // Ensure the file path ends with .csv
            if (!filePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                filePath = Path.ChangeExtension(filePath, ".csv");
            }

            // Define the headers in the desired order
            string[] headers = { "Recipient First", "Recipient Last", "RecipientCompany", "Recipient Address1", "Recipient Address2", "Recipient City", "Recipient State", "Recipient Zip", "Reference", "Internal Reference" };

            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Write headers
                writer.WriteLine(string.Join(",", headers.Select(EscapeCsvField)));

                // Write data rows
                foreach (var row in data)
                {
                    string[] fields = headers.Select(header => row.ContainsKey(header) ? row[header] : "").ToArray();
                    writer.WriteLine(string.Join(",", fields.Select(EscapeCsvField)));
                }
            }
        }

        // Helper method to properly escape and quote CSV fields
        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return "";

            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                // Escape double quotes by doubling them
                field = field.Replace("\"", "\"\"");
                // Wrap the field in double quotes
                return $"\"{field}\"";
            }

            return field;
        }

        // Functionality for "Select Custom Destination" Button
        private void DestinationButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowser = new FolderBrowserDialog())
            {
                folderBrowser.Description = "Select a folder to save the Excel file";

                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    customSaveLocation = folderBrowser.SelectedPath;
                    saveLocationLabel.Text = $"Custom Save Location: {customSaveLocation}";
                }
            }
        }

        // Functionality for "Help" Button (Opens documentation when stored locally)
        private void HelpButton_Click(object sender, EventArgs e)
        {
            string helpFilePath = System.IO.Path.Combine(Application.StartupPath, helpDocumentName);

            if (File.Exists(helpFilePath))
            {
                try
                {
                    // Use ProcessStartInfo to specify how to open the file
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = helpFilePath,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening help document: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Help document not found. Please contact your system administrator.", "Document Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Functionality for "Exit" Button
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
