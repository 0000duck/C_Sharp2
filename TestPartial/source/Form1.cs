//-----------------------------------------------------------------
//  Partial Class の機能確認サンプル
//
//  [ 内容 ]
//  partial で TestPartialクラスを作成し、そのクラス内のMethodで
//  partial機能を確認。 PartialMethod()実装がないが
//  コンパイルもできる。実行しても何もおきません。
//  以下 Partialクラスについて Webより取得した情報
//
//「パーシャル(Partial)クラス」内限定で、 
// メソッドに partial を付けると メソッドの宣言と定義を分けることができる。
//
//
// partial 修飾子を付けてメソッドを宣言
// 必ずパーシャルクラス内になければならない。
// private でなければならない。
// 戻り値は void 以外不可。
// 引数は自由に取れる。ref, this, params も利用可能。ただし、out 引数は不可。
// クラスメソッド（static）でもインスタンスメソッド（非 static）でも OK。
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPartial
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /**
         *  @brief      TestPartialクラス実行 ボタン処理
         *  @param[in]  object  sender
         *  @param[in]  EventArgs   e
         *  @return     void
         */
        private void button1_Click(object sender, EventArgs e)
        {
            // Partialクラス生成、実行メソッド実行
            TestPartial partialMthod_Cls = new TestPartial();
            partialMthod_Cls.testMethod();
        }
    }


    /**
      *  @brief      TestPartial クラス 
      *  @note       Partial機能確認クラス。PartialメソッドとNoPartrialメソッドを持つ
      */
    partial class TestPartial
    {
        /**
         *  @brief      PartialMethod
         *  @param[in]  void
         *  @return     void
         *  @note       Partial機能確認用
         */
        partial void PartialMethod();   // 実体なし、コンパイルできるし、実行しても何もおきない


        /**
         *  @brief      NoPartialMethod
         *  @param[in]  void
         *  @return     void
         *  @note       Partial機能でない通常のMethod
         */
        void NoPartialMethod1()
        {
            Console.WriteLine("executed NoPartialMethod1.");
            MessageBox.Show("executed NoPartialMethod1.");
        }

        /**
         *  @brief      testMethod
         *  @param[in]  void
         *  @return     void
         *  @note       PartialMethod と NoPartialMethod を実行
         */
        public void testMethod()
        {
            PartialMethod();        // 実行しても何もおきない
            NoPartialMethod1();
            NoPartialMethod2();
        }
    }


    /**
     *  @brief      TestPartial クラス 
     *  @note       TestPartialクラスに Method　を追加できる機能の確認用
     */
    partial class TestPartial
    {
        /**
         *  @brief      NoPartialMethod2
         *  @param[in]  void
         *  @return     void
         *  @note       Partial機能でない通常のMethod
         */
        void NoPartialMethod2()
        {
            Console.WriteLine("executed NoPartialMethod2.");
            MessageBox.Show("executed NoPartialMethod2.");
        }
    }
}

