namespace Chat_App
{
    partial class Chat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.InputBox = new System.Windows.Forms.TextBox();
            ChatBox = new System.Windows.Forms.RichTextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(12, 200);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(260, 20);
            this.InputBox.TabIndex = 0;
            this.InputBox.KeyDown += InputBox_KeyDown;
            this.InputBox.MaxLength = 256;
            // 
            // ChatBox
            // 
            ChatBox.Location = new System.Drawing.Point(12, 12);
            ChatBox.Name = "ChatBox";
            ChatBox.ReadOnly = true;
            ChatBox.Size = new System.Drawing.Size(260, 182);
            ChatBox.TabIndex = 1;
            ChatBox.Text = "";
            ChatBox.KeyDown += ChatBox_KeyDown;
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(12, 226);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(260, 23);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(ChatBox);
            this.Controls.Add(this.InputBox);
            this.Name = "Chat";
            this.Text = "Chat App | C# Client";
            this.Load += new System.EventHandler(this.Chat_Load);
            this.FormClosing += this.Chat_FormClosing;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox InputBox;
        private static System.Windows.Forms.RichTextBox ChatBox;
        private System.Windows.Forms.Button SendButton;
    }
}

