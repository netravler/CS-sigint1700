// Decompiled with JetBrains decompiler
// Type: sigint1700.Form1
// Assembly: sigint1700, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB900B7A-36F9-4D85-BC77-32D952F1F622
// Assembly location: C:\Users\pheintz\Desktop\sigint1700.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;
using Diagnostics = System.Diagnostics;
using fastSerial;
using System.IO;
using System.Collections;

namespace sigint1700
{
  public class Form1 : Form
  {
    private IContainer components = (IContainer) null;
    private SerialPort serialPort1;
    private ListBox lsbCommands;
    private Button btn10M;
    private Button btn20M;
    private Button btn30M;
    private Button btn15M;
    private Button btn60M;
    private Button btn80M;
    private Button btn17M;
    private Button btn40M;
    private Button btn160M;
    private Label lblBands;
    private TextBox tbxFreq;
    private Button btnUPK;
    private Button btnDOWNK;
    private Button btnDOWNM;
    private Button btnUPM;
    private Button btnMUSB;
    private Button btnLSB;
    private Button btnCW;
    private Button btnAM;
    private Button btnFM;
    private Button btn12M;
    private Button btnGO;
    private Label lblMode;
    private TextBox tbxFreq2;
    private Label label2;
    private Label label3;
    private Button button1;
    private Button button2;
    private Button button3;
    private Button button4;
    private Label label4;
    private ListBox listBox1;
    private Button button5;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem setToolStripMenuItem;
    private ToolStripMenuItem printerToolStripMenuItem;
    private ToolStripMenuItem cOMPortToolStripMenuItem;
    private Label label1;
    private TextBox textBox1;
    private Button button6;
    private ToolStripMenuItem radioToolStripMenuItem;
    private ToolStripMenuItem vX1700ToolStripMenuItem;
    private ToolStripMenuItem micom2BFToolStripMenuItem;
    private ToolStripMenuItem barrett205020502090ToolStripMenuItem;
    private ToolStripMenuItem codan9360ToolStripMenuItem;
    private ToolStripMenuItem dXSR8TToolStripMenuItem;
    private ToolStripMenuItem iCOMM802ToolStripMenuItem;
    private ToolStripMenuItem e4406AToolStripMenuItem1;
    private ToolStripMenuItem radioServiceToolStripMenuItem;
    private ToolStripMenuItem marineToolStripMenuItem;
    private ToolStripMenuItem commercialToolStripMenuItem;
    private ToolStripMenuItem hamToolStripMenuItem;
    private ToolStripMenuItem e4406AToolStripMenuItem;
    private ToolStripMenuItem simVFOToolStripMenuItem;
    private ToolStripMenuItem cOM1ToolStripMenuItem;
    private ToolStripMenuItem cOM2ToolStripMenuItem;
    private ToolStripMenuItem cOM3ToolStripMenuItem;
    private ToolStripMenuItem cOM4ToolStripMenuItem;
    private ToolStripMenuItem cOM5ToolStripMenuItem;
    private Button btnSplit;

    // rig commands
    string[,] alincoCommands = new string[,]
    {
	    {"AL~RW_RXF14212000","Set frequency (e.g. 14.212000 MHz)"},
	    {"AL~RR_RXF","Read current frequency"},
	    {"AL~RW_RFG00","RF gain0dB"},
	    {"AL~RW_RFG01","RF gain -10dB"},
	    {"AL~RW_RFG02","RF gain -20dB"},
	    {"AL~RW_RFG03","RF gain +10dB"},
	    {"AL~RR_RFG","Read current RF gain (codes as above)"},
	    {"AL~RW_RFM00","USB mode"},
	    {"AL~RW_RFM01","LSB mode"},
	    {"AL~RW_RFM02","CWU mode"},
	    {"AL~RW_RFM03","CWL mode"},
	    {"AL~RW_RFM04","AM mode"},
	    {"AL~RW_RFM05","FM mode"},
	    {"AL~RR_RFM","Read current mode (same codes as above)"},
	    {"AL~RW_PWR00","High power"},
	    {"AL~RW_PWR01","Low power"},
	    {"AL~RW_PWR02","Sub low power (QRP mode)"},
	    {"AL~RR_PWR","Read current power (codes as above)"},
	    {"AL~RW_PTT00","PTT release"},
	    {"AL~RW_PTT01","PTT press"},
	    {"AL~RR_PTT","Read current PTT status (codes as above)"},
	    {"AL~RW_NAR00","Set wide bandwidth"},
	    {"AL~RW_NAR01","Set narrow bandwidth"},
	    {"AL~RR_NAR","Read current bandwidth (codes as above)"},
	    {"AL~RW_AGC00","Set fast AGC"},
	    {"AL~RW_AGC01","Set slow AGC"},
	    {"AL~RR_AGC","Read current AGC (codes as above)"},
	    {"AL~RW_NZB00","Turn noise blanker off"},
	    {"AL~RW_NZB01","Turn noise blanker on"},
	    {"AL~RR_NZB","Read noise blanker status (codes as above)"},
	    {"AL~RW_ULT00","De-select UT/LT filter, if in SSB mode"},
	    {"AL~RW_ULT01","Activate UT/LT filter, if in USB/LSB correspondingly"},
	    {"AL~RR_ULT","Read current UT/LT status, if in SSB (codes as above)"},
	    {"AL~RS_TUN","Signal external antenna tuner to tune"},
	    {"AL~WHO","Returns radio model number"}
    };

