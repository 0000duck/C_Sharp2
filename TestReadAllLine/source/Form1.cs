//-----------------------------------------------------------------------------
//  LINQ学習　サンプル
//
//  [ 内容 ]
//    LINQ と Read/WriteAllLines を使った csvファイル操作。
//    LINQ let句　学習
//    LINQ Group　学習
//    List Distinct学習
//    Where内にラムダ式 学習
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestReadAllLine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /**
          * @brief   　 csvFile Read/Write ボタン処理
          * @param[in]  object     e
          * @param[in]  EventArgs  e 
          * @return     void 
          * @note       csv読み込み、並び変え、要素並び替え
          */
        private void button1_Click(object sender, EventArgs e)
        {
            if(System.IO.File.Exists("C:\\CSVFiles\\Test1.csv") == false)
            {
                return;
            }

            try {
                // Read csvFile
                string[] lines = System.IO.File.ReadAllLines("C:\\CSVFiles\\Test1.csv");

                // x[0]:FirstName ?
                // x[1]:SecondName ?
                // x[2]:Zcode

                // letクエリ内の変数
                // orderby:昇順並び替え
                // select:選択。
                IEnumerable<string> query =
                    from line in lines
                    let x = line.Split(',')
                    orderby x[2]
                    select x[2] + ", " + (x[1] + " " + x[0]);   // Zcode, SecondName FirstName

                // Write csvFile
                System.IO.File.WriteAllLines("C:\\CSVFiles\\Test2.csv", query.ToArray());

            }
            catch(System.IO.IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /**
          * @brief   　 LINQ let句　確認　ボタン処理
          * @param[in]  object     e
          * @param[in]  EventArgs  e 
          * @return     void   
          */
        private void button2_Click(object sender, EventArgs e)
        {
            string wkstr;

            string[] strings =
            {
                "A penny saved is a penny earned.",
                "The early bird catches the worm.",
                "The pen is mightier than the sword."
            };

            // スペースで単語に区切って、小文字にして、単語が
            // 母音ではじまる単語のみ抜き出し、earlyBirdQuery 
            var earlyBirdQuery =
                from sentence in strings
                let words = sentence.Split(' ')
                from word in words
                let w = word.ToLower()
                where w[0] == 'a' || w[0] == 'e'
                    || w[0] == 'i' || w[0] == 'o'
                    || w[0] == 'u'
                select word;

            // Execute the query.
            foreach (var v in earlyBirdQuery)
            {
                //Console.WriteLine("\"{0}\" starts with a vowel", v);
                wkstr = v + " starts with a vowel\n" ;
                textBox1.AppendText(wkstr);
            }
        }

        /**
          * @brief   　 LINQ Group　確認　ボタン処理
          * @param[in]  object     e
          * @param[in]  EventArgs  e 
          * @return     void 
          */
        private void button3_Click(object sender, EventArgs e)
        {
            string[] fNameData = { "NameA.doc", "NameB.xls", "NameC.xls",
                                  "NameD.txt", "NameE.xls", "NameF.cs",
                                  "NameG.txt", "NameH.cs", "NameI.doc" };

            // 拡張子ごとに group化
            var fileNames = from fnames in fNameData
                                //where fnames.LastIndexOf(".") != -1
                                // 上記、以下の2行でも可
                            let idx = fnames.LastIndexOf(".")
                            where idx != -1
                            //group fnames by fnames.Substring(fnames.LastIndexOf("."));
                            // into で続きやるなら 上記行 ';' 削除して 以下採用
                            group fnames by fnames.Substring(fnames.LastIndexOf("."))
                            into keep
                            where keep.Count() > 2
                            select keep;

            // query から 1つづつGroupデータを取り出し、Groupから1つづつ名前をとりだす
            foreach(var names in fileNames)
            {
                textBox2.AppendText("group by " + names.Key + "\n");
                foreach(var name in names)
                {
                    textBox2.AppendText(" " + name + "\n");
                }
            }

        }

        /**
          * @brief   　 Distinct機能確認　ボタン処理
          * @param[in]  object     e
          * @param[in]  EventArgs  e 
          * @return     void 
          */
        private void button4_Click(object sender, EventArgs e)
        {
            string[] fNameData = { "NameA.doc", "NameB.xls", "NameC.xls",
                                  "NameD.txt", "NameE.xls", "NameF.cs",
                                  "NameA.doc", "NameB.xls", "NameC.xls" };

            // 重複データを抜いて List化
            List<string>lists = fNameData.Distinct().ToList<string>();
            foreach(var n in lists)
            {
                textBox3.AppendText(n.ToString() + "\n");
            }
        }

        /**
          * @brief   　 Where内にラムダ式確認　ボタン処理
          * @param[in]  object     e
          * @param[in]  EventArgs  e 
          * @return     void 
          */
        private void button5_Click(object sender, EventArgs e)
        {
            // personList 3人分作成
            var personsList= new List<Person>();

            Person one = new Person();
            one.first = "Name1";
            one.last = "NameLast1";
            Person two = new Person();
            two.first = "Name2";
            two.last = "NameLast2";
            personsList.Add(two);
            Person three = new Person();
            three.first = "Name3";
            three.last = "NameLast3";
            personsList.Add(three);

            // ListのWhere内ラムダ式で絞り込み
            var list = personsList.Where(p => p.last=="NameLast3");
            // 上記 Where文内の ラムダ式は以下？
            // Func<Person, bool> pf = (x) => { if (x.last == "NameLast3") return true; else return false; };

            foreach (var l in list)
            {
                //Console.WriteLine(l.first);
                textBox4.AppendText(l.first + "\n");
            }

            

            // LINQのwhere内ラムダ式で絞り込み
            var query = from fnames in personsList
                       where fnames.last == "NameLast3"
                       select fnames;
            foreach (var n in query)
            {
                //Console.WriteLine(n.first);
                textBox4.AppendText(n.first + "\n");
            }

        }
    }


    /**
    　* @brief   　 人名保持用クラス
   　 * 
   　 */
    class Person
    {
        public string first { get; set; }
        public string last { get; set; }
    }

}
