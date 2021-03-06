﻿namespace FtdiFifo
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
            this.identifyButton = new System.Windows.Forms.Button();
            this.deviveComboBox = new System.Windows.Forms.ComboBox();
            this.openButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.transmitButton = new System.Windows.Forms.Button();
            this.measureButton = new System.Windows.Forms.Button();
            this.speedLabel = new System.Windows.Forms.Label();
            this.receiveContButton = new System.Windows.Forms.Button();
            this.testReliabilityButton = new System.Windows.Forms.Button();
            this.iterCountLabel = new System.Windows.Forms.Label();
            this.readSequenceButton = new System.Windows.Forms.Button();
            this.sendDataButton = new System.Windows.Forms.Button();
            this.fileTextBox = new System.Windows.Forms.TextBox();
            this.selectFileButton = new System.Windows.Forms.Button();
            this.sendFileDataButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // identifyButton
            // 
            this.identifyButton.Location = new System.Drawing.Point(139, 3);
            this.identifyButton.Name = "identifyButton";
            this.identifyButton.Size = new System.Drawing.Size(77, 26);
            this.identifyButton.TabIndex = 0;
            this.identifyButton.Text = "Identify";
            this.identifyButton.UseVisualStyleBackColor = true;
            this.identifyButton.Click += new System.EventHandler(this.identifyButton_Click);
            // 
            // deviveComboBox
            // 
            this.deviveComboBox.FormattingEnabled = true;
            this.deviveComboBox.Location = new System.Drawing.Point(12, 7);
            this.deviveComboBox.Name = "deviveComboBox";
            this.deviveComboBox.Size = new System.Drawing.Size(121, 21);
            this.deviveComboBox.TabIndex = 1;
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(222, 3);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 26);
            this.openButton.TabIndex = 2;
            this.openButton.Text = "Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(222, 35);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // transmitButton
            // 
            this.transmitButton.Location = new System.Drawing.Point(141, 35);
            this.transmitButton.Name = "transmitButton";
            this.transmitButton.Size = new System.Drawing.Size(75, 23);
            this.transmitButton.TabIndex = 4;
            this.transmitButton.Text = "Transmit";
            this.transmitButton.UseVisualStyleBackColor = true;
            this.transmitButton.Click += new System.EventHandler(this.transmitButton_Click);
            // 
            // measureButton
            // 
            this.measureButton.Location = new System.Drawing.Point(141, 64);
            this.measureButton.Name = "measureButton";
            this.measureButton.Size = new System.Drawing.Size(75, 23);
            this.measureButton.TabIndex = 5;
            this.measureButton.Text = "Measure speed";
            this.measureButton.UseVisualStyleBackColor = true;
            this.measureButton.Click += new System.EventHandler(this.measureButton_Click);
            // 
            // speedLabel
            // 
            this.speedLabel.AutoSize = true;
            this.speedLabel.Location = new System.Drawing.Point(230, 70);
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(0, 13);
            this.speedLabel.TabIndex = 6;
            // 
            // receiveContButton
            // 
            this.receiveContButton.Location = new System.Drawing.Point(142, 93);
            this.receiveContButton.Name = "receiveContButton";
            this.receiveContButton.Size = new System.Drawing.Size(155, 23);
            this.receiveContButton.TabIndex = 7;
            this.receiveContButton.Text = "Receive continuously";
            this.receiveContButton.UseVisualStyleBackColor = true;
            this.receiveContButton.Click += new System.EventHandler(this.receiveContButton_Click);
            // 
            // testReliabilityButton
            // 
            this.testReliabilityButton.Location = new System.Drawing.Point(142, 122);
            this.testReliabilityButton.Name = "testReliabilityButton";
            this.testReliabilityButton.Size = new System.Drawing.Size(155, 23);
            this.testReliabilityButton.TabIndex = 8;
            this.testReliabilityButton.Text = "Test reliability";
            this.testReliabilityButton.UseVisualStyleBackColor = true;
            this.testReliabilityButton.Click += new System.EventHandler(this.testReliabilityButton_Click);
            // 
            // iterCountLabel
            // 
            this.iterCountLabel.AutoSize = true;
            this.iterCountLabel.Location = new System.Drawing.Point(98, 127);
            this.iterCountLabel.Name = "iterCountLabel";
            this.iterCountLabel.Size = new System.Drawing.Size(0, 13);
            this.iterCountLabel.TabIndex = 9;
            // 
            // readSequenceButton
            // 
            this.readSequenceButton.Location = new System.Drawing.Point(42, 34);
            this.readSequenceButton.Name = "readSequenceButton";
            this.readSequenceButton.Size = new System.Drawing.Size(91, 23);
            this.readSequenceButton.TabIndex = 10;
            this.readSequenceButton.Text = "Read sequence";
            this.readSequenceButton.UseVisualStyleBackColor = true;
            this.readSequenceButton.Click += new System.EventHandler(this.readSequenceButton_Click);
            // 
            // sendDataButton
            // 
            this.sendDataButton.Location = new System.Drawing.Point(142, 151);
            this.sendDataButton.Name = "sendDataButton";
            this.sendDataButton.Size = new System.Drawing.Size(75, 23);
            this.sendDataButton.TabIndex = 11;
            this.sendDataButton.Text = "Send data";
            this.sendDataButton.UseVisualStyleBackColor = true;
            this.sendDataButton.Click += new System.EventHandler(this.sendDataButton_Click);
            // 
            // fileTextBox
            // 
            this.fileTextBox.Location = new System.Drawing.Point(12, 189);
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.Size = new System.Drawing.Size(216, 20);
            this.fileTextBox.TabIndex = 12;
            // 
            // selectFileButton
            // 
            this.selectFileButton.Location = new System.Drawing.Point(234, 186);
            this.selectFileButton.Name = "selectFileButton";
            this.selectFileButton.Size = new System.Drawing.Size(75, 23);
            this.selectFileButton.TabIndex = 13;
            this.selectFileButton.Text = "Select file";
            this.selectFileButton.UseVisualStyleBackColor = true;
            this.selectFileButton.Click += new System.EventHandler(this.selectFileButton_Click);
            // 
            // sendFileDataButton
            // 
            this.sendFileDataButton.Location = new System.Drawing.Point(12, 215);
            this.sendFileDataButton.Name = "sendFileDataButton";
            this.sendFileDataButton.Size = new System.Drawing.Size(86, 23);
            this.sendFileDataButton.TabIndex = 14;
            this.sendFileDataButton.Text = "Send from file";
            this.sendFileDataButton.UseVisualStyleBackColor = true;
            this.sendFileDataButton.Click += new System.EventHandler(this.sendFileDataButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 277);
            this.Controls.Add(this.sendFileDataButton);
            this.Controls.Add(this.selectFileButton);
            this.Controls.Add(this.fileTextBox);
            this.Controls.Add(this.sendDataButton);
            this.Controls.Add(this.readSequenceButton);
            this.Controls.Add(this.iterCountLabel);
            this.Controls.Add(this.testReliabilityButton);
            this.Controls.Add(this.receiveContButton);
            this.Controls.Add(this.speedLabel);
            this.Controls.Add(this.measureButton);
            this.Controls.Add(this.transmitButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.deviveComboBox);
            this.Controls.Add(this.identifyButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button identifyButton;
        private System.Windows.Forms.ComboBox deviveComboBox;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button transmitButton;
        private System.Windows.Forms.Button measureButton;
        private System.Windows.Forms.Label speedLabel;
        private System.Windows.Forms.Button receiveContButton;
        private System.Windows.Forms.Button testReliabilityButton;
        private System.Windows.Forms.Label iterCountLabel;
        private System.Windows.Forms.Button readSequenceButton;
        private System.Windows.Forms.Button sendDataButton;
        private System.Windows.Forms.TextBox fileTextBox;
        private System.Windows.Forms.Button selectFileButton;
        private System.Windows.Forms.Button sendFileDataButton;
    }
}

