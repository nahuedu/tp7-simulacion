namespace tp7_simulacion
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
            this.txtbox_cant_juniors = new System.Windows.Forms.TextBox();
            this.txtbox_cant_seniors = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_simular = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_promedio_permanencia = new System.Windows.Forms.Label();
            this.lbl_ptoj = new System.Windows.Forms.Label();
            this.lbl_ptos = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtbox_cant_juniors
            // 
            this.txtbox_cant_juniors.Location = new System.Drawing.Point(121, 38);
            this.txtbox_cant_juniors.Name = "txtbox_cant_juniors";
            this.txtbox_cant_juniors.Size = new System.Drawing.Size(100, 20);
            this.txtbox_cant_juniors.TabIndex = 0;
            // 
            // txtbox_cant_seniors
            // 
            this.txtbox_cant_seniors.Location = new System.Drawing.Point(121, 86);
            this.txtbox_cant_seniors.Name = "txtbox_cant_seniors";
            this.txtbox_cant_seniors.Size = new System.Drawing.Size(100, 20);
            this.txtbox_cant_seniors.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Cantidad de Juniors:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Cantidad de Seniors:";
            // 
            // btn_simular
            // 
            this.btn_simular.Location = new System.Drawing.Point(103, 122);
            this.btn_simular.Name = "btn_simular";
            this.btn_simular.Size = new System.Drawing.Size(75, 23);
            this.btn_simular.TabIndex = 4;
            this.btn_simular.Text = "Simular";
            this.btn_simular.UseVisualStyleBackColor = true;
            this.btn_simular.Click += new System.EventHandler(this.btn_simular_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Promedio permanencia (en min): ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 196);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "PTOJ: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 222);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "PTOS: ";
            // 
            // lbl_promedio_permanencia
            // 
            this.lbl_promedio_permanencia.AutoSize = true;
            this.lbl_promedio_permanencia.Location = new System.Drawing.Point(181, 170);
            this.lbl_promedio_permanencia.Name = "lbl_promedio_permanencia";
            this.lbl_promedio_permanencia.Size = new System.Drawing.Size(21, 13);
            this.lbl_promedio_permanencia.TabIndex = 8;
            this.lbl_promedio_permanencia.Text = "R1";
            // 
            // lbl_ptoj
            // 
            this.lbl_ptoj.AutoSize = true;
            this.lbl_ptoj.Location = new System.Drawing.Point(60, 196);
            this.lbl_ptoj.Name = "lbl_ptoj";
            this.lbl_ptoj.Size = new System.Drawing.Size(21, 13);
            this.lbl_ptoj.TabIndex = 9;
            this.lbl_ptoj.Text = "R1";
            // 
            // lbl_ptos
            // 
            this.lbl_ptos.AutoSize = true;
            this.lbl_ptos.Location = new System.Drawing.Point(60, 222);
            this.lbl_ptos.Name = "lbl_ptos";
            this.lbl_ptos.Size = new System.Drawing.Size(21, 13);
            this.lbl_ptos.TabIndex = 10;
            this.lbl_ptos.Text = "R1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 290);
            this.Controls.Add(this.lbl_ptos);
            this.Controls.Add(this.lbl_ptoj);
            this.Controls.Add(this.lbl_promedio_permanencia);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_simular);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbox_cant_seniors);
            this.Controls.Add(this.txtbox_cant_juniors);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtbox_cant_juniors;
        private System.Windows.Forms.TextBox txtbox_cant_seniors;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_simular;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl_promedio_permanencia;
        private System.Windows.Forms.Label lbl_ptoj;
        private System.Windows.Forms.Label lbl_ptos;
    }
}

