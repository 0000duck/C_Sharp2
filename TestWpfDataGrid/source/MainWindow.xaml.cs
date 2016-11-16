//-------------------------------------------------------------------
// WPF で DataGridView を試したサンプル
//
//  [ 内容 ]
//    VS2010 TestDataGrid で試した内容を
//    WPF で実現するとどうなるか試したものです。
//    WPF では、DataTable と DataGrid をつなぐのがふつう見たいなので
//    今回の方法は、かなり強引なやり方かもしれません。
//    また、VS2010 では、セルを自由にマウスで選択できましたが
//    今回は、行単位でしか選択できませんでした。(方法みつからず)
//    Ctrlを押しながらクリックした行のセルは自由に選択できます。
//    そのため、選択したセルの表示も　行単位での処理になっています。
//    マウスで選択してもtextBox2 に表示するので、Ctrl+マウスのときは
//    textBox2 に表示されたあと、Clearボタン多し、その後
//   ShowSelectData to textBoxボタンを押すと、それぞれの処理が確認しやすいです。
//   セルに ContextMenu入りつけるのは XAML側(style書いたり)でもかなりチャレンジしましたが
//   セルごとに Menu出すところまではいけたのですが、Menu選択後 Event発生せず
//   今回は断念しました。 
//   以下その一部
//    <DataGrid.Resources>
//        <Style TargetType = "DataGridCell" >
//            < Setter Property="ContextMenuService.ContextMenu">
//                <Setter.Value>
//                    <ContextMenu>
//                    <MenuItem Header = "Menu 1" Click="MyDataGrid_ContextMenu1_Click"/>
//                    <MenuItem Header = "Menu 2" Click="MyDataGrid_ContextMenu2_Click"/>
//                    </ContextMenu>
//                </Setter.Value>
//            </Setter>
//        </Style>
//
//-------------------------------------------------------------------
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



