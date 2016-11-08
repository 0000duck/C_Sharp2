//------------------------------------------------------------
//  スレッド優先度確認サンプル
//
//  thread Hi/Low/Normal で優先度を確認
//  各スレッドは、callback を使用して textBox1 にそれぞれ文字列追加
//  textBox を参照することで、どのスレッドが優先的に動作したか確認できる
//  といった考えでサンプルを作成しています。
//  (試してみると、Hiはそれなりに有効みたいですが
//   他の優先度の差はあまりでなかったり、でたりしています。
//  ★細かいスケジューリングはOSに依存するので注意。）
//
//------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;


namespace TestThreadPriority
{
    public delegate void void_string_delegate(string str);


    public partial class Form1 : Form
    {
        Thread threadHi;
        Thread threadNrml;
        Thread threadLow;

        //----------------------------------------------------------------------------------
        /**
         *  @brief      確認用 TopFormクラス
         */
        //----------------------------------------------------------------------------------
        public Form1()
        {
            InitializeComponent();
        }

        /**
         *  @brief      スレッド開始ボタン処理
         *  @param[in]  object  sender
         *  @param[in]  EventArgs    e
         *  @return     void
         *  @note       スレッド開始
         */
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            // 優先度Low/Normal/Hi でスレッド生成、開始
            myThreadClass threadLowCls = new myThreadClass("Thread Low.", AddTextBox1);
            threadLow = new Thread(new ThreadStart(threadLowCls.myThreadFunc));
            threadLow.Priority = ThreadPriority.Lowest;
            threadLow.Start();

            myThreadClass threadNmlCls = new myThreadClass("Thread Nrml.", AddTextBox1);
            threadNrml = new Thread(new ThreadStart(threadNmlCls.myThreadFunc));
            threadNrml.Priority = ThreadPriority.Normal;
            threadNrml.Start();

            myThreadClass threadHiCls = new myThreadClass("Thread Hi.", AddTextBox1);
            threadHi = new Thread(new ThreadStart(threadHiCls.myThreadFunc));
            threadHi.Priority = ThreadPriority.Highest;
            threadHi.Start();
        }

        /**
         *  @brief      スレッド から textBox1 に追加する関数
         *  @param[in]  string  設定文字列
         *  @return     void
         *  @note       textBox1 に文字列設定
         */
        public void AddTextBox1(string value)
        {
            if (InvokeRequired)
            {
                Invoke(new void_string_delegate(AddTextBox1), new object[] { value });
            }
            else
            {
                textBox1.AppendText(value);
            }
        }
    }

    //----------------------------------------------------------------------------------
    /**
     *  @brief      独自スレッド用クラス
     */
    //----------------------------------------------------------------------------------
    public class myThreadClass
    {
        string msg;
        void_string_delegate callback;

        /**
         *  @brief      コンストラクタ
         *  @param[in]  string  設定文字列
         *  @param[in]  void_string_delegate callbacメソッド登録
         *  @note       
         */
        public myThreadClass(string _msg,  void_string_delegate func)
        {
            this.msg = _msg;
            this.callback = func;
        }

        /**
         *  @brief      スレッド関数
         *  @note       コンストラクタで取得した、msg 使用。
         *              50回処理して終了
         */
        public void myThreadFunc()
        {
            string wkstr;
            int c;

            c = 0;

            while (c < 50)
            {
                wkstr = msg + "\n";
                callback(wkstr);
                Thread.Sleep(0);
                c++;
            }
        }
    }
}
