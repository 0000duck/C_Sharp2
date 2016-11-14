//-------------------------------------------------------------------------
//  文字列Check　確認サンプル
//
//  [ 内容 ]
//  正規表現と System.Text.RegularExpressions.Regex を使って
//  textBox に入力された文字列を Check。
//  Error時は、ErrorProvider(デザインで貼り付けた) を使用して
//  (i)マークを表示。
//-------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Text.RegularExpressions;   // add

namespace TestChkText
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        /**
          * @brief   　 a-zの文字列Chek　ボタン処理
          * @param[in]  object     e
          * @param[in]  EventArgs  e 
          * @return     void   
          */
        private void Btn_ChkTxt1_Click(object sender, EventArgs e)
        {
            // a-z のみの文字列
            bool bret = Regex.IsMatch(textBox1.Text, @"^[a-z]+$");
            if (bret==false)
            {
                //this.errorProvider1.SetError((TextBox)sender, "a-z のみの文字列");
                this.errorProvider1.SetError(textBox1, "a-z のみの文字列を入力してください。");
                textBox2.Text = "NG";
            }
            else
            {
                this.errorProvider1.SetError(textBox1, ""); // iマーククリア
                textBox2.Text = "OK";
            }
        }

        /**
          * @brief   　 a-z と 0-9の文字列Chek　ボタン処理
          * @param[in]  object     e
          * @param[in]  EventArgs  e 
          * @return     void   
          */
        private void Btn_ChkTxt3_Click(object sender, EventArgs e)
        {
            // a-z と 0-9 のみの文字列
            bool bret = Regex.IsMatch(textBox3.Text, @"^[a-z0-9]+$");
            if (bret == false)
            {
                this.errorProvider1.SetError(textBox3, "a-z と 0-9 のみの文字列を入力してください。");
                textBox4.Text = "NG";
            }
            else
            {
                this.errorProvider1.SetError(textBox3, ""); // iマーククリア
                textBox4.Text = "OK";
            }
        }

        /**
          * @brief   　  A-Z と tab と spaceの文字列Chek　ボタン処理
          * @param[in]  object     e
          * @param[in]  EventArgs  e 
          * @return     void   
          */
        private void Btn_ChkTxt5_Click(object sender, EventArgs e)
        {
            // textBox5.Multiline = true と
            // textBox5.AcceptsTab = true にして、tab入力できるようにしてある

            // A-Z と tab と space  のみの文字列
            bool bret = Regex.IsMatch(textBox5.Text, @"^[A-Z\t\x20]+$");
            if (bret == false)
            {
                this.errorProvider1.SetError(textBox5, "A-Z と tab と space のみの文字列を入力してください。");
                textBox6.Text = "NG";
            }
            else
            {
                this.errorProvider1.SetError(textBox5, ""); // iマーククリア
                textBox6.Text = "OK";
            }
        }
    }
}
