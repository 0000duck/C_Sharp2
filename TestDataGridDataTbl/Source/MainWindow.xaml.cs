//----------------------------------------------------
//  DataTable を使った DataGrid のサンプル
//
//  [ 内容 ]
//   DataTable にデータを作成し、DataGrid に Bindして
//   DataTableの内容を DataGridに表示する。
//   Dataの追加削除は、基本 DataTable を制御する。
//----------------------------------------------------
using System;
using System.Windows;

using System.Text.RegularExpressions;   // add


namespace TestWpfDataGridDataTbl
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Data.DataTable dtTbl;

        public MainWindow()
        {
            InitializeComponent();

            dtTbl = new System.Data.DataTable();    // DataTable枠作成

            Btn_AddData.IsEnabled = false;
            BtnDelLastData.IsEnabled = false;
            Btn_ChgData.IsEnabled = false;
        }


        /**
         *  @brief      SetDataボタン処理
         *  @param[in]  object  sender
         *  @param[in]  EventArgs   e
         *  @return     void
         *  @note       10行4列のデータを dataGird1 に設定
         */
        private void Btn_SetData_Click(object sender, RoutedEventArgs e)
        {
            // DataTable 列の Header作成
            dtTbl.Columns.Add("Column0");
            dtTbl.Columns.Add("Column1");
            dtTbl.Columns.Add("Column2");
            dtTbl.Columns.Add("Column3");

            // DataTable 10行4列データ作成
            for (int i = 0; i < 10; i++)
            {
                var row = dtTbl.NewRow();
                for (int j = 0; j < dtTbl.Columns.Count; j++)
                {
                    row[j] = "r=" + i.ToString() + " ," + "column=" + j.ToString() ;
                }
                dtTbl.Rows.Add(row);
            }

            // DataTable を DataGrid に反映
            this.dataGrid1.DataContext = dtTbl;     // ItemsSource は Binding 指定 なので、ここで DataTable と Binding

            Btn_AddData.IsEnabled = true;
            BtnDelLastData.IsEnabled = true;
            Btn_ChgData.IsEnabled = true;
        }

        /**
         *  @brief      AddDataボタン処理
         *  @param[in]  object  sender
         *  @param[in]  EventArgs   e
         *  @return     void
         *  @note       最終行にデータ追加
         */
        private void Btn_AddData_Click(object sender, RoutedEventArgs e)
        {
            // 最終の行数を見つけて、全列分追加
            int max = dtTbl.Rows.Count;
            var row = dtTbl.NewRow();

            for (int j = 0; j < dtTbl.Columns.Count; j++)
            //for (int j = 1; j < 3; j++)
            {
                row[j] = "r=" + max.ToString() + " ," + "column=" + j.ToString();
            }
            //row[0] = null;    // ← column[0] が無しも試してみた。できた。

            dtTbl.Rows.Add(row);
        }

        /**
         *  @brief      DelLastData ボタン処理
         *  @param[in]  object  sender
         *  @param[in]  EventArgs   e
         *  @return     void
         *  @note       最終行 データ削除
         */
        private void BtnDelLastData_Click(object sender, RoutedEventArgs e)
        {
            // 最終の行数を見つけて、削除
            int max = dtTbl.Rows.Count;
            if(max>0)
                dtTbl.Rows.RemoveAt(max-1);
        }

        /**
         *  @brief      ChgData ボタン処理
         *  @param[in]  object  sender
         *  @param[in]  EventArgs   e
         *  @return     void
         *  @note       指定された行例のデータを指定された文字列に変更
         */
        private void Btn_ChgData_Click(object sender, RoutedEventArgs e)
        {
            if( !(Regex.IsMatch(txtBoxRow.Text, @"^[0-9]+$"))) { return; }
            if (!(Regex.IsMatch(txtBoxColumn.Text, @"^[0-9]+$"))) { return; }

            int r = Convert.ToInt32(txtBoxRow.Text);
            int c = Convert.ToInt32(txtBoxColumn.Text);

            int rmax = dtTbl.Rows.Count;
            int cmax = dtTbl.Columns.Count;

            if ( r>rmax-1) { return; }
            if ( c>cmax-1) { return; }

            dtTbl.Rows[r][c] = txtBoxData.Text; // 指定された行列にデータ追加
        }

        /**
          *  @brief      InsertData ボタン処理
          *  @param[in]  object  sender
          *  @param[in]  EventArgs   e
          *  @return     void
          *  @note       指定された行に固定データで、データ挿入
          */
        private void Btn_InsertData_Click(object sender, RoutedEventArgs e)
        {
            if (!(Regex.IsMatch(txtBoxInsertRow.Text, @"^[0-9]+$"))) { return; }

            int r = Convert.ToInt32(txtBoxInsertRow.Text);

            int rmax = dtTbl.Rows.Count;

            if (r > rmax - 1) { return; }

            System.Data.DataRow row = dtTbl.NewRow();
                row[0] = "New0";
                row[1] = "New1";
                row[2] = "New2";
                row[3] = "New3";

            dtTbl.Rows.InsertAt(row, r);
        }
    }
}

