namespace Client
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SendButton = new System.Windows.Forms.Button();
            this.listOnServer = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ChoiceFile = new System.Windows.Forms.ToolStripMenuItem();
            this.Send_ReceivePrBar = new System.Windows.Forms.ProgressBar();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SendButton
            // 
            this.SendButton.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.SendButton.Location = new System.Drawing.Point(262, 361);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(276, 67);
            this.SendButton.TabIndex = 0;
            this.SendButton.Text = "Отправить файл";
            this.SendButton.UseVisualStyleBackColor = true;
            // 
            // listOnServer
            // 
            this.listOnServer.FormattingEnabled = true;
            this.listOnServer.ItemHeight = 20;
            this.listOnServer.Location = new System.Drawing.Point(12, 32);
            this.listOnServer.Name = "listOnServer";
            this.listOnServer.Size = new System.Drawing.Size(359, 304);
            this.listOnServer.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChoiceFile});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ChoiceFile
            // 
            this.ChoiceFile.Name = "ChoiceFile";
            this.ChoiceFile.Size = new System.Drawing.Size(219, 24);
            this.ChoiceFile.Text = "Выбрать файл для отправки";
            this.ChoiceFile.Click += new System.EventHandler(this.ChoiceFile_Click);
            // 
            // Send_ReceivePrBar
            // 
            this.Send_ReceivePrBar.Location = new System.Drawing.Point(396, 32);
            this.Send_ReceivePrBar.Name = "Send_ReceivePrBar";
            this.Send_ReceivePrBar.Size = new System.Drawing.Size(375, 34);
            this.Send_ReceivePrBar.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Send_ReceivePrBar);
            this.Controls.Add(this.listOnServer);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ClientForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button SendButton;
        private ListBox listOnServer;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem ChoiceFile;
        private ProgressBar Send_ReceivePrBar;
    }
}