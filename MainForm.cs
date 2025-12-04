using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using WinVerifyTrust.Custom_Controls;

namespace WinVerifyTrust
{
    public class MainForm : Form
    {
        private TextBox txtFilePath;
        private ModernButton btnBrowse;
        private ModernButton btnVerify;
        private Panel pnlResult;
        private Label lblStatus;
        private Label lblTrustStatus;
        private ModernGroupBox grpCertificateDetails;
        private Label lblPublisher;
        private Label lblPublisherValue;
        private Label lblIssuer;
        private Label lblIssuerValue;
        private Label lblValidFrom;
        private Label lblValidFromValue;
        private Label lblValidTo;
        private Label lblValidToValue;
        private Label lblSerialNumber;
        private Label lblSerialNumberValue;
        private Label lblThumbprint;
        private Label lblThumbprintValue;
        private RichTextBox txtDetails;
        private Panel progressPanel;
        private PictureBox picStatus;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Win Verify Trust - Digital Signature Checker";
            this.Size = new Size(950, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Font = new Font("Segoe UI", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Header Panel with gradient
            Panel pnlHeader = new GradientPanel
            {
                Dock = DockStyle.Top,
                Height = 110,
                GradientColor1 = Color.FromArgb(33, 97, 140),
                GradientColor2 = Color.FromArgb(52, 152, 219)
            };

            Label lblTitle = new()
            {
                Text = "Digital Signature Verifier",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(20, 20)
            };

            Label lblSubtitle = new()
            {
                Text = "Authenticode integrity & trust validation using WinVerifyTrust",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.WhiteSmoke,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(20, lblTitle.Bottom + 20)
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // File Selection Panel
            Panel pnlFile = new()
            {
                Location = new Point(0, 100),
                Size = new Size(950, 100),
                BackColor = Color.White,
                Padding = new Padding(25, 20, 25, 20)
            };

            Label lblFile = new()
            {
                Text = "File to Verify:",
                Location = new Point(25, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            txtFilePath = new TextBox
            {
                Location = new Point(25, 45),
                Width = 690,
                Height = 30,
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle
            };

            btnBrowse = new ModernButton
            {
                Text = "📁 Browse",
                Location = new Point(725, 43),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White
            };
            btnBrowse.Click += BtnBrowse_Click;

            btnVerify = new ModernButton
            {
                Text = "✓ Verify",
                Location = new Point(825, 43),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White
            };
            btnVerify.Click += BtnVerify_Click;

            pnlFile.Controls.AddRange(new Control[] { lblFile, txtFilePath, btnBrowse, btnVerify });

            // Progress Panel
            progressPanel = new Panel
            {
                Location = new Point(25, 220),
                Size = new Size(890, 50),
                BackColor = Color.FromArgb(241, 196, 15),
                Visible = false
            };

            Label lblProgress = new()
            {
                Text = "⏳ Verifying signature...",
                Location = new Point(15, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White
            };
            progressPanel.Controls.Add(lblProgress);

            // Result Panel with rounded corners
            pnlResult = new RoundedPanel
            {
                Location = new Point(25, 220),
                Size = new Size(890, 100),
                BorderRadius = 10,
                Visible = false
            };

            picStatus = new PictureBox
            {
                Location = new Point(20, 20),
                Size = new Size(60, 60),
                SizeMode = PictureBoxSizeMode.CenterImage
            };

            lblStatus = new Label
            {
                Location = new Point(95, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold)
            };

            lblTrustStatus = new Label
            {
                Location = new Point(95, 50),
                Width = 750,
                Height = 40,
                Font = new Font("Segoe UI", 10F)
            };

            pnlResult.Controls.AddRange(new Control[] { picStatus, lblStatus, lblTrustStatus });

            // Certificate Details
            grpCertificateDetails = new ModernGroupBox
            {
                Text = "Certificate Information",
                Location = new Point(25, 335),
                Size = new Size(890, 230),
                Visible = false
            };

            int labelX = 20, valueX = 150;
            int startY = 35, spacing = 30;

            lblPublisher = CreateLabel("Publisher:", labelX, startY, true);
            lblPublisherValue = CreateLabel("", valueX, startY, false, 280);

            lblIssuer = CreateLabel("Issued By:", labelX, startY + spacing, true);
            lblIssuerValue = CreateLabel("", valueX, startY + spacing, false, 280);

            lblValidFrom = CreateLabel("Valid From:", labelX, startY + (spacing * 2), true);
            lblValidFromValue = CreateLabel("", valueX, startY + (spacing * 2), false, 280);

            lblValidTo = CreateLabel("Valid To:", labelX, startY + (spacing * 3), true);
            lblValidToValue = CreateLabel("", valueX, startY + (spacing * 3), false, 280);

            lblSerialNumber = CreateLabel("Serial Number:", labelX, startY + (spacing * 4), true);
            lblSerialNumberValue = CreateLabel("", valueX, startY + (spacing * 4), false, 670);

            lblThumbprint = CreateLabel("Thumbprint:", labelX, startY + (spacing * 5), true);
            lblThumbprintValue = CreateLabel("", valueX, startY + (spacing * 5), false, 670);

            grpCertificateDetails.Controls.AddRange(new Control[] {
                lblPublisher, lblPublisherValue,
                lblIssuer, lblIssuerValue,
                lblValidFrom, lblValidFromValue,
                lblValidTo, lblValidToValue,
                lblSerialNumber, lblSerialNumberValue,
                lblThumbprint, lblThumbprintValue
            });

            // Details Section
            Label lblDetailsHeader = new()
            {
                Text = "📋 Additional Details",
                Location = new Point(25, 575),
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            txtDetails = new RichTextBox
            {
                Location = new Point(25, 605),
                Size = new Size(890, 90),
                ReadOnly = true,
                BackColor = Color.White,
                Font = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.FixedSingle
            };

            this.Controls.AddRange(new Control[] {
                pnlHeader, pnlFile, progressPanel, pnlResult,
                grpCertificateDetails, lblDetailsHeader, txtDetails
            });
        }

        private Label CreateLabel(string text, int x, int y, bool bold, int width = 120)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = false,
                Size = new Size(width, 22),
                Font = new Font("Segoe UI", 9F, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = bold ? Color.FromArgb(52, 73, 94) : Color.FromArgb(127, 140, 141)
            };
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new();
            ofd.Filter = "Executable Files (*.exe;*.dll;*.sys)|*.exe;*.dll;*.sys|Cabinet Files (*.cab)|*.cab|MSI Files (*.msi)|*.msi|All Files (*.*)|*.*";
            ofd.Title = "Select File to Verify Digital Signature";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = ofd.FileName;
            }
        }

        private async void BtnVerify_Click(object sender, EventArgs e)
        {
            string filePath = txtFilePath.Text.Trim();

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Please select a file to verify.", "No File Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(filePath))
            {
                MessageBox.Show("The selected file does not exist.", "File Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnVerify.Enabled = false;
            btnBrowse.Enabled = false;
            progressPanel.Visible = true;
            pnlResult.Visible = false;
            grpCertificateDetails.Visible = false;
            txtDetails.Clear();

            await System.Threading.Tasks.Task.Run(() =>
            {
                SignatureVerifier verifier = new();
                VerificationResult result = verifier.VerifyFile(filePath);

                this.Invoke((MethodInvoker)delegate
                {
                    DisplayResults(result);
                    btnVerify.Enabled = true;
                    btnBrowse.Enabled = true;
                    progressPanel.Visible = false;
                });
            });
        }

        private void DisplayResults(VerificationResult result)
        {
            pnlResult.Visible = true;
            lblStatus.Text = result.Status;
            lblTrustStatus.Text = result.TrustStatus;

            // Color coding and icons
            if (result.IsTrusted)
            {
                pnlResult.BackColor = Color.FromArgb(212, 239, 223);
                lblStatus.ForeColor = Color.FromArgb(39, 174, 96);
                picStatus.Image = CreateStatusIcon("✓", Color.FromArgb(39, 174, 96));
            }
            else
            {
                pnlResult.BackColor = Color.FromArgb(248, 215, 218);
                lblStatus.ForeColor = Color.FromArgb(220, 53, 69);
                picStatus.Image = CreateStatusIcon("✗", Color.FromArgb(220, 53, 69));
            }

            // Display certificate details
            if (result.Certificate != null)
            {
                grpCertificateDetails.Visible = true;

                lblPublisherValue.Text = ExtractCommonName(result.Certificate.Subject);
                lblIssuerValue.Text = ExtractCommonName(result.Certificate.Issuer);
                lblValidFromValue.Text = result.Certificate.NotBefore.ToString("yyyy-MM-dd HH:mm:ss");
                lblValidToValue.Text = result.Certificate.NotAfter.ToString("yyyy-MM-dd HH:mm:ss");

                // Check if expired
                if (result.Certificate.NotAfter < DateTime.Now)
                {
                    lblValidToValue.Text += " ⚠️ EXPIRED";
                    lblValidToValue.ForeColor = Color.Red;
                }
                else
                {
                    lblValidToValue.ForeColor = Color.FromArgb(127, 140, 141);
                }

                lblSerialNumberValue.Text = result.Certificate.SerialNumber;
                lblThumbprintValue.Text = result.Certificate.Thumbprint;

                // Additional details
                txtDetails.Clear();
                txtDetails.AppendText($"File: {result.Certificate.Subject}\n");
                txtDetails.AppendText($"Certificate Version: {result.Certificate.Version}\n");
                txtDetails.AppendText($"Signature Algorithm: {result.Certificate.SignatureAlgorithm.FriendlyName}\n");
                txtDetails.AppendText($"Public Key: {result.Certificate.PublicKey.Oid.FriendlyName} ({result.Certificate.PublicKey.Key.KeySize} bits)\n");

                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    txtDetails.AppendText($"\n{result.ErrorMessage}");
                }
            }
            else
            {
                grpCertificateDetails.Visible = false;
                txtDetails.AppendText("No certificate information available.\n");
                txtDetails.AppendText("The file may not be digitally signed.\n\n");

                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    txtDetails.AppendText($"Details: {result.ErrorMessage}");
                }
            }
        }

        private static string ExtractCommonName(string distinguishedName)
        {
            string[] parts = distinguishedName.Split(',');
            foreach (string part in parts)
            {
                string trimmed = part.Trim();
                if (trimmed.StartsWith("CN="))
                {
                    return trimmed[3..];
                }
            }
            return distinguishedName;
        }

        private Bitmap CreateStatusIcon(string symbol, Color color)
        {
            Bitmap bmp = new(60, 60);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                using Font font = new("Segoe UI", 32, FontStyle.Bold);
                using Brush brush = new SolidBrush(color);
                StringFormat sf = new()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(symbol, font, brush, new RectangleF(0, 0, 60, 60), sf);
            }
            return bmp;
        }
    }
}
