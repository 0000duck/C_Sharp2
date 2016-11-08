//------------------------------------------------------------------------------------
//  Monitor と Queue 確認サンプル
//
//  [ 内容 ]
//     QueueをMonitor の Object とし、lock/unlock を
//     スレッドとTimer1 で使用
//     スレッドで、 Queue に約秒おきに現在時間文字列を設定し
//     Timer1で 10msecごとにモニタし、Queueにデータがあったら読み出して
//     textBox1に追加する。
//  
//------------------------------------------------------------------------------------
using System;
using System.Windows.Forms;

using System.Threading;     // Add
using System.Collections;   // Queue


namespace TestMoniAndQ
{
    public partial class Form1 : Form
    {
        Queue myQ;
        Thread thread1;

        public Form1()
        {
            InitializeComponent();

            thread1 = null;
            myQ = new Queue();  // キュー生成
        }


        /**
　       *  @brief      スレッド、タイマ開始ボタン処理
　       *  @param[in]  object  sender
　       *  @param[in]  EventArgs    e
　       *  @return     void
　       *  @note       timer1開始、スレッド起動
　       */
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            thread1 = new Thread(new ThreadStart(myThreadFunc));
            thread1.Start();

            timer1.Start();
        }

        /**
         *  @brief       独自スレッド関数
         *  @param[in]   void
         *  @return      void
         *  @note        lock取得しながら 約1秒間隔でキューに現在時間文字列を設定
         */
        void myThreadFunc()
        {
            int ii;

            try {
                for (ii = 0; ii < 10; ii++)
                {
                    Monitor.Enter(myQ);     // キュー待機

                    myQ.Enqueue(DateTime.Now.ToString());

                    Monitor.Exit(myQ);      // キュー解放
                    Thread.Sleep(1000);
                }
            }
            catch (ThreadAbortException ex) // Abortされたとき
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show("Thread Aborted.");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /**
         *  @brief      10m間隔タイマー処理
         *  @param[in]  object  sender
         *  @param[in]  EventArgs    e
         *  @return     void
         *  @note       lock取得しながら キューから文字列を取得し、textBox1に追加
         */
        private void timer1_Tick(object sender, EventArgs e)
        {
            string wkstr;

            Monitor.Enter(myQ);         // キュー待機

            if(myQ.Count>0)
            {
                wkstr = (string)(myQ.Dequeue()) + "\n"; // キューから取得
                textBox1.AppendText(wkstr);    
            }

            Monitor.Exit(myQ);          // キュー解放
        }

        /**
       　 *  @brief      アプリケーション終了処理
      　  *  @param[in]  object  sender
       　 *  @param[in]  EventArgs    e
       　 *  @return     void
       　 *  @note       timer1停止、スレッド起動していたら Abort
       　 */
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                timer1.Stop();

                if (thread1 != null)
                    thread1.Abort();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
