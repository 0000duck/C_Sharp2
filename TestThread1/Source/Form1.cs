//----------------------------------------------------------------------
//  スレッド機能確認サンプル
//
//  thread1：スレッド Abort 確認
//  thread2：スレッドへの引数設定とcallback確認
//  thread3：IsBackground　確認
//
//----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;


namespace TestThread1
{
    // 使用する delegate の宣言
    public delegate void void_string_delegate(string str);


    public partial class Form1 : Form
    {
        Thread thread1;
        Thread thread2;
        Thread thread3;

        public Form1()
        {
            InitializeComponent();

            Btn_ThreadAbt.Enabled = false;
        }

        /**
         *  @brief       独自スレッド関数
         *  @param[in]   void
         *  @return      void
         *  @note        重い処理の変わりに 5sec sleep
         */
        void myThreadFunc()
        {
            Thread.Sleep(5000); // 重い処理のつもり
        }

        /**
         *  @brief       独自スレッド関数
         *  @param[in]   void
         *  @return      void
         *  @note        重い処理の変わりに 20sec sleep
         */
        void myThreadFunc2()
        {
            Thread.Sleep(20000); // 重い処理のつもり

            // 30秒の間に本アプリを終了させれば以下表示されない
            MessageBox.Show("myThreadFunc2 end.");
        }

        /**
         *  @brief      スレッド開始ボタン処理
         *  @param[in]  object  sender
         *  @param[in]  EventArgs    e
         *  @return     void
         *  @note       スレッド開始
         */
        private void Btn_ThreadStart_Click(object sender, EventArgs e)
        {
            Btn_ThreadStart.Enabled = false;
            Btn_ThreadAbt.Enabled = true;

            // Thread abort 確認用スレッド開始
            thread1 = new Thread(new ThreadStart(myThreadFunc));
            thread1.Start();

            // スレッドに情報を渡すため、myThreadClass を使用しスレッド開始
            myThreadClass threadCls = new myThreadClass("Thread2 1.", 3000, this, SetTextBox1);
            thread2 = new Thread(new ThreadStart(threadCls.myThreadFunc));
            thread2.Start();

            // Thread IsBackground 確認用スレッド開始
            thread3 = new Thread(new ThreadStart(myThreadFunc2));
            thread3.IsBackground = true;    // ★フォアグラウンドスレッド停止時に、このスレッドも終了することになる。
            thread3.Start();
        }

        /**
         *  @brief      スレッド停止ボタン処理
         *  @param[in]  object  sender
         *  @param[in]  EventArgs    e
         *  @return     void
         *  @note       スレッド終了、終了待ち
         */
        private void Btn_ThreadAbt_Click(object sender, EventArgs e)
        {
            bool bret;

            thread1.Abort();     // thread停止

            bret = thread1.Join(1000);
            if (bret == true)
                Console.WriteLine("thread end.");
            else
                Console.WriteLine("thread timeout end.");
            

            Btn_ThreadAbt.Enabled = false;
        }


        /**
         *  @brief      スレッド から textBox1 に設定する関数
         *  @param[in]  string  設定文字列
         *  @return     void
         *  @note       textBox1 に文字列設定
         */
        public void SetTextBox1(string value)
        {
            if (InvokeRequired)
            {
                Invoke(new void_string_delegate(SetTextBox1), new object[] { value });
            }
            else
            {
                textBox1.Text = value;
            }
        }
    }

    /**
     *  @brief      独自スレッド用クラス
     */
    public class myThreadClass
    {
        private string msg;
        private int wait;
        private Form1 topfrm;
        void_string_delegate callback;

        /**
         *  @brief      コンストラクタ
         *  @param[in]  string  設定文字列
         *  @param[in]  int     スレッドSleep時間(msec)
         *  @param[in]  Form1   topFormクラス Object
         *  @param[in]  void_string_delegate callbacメソッド登録
         *  @note       
         */
        public myThreadClass(string msg, int wait, Form1 topfrm, void_string_delegate func)
        {
            this.msg = msg;
            this.wait = wait;
            this.topfrm = topfrm;
            callback = func;
        }

        /**
         *  @brief      スレッド関数
         *  @note       コンストラクタで取得した、msg と wait を使用。
         */
        public void myThreadFunc()
        {
            topfrm.SetTextBox1(msg);
            Thread.Sleep(wait);

            callback("Thread2 2.");     // スレッド起動元のメソッドを call
            Thread.Sleep(wait);
        }

    }
}