    // set upper and lower ranges for guarding...

    public double lowerRange = 0;
    public double upperRange = 0;

    public Form1()
    {
      this.InitializeComponent();
      this.serialPort1.PortName = "COM1";
      this.serialPort1.BaudRate = 9600;
      this.serialPort1.DataBits = 8;



      // grab and display COM Ports
      string[] ports = SerialPort.GetPortNames();
      
      foreach (string portName in ports)
      {
        lsbCommands.Items.Add(portName);
      }

      // threads for serial activity


      // we are tracking the focus on the tx/rx textboxes.  As it changes we will force it to 
      // the sendToSerial function.  Note the recieve FromSerial is independent and will continually 
      // look for data arrivals and push that to the lsbcommands listbox.
      //this.MouseWheel += new MouseEventHandler(all_MouseWheel);
    }


    private void sendToSerial(object send, SerialDataReceivedEventArgs e)
    {
  
    }

    private void recieveFromSerial(object send, SerialDataReceivedEventArgs e)
    {
        byte[] buffer = new byte[100];
        Action kickoffRead = null;
        kickoffRead = delegate
        {
            serialPort1.BaseStream.BeginRead(buffer, 0, buffer.Length, delegate(IAsyncResult ar)
            {
                try
                {
                    int actualLength = serialPort1.BaseStream.EndRead(ar);
                    byte[] received = new byte[actualLength];
                    Buffer.BlockCopy(buffer, 0, received, 0, actualLength);
                    //raiseAppSerialDataEvent(received);
                }
                catch (IOException exc)
                {
                    //handleAppSerialError(exc);
                }
                kickoffRead();
            }, null);
        };
        kickoffRead();
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }
    void all_MouseWheel(object sender, MouseEventArgs e)
    {
        if (tbxFreq.Focused)
        {
            if (e.Delta > 0)
                this.tbxFreq.Text = (double.Parse(this.tbxFreq.Text) + .001).ToString();//.PadRight(6, '0');
            else
                this.tbxFreq.Text = (double.Parse(this.tbxFreq.Text) - .001).ToString();//.PadRight(6, '0');
        }

        this.lsbCommands.Items.Add((object)("TX Freq Set to: " + this.tbxFreq.Text));

        if (tbxFreq2.Focused)
        {
            if (e.Delta > 0)
                this.tbxFreq2.Text = (double.Parse(this.tbxFreq2.Text) + .001).ToString();//.PadRight(6, '0');
            else
                this.tbxFreq2.Text = (double.Parse(this.tbxFreq2.Text) - .001).ToString();//).PadRight(6, '0');
        }

        this.lsbCommands.Items.Add((object)("RX Freq Set to: " + this.tbxFreq2.Text));
    }

    private void tbxFreq_TextChanged(object sender, EventArgs e)
    {
    }

    private void btn12M_Click(object sender, EventArgs e)
    {
      this.tbxFreq.Text = "24.890";
      this.tbxFreq2.Text = this.tbxFreq.Text;
      this.lsbCommands.Items.Add((object) ("Freq Set to: " + this.tbxFreq.Text));
      // limits
      lowerRange = 24.890;
      upperRange = 24.990;
    }

    private void btn15M_Click(object sender, EventArgs e)
    {
      this.tbxFreq.Text = "21.025";
      this.tbxFreq2.Text = this.tbxFreq.Text;
      this.lsbCommands.Items.Add((object) ("Freq Set to: " + this.tbxFreq.Text));
      // limits
      lowerRange = 21.025;
      upperRange = 21.450;
    }

    private void btn17M_Click(object sender, EventArgs e)
    {
      this.tbxFreq.Text = "18.068";
      this.tbxFreq2.Text = this.tbxFreq.Text;
      this.lsbCommands.Items.Add((object) ("Freq Set to: " + this.tbxFreq.Text));
      // limits
      lowerRange = 18.068;
      upperRange = 18.168;
    }

    private void btn20M_Click(object sender, EventArgs e)
    {
      this.tbxFreq.Text = "14.025";
      this.tbxFreq2.Text = this.tbxFreq.Text;
      this.lsbCommands.Items.Add((object) ("Freq Set to: " + this.tbxFreq.Text));
      // limits
      lowerRange = 14.025;
      upperRange = 14.350;
    }

    private void btn30M_Click(object sender, EventArgs e)
    {
      this.tbxFreq.Text = "10.100";
      this.tbxFreq2.Text = this.tbxFreq.Text;
      this.lsbCommands.Items.Add((object) ("Freq Set to: " + this.tbxFreq.Text));
      lowerRange = 10.100;
      upperRange = 10.150;
    }

    private void btn40M_Click(object sender, EventArgs e)
    {
      this.tbxFreq.Text = "7.025";
      this.tbxFreq2.Text = this.tbxFreq.Text;
      this.lsbCommands.Items.Add((object) ("Freq Set to: " + this.tbxFreq.Text));
      lowerRange = 7.025;
      upperRange = 7.300;
    }

    private void btn60M_Click(object sender, EventArgs e)
    {
      this.tbxFreq.Text = "5330.5";
      this.tbxFreq2.Text = this.tbxFreq.Text;
      this.lsbCommands.Items.Add((object) ("Freq Set to: " + this.tbxFreq.Text));
      lowerRange = 5330.5;
      upperRange = 5403.5;
    }

