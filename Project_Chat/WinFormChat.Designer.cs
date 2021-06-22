
namespace Project_Chat
{
    partial class WinFormChat
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
            this.btnSend = new System.Windows.Forms.Button();
            this.listChatBox = new System.Windows.Forms.ListBox();
            this.textMsg = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(651, 343);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(137, 95);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "보내기";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // listChatBox
            // 
            this.listChatBox.FormattingEnabled = true;
            this.listChatBox.ItemHeight = 20;
            this.listChatBox.Location = new System.Drawing.Point(12, 12);
            this.listChatBox.Name = "listChatBox";
            this.listChatBox.ScrollAlwaysVisible = true;
            this.listChatBox.Size = new System.Drawing.Size(776, 324);
            this.listChatBox.TabIndex = 1;
            this.listChatBox.UseTabStops = false;
            // 
            // textMsg
            // 
            this.textMsg.Location = new System.Drawing.Point(12, 377);
            this.textMsg.Name = "textMsg";
            this.textMsg.Size = new System.Drawing.Size(633, 26);
            this.textMsg.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textMsg);
            this.Controls.Add(this.listChatBox);
            this.Controls.Add(this.btnSend);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ListBox listChatBox;
        private System.Windows.Forms.TextBox textMsg;
    }
}