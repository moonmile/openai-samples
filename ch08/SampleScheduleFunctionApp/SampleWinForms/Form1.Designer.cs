namespace SampleWinForms
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
            textInput = new TextBox();
            textOutput = new TextBox();
            buttonSend = new Button();
            SuspendLayout();
            // 
            // textInput
            // 
            textInput.Location = new Point(12, 12);
            textInput.Name = "textInput";
            textInput.Size = new Size(438, 31);
            textInput.TabIndex = 0;
            // 
            // textOutput
            // 
            textOutput.Location = new Point(12, 49);
            textOutput.Multiline = true;
            textOutput.Name = "textOutput";
            textOutput.Size = new Size(438, 288);
            textOutput.TabIndex = 1;
            // 
            // buttonSend
            // 
            buttonSend.Location = new Point(456, 12);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(163, 72);
            buttonSend.TabIndex = 2;
            buttonSend.Text = "送信";
            buttonSend.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(633, 358);
            Controls.Add(buttonSend);
            Controls.Add(textOutput);
            Controls.Add(textInput);
            Name = "Form1";
            Text = "AIスケジューラー";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textInput;
        private TextBox textOutput;
        private Button buttonSend;
    }
}
