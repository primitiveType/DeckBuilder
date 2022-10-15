namespace PrefabEditor
{
    partial class PrefabEditor
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
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.buttonPrefabDirectory = new System.Windows.Forms.Button();
            this.componentsListBox = new System.Windows.Forms.ListBox();
            this.prefabsListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.loadPrefabButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.addComponentListBox = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            this.prefabsSearch = new System.Windows.Forms.TextBox();
            this.currentComponentsSearch = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(829, 377);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(476, 347);
            this.propertyGrid1.TabIndex = 0;
            // 
            // buttonPrefabDirectory
            // 
            this.buttonPrefabDirectory.Location = new System.Drawing.Point(29, 12);
            this.buttonPrefabDirectory.Name = "buttonPrefabDirectory";
            this.buttonPrefabDirectory.Size = new System.Drawing.Size(177, 23);
            this.buttonPrefabDirectory.TabIndex = 1;
            this.buttonPrefabDirectory.Text = "Choose Prefabs Directory";
            this.buttonPrefabDirectory.UseVisualStyleBackColor = true;
            // 
            // componentsListBox
            // 
            this.componentsListBox.FormattingEnabled = true;
            this.componentsListBox.Location = new System.Drawing.Point(382, 408);
            this.componentsListBox.Name = "componentsListBox";
            this.componentsListBox.Size = new System.Drawing.Size(386, 316);
            this.componentsListBox.TabIndex = 2;
            this.componentsListBox.SelectedIndexChanged += new System.EventHandler(this.componentsListBox_SelectedIndexChanged_1);
            // 
            // prefabsListBox
            // 
            this.prefabsListBox.FormattingEnabled = true;
            this.prefabsListBox.Location = new System.Drawing.Point(29, 41);
            this.prefabsListBox.Name = "prefabsListBox";
            this.prefabsListBox.Size = new System.Drawing.Size(292, 290);
            this.prefabsListBox.TabIndex = 3;
            this.prefabsListBox.SelectedIndexChanged += new System.EventHandler(this.prefabsListBox_SelectedIndexChanged);
            this.prefabsListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 4;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // loadPrefabButton
            // 
            this.loadPrefabButton.Location = new System.Drawing.Point(382, 41);
            this.loadPrefabButton.Name = "loadPrefabButton";
            this.loadPrefabButton.Size = new System.Drawing.Size(75, 23);
            this.loadPrefabButton.TabIndex = 5;
            this.loadPrefabButton.Text = "Load Prefab";
            this.loadPrefabButton.UseVisualStyleBackColor = true;
            this.loadPrefabButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1230, 761);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Save Prefab";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // addComponentListBox
            // 
            this.addComponentListBox.FormattingEnabled = true;
            this.addComponentListBox.Location = new System.Drawing.Point(29, 408);
            this.addComponentListBox.Name = "addComponentListBox";
            this.addComponentListBox.Size = new System.Drawing.Size(329, 316);
            this.addComponentListBox.TabIndex = 7;
            this.addComponentListBox.SelectedIndexChanged += new System.EventHandler(this.addComponentListBox_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(597, 379);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Remove Component";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 389);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "All Components. Double Click to Add.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(382, 388);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(168, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Current Components. Click to Edit.";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(29, 744);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(329, 20);
            this.textBox1.TabIndex = 11;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 516);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Filter:";
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.Location = new System.Drawing.Point(136, 1061);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.Size = new System.Drawing.Size(130, 130);
            this.propertyGrid2.TabIndex = 13;
            // 
            // prefabsSearch
            // 
            this.prefabsSearch.Location = new System.Drawing.Point(29, 343);
            this.prefabsSearch.Name = "prefabsSearch";
            this.prefabsSearch.Size = new System.Drawing.Size(292, 20);
            this.prefabsSearch.TabIndex = 14;
            this.prefabsSearch.TextChanged += new System.EventHandler(this.prefabsSearch_TextChanged);
            // 
            // currentComponentsSearch
            // 
            this.currentComponentsSearch.Location = new System.Drawing.Point(385, 744);
            this.currentComponentsSearch.Name = "currentComponentsSearch";
            this.currentComponentsSearch.Size = new System.Drawing.Size(329, 20);
            this.currentComponentsSearch.TabIndex = 15;
            this.currentComponentsSearch.TextChanged += new System.EventHandler(this.currentComponentsSearch_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(328, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(244, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "<- Right click a prefab name to copy it to clipboard";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // PrefabEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1426, 816);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.currentComponentsSearch);
            this.Controls.Add(this.prefabsSearch);
            this.Controls.Add(this.propertyGrid2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.addComponentListBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.loadPrefabButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.prefabsListBox);
            this.Controls.Add(this.componentsListBox);
            this.Controls.Add(this.buttonPrefabDirectory);
            this.Controls.Add(this.propertyGrid1);
            this.Name = "PrefabEditor";
            this.Text = "Prefab Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button buttonPrefabDirectory;
        private System.Windows.Forms.ListBox componentsListBox;
        private System.Windows.Forms.ListBox prefabsListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button loadPrefabButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox addComponentListBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PropertyGrid propertyGrid2;
        private System.Windows.Forms.TextBox prefabsSearch;
        private System.Windows.Forms.TextBox currentComponentsSearch;
        private System.Windows.Forms.Label label5;
    }
}