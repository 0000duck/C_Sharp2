//--------------------------------------------------------------------------------------
//    WPF DataGrid に Combobox/CheckBoxを付けたサンプル
//
//    [内容]
//    DataGrid に Combobox/CheckBox を付けてデータを設定する方法を試したもの。
//    NotifyIdentifer、デザイナからの Binding は使用せず
//    List<Machine>Machines と List<string>ModeStr を用意し、
//    XAMLに直接Binding を記述しました。
//    ポイントは以下と思います。
//        < cs側 >
//        myGrid.ItemsSource = Machines;          // DataGrid へデータ設定  
//        myGridCmBoxData.ItemsSource = ModeStr;  // DataGrid内のComboboxにメンバ設定
//        < xaml側 >
//        <DataGridCheckBoxColumn Header = "Used" Binding="{Binding Used,  Mode=TwoWay}" Width="40" />
//          <DataGridComboBoxColumn Header = "Mode"  x:Name="myGridCmBoxData" 
//              SelectedValueBinding="{Binding Mode, Mode=TwoWay}"  Width="*"
//              DisplayMemberPath="{Binding Mode}" />
//
//--------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Windows;


//using System.Text.RegularExpressions;
//using System.ComponentModel;
//using System.Collections.ObjectModel;


namespace TestWpfDataGridCmBox
{
    /**
     *  @brief      DataGrid確認用サンプルMainWindowクラス
     *  @note       DataGrid用データ Machineと ModeStr の生成、DataGridへの設定
     */
    public partial class MainWindow : Window
    {
        public List<Machine> Machines { get; set; }     // DataGrid用データ
        public List<string> ModeStr { get; set; }       // DataGrid内Combobox用メンバデータ


        public MainWindow()
        {
            InitializeComponent();

            // DataGridデータ作成、とりあえず3個。
            Machines = new List<Machine>()
            {
                new Machine() { Name = "Machine1", Mode = "", Used = true },
                new Machine() { Name = "Machine2", Mode = "", Used = false },
                new Machine() { Name = "Machine3", Mode = "", Used = false }
            };

            // DataGrid内Combobox用メンバデータ作成
            ModeStr = new List<string>();
            ModeStr.Add("Low");
            ModeStr.Add("Nml");
            ModeStr.Add("Hi");  

            myGrid.ItemsSource = Machines;          // DataGrid へデータ設定  
            myGridCmBoxData.ItemsSource = ModeStr;  // DataGrid内のComboboxにメンバ設定
        }

        /**
         *  @brief      BtnShowDataGirdDataボタン処理
         *  @param[in]  object  sender
         *  @param[in]  EventArgs   e
         *  @return     void
         *  @note       DataGridの情報を１行づつ MsgBoxで表示
         */
        private void BtnShowDataGirdData_Click(object sender, RoutedEventArgs e)
        {
            foreach (Machine m in Machines)
            {
                string text = string.Empty;
                text = "Name : " + m.Name + Environment.NewLine;
                text += "Mode : " + m.Mode + Environment.NewLine;
                text += "IsCheck : " + m.Used.ToString() + Environment.NewLine;
                MessageBox.Show(text);
            }
        }

    }

    /**
     *  @brief      DataGrid用データクラス
     *  @note       DataGrid用データ
     */
    public class Machine
    {
        public string Name { get; set; }
        public string Mode { get; set; }
        public bool Used { get; set; }
    }

}
