
namespace HeavyClient
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.logo = new System.Windows.Forms.PictureBox();
            this.departureTextbox = new System.Windows.Forms.TextBox();
            this.arrivalTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.searchButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.SuspendLayout();
            // 
            // logo
            // 
            this.logo.Image = ((System.Drawing.Image)(resources.GetObject("logo.Image")));
            this.logo.Location = new System.Drawing.Point(128, 38);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(527, 122);
            this.logo.TabIndex = 0;
            this.logo.TabStop = false;
            // 
            // departureTextbox
            // 
            this.departureTextbox.Location = new System.Drawing.Point(279, 238);
            this.departureTextbox.Name = "departureTextbox";
            this.departureTextbox.Size = new System.Drawing.Size(312, 20);
            this.departureTextbox.TabIndex = 1;
            this.departureTextbox.TextChanged += new System.EventHandler(this.departureTextbox_TextChanged);
            // 
            // arrivalTextBox
            // 
            this.arrivalTextBox.Location = new System.Drawing.Point(279, 294);
            this.arrivalTextBox.Name = "arrivalTextBox";
            this.arrivalTextBox.Size = new System.Drawing.Size(312, 20);
            this.arrivalTextBox.TabIndex = 2;
            this.arrivalTextBox.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(147, 225);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 39);
            this.label1.TabIndex = 3;
            this.label1.Text = "Departure:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(156, 288);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 27);
            this.label2.TabIndex = 4;
            this.label2.Text = "Arrival:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(279, 356);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(239, 61);
            this.searchButton.TabIndex = 5;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.arrivalTextBox);
            this.Controls.Add(this.departureTextbox);
            this.Controls.Add(this.logo);
            this.Name = "Main";
            this.Text = "Let\'s Go Biking";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.TextBox departureTextbox;
        private System.Windows.Forms.TextBox arrivalTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button searchButton;
    }
}

