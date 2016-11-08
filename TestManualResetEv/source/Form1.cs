//--------------------------------------------------------------------
//    ManualResetEvent 動作確認サンプル
//
//      myThreadFunc内、WaitOneでシグナル待ちを2回繰り返し
//      2回受信したらスレッドを終了します。
//      button1を押すたびにシグナルをセットします。
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

using System.Threading;


namespace TestManualResetEv
{

    public partial class Form1 : Form
    {
        ManualResetEvent mEv;
        Thread thread1;

        public Form1()
        {
            InitializeComponent();

            mEv = new ManualResetEvent(false);  // false = 初期状態,非シグナル

            thread1 = new Thread(new ThreadStart(myThreadFunc));
            thread1.Start();

        }

        /**
         *  @brief      シグナルSet処理　ボタン
         *  @param[in]  object  sender
         *  @param[in]  EventArgs    e
         *  @return     void
         *  @note       
         */
        private void button1_Click(object sender, EventArgs e)
        {
            mEv.Set();  // イベントの状態をシグナル状態に設定し、待機している 1 つ以上のスレッドが進行できるようにする。
        }

        /**
         *  @brief       独自スレッド関数
         *  @param[in]   void
         *  @return      void
         *  @note        WaitOne で シグナル 2回待って終了
         */
        private void myThreadFunc()
        {
            int c;

            c = 0;
            while (c < 2)
            {
                mEv.WaitOne();      // Set待ち
                mEv.Reset();        // イベントの状態を非シグナル状態に設定
                c++;
            }

            MessageBox.Show("Thread1 end.");
        }

        /**
         *  @brief      TopForm終了処理
         *  @param[in]  object  sender
         *  @param[in]  FormClosingEventArgs    e
         *  @return     void
         *  @note       スレッドが生きている限りスルー
         */
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread1.IsAlive)    // スレッドが生きている間は、アプリ終了させない
            {
                e.Cancel = true;
            }
            else {
                mEv.Close();
                mEv.Dispose();
            }
        }
    }
}
