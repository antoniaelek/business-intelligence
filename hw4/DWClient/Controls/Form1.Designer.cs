namespace DWClient.Controls
{
    partial class Form1
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
            this.splitContainerInner = new System.Windows.Forms.SplitContainer();
            this.splitContainerOuter = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInner)).BeginInit();
            this.splitContainerInner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOuter)).BeginInit();
            this.splitContainerOuter.Panel1.SuspendLayout();
            this.splitContainerOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerInner
            // 
            this.splitContainerInner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerInner.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerInner.IsSplitterFixed = true;
            this.splitContainerInner.Location = new System.Drawing.Point(3, 3);
            this.splitContainerInner.Name = "splitContainerInner";
            this.splitContainerInner.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainerInner.Size = new System.Drawing.Size(502, 483);
            this.splitContainerInner.SplitterDistance = 318;
            this.splitContainerInner.TabIndex = 0;
            // 
            // splitContainerOuter
            // 
            this.splitContainerOuter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerOuter.Location = new System.Drawing.Point(12, 12);
            this.splitContainerOuter.Name = "splitContainerOuter";
            // 
            // splitContainerOuter.Panel1
            // 
            this.splitContainerOuter.Panel1.Controls.Add(this.splitContainerInner);
            this.splitContainerOuter.Size = new System.Drawing.Size(908, 489);
            this.splitContainerOuter.SplitterDistance = 508;
            this.splitContainerOuter.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 513);
            this.Controls.Add(this.splitContainerOuter);
            this.Name = "Form1";
            this.Text = "DW Client";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInner)).EndInit();
            this.splitContainerInner.ResumeLayout(false);
            this.splitContainerOuter.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOuter)).EndInit();
            this.splitContainerOuter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.SplitContainer splitContainerInner;
        internal System.Windows.Forms.SplitContainer splitContainerOuter;
    }
}

