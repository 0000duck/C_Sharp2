//--------------------------------------------------------------------
//  子FormのEvent時、親FormのMethod callするサンプル
//
//  [ 内容 ]
//  子Form生成時、Event に 親のcallしてほしいMethodを登録
//  子Formのbutton1 を押すと Event が発生し、登録した親Method がcallされる
//
//--------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestGetChildFormEv
{
    public partial class Form1 : Form
    {
        Form2 childform;

        public Form1()
        {
            InitializeComponent();
        }

        /**
         *  @brief      子Form起動　ボタン処理
         *  @param[in]  object  sender
         *  @param[in]  EventArgs    e
         *  @return     void
         *  @note       子Form生成し、Event で callされる Method を登録。
         */
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            childform = new Form2();
            childform.myCommEvHandler += new Form2.commEvHandler(EventFromChildForm);
            childform.Show();
        }

        /**
         *  @brief      子FormからのEventの引数のstring を textBox1に表示
         *  @param[in]  string  msg
         *  @return     void
         *  @note       子FormのcommEvHandler event発生時、callされる
         */
        void EventFromChildForm(string msg)
        {
            textBox1.Text = msg;
        }
    }
}
