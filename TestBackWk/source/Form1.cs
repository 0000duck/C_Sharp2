//-----------------------------------------------------
//  BackGroundWorker 確認サンプルプログラム
//
//  backGroundWorker を使用し、1秒ごとに
//  ReportProgress で textBox を更新する。
//  排他制御 lock の確認 myThreadFunc と button2_Click で lock/unlock
//  backGroundWorkerの確認と thread2 の lockの確認は、別々で行わないと
//  動作がわかりにくいです。 ごめんなさい。
//  (Getlock で 待つ間、textBox1の更新が止まるため。)
//
//-----------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;


namespace TestBackWk
{
    public partial class Form1 : Form
    {

        int count;  // backgroundで CountUpして、backWork1_ProgressChanged で数値を TextBoxに表示

        int count2; // thread1 sleep count用
        Thread thread1;
        readonly Object LockObj;


        //
        public Form1()
        {
            InitializeComponent();

            count = 0;
            LockObj = new Object();     // Lock用
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

            // background 確認用
            backWork1.WorkerReportsProgress = true;     // backWork1_ProgressChanged有効に
            backWork1.RunWorkerAsync();                 // BackGround Worker Start

            // lock確認用
            thread1 = new Thread(new ThreadStart(myThreadFunc));
            thread1.Start();
        }

        /**
         *  @brief      backgroundWorker 処理本体
         *  @param[in]  object      sender
         *  @param[in]  DoWorkEventArgs   e
         *  @return     void
         *  @note       100msec ごとにカウントし、約1sec で ReportProgress()を
         *              使用して、textBox1 に count値表示
         */
        private void backWork1_DowWork(object sender, DoWorkEventArgs e)
        {
            int c;

            c = 0;

            while (c <= 50)     // 約 5秒やったら終了
            {
                c++;
                if (c % 10 == 0)
                {
                    count++;
                    backWork1.ReportProgress(1);    // backWork1_ProgressChanged() 呼び出し。 %値を1固定にしている。
                }
                Thread.Sleep(100);

                if (backWork1.CancellationPending)  // CancelAsync() で true になる
                {
                    e.Cancel = true;
                    return;
                }
            }
        }


        /**
         *  @brief      backgroundWorker 処理本体
         *  @param[in]  object      sender
         *  @param[in]  ProgressChangedEventArgs   e
         *  @return     void
         *  @note       textBox1に count値反映
         */
        private void backWork1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int pt = e.ProgressPercentage;  // %値、読み出してみただけ。

            textBox1.Text = "set backWork1. count="+count.ToString();
        }


        /**
         *  @brief      backgroundWorker 終了時処理
         *  @param[in]  object      sender
         *  @param[in]  RunWorkerCompletedEventArgs   e
         *  @return     void
         */
        private void backWork1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("backWork1 end.");
        }

        /**
         *  @brief      lock 確認用スレッド
         *  @return     void
         *  @note       5sec 起きに count up
         */
        void myThreadFunc()
        {
            
            count2 = 0;

            while (count2 < 2)
            {
                lock (LockObj)  // lock
                {
                    count2++;
                    Thread.Sleep(5000);     // 5sec Lock を keep

                }               // unlock
            }
            Console.WriteLine("thread2 exit.");
        }

        /**
         *  @brief      button2 処理
         *  @param[in]  object      sender
         *  @param[in]  EventArgs   e
         *  @return     void
         *  @note       myThreadFunc とで LockObj の取りあいを確認するために用意したもの
         */
        private void button2_Click(object sender, EventArgs e)
        {
            int c;

            lock (LockObj)  // lock
            {
                c = count2; // count2参照(参照のみなので、本当は lockいらないかおｍ)
                            // lock 確認用に単に書いてみただけ

            }               // unlock
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
            // backWork1 動作中なら終了要求
            if (backWork1 != null)
            {
                backWork1.WorkerSupportsCancellation = true;    // これにしないと CancelAsyncできないらしい。
                backWork1.CancelAsync();                        // CancellationPending trueへ
            }

            // backWork 処理中なら Closing処理キャンセル
            if (backWork1.IsBusy)
            {
                e.Cancel = true;
            }
            else
            {
                // thread1 終了まで待ってから終了
                Console.WriteLine("wait thread2 end. ");
                if(thread1!=null)
                    thread1.Join();
                Console.WriteLine("app end.");
            }
        }

        
    }
}
