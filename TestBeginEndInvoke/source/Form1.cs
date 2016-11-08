//------------------------------------------------------------------------------
//    BeginInvoke/EndInvoke 確認サンプル
//
//    [ 内容 ]
//      非同期実行用に MyAsyncClass 用意。実際には orignalMethod が実行される。
//      button1 で、myAsyncCls.originalMethod を delegate ととし、
//      それの BeginInvoke で非同期処理開始。と同時に終了時の callbackMethodを登録
//      ( MyCallBackMethod() )。　callback後、EndInvoke で非同期処理の return値を取得。
//      また同様に、 orignalMethod2も BeginInvokeして、終了時同じ Method を callBackしている。
//      非同期実行なので、その間 DummyButtonが押すことができることも確認できるようにしている。
//------------------------------------------------------------------------------

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


namespace TestBeginEndInvoke
{
    public partial class Form1 : Form
    {
        delegate string string_int_delegate(int wait);      // BeginInvoke用
        delegate void void_string_delegate(string str);     // textBox1アクセス用

        public Form1()
        {
            InitializeComponent();
        }


        /**
         *  @brief      非同期処理開始 ボタン処理
         *  @param[in]  object  sender
         *  @param[in]  EventArgs    e
         *  @return     void
         *  @note       BeginInvoke で myAsyncCls.originalMethod 開始
         */
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            // 別クラスの Methodを非同期実行
            MyAsyncClass myAsyncCls = new MyAsyncClass();
            string_int_delegate dlgt = new string_int_delegate(myAsyncCls.originalMethod);

            // 非同期処理呼び出し、終了時の callback Method登録
            dlgt.BeginInvoke(3000, new AsyncCallback(MyCallBackMethod), dlgt);


            // 自クラスの Methodを非同期実行
            string_int_delegate dlgt2 = new string_int_delegate(SeltOriginalMethod);

            // 非同期処理呼び出し、終了時の callback Method登録
            dlgt2.BeginInvoke(6000, new AsyncCallback(MyCallBackMethod), dlgt2);
        }


        /**
         *  @brief      非同期終了時にで呼ばれるように登録して使用するメソッド
         *  @param[in]  IAsyncResult    非同期実行したdelegate
         *  @return     void
         *  @note       EndInvoke で 非同期実行した Methodのreturn値を取得し textBox1 に設定
         */
        private void MyCallBackMethod(IAsyncResult ar)
        {
            string_int_delegate dlgt = (string_int_delegate)ar.AsyncState;


            //textBox1.Text = dlgt.EndInvoke(ar);
            SetTextBox1(dlgt.EndInvoke(ar));        // 結果を取得
        }


        /**
         *  @brief      非同期として、BeginInvokeで呼ばれる
         *  @param[in]  int     Sleep時間(msec)
         *  @return     string  終了を示す文字列
         *  @note       
         */
        string SeltOriginalMethod(int wait)
        {
            Thread.Sleep(wait);     // 重い処理のつもり

            return "SelfOriginalMethod End";
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
     *  @brief      独自 非同期実行用クラス
     */
    public class MyAsyncClass
    {
        /**
         *  @brief      非同期として、BeginInvokeで呼ばれる
         *  @param[in]  int     Sleep時間(msec)
         *  @return     string  終了を示す文字列
         *  @note       
         */
        public string originalMethod(int wait)
        {
            Thread.Sleep(wait);     // 重い処理のつもり

            return "OriginalMethod End";
        }
    }

}
