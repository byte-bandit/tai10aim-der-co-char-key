namespace ServerSoftware
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
			this.gb_cc = new System.Windows.Forms.GroupBox();
			this.btnStartServer = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lb_cp = new System.Windows.Forms.ListBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.rtb_log = new System.Windows.Forms.RichTextBox();
			this.onlineIndicator = new System.Windows.Forms.Panel();
			this.gb_cc.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// gb_cc
			// 
			this.gb_cc.Controls.Add(this.onlineIndicator);
			this.gb_cc.Controls.Add(this.btnStartServer);
			this.gb_cc.Location = new System.Drawing.Point(12, 12);
			this.gb_cc.Name = "gb_cc";
			this.gb_cc.Size = new System.Drawing.Size(200, 51);
			this.gb_cc.TabIndex = 0;
			this.gb_cc.TabStop = false;
			this.gb_cc.Text = "Server Controls";
			// 
			// btnStartServer
			// 
			this.btnStartServer.Location = new System.Drawing.Point(35, 19);
			this.btnStartServer.Name = "btnStartServer";
			this.btnStartServer.Size = new System.Drawing.Size(159, 23);
			this.btnStartServer.TabIndex = 0;
			this.btnStartServer.Text = "BTN_START_SERVER";
			this.btnStartServer.UseVisualStyleBackColor = true;
			this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lb_cp);
			this.groupBox2.Location = new System.Drawing.Point(12, 69);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(200, 366);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Connected Players";
			// 
			// lb_cp
			// 
			this.lb_cp.FormattingEnabled = true;
			this.lb_cp.Location = new System.Drawing.Point(6, 19);
			this.lb_cp.Name = "lb_cp";
			this.lb_cp.Size = new System.Drawing.Size(188, 329);
			this.lb_cp.TabIndex = 0;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.rtb_log);
			this.groupBox3.Location = new System.Drawing.Point(218, 12);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(439, 423);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Log";
			// 
			// rtb_log
			// 
			this.rtb_log.Location = new System.Drawing.Point(6, 19);
			this.rtb_log.Name = "rtb_log";
			this.rtb_log.Size = new System.Drawing.Size(427, 389);
			this.rtb_log.TabIndex = 0;
			this.rtb_log.Text = "";
			// 
			// onlineIndicator
			// 
			this.onlineIndicator.Location = new System.Drawing.Point(6, 19);
			this.onlineIndicator.Name = "onlineIndicator";
			this.onlineIndicator.Size = new System.Drawing.Size(23, 23);
			this.onlineIndicator.TabIndex = 2;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(669, 447);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.gb_cc);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.gb_cc.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gb_cc;
		private System.Windows.Forms.Button btnStartServer;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ListBox lb_cp;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RichTextBox rtb_log;
		private System.Windows.Forms.Panel onlineIndicator;
	}
}