    private void btn80M_Click(object sender, EventArgs e)
    {
      this.tbxFreq.Text = "3.525";
      this.tbxFreq2.Text = this.tbxFreq.Text;
      this.lsbCommands.Items.Add((object) ("Freq Set to: " + this.tbxFreq.Text));
      lowerRange = 3.525;
      upperRange = 4.000;
    }

    private void btn160M_Click(object sender, EventArgs e)
    {
      this.tbxFreq.Text = "1.800";
      this.tbxFreq2.Text = this.tbxFreq.Text;
      this.lsbCommands.Items.Add((object) ("Freq Set to: " + this.tbxFreq.Text));
      lowerRange = 1.800;
      upperRange = 2.000;
    }

    private void btn10M_Click(object sender, EventArgs e)
    {
      this.tbxFreq.Text = "28.000";
      this.tbxFreq2.Text = this.tbxFreq.Text;
      this.lsbCommands.Items.Add((object) ("Freq Set to: " + this.tbxFreq.Text));
      // limits
      lowerRange = 28.000;
      upperRange = 28.700;
    }

    private void lsbCommands_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void btnGO_Click(object sender, EventArgs e)
    {
      this.lsbCommands.Items.Add((object) ("TX Freq Set to: " + this.tbxFreq.Text));
      this.lsbCommands.Items.Add((object)("RX Freq Set to: " + this.tbxFreq2.Text));
    }

    private void btnMUSB_Click(object sender, EventArgs e)
    {
      this.lblMode.Text = "USB";
      this.lsbCommands.Items.Add((object) "Mode Set to: USB");
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      // these items arbitrary
        turnOffHam();
      this.tbxFreq.Text = "28.300";
      this.lsbCommands.Items.Add((object) ("TX Set to: " + this.tbxFreq.Text));
      this.tbxFreq2.Text = "28.300";
      this.lsbCommands.Items.Add((object) ("RX Set to: " + this.tbxFreq2.Text));
      this.lblMode.Text = "USB";
      this.lsbCommands.Items.Add((object) "Mode Set to: USB");
    }

    private void btnLSB_Click(object sender, EventArgs e)
    {
      this.lblMode.Text = "LSB";
      this.lsbCommands.Items.Add((object) "Mode Set to: LSB");
    }

    private void btnCW_Click(object sender, EventArgs e)
    {
      this.lblMode.Text = "CW";
      this.lsbCommands.Items.Add((object) "Mode Set to: CW");
    }

    private void btnAM_Click(object sender, EventArgs e)
    {
      this.lblMode.Text = "AM";
      this.lsbCommands.Items.Add((object) "Mode Set to: AM");
    }

    private void btnFM_Click(object sender, EventArgs e)
    {
      this.lblMode.Text = "FM";
      this.lsbCommands.Items.Add((object) "Mode Set to: FM");
    }

    private void btnUPM_Click(object sender, EventArgs e)
    {
      this.tbxFreq.Text = (double.Parse(this.tbxFreq.Text) + 1.0).ToString().PadRight(6, '0');
    }

    private void btnDOWNM_Click(object sender, EventArgs e)
    {
      this.tbxFreq.Text = (double.Parse(this.tbxFreq.Text) - 1.0).ToString().PadRight(6, '0');
    }

    private void btnUPK_Click(object sender, EventArgs e)
    {
      this.tbxFreq.Text = (double.Parse(this.tbxFreq.Text) + 0.1).ToString().PadRight(6, '0');
    }

    private void btnDOWNK_Click(object sender, EventArgs e)
    {
      this.tbxFreq.Text = (double.Parse(this.tbxFreq.Text) - 0.1).ToString().PadRight(6, '0');
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
    }

