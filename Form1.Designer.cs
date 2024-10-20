namespace GutenbergAuthorBookDownloader
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.textBoxAuthorName = new System.Windows.Forms.TextBox();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxAuthorName
            // 
            this.textBoxAuthorName.Location = new System.Drawing.Point(12, 12);
            this.textBoxAuthorName.Size = new System.Drawing.Size(400, 22);
            this.textBoxAuthorName.Text = "Введите имя и фамилию автора"; 
            this.textBoxAuthorName.GotFocus += (s, e) => { if (textBoxAuthorName.Text == "Введите имя и фамилию автора") textBoxAuthorName.Text = ""; };
            this.textBoxAuthorName.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(textBoxAuthorName.Text)) textBoxAuthorName.Text = "Введите имя и фамилию автора"; };
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(420, 10);
            this.buttonDownload.Size = new System.Drawing.Size(75, 25);
            this.buttonDownload.Text = "Скачать";
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(507, 50);
            this.Controls.Add(this.buttonDownload);
            this.Controls.Add(this.textBoxAuthorName);
            this.Name = "Form1";
            this.Text = "Скачивание книг автора из Гутенберга";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox textBoxAuthorName;
        private System.Windows.Forms.Button buttonDownload;
    }
}
