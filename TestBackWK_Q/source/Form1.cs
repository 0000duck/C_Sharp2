//------------------------------------------------------------------
//  BackGroundWorker で Queue を使った場合の確認サンプルプログラム
//
//  [ 内容 ]
//    BackGroundWorker で 約50msecごとに  キューにカウント値を入れ、
//   ReportProgress()側で、キューからカウント値読み出し、
//   textBox1 に count値表示(100回実行し終了)
//------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Windows.Forms;

using System.Threading;
using System.Collections;


namespace TestBackWk_Q
{
    public partial class Form1 : Form
    {
        Queue myQ;

        public Form1()
        {
            InitializeComponent();

            myQ = new Queue();                      // キュー作成
            backWork1.WorkerReportsProgress = true; // 進捗報告
        }

        /**
         *  @brief      backgroundWorker 開始ボタン処理
         *  @param[in]  object      sender
         *  @param[in]  EventArgs   e
         *  @return     void
         */
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            backWork1.RunWorkerAsync();             // backgroundWorker開始
        }


        /**
         *  @brief      backgroundWorker 処理本体
         *  @param[in]  object      sender
         *  @param[in]  DoWorkEventArgs   e
         *  @return     void
         *  @note       約50msec で キューにカウント値を入れ、ReportProgress()を
         *              使用して、textBox1 に count値表示　(100回実行し終了)
         */
        private void backWork1_DoWork(object sender, DoWorkEventArgs e)
        {
            int ii;
            int counter = 0;

            try
            {
                for(ii=0; ii<100; ii++)
                {
                    counter++;
                    Thread.Sleep(50);

                    Monitor.Enter(myQ);     // lock
                        myQ.Enqueue(counter.ToString());
                        backWork1.ReportProgress(0);    // 確認してみたが、この行実行してもすぐ
                                                        // backWork1_ProgressChanged へ移らない。 Monitor.Exit後、後の後ぐらいでようやく動いた
                    Monitor.Exit(myQ);      // unlock
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }


        /**
         *  @brief      backgroundWorker からの通知処理
         *  @param[in]  object      sender
         *  @param[in]  RunWorkerCompletedEventArgs   e
         *  @return     void
         *  @note       キューから読み出し、textBox1に設定
         */
        private void backWork1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                Monitor.Enter(myQ);         // lock
                    while(myQ.Count > 0)
                    {
                        textBox1.Text = (string)myQ.Dequeue();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Monitor.Exit(myQ);          // unlock
            }
        }


        /**
         *  @brief      終了 処理
         *  @param[in]  object      sender
         *  @param[in]  FormClosingEventArgs   e
         *  @return     void
         *  @note       
         */
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backWork1.IsBusy)
            {
                e.Cancel = true;
            }
        }
    }
}
