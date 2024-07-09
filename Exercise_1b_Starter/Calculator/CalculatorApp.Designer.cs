namespace Calculator;

partial class CalculatorApp
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
        txtA = new TextBox();
        label1 = new Label();
        label2 = new Label();
        txtB = new TextBox();
        lblAnswer = new Label();
        button1 = new Button();
        pgBar = new ProgressBar();
        SuspendLayout();
        // 
        // txtA
        // 
        txtA.Font = new Font("Segoe UI", 24F);
        txtA.Location = new Point(93, 34);
        txtA.Name = "txtA";
        txtA.Size = new Size(163, 50);
        txtA.TabIndex = 0;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Segoe UI", 24F);
        label1.Location = new Point(46, 37);
        label1.Name = "label1";
        label1.Size = new Size(41, 45);
        label1.TabIndex = 1;
        label1.Text = "A";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Segoe UI", 24F);
        label2.Location = new Point(46, 121);
        label2.Name = "label2";
        label2.Size = new Size(38, 45);
        label2.TabIndex = 3;
        label2.Text = "B";
        // 
        // txtB
        // 
        txtB.Font = new Font("Segoe UI", 24F);
        txtB.Location = new Point(93, 118);
        txtB.Name = "txtB";
        txtB.Size = new Size(163, 50);
        txtB.TabIndex = 2;
        // 
        // lblAnswer
        // 
        lblAnswer.AutoSize = true;
        lblAnswer.Font = new Font("Segoe UI", 24F);
        lblAnswer.Location = new Point(93, 221);
        lblAnswer.Name = "lblAnswer";
        lblAnswer.Size = new Size(163, 45);
        lblAnswer.TabIndex = 4;
        lblAnswer.Text = "___________";
        // 
        // button1
        // 
        button1.FlatStyle = FlatStyle.System;
        button1.Font = new Font("Segoe UI", 100F);
        button1.Location = new Point(345, 34);
        button1.Name = "button1";
        button1.Size = new Size(197, 205);
        button1.TabIndex = 5;
        button1.Text = "+";
        button1.TextAlign = ContentAlignment.TopCenter;
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // pgBar
        // 
        pgBar.Location = new Point(46, 302);
        pgBar.Name = "pgBar";
        pgBar.Size = new Size(496, 23);
        pgBar.TabIndex = 6;
        // 
        // CalculatorApp
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(611, 350);
        Controls.Add(pgBar);
        Controls.Add(button1);
        Controls.Add(lblAnswer);
        Controls.Add(label2);
        Controls.Add(txtB);
        Controls.Add(label1);
        Controls.Add(txtA);
        Name = "CalculatorApp";
        Text = "Form1";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TextBox txtA;
    private Label label1;
    private Label label2;
    private TextBox txtB;
    private Label lblAnswer;
    private Button button1;
    private ProgressBar pgBar;
}