namespace TestWpfDataGrid
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();


            // 各種設定
            dtGrid1.AutoGenerateColumns = false;    // 列が自動的に作成されないようにする
            dtGrid1.CanUserDeleteRows = false;      // 行をユーザーが削除できないようにする
            dtGrid1.CanUserResizeColumns = false;   // 列の幅をユーザーが変更できないようにする
            dtGrid1.CanUserResizeRows = false;      // 行の高さをユーザーが変更できないようにする
                                                    //dtGrid1.RowHeadersVisible = false;    // 行ヘッダ非表示

            // 列ヘッダーの高さを任意に変えるためにColumnHeadersHeightSizeModeを設定
            //dtGrid1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dtGrid1.ColumnHeaderHeight = 30;        // 列ヘッダの高さ指定
            dtGrid1.RowHeaderWidth = 40;            // 行ヘッダの幅指定

            dtGrid1.HeadersVisibility = DataGridHeadersVisibility.Column;   // 列ヘッダのみ表示
        }


        /**
         *  @brief      SetDataボタン処理
         *  @param[in]  object  sender
         *  @param[in]  EventArgs   e
         *  @return     void
         *  @note       6行4列のデータを dataGird1 に設定
         */
        private void button_Click(object sender, RoutedEventArgs e)
        {
            // 4列 Headerのみ作成
            System.Data.DataTable dt_tbl = new System.Data.DataTable("testTbl");    // WPF only
            for (int i = 0; i < 4; i++)
            {
                //DataGridViewTextBoxColumn wkColumn = new DataGridViewTextBoxColumn();
                DataGridTextColumn wkColumn = new DataGridTextColumn();

                //wkColumn.Name = "Column" + i.ToString();
                wkColumn.Binding = new Binding("Column" + i.ToString());

                //wkColumn.HeaderText = "Column" + i.ToString();
                wkColumn.Header = "Column" + i.ToString();

                //wkColumn.SortMode = DataGridViewColumnSortMode.NotSortable;   // Header押しても列でソートさせない。
                wkColumn.CanUserSort = false;

                wkColumn.IsReadOnly = true;         // ReadOnly
                wkColumn.Width = 100;               // 列の幅指定

                dtGrid1.Columns.Add(wkColumn);      // DataGrid に 列 設定

                // DataGrid用の Table にも列 設定
                dt_tbl.Columns.Add(new System.Data.DataColumn("Column" + i.ToString(), typeof(string)));
            }

            // 6行4列データ作成
            System.Data.DataRow wkRow;
            for (int j = 0; j < 6; j++)         // 行
            {
                wkRow = dt_tbl.NewRow();

                for (int i = 0; i < 4; i++)     // 列
                {
                    // wkRow[i] でも設定できるが、Binding名で指定
                    wkRow["Column" + i.ToString()] = "data_" + j.ToString() + i.ToString();
                }
                dt_tbl.Rows.Add(wkRow);
            }


            dtGrid1.DataContext = dt_tbl;


            //dtGrid1.RowHeadersVisible = false;  // 三角マーク非表示 (三角マークの列も消えてしまう)

            //dtGrid1.FirstDisplayedScrollingRowIndex = dtGrid1.RowCount - 1; // 最終行へスクロール
            //dtGrid1.CurrentCell = dtGrid1[0, 5];    // フォーカスを 5行0列目に移動
            //dtGrid1.ScrollIntoView(dtGrid1.Items.GetItemAt(dtGrid1.Items.Count - 1));
            // 上記が使えなかったので以下を採用
            var border = VisualTreeHelper.GetChild(dtGrid1, 0) as Decorator;
            if (border != null)
            {
                var scroll = border.Child as ScrollViewer;
                if (scroll != null) scroll.ScrollToEnd();
            }

            // (注)Columns, Rows 追加後に以下を禁止しないで先にやると追加できない
            //dtGrid1.AllowUserToAddRows = false;     // 最終新規行非表示
            dtGrid1.CanUserAddRows = false;
        }


        /**
         * @brief   　 MouseUp時、Cellをクリックされていたら、そのセルの行と列を取得し
         *             labelにそれぞれ表示する MouseUpEvent処理
         * @param[in]  object     e
         * @param[in]  EventArgs  e 
         * @return     void   
         * @note       VisualTreeHelperを使ってDataGridCellとDataGridRowをたぐってcolumnとrowを調べる
        */
        private void dtGrid1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int column = 0;
            int row = 0;


            HitTestResult r = VisualTreeHelper.HitTest(dtGrid1, e.GetPosition(dtGrid1));
            if (r != null)
            {
                // Object をたどって、DataGridCell.Column.DisplayIndex で 列取得

                DependencyObject depObj = r.VisualHit;

                //while (!(depObj is DataGridCell || depObj is DataGridRow))
                while (!(depObj is DataGridCell))
                {
                    if (depObj == null)
                        return;
                    depObj = VisualTreeHelper.GetParent(depObj);
                }

                if (depObj is DataGridCell)
                {
                    var cell = depObj as DataGridCell;
                    column = cell.Column.DisplayIndex;
                    label1.Content = column.ToString();
                }

                // DataGridRow でも探しして、Index で行取得
                DependencyObject depObj2 = r.VisualHit; // <--同じ層らしく、これ無で depObjで以下実行可能だった
                while (!(depObj2 is DataGridRow))
                {
                    if (depObj2 == null)
                    {
                        return;
                    }
                    depObj2 = VisualTreeHelper.GetParent(depObj2);
                }
                DataGridRow dgr = depObj2 as DataGridRow;
                row = dgr.GetIndex();
                label4.Content = row.ToString();
            }
        }


        /**
         *  @brief      セルデータ Copy処理 Menu選択処理
         *  @param[in]  object  sender
         *  @param[in]  MouseButtonEventArgs   e
         *  @return     void
         *  @note       選択した セルのデータを textBox1 に Copy
         */
        private void MyDataGrid_ContextMenu1_Click(object sender, MouseButtonEventArgs e)
        {
            if (dtGrid1.CurrentCell.Column != null)
            {
                int c = dtGrid1.CurrentCell.Column.DisplayIndex;
                int r = dtGrid1.Items.IndexOf(dtGrid1.SelectedItem);
                TextBlock txtBlk = (TextBlock)(dtGrid1.Columns[c].GetCellContent(dtGrid1.SelectedItem));
                textBox1.Text = txtBlk.Text;
            }
        }

        /**
         *  @brief      Dummy Menu選択処理
         *  @param[in]  object  sender
         *  @param[in]  MouseButtonEventArgs   e
         *  @return     void
         *  @note       Dummy Menu用なので、特に処理なし
         */
        private void MyDataGrid_ContextMenu2_Click(object sender, MouseButtonEventArgs e)
        {
            ;
        }


        /**
        　*  @brief      dtGrid1 データ作成時処理
        　*  @param[in]  object  sender
        　*  @param[in]  DataGridRowEventArgs   e
        　*  @return     void
        　*  @note       セルデータ作成時に、Menu機能も追加するための処理
          *              本Methodは dtGrid1 のデザイナから EventMethod追加したもの
        　*/
        private void dtGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            // 右クリックメニューの追加。XAMLでやると表示されない?ので、LoadingRowのタイミングで行った

            DataGridRow datagrid_row = e.Row;
            //var viewmodel = datagrid_row.DataContext as ListItem_ViewModel;
            //if (viewmodel == null) return;

            //datagrid_row.MouseRightButtonUp += new MouseButtonEventHandler(dtGrid1_MouseRightButtonUp);
            //datagrid_row.MouseLeftButtonUp  += new MouseButtonEventHandler(dtGrid1_MouseLeftButtonUp);

            //  メニュー追加
            ContextMenu cm = new ContextMenu();

            // Menu1 追加
            MenuItem m1 = new MenuItem();
            m1.Header = "Copy Data";
            //m1.Background = new SolidColorBrush(Colors.LightGray);
            //m1.MouseLeftButtonUp += new MouseButtonEventHandler(MyDataGrid_ContextMenu1_Click);
            // ★ 上記ではEvent起きず、下記のようににして、第3引数 true にしたら Event発生
            //    ( 第3引数をtrue は、処理済みのルーティングイベントにも反応とのこと )
            m1.AddHandler(MenuItem.MouseLeftButtonUpEvent, new MouseButtonEventHandler(MyDataGrid_ContextMenu1_Click), true);
            cm.Items.Add(m1);
            //cm.Background = new SolidColorBrush(Colors.LightGray);

            //セパレータ
            cm.Items.Add( new Separator() );

            // Menu2 追加
            MenuItem m2 = new MenuItem();
            m2.Header = "Dummy Menu";
            //m2.MouseLeftButtonUp += new MouseButtonEventHandler(MyDataGrid_ContextMenu2_Click);
            m2.AddHandler(MenuItem.MouseLeftButtonUpEvent, new MouseButtonEventHandler(MyDataGrid_ContextMenu2_Click), true);
            cm.Items.Add(m2);
            
            //cm.MouseLeftButtonUp += new MouseButtonEventHandler(MyDataGrid_ContextMenu1_Click);
            ContextMenuService.SetContextMenu(datagrid_row, cm);
        }

        /**
         *  @brief      ShowSelectData to textBox ボタン処理
         *  @param[in]  object  sender
         *  @param[in]  EventArgs   e
         *  @return     void
         *  @note       選択された Cellデータを textBox2に表示
         */
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            /*
            int c;
            TextBlock txtBlk;
            
            //int[]rows = dtGrid1.SelectedItems.OfType<DataGridCell>().Select(i => dtGrid1.Items.IndexOf(i)).ToArray();
            

            // 複数行選択の場合、1つの行分しか取得できてこない。調査中。
            foreach (DataGridCellInfo cell in dtGrid1.SelectedCells)
            {

                c = cell.Column.DisplayIndex;   // 列 取得
                txtBlk = (TextBlock)(dtGrid1.Columns[c].GetCellContent(dtGrid1.SelectedItem));

                textBox2.AppendText(txtBlk.Text + "\n");
            }
            */

            textBox2.Clear();

            int rowMax = dtGrid1.Items.Count;       // 存在する行数
            int columnMax = dtGrid1.Columns.Count;  // 存在する列数

            int row;
            int column;
            DataGridRow dgr;
            string wkstr;

            // 全行サーチ
            for (row = 0; row < rowMax; row++)
            {
                dgr = dtGrid1.ItemContainerGenerator.ContainerFromIndex(row) as DataGridRow;
                // 選択された行のみ取得。
                if (dgr.IsSelected == true)
                {
                    // 1行前列で表示。　本当は　個別の列選択も取得したいが、できない？(不明)
                    for (column = 0; column < columnMax; column++)
                    {
                        wkstr = ((TextBlock)dtGrid1.Columns[column].GetCellContent(dgr)).Text;
                        textBox2.AppendText(wkstr + "\n");
                    }
                }
            }
        }



        /**
         *  @brief      セル選択時Event処理
         *  @param[in]  object  sender
         *  @param[in]  SelectedCellsChangedEventArgs   e
         *  @return     void
         *  @note       マウスセル選択時に選択データをtextBox1に追加する
         */
        private void dtGrid1_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            int columnCnt;                          // 選択された列数
            int row = -1;                           // 選択された行


            // 列用で、今回は 4列しかないので 4枠とった
            List<int> cindex = new List<int>(4);    // 選択された列の位置(0起算。 -1:none)
                cindex.Add(-1);
                cindex.Add(-1);
                cindex.Add(-1);
                cindex.Add(-1);

            // 列の位置を取得
            columnCnt = e.AddedCells.Count;     // 選択された列の個数
            if (columnCnt == 0)
                return;

            for(int i=0; i< columnCnt; i++)
            {
                cindex[i] = dtGrid1.Columns.IndexOf(e.AddedCells[i].Column);
            }


            // 選択されたセルのある行に入っているオブジェクトを取得
            object obj = e.AddedCells[0].Item;

            // 選択された行のObjectと一致するところが選択された行としている
            for (int i = 0; i < dtGrid1.Items.Count; i++)
            {
                if (dtGrid1.Items[i] == obj)
                {
                    row = i;
                    break;
                }
            }
            if (row == -1)
                return;

            // DataGridの行オブジェクトを取得
            DataGridRow dgr = dtGrid1.ItemContainerGenerator.ContainerFromIndex(row) as DataGridRow;

            // 選択されているセルの内容を取得
            string wkstr;
            for (int i = 0; i < columnCnt; i++)
            {
                wkstr = ((TextBlock)e.AddedCells[i].Column.GetCellContent(dgr)).Text;
                textBox2.AppendText(wkstr + "\n");
            }
        }

        /**
         *  @brief      textBox2クリア ボタン処理
         *  @param[in]  object  sender
         *  @param[in]  EventArgs   e
         *  @return     void
         *  @note       textBox2の内容クリア
         */
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            textBox2.Clear();
        }
    }


}

