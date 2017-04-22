using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace EnCryptDecrypt
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private long m_originalLength;

        private void EnCryptDecrypt_Load(object sender, EventArgs e)
        {
            m_originalLength = 0;
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            string key = DES.GenerateKey();
            GCHandle gch = GCHandle.Alloc(key, GCHandleType.Pinned);

            //if (txtClearText.Text == "")
            //{
            //    error.SetError(txtClearText, "Enter the text you want to encrypt");
            //}
            //else
            //{
            //    error.Clear();
            //    string clearText = txtClearText.Text.Trim();
            //    string key = textBox1.Text.Trim();
            //    string cipherText = CryptorEngine.Encrypt(clearText, true, key);
            //    txtDecryptedText.Visible = false;
            //    label3.Visible = false;
            //    txtCipherText.Text = cipherText;
            //    btnDecrypt.Enabled = true;
            //}
            DES.EncryptFile(txtClearText.Text.Trim(), txtClearText.Text.Trim(), key);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            string key = DES.GenerateKey();
            GCHandle gch = GCHandle.Alloc(key, GCHandleType.Pinned);
            //string cipherText = txtCipherText.Text.Trim();
            //string key = textBox1.Text.Trim();
            //string decryptedText = CryptorEngine.Decrypt(cipherText, true, key);
            //txtDecryptedText.Text = decryptedText;
            //txtDecryptedText.Visible = true;
            //label3.Visible = true;

            DES.DecryptFile(txtClearText.Text.Trim(), txtClearText.Text.Trim(), key);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //openFileDialog1.Filter = "Image files (*.bmp,*.png,*.jpg,*.tif)|*.bmp;*.png;*.jpg;*.tif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtClearText.Text = openFileDialog1.FileName;
            }
        }
    }
}