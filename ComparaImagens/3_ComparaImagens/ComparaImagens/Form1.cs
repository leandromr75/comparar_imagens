using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace ImageComparison
{
    public partial class Form1 : Form
    {
        protected bool validData;

        

        public Form1()
        {
            InitializeComponent();
        }

        static string nomeArquivo1, nomeArquivo2;
        Bitmap img1, img2;
        int contador1 = 0, contador2 = 0;
        bool flag = true;
        int vai = 0;
        int arrastou = 0;
        

        private void Form1_Load(object sender, EventArgs e)
        {
            pgBar1.Visible = false;
            picImagem1.Visible = true;
            picImagem2.Visible = true;
            CheckForIllegalCrossThreadCalls = false;

            picImagem1.AllowDrop = true;
            picImagem2.AllowDrop = true;

        }

        private void lnklbImagem1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (vai > 0)
            {
                MessageBox.Show("Necessário Limpar");
                return;
            }

            ofd1.FileName = "";
            ofd1.Title = "Imagens"; 
            ofd1.Filter = "Todas Imagens|*.jpg; *.bmp; *.png";
            pgBar1.Value = 0;
            pgBar1.Visible = false;

            ofd1.ShowDialog(); 
            if (ofd1.FileName.ToString() != "")
            {
                nomeArquivo1 = ofd1.FileName.ToString();
                picImagem1.Image = new Bitmap(nomeArquivo1);
                label1.Text = nomeArquivo1;
            }
        }

        private void lnklbImagem2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (vai > 0)
            {
                MessageBox.Show("Necessário Limpar");
                return;
            }

            ofd2.FileName = ""; 
            ofd2.Title = "Imagens"; 
            ofd2.Filter = "Todas Imagens|*.jpg; *.bmp; *.png";
           

            ofd2.ShowDialog();
            if (ofd2.FileName.ToString() != "")
            {
                nomeArquivo2 = ofd2.FileName.ToString();
                picImagem2.Image = new Bitmap(nomeArquivo2);
                label2.Text = nomeArquivo2;
            }
        }

        private void btnCompararImagens_Click(object sender, EventArgs e)
        {
            if (picImagem1.Image == null || picImagem2.Image == null)
            {
                MessageBox.Show("Por favor selecione as imagens para comparação");
                return;
            }
            if (vai > 0)
            {
                MessageBox.Show("Necessário Limpar");
                return;
            }
            vai++;
            pgBar1.Visible = true;
            Thread thread = new Thread(() => { comparar();});
            thread.Start();
         
            
            //pictureBox1.Visible = false;
            
        }
        public void encerraAminacao()
        {
            pictureBox1.Visible = false;
        } 
        public void comparar()
        {
            

            string img1_ref, img2_ref;
            if (arrastou <= 0)
            {
                img1 = new Bitmap(nomeArquivo1);
                img2 = new Bitmap(nomeArquivo2);
            }
            

            pgBar1.Maximum = img1.Width;
            if (img1.Width == img2.Width && img1.Height == img2.Height)
            {
                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        img1_ref = img1.GetPixel(i, j).ToString();
                        img2_ref = img2.GetPixel(i, j).ToString();
                        if (img1_ref != img2_ref)
                        {
                            contador2++;
                            flag = false;
                            break;
                        }
                        contador1++;
                    }
                    pgBar1.Value++;
                }

                if (flag == false)
                {
                    //MessageBox.Show("As imagens não são as mesmas, " + contador2 + " pixels diferentes foram encontrados");

                    label3.Text = "As imagens não são as mesmas, " + contador2 + " pixels diferentes foram encontrados";
                    pictureBox1.Visible = false;
                    pgBar1.Visible = false;
                    img1_ref = "";
                    img2_ref = "";
                    pgBar1.Value = 0;
                    //encerraAminacao();
                    

                }
                else
                {
                    //MessageBox.Show("As imagens são as mesmas, " + contador1 + " pixels iguais foram encontrados, e  " + contador2 + " pixels incorretos foram encontrados");
                    label3.Text = "As imagens são as mesmas, " + contador1 + " pixels iguais foram encontrados, \ne  " + contador2 + " pixels incorretos foram encontrados";
                    pictureBox1.Visible = false;
                    pgBar1.Visible = false;
                    img1_ref = "";
                    img2_ref = "";
                    pgBar1.Value = 0;
                    //encerraAminacao();
                    
                }
            }
            else
            {
                //MessageBox.Show("As imagens não podem ser comparadas");
                label3.Text = "As imagens não podem ser comparadas";
                pictureBox1.Visible = false;
                pgBar1.Visible = false;
                img1_ref = "";
                img2_ref = "";
                pgBar1.Value = 0;
                //encerraAminacao();
            }
        }

        private void btnEncerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            picImagem1.Image = null;
            picImagem2.Image = null;
                       
            pgBar1.Value = 0;
            Application.Restart();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void picImagem1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;

        }

        private void picImagem1_DragDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(DataFormats.FileDrop);
            if (data != null)
            {
                var filename = data as string[];
                if (filename.Length > 0)
                {
                    picImagem1.Image = Image.FromFile(filename[0]);
                    img1 = new Bitmap(filename[0]);
                    arrastou++;
                    label1.Text = filename[0];
                }
            }
        }

        private void picImagem2_DragDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(DataFormats.FileDrop);
            if (data != null)
            {
                var filename = data as string[];
                if (filename.Length > 0)
                {
                    picImagem2.Image = Image.FromFile(filename[0]);
                    img2 = new Bitmap(filename[0]);
                    arrastou++;
                    label2.Text = filename[0];
                }
            }
        }

        private void picImagem2_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        
       
        

        
    }
}