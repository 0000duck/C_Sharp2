//----------------------------------------------------------------------------
//  独自クラスを LINQ で制御できないかサンプル
//
//  [ 内容 ]
//  独自クラスを LINQ で制御できるようにできないか試しました。
//  (これが正解とは、かぎりませんのでご了承ください。)
//  データのクラスを MyDataクラス とし、それを LINQ で制御できるように
//  MyDataList クラスで 配列化し、制御してみました。
//  
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace TestMyIEnumerable
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        MyDataList mylist;

        public MainWindow()
        {
            InitializeComponent();
        }

        /**
          *  @brief      TopForm起動時処理
          *  @param[in]  object  sender
          *  @param[in]  RoutedEventArgs   e
          *  @return     void
          *  @note      MyDataList に MyData を 10個追加
          */
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int i;
            bool bret;

            mylist = new MyDataList(10);    // 10個までとして MyDataList 作成

            // MyDataList にデータ追加
            for (i = 0; i < 11; i++)        // わざと11個作成し、11個目は、無視されることを確認
            {
                MyData tmp = new MyData();
                tmp.data1 = i;
                bret = mylist.Add(tmp);
            }
        }

        /**
         *  @brief      data1が5以上の MyDataクラスを取得 textBoxに表示する ボタン処理
         *  @param[in]  object  sender
         *  @param[in]  RoutedEventArgs   e
         *  @return     void
         */
        private void Btn_GetMyData_Click(object sender, RoutedEventArgs e)
        {
            string wkstr;

            // ★ LINQ を使って MyDataList を操作
            var fiveover = from item in mylist
                           where item.data1 >= 5    // data1 が 5以上のみに絞り込み
                           select item;

            // 絞り込んだデータをtextBoxに表示
            foreach( var n in fiveover)
            {
                wkstr = n.data1.ToString() + "\n";
                Console.WriteLine(n.data1.ToString());
                textBox.AppendText(wkstr);
            }

            // MyDataList もすべて textBoxに表示させてみた
            textBox.AppendText("---- all MyDataList -----------------\n");

            foreach ( var n in mylist)
            {
                wkstr = n.data1.ToString() + "\n";
                Console.WriteLine(n.data1.ToString());
                textBox.AppendText(wkstr);
            }
        }
    }

   /**
    *  @brief      動作確認用 適当なData クラス 
    *  @note       Dataクラスを IEnumerable で制御できないか確認するためのクラス
    */
    public class MyData
    {
        public int data1 { get; set; }
    }

　　/**
　　 *  @brief      MyDataクラスを List保持するクラス
　　 *  @note       MyDataクラスを List保持し、IEnumerable で、アクセス可能にしてみた
 　　*/
    public class MyDataList : IEnumerable<MyData>
    {
        List<MyData>_list;  // MyData List
        int next_index;     // _list の現在 Add数
        int max;            // _list の最大Add可能個数

        /**
　　     *  @brief      MyDataListクラスコンストラクタ
         *  @param[in]  int _max    MyDataクラスの最大List数
　　     *  @note       max で指定された分の MyDataクラスList枠生成
　　     */
        public MyDataList(int _max)
        {
            next_index = 0;
            max = _max;
            _list = new List<MyData>();   
        }

        /**
　　     *  @brief      MyDataをListに追加
         *  @param[in]  MyData  MyDataクラス
         *  @return     bool true:Add OK, false: Add failure
　　     *  @note       max で指定された個数以上 Addすると falseで戻る
　　     */
        public bool Add(MyData data)
        {
            if (max <= next_index) {
                return false;
            }
            else {
                _list.Add(data);    // MyDataクラスを追加
                next_index++;
                return true;
            }
        }

        /**
　　     *  @brief      MyData List を Emuerator で返す
         *  @return     コレクションを反復処理する列挙子を返す。
　　     *  @note       MyData List の列挙を返す。
　　     */
        public IEnumerator<MyData> GetEnumerator()
        {
            for (int i = 0; i < next_index; i++)
                yield return _list[i];              // yield で _list[i]１つづつ、return を繰り返す
        }


        /**
         *  @brief  IEnumerable.GetEnumerator()の実装
         *  @return IEnumerator<T>
         *  @note   IEnumerableインターフェイス継承のためIEnumerable.GetEnumerator() を実装 が必要で
         *          返したい列挙を返す、独自Method を callするようにしている
         */
        System.Collections.IEnumerator
            System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
       
        
    }
}