    private void btnSplit_Click(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.lsbCommands = new System.Windows.Forms.ListBox();
            this.btn10M = new System.Windows.Forms.Button();
            this.btn20M = new System.Windows.Forms.Button();
            this.btn30M = new System.Windows.Forms.Button();
            this.btn15M = new System.Windows.Forms.Button();
            this.btn60M = new System.Windows.Forms.Button();
            this.btn80M = new System.Windows.Forms.Button();
            this.btn17M = new System.Windows.Forms.Button();
            this.btn40M = new System.Windows.Forms.Button();
            this.btn160M = new System.Windows.Forms.Button();
            this.lblBands = new System.Windows.Forms.Label();
            this.tbxFreq = new System.Windows.Forms.TextBox();
            this.btnUPK = new System.Windows.Forms.Button();
            this.btnDOWNK = new System.Windows.Forms.Button();
            this.btnDOWNM = new System.Windows.Forms.Button();
            this.btnUPM = new System.Windows.Forms.Button();
            this.btnMUSB = new System.Windows.Forms.Button();
            this.btnLSB = new System.Windows.Forms.Button();
            this.btnCW = new System.Windows.Forms.Button();
            this.btnAM = new System.Windows.Forms.Button();
            this.btnFM = new System.Windows.Forms.Button();
            this.btn12M = new System.Windows.Forms.Button();
            this.btnGO = new System.Windows.Forms.Button();
            this.lblMode = new System.Windows.Forms.Label();
            this.tbxFreq2 = new System.Windows.Forms.TextBox();
            this.btnSplit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button5 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.setToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.radioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vX1700ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.micom2BFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barrett205020502090ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codan9360ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dXSR8TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iCOMM802ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.e4406AToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.radioServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.marineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commercialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.e4406AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simVFOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOMPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOM1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOM2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOM3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOM4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOM5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lsbCommands
            // 
            this.lsbCommands.FormattingEnabled = true;
            this.lsbCommands.ItemHeight = 16;
            this.lsbCommands.Location = new System.Drawing.Point(141, 367);
            this.lsbCommands.Name = "lsbCommands";
            this.lsbCommands.Size = new System.Drawing.Size(684, 180);
            this.lsbCommands.TabIndex = 1;
            this.lsbCommands.SelectedIndexChanged += new System.EventHandler(this.lsbCommands_SelectedIndexChanged);
            // 
            // btn10M
            // 
            this.btn10M.Location = new System.Drawing.Point(138, 45);
            this.btn10M.Name = "btn10M";
            this.btn10M.Size = new System.Drawing.Size(35, 25);
            this.btn10M.TabIndex = 2;
            this.btn10M.Text = "10";
            this.btn10M.UseVisualStyleBackColor = true;
            this.btn10M.Click += new System.EventHandler(this.btn10M_Click);
            // 
            // btn20M
            // 
            this.btn20M.Location = new System.Drawing.Point(302, 45);
            this.btn20M.Name = "btn20M";
            this.btn20M.Size = new System.Drawing.Size(35, 25);
            this.btn20M.TabIndex = 3;
            this.btn20M.Text = "20";
            this.btn20M.UseVisualStyleBackColor = true;
            this.btn20M.Click += new System.EventHandler(this.btn20M_Click);
            // 
            // btn30M
            // 
            this.btn30M.Location = new System.Drawing.Point(343, 45);
            this.btn30M.Name = "btn30M";
            this.btn30M.Size = new System.Drawing.Size(35, 25);
            this.btn30M.TabIndex = 4;
            this.btn30M.Text = "30";
            this.btn30M.UseVisualStyleBackColor = true;
            this.btn30M.Click += new System.EventHandler(this.btn30M_Click);
            // 
            // btn15M
            // 
            this.btn15M.Location = new System.Drawing.Point(220, 45);
            this.btn15M.Name = "btn15M";
            this.btn15M.Size = new System.Drawing.Size(35, 25);
            this.btn15M.TabIndex = 5;
            this.btn15M.Text = "15";
            this.btn15M.UseVisualStyleBackColor = true;
            this.btn15M.Click += new System.EventHandler(this.btn15M_Click);
            // 
            // btn60M
            // 
            this.btn60M.Location = new System.Drawing.Point(425, 45);
            this.btn60M.Name = "btn60M";
            this.btn60M.Size = new System.Drawing.Size(35, 25);
            this.btn60M.TabIndex = 6;
            this.btn60M.Text = "60";
            this.btn60M.UseVisualStyleBackColor = true;
            this.btn60M.Click += new System.EventHandler(this.btn60M_Click);
            // 
            // btn80M
            // 
            this.btn80M.Location = new System.Drawing.Point(466, 45);
            this.btn80M.Name = "btn80M";
            this.btn80M.Size = new System.Drawing.Size(35, 25);
            this.btn80M.TabIndex = 7;
            this.btn80M.Text = "80";
            this.btn80M.UseVisualStyleBackColor = true;
            this.btn80M.Click += new System.EventHandler(this.btn80M_Click);
            // 
            // btn17M
            // 
            this.btn17M.Location = new System.Drawing.Point(261, 45);
            this.btn17M.Name = "btn17M";
            this.btn17M.Size = new System.Drawing.Size(35, 25);
            this.btn17M.TabIndex = 8;
            this.btn17M.Text = "17";
            this.btn17M.UseVisualStyleBackColor = true;
            this.btn17M.Click += new System.EventHandler(this.btn17M_Click);
            // 
            // btn40M
            // 
            this.btn40M.Location = new System.Drawing.Point(384, 45);
            this.btn40M.Name = "btn40M";
            this.btn40M.Size = new System.Drawing.Size(35, 25);
            this.btn40M.TabIndex = 9;
            this.btn40M.Text = "40";
            this.btn40M.UseVisualStyleBackColor = true;
            this.btn40M.Click += new System.EventHandler(this.btn40M_Click);
            // 
            // btn160M
            // 
            this.btn160M.Location = new System.Drawing.Point(507, 45);
            this.btn160M.Name = "btn160M";
            this.btn160M.Size = new System.Drawing.Size(53, 25);
            this.btn160M.TabIndex = 10;
            this.btn160M.Text = "160";
            this.btn160M.UseVisualStyleBackColor = true;
            this.btn160M.Click += new System.EventHandler(this.btn160M_Click);
            // 
            // lblBands
            // 
            this.lblBands.AutoSize = true;
            this.lblBands.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBands.Location = new System.Drawing.Point(19, 39);
            this.lblBands.Name = "lblBands";
            this.lblBands.Size = new System.Drawing.Size(108, 31);
            this.lblBands.TabIndex = 11;
            this.lblBands.Text = "BANDS";
            // 
            // tbxFreq
            // 
            this.tbxFreq.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tbxFreq.Font = new System.Drawing.Font("Arial Narrow", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxFreq.Location = new System.Drawing.Point(139, 72);
            this.tbxFreq.Name = "tbxFreq";
            this.tbxFreq.Size = new System.Drawing.Size(536, 99);
            this.tbxFreq.TabIndex = 12;
            this.tbxFreq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbxFreq.TextChanged += new System.EventHandler(this.tbxFreq_TextChanged);
            // 
            // btnUPK
            // 
            this.btnUPK.Location = new System.Drawing.Point(755, 76);
            this.btnUPK.Name = "btnUPK";
            this.btnUPK.Size = new System.Drawing.Size(70, 21);
            this.btnUPK.TabIndex = 13;
            this.btnUPK.Text = "^ K";
            this.btnUPK.UseVisualStyleBackColor = true;
            this.btnUPK.Click += new System.EventHandler(this.btnUPK_Click);
            // 
            // btnDOWNK
            // 
            this.btnDOWNK.Location = new System.Drawing.Point(755, 103);
            this.btnDOWNK.Name = "btnDOWNK";
            this.btnDOWNK.Size = new System.Drawing.Size(70, 21);
            this.btnDOWNK.TabIndex = 14;
            this.btnDOWNK.Text = "v K";
            this.btnDOWNK.UseVisualStyleBackColor = true;
            this.btnDOWNK.Click += new System.EventHandler(this.btnDOWNK_Click);
            // 
            // btnDOWNM
            // 
            this.btnDOWNM.Location = new System.Drawing.Point(681, 103);
            this.btnDOWNM.Name = "btnDOWNM";
            this.btnDOWNM.Size = new System.Drawing.Size(70, 21);
            this.btnDOWNM.TabIndex = 15;
            this.btnDOWNM.Text = "v M";
            this.btnDOWNM.UseVisualStyleBackColor = true;
            this.btnDOWNM.Click += new System.EventHandler(this.btnDOWNM_Click);
            // 
            // btnUPM
            // 
            this.btnUPM.Location = new System.Drawing.Point(681, 76);
            this.btnUPM.Name = "btnUPM";
            this.btnUPM.Size = new System.Drawing.Size(70, 21);
            this.btnUPM.TabIndex = 16;
            this.btnUPM.Text = "^ M";
            this.btnUPM.UseVisualStyleBackColor = true;
            this.btnUPM.Click += new System.EventHandler(this.btnUPM_Click);
            // 
            // btnMUSB
            // 
            this.btnMUSB.Location = new System.Drawing.Point(138, 305);
            this.btnMUSB.Name = "btnMUSB";
            this.btnMUSB.Size = new System.Drawing.Size(103, 25);
            this.btnMUSB.TabIndex = 17;
            this.btnMUSB.Text = "USB";
            this.btnMUSB.UseVisualStyleBackColor = true;
            this.btnMUSB.Click += new System.EventHandler(this.btnMUSB_Click);
            // 
            // btnLSB
            // 
            this.btnLSB.Location = new System.Drawing.Point(261, 305);
            this.btnLSB.Name = "btnLSB";
            this.btnLSB.Size = new System.Drawing.Size(95, 25);
            this.btnLSB.TabIndex = 18;
            this.btnLSB.Text = "LSB";
            this.btnLSB.UseVisualStyleBackColor = true;
            this.btnLSB.Click += new System.EventHandler(this.btnLSB_Click);
            // 
            // btnCW
            // 
            this.btnCW.Location = new System.Drawing.Point(367, 305);
            this.btnCW.Name = "btnCW";
            this.btnCW.Size = new System.Drawing.Size(93, 25);
            this.btnCW.TabIndex = 19;
            this.btnCW.Text = "CW";
            this.btnCW.UseVisualStyleBackColor = true;
            this.btnCW.Click += new System.EventHandler(this.btnCW_Click);
            // 
            // btnAM
            // 
            this.btnAM.Location = new System.Drawing.Point(477, 305);
            this.btnAM.Name = "btnAM";
            this.btnAM.Size = new System.Drawing.Size(95, 25);
            this.btnAM.TabIndex = 20;
            this.btnAM.Text = "AM";
            this.btnAM.UseVisualStyleBackColor = true;
            this.btnAM.Click += new System.EventHandler(this.btnAM_Click);
            // 
            // btnFM
            // 
            this.btnFM.Location = new System.Drawing.Point(588, 305);
            this.btnFM.Name = "btnFM";
            this.btnFM.Size = new System.Drawing.Size(86, 25);
            this.btnFM.TabIndex = 21;
            this.btnFM.Text = "FM";
            this.btnFM.UseVisualStyleBackColor = true;
            this.btnFM.Click += new System.EventHandler(this.btnFM_Click);
            // 
            // btn12M
            // 
            this.btn12M.Location = new System.Drawing.Point(179, 45);
            this.btn12M.Name = "btn12M";
            this.btn12M.Size = new System.Drawing.Size(35, 25);
            this.btn12M.TabIndex = 22;
            this.btn12M.Text = "12";
            this.btn12M.UseVisualStyleBackColor = true;
            this.btn12M.Click += new System.EventHandler(this.btn12M_Click);
            // 
            // btnGO
            // 
            this.btnGO.BackColor = System.Drawing.SystemColors.Desktop;
            this.btnGO.ForeColor = System.Drawing.SystemColors.Control;
            this.btnGO.Location = new System.Drawing.Point(681, 271);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(144, 90);
            this.btnGO.TabIndex = 23;
            this.btnGO.Text = "GO";
            this.btnGO.UseVisualStyleBackColor = false;
            this.btnGO.Click += new System.EventHandler(this.btnGO_Click);
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMode.Location = new System.Drawing.Point(592, 45);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(0, 29);
            this.lblMode.TabIndex = 24;
            this.lblMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbxFreq2
            // 
            this.tbxFreq2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tbxFreq2.Font = new System.Drawing.Font("Arial Narrow", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxFreq2.Location = new System.Drawing.Point(138, 200);
            this.tbxFreq2.Name = "tbxFreq2";
            this.tbxFreq2.Size = new System.Drawing.Size(536, 99);
            this.tbxFreq2.TabIndex = 25;
            this.tbxFreq2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbxFreq2.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnSplit
            // 
            this.btnSplit.Location = new System.Drawing.Point(138, 173);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(537, 28);
            this.btnSplit.TabIndex = 26;
            this.btnSplit.Text = "Split";
            this.btnSplit.UseVisualStyleBackColor = true;
            this.btnSplit.Visible = false;
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(78, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 31);
            this.label2.TabIndex = 28;
            this.label2.Text = "TX";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(75, 234);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 31);
            this.label3.TabIndex = 29;
            this.label3.Text = "RX";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(681, 200);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(70, 21);
            this.button1.TabIndex = 30;
            this.button1.Text = "^ M";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(681, 227);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 21);
            this.button2.TabIndex = 31;
            this.button2.Text = "v M";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(757, 200);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(68, 21);
            this.button3.TabIndex = 32;
            this.button3.Text = "^ K";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(755, 227);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(70, 21);
            this.button4.TabIndex = 33;
            this.button4.Text = "v K";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(854, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 31);
            this.label4.TabIndex = 34;
            this.label4.Text = "Memory";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(833, 111);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(158, 436);
            this.listBox1.TabIndex = 35;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(833, 79);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(158, 26);
            this.button5.TabIndex = 36;
            this.button5.Text = "SCAN";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setToolStripMenuItem,
            this.cOMPortToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1020, 28);
            this.menuStrip1.TabIndex = 39;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // setToolStripMenuItem
            // 
            this.setToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printerToolStripMenuItem,
            this.radioToolStripMenuItem,
            this.radioServiceToolStripMenuItem});
            this.setToolStripMenuItem.Name = "setToolStripMenuItem";
            this.setToolStripMenuItem.Size = new System.Drawing.Size(97, 24);
            this.setToolStripMenuItem.Text = "Preferences";
            // 
            // printerToolStripMenuItem
            // 
            this.printerToolStripMenuItem.Name = "printerToolStripMenuItem";
            this.printerToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.printerToolStripMenuItem.Text = "Printer";
            // 
            // radioToolStripMenuItem
            // 
            this.radioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.vX1700ToolStripMenuItem,
            this.micom2BFToolStripMenuItem,
            this.barrett205020502090ToolStripMenuItem,
            this.codan9360ToolStripMenuItem,
            this.dXSR8TToolStripMenuItem,
            this.iCOMM802ToolStripMenuItem,
            this.e4406AToolStripMenuItem1});
            this.radioToolStripMenuItem.Name = "radioToolStripMenuItem";
            this.radioToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.radioToolStripMenuItem.Text = "Radio";
            // 
            // vX1700ToolStripMenuItem
            // 
            this.vX1700ToolStripMenuItem.Name = "vX1700ToolStripMenuItem";
            this.vX1700ToolStripMenuItem.Size = new System.Drawing.Size(159, 24);
            this.vX1700ToolStripMenuItem.Text = "VX-1700";
            this.vX1700ToolStripMenuItem.Click += new System.EventHandler(this.vX1700ToolStripMenuItem_Click);
            // 
            // micom2BFToolStripMenuItem
            // 
            this.micom2BFToolStripMenuItem.Name = "micom2BFToolStripMenuItem";
            this.micom2BFToolStripMenuItem.Size = new System.Drawing.Size(159, 24);
            this.micom2BFToolStripMenuItem.Text = "Micom 2BF";
            this.micom2BFToolStripMenuItem.Click += new System.EventHandler(this.micom2BFToolStripMenuItem_Click);
            // 
            // barrett205020502090ToolStripMenuItem
            // 
            this.barrett205020502090ToolStripMenuItem.Name = "barrett205020502090ToolStripMenuItem";
            this.barrett205020502090ToolStripMenuItem.Size = new System.Drawing.Size(159, 24);
            this.barrett205020502090ToolStripMenuItem.Text = "Barrett 2050";
            this.barrett205020502090ToolStripMenuItem.Click += new System.EventHandler(this.barrett205020502090ToolStripMenuItem_Click);
            // 
            // codan9360ToolStripMenuItem
            // 
            this.codan9360ToolStripMenuItem.Name = "codan9360ToolStripMenuItem";
            this.codan9360ToolStripMenuItem.Size = new System.Drawing.Size(159, 24);
            this.codan9360ToolStripMenuItem.Text = "Codan 9360";
            this.codan9360ToolStripMenuItem.Click += new System.EventHandler(this.codan9360ToolStripMenuItem_Click);
            // 
            // dXSR8TToolStripMenuItem
            // 
            this.dXSR8TToolStripMenuItem.Name = "dXSR8TToolStripMenuItem";
            this.dXSR8TToolStripMenuItem.Size = new System.Drawing.Size(159, 24);
            this.dXSR8TToolStripMenuItem.Text = "DX-SR8T";
            this.dXSR8TToolStripMenuItem.Click += new System.EventHandler(this.dXSR8TToolStripMenuItem_Click);
            // 
            // iCOMM802ToolStripMenuItem
            // 
            this.iCOMM802ToolStripMenuItem.Name = "iCOMM802ToolStripMenuItem";
            this.iCOMM802ToolStripMenuItem.Size = new System.Drawing.Size(159, 24);
            this.iCOMM802ToolStripMenuItem.Text = "ICOM M802";
            this.iCOMM802ToolStripMenuItem.Click += new System.EventHandler(this.iCOMM802ToolStripMenuItem_Click);
            // 
            // e4406AToolStripMenuItem1
            // 
            this.e4406AToolStripMenuItem1.Name = "e4406AToolStripMenuItem1";
            this.e4406AToolStripMenuItem1.Size = new System.Drawing.Size(159, 24);
            this.e4406AToolStripMenuItem1.Text = "E4406A";
            // 
            // radioServiceToolStripMenuItem
            // 
            this.radioServiceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.marineToolStripMenuItem,
            this.commercialToolStripMenuItem,
            this.hamToolStripMenuItem,
            this.e4406AToolStripMenuItem,
            this.simVFOToolStripMenuItem});
            this.radioServiceToolStripMenuItem.Name = "radioServiceToolStripMenuItem";
            this.radioServiceToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.radioServiceToolStripMenuItem.Text = "Radio Service";
            // 
            // marineToolStripMenuItem
            // 
            this.marineToolStripMenuItem.Name = "marineToolStripMenuItem";
            this.marineToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.marineToolStripMenuItem.Text = "Marine";
            this.marineToolStripMenuItem.Click += new System.EventHandler(this.marineToolStripMenuItem_Click);
            // 
            // commercialToolStripMenuItem
            // 
            this.commercialToolStripMenuItem.Name = "commercialToolStripMenuItem";
            this.commercialToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.commercialToolStripMenuItem.Text = "Commercial";
            this.commercialToolStripMenuItem.Click += new System.EventHandler(this.commercialToolStripMenuItem_Click);
            // 
            // hamToolStripMenuItem
            // 
            this.hamToolStripMenuItem.Name = "hamToolStripMenuItem";
            this.hamToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.hamToolStripMenuItem.Text = "Ham";
            this.hamToolStripMenuItem.Click += new System.EventHandler(this.hamToolStripMenuItem_Click);
            // 
            // e4406AToolStripMenuItem
            // 
            this.e4406AToolStripMenuItem.Name = "e4406AToolStripMenuItem";
            this.e4406AToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.e4406AToolStripMenuItem.Text = "E4406A";
            this.e4406AToolStripMenuItem.Click += new System.EventHandler(this.e4406AToolStripMenuItem_Click);
            // 
            // simVFOToolStripMenuItem
            // 
            this.simVFOToolStripMenuItem.Name = "simVFOToolStripMenuItem";
            this.simVFOToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.simVFOToolStripMenuItem.Text = "simVFO";
            this.simVFOToolStripMenuItem.Click += new System.EventHandler(this.simVFOToolStripMenuItem_Click);
            // 
            // cOMPortToolStripMenuItem
            // 
            this.cOMPortToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cOM1ToolStripMenuItem,
            this.cOM2ToolStripMenuItem,
            this.cOM3ToolStripMenuItem,
            this.cOM4ToolStripMenuItem,
            this.cOM5ToolStripMenuItem});
            this.cOMPortToolStripMenuItem.Name = "cOMPortToolStripMenuItem";
            this.cOMPortToolStripMenuItem.Size = new System.Drawing.Size(85, 24);
            this.cOMPortToolStripMenuItem.Text = "COM Port";
            this.cOMPortToolStripMenuItem.Click += new System.EventHandler(this.cOMPortToolStripMenuItem_Click);
            // 
            // cOM1ToolStripMenuItem
            // 
            this.cOM1ToolStripMenuItem.Name = "cOM1ToolStripMenuItem";
            this.cOM1ToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.cOM1ToolStripMenuItem.Text = "COM1";
            this.cOM1ToolStripMenuItem.Click += new System.EventHandler(this.cOM1ToolStripMenuItem_Click);
            // 
            // cOM2ToolStripMenuItem
            // 
            this.cOM2ToolStripMenuItem.Name = "cOM2ToolStripMenuItem";
            this.cOM2ToolStripMenuItem.Size = new System.Drawing.Size(119, 24);
            this.cOM2ToolStripMenuItem.Text = "COM2";
            this.cOM2ToolStripMenuItem.Click += new System.EventHandler(this.cOM2ToolStripMenuItem_Click);
            // 
            // cOM3ToolStripMenuItem
            // 
            this.cOM3ToolStripMenuItem.Name = "cOM3ToolStripMenuItem";
            this.cOM3ToolStripMenuItem.Size = new System.Drawing.Size(119, 24);
            this.cOM3ToolStripMenuItem.Text = "COM3";
            this.cOM3ToolStripMenuItem.Click += new System.EventHandler(this.cOM3ToolStripMenuItem_Click);
            // 
            // cOM4ToolStripMenuItem
            // 
            this.cOM4ToolStripMenuItem.Name = "cOM4ToolStripMenuItem";
            this.cOM4ToolStripMenuItem.Size = new System.Drawing.Size(119, 24);
            this.cOM4ToolStripMenuItem.Text = "COM4";
            this.cOM4ToolStripMenuItem.Click += new System.EventHandler(this.cOM4ToolStripMenuItem_Click);
            // 
            // cOM5ToolStripMenuItem
            // 
            this.cOM5ToolStripMenuItem.Name = "cOM5ToolStripMenuItem";
            this.cOM5ToolStripMenuItem.Size = new System.Drawing.Size(119, 24);
            this.cOM5ToolStripMenuItem.Text = "COM5";
            this.cOM5ToolStripMenuItem.Click += new System.EventHandler(this.cOM5ToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(135, 335);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 31);
            this.label1.TabIndex = 27;
            this.label1.Text = "Commands/Messeges";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(681, 159);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(144, 22);
            this.textBox1.TabIndex = 40;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(681, 130);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(144, 26);
            this.button6.TabIndex = 41;
            this.button6.Text = "Scan Band Interval";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1020, 565);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSplit);
            this.Controls.Add(this.tbxFreq2);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.btnGO);
            this.Controls.Add(this.btn12M);
            this.Controls.Add(this.btnFM);
            this.Controls.Add(this.btnAM);
            this.Controls.Add(this.btnCW);
            this.Controls.Add(this.btnLSB);
            this.Controls.Add(this.btnMUSB);
            this.Controls.Add(this.btnUPM);
            this.Controls.Add(this.btnDOWNM);
            this.Controls.Add(this.btnDOWNK);
            this.Controls.Add(this.btnUPK);
            this.Controls.Add(this.tbxFreq);
            this.Controls.Add(this.lblBands);
            this.Controls.Add(this.btn160M);
            this.Controls.Add(this.btn40M);
            this.Controls.Add(this.btn17M);
            this.Controls.Add(this.btn80M);
            this.Controls.Add(this.btn60M);
            this.Controls.Add(this.btn15M);
            this.Controls.Add(this.btn30M);
            this.Controls.Add(this.btn20M);
            this.Controls.Add(this.btn10M);
            this.Controls.Add(this.lsbCommands);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "SigInt1700";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    private void label1_Click_1(object sender, EventArgs e)
    {

    }

    private void button1_Click(object sender, EventArgs e)
    {
        this.tbxFreq2.Text = (double.Parse(this.tbxFreq2.Text) + 1.0).ToString().PadRight(6, '0');

    }

    private void button3_Click(object sender, EventArgs e)
    {
        this.tbxFreq2.Text = (double.Parse(this.tbxFreq2.Text) + 0.1).ToString().PadRight(6, '0');

    }

    private void button2_Click(object sender, EventArgs e)
    {
        this.tbxFreq2.Text = (double.Parse(this.tbxFreq2.Text) - 1.0).ToString().PadRight(6, '0');
    }

    private void button4_Click(object sender, EventArgs e)
    {
        this.tbxFreq2.Text = (double.Parse(this.tbxFreq2.Text) - 0.1).ToString().PadRight(6, '0');
    }

    private void cOMPortToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void dXSR8TToolStripMenuItem_Click(object sender, EventArgs e)
    {
        // show commands...
        for (int i = 0; i < alincoCommands.GetLength(0); i++)
        {
            lsbCommands.Items.Add(alincoCommands[i, 0] + " " + alincoCommands[i, 1]);
        }
    }

    private void vX1700ToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void micom2BFToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void barrett205020502090ToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void codan9360ToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void iCOMM802ToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void simVFOToolStripMenuItem_Click(object sender, EventArgs e)
    {
        this.MouseWheel += new MouseEventHandler(all_MouseWheel);
    }

    private void e4406AToolStripMenuItem_Click(object sender, EventArgs e)
    {
        this.MouseWheel += new MouseEventHandler(all_MouseWheel);
    }

    private void marineToolStripMenuItem_Click(object sender, EventArgs e)
    {
        turnOffHam();
        this.MouseWheel -= new MouseEventHandler(all_MouseWheel);
    }

    private void commercialToolStripMenuItem_Click(object sender, EventArgs e)
    {
        turnOffHam();
        this.MouseWheel -= new MouseEventHandler(all_MouseWheel);
    }

    private void hamToolStripMenuItem_Click(object sender, EventArgs e)
    {
        turnOnHam();
        this.MouseWheel += new MouseEventHandler(all_MouseWheel);
    }

    private void cOM1ToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void cOM2ToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void cOM3ToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void cOM4ToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void cOM5ToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void turnOffHam()
    {
        lblBands.Visible = false;
        btn10M.Visible = false;
        btn12M.Visible = false;
        btn15M.Visible = false;
        btn160M.Visible = false;
        btn17M.Visible = false;
        btn20M.Visible = false;
        btn30M.Visible = false;
        btn40M.Visible = false;
        btn60M.Visible = false;
        btn80M.Visible = false;
    }

    private void turnOnHam()
    {
        lblBands.Visible = true;
        btn10M.Visible = true;
        btn12M.Visible = true;
        btn15M.Visible = true;
        btn160M.Visible = true;
        btn17M.Visible = true;
        btn20M.Visible = true;
        btn30M.Visible = true;
        btn40M.Visible = true;
        btn60M.Visible = true;
        btn80M.Visible = true;
    }

    private bool requestWithinRange(double lowER, double highER, string freqValue)
    {
        return true;
    }
  }
}
