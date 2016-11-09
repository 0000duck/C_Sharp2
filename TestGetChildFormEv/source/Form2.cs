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
    public partial class Form2 : Form
    {
        public delegate void commEvHandler(String msg);
        public event commEvHandler myCommEvHandler;         // event

        public Form2()
        {
            InitializeComponent();
        }


        /**
         *  @brief      Event発生し、親Form側のMethodをcall
         *  @param[in]  object  sender
         *  @param[in]  EventArgs    e
         *  @return     void
         *  @note       親Form(Form1)で、登録したMethod(EventFromChildForm) を myCommEvHandler で
         *              Event を起こすことで、Method(EventFromChildForm)を呼ぶ
         */
        private void button1_Click(object sender, EventArgs e)
        {
            myCommEvHandler(textBox1.Text);     // Event発生 textBox1 の内容を引数渡し
        }
    }
}
