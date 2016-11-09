//-------------------------------------------------------------------
//  Processクラスの Exited Event確認サンプルプログラム
//
//  [ 内容 ]
//   Form1.cs[デザイン] に Processコントロール追加し、process1 を作成。
//   button1 で textBox1 に記載された exeの Pathを process1に指定しexe開始。
//   exe終了時に process1_Exited() が callされて、実行した exeの終了を知る。
//   exe終了前に 本アプリを終了しても特に問題なさそうでした。
//-------------------------------------------------------------------
using System;
using System.Windows.Forms;

namespace TestProcessExit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /**
          * @brief   　 exe開始 ボタン処理
          * @param[in]  object     e
          * @param[in]  EventArgs  e 
          * @return     void 
          * @note       textBox1に指定された exeを実行
          */
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            try {
                process1.EnableRaisingEvents = true;            // process1_Exited有効
                process1.StartInfo.FileName = textBox1.Text;
                process1.Start();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        /**
          * @brief      実行した exeが終了されたときに発生するEventMethod
          * @param[in]  object     e
          * @param[in]  EventArgs  e 
          * @return     void 
          * @note       本Method は Form1.cs[デザイン]の Eventプロパティより追加しました。
          */
        private void process1_Exited(object sender, EventArgs e)
        {
            MessageBox.Show("実行したアプリが終了しました。");
        }
    }
}
