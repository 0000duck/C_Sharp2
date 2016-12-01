//-------------------------------------------------------------------------------
//  OpenGL で四角を描くサンプル
//
//    GL.MatrixMode(MatrixMode.Projection)、
//    Matrix4.CreateOrthographic() を使用
//    ★すみません。Matrix4.LookAt　の視点方向のあたりは、良く理解できていません。
//      また、アスペクト比　を幅だけ考慮して、うまくいっているのもちょっと、？です。
//  
//  [ 作成方法 ]
//    [参照]→"参照マネージャ"→"アセンブリ"→"フレームワーク"→"拡張"
//		→ OpenTK、OpenTK.GLControl を Check
//    ( NuGetでも追加できる様子。
//      今回は、opentk-2014-07-23.exe download し、実行してインストールしました。)
//
//    Form1.cs に 
//        using OpenTK;
//        using OpenTK.Graphics.OpenGL;
//	  追加
//    Form1.Designer.cs のInitializeComponent() の内容を以下に変更
//
//    this.components = new System.ComponentModel.Container();
//    this.glControl1 = new OpenTK.GLControl();
//    this.SuspendLayout();
//    this.glControl1.Name = "glControl";
//
//    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//    this.Controls.Add(this.glControl1);
//    this.Text = "Form1";
//    this.ResumeLayout(false);
//
//     その後、 Form1.s デザインへ
//     デザイン上で、glControl1 を選択し、 Dockプロパティを "Fill" にし
//     glControl1_Load、glControl1_Paint を実装。
//   
//-------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace TestOpenTKMtxPrj
{
    public partial class Form1 : Form
    {
        GLControl glControl1;

        public Form1()
        {
            InitializeComponent();

            // イベント追加
            glControl1.Load += glControl1_Load;
            glControl1.Paint += glControl1_Paint;
        }

        /**
         * @brief   　 GL Control の LoadEvent処理
         * @param[in]  object     e
         * @param[in]  PaintEventArgs  e 
         * @return     void   
         * @note       独自にコード追加。
         *             ViewPort と 視体積、視界の設定。
         */
        private void glControl1_Load(object sender, EventArgs e)
        {
            // 初期化色設定
            GL.ClearColor(Color.CornflowerBlue);

            // (1)s
            // ビューポートの設定
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);

            // 視体積の設定
            //   
            GL.MatrixMode(MatrixMode.Projection);                           // Projection(投影)
            float h = 4.0f, w = h * glControl1.AspectRatio;                 // (幅) = (高さ) × (アスペクト比) で歪みが出ないようにしている
            Matrix4 proj = Matrix4.CreateOrthographic(w, h, 0.01f, 2.0f);   // Orthographic (正射影) width, height、 手前と奥の距離を指定
            GL.LoadMatrix(ref proj);

            // 視界の設定
            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 look = Matrix4.LookAt(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY);
            GL.LoadMatrix(ref look);
            // (1)e
        }


        /**
         * @brief   　 GL Control の PaintEvent処理
         * @param[in]  object     e
         * @param[in]  PaintEventArgs  e 
         * @return     void   
         * @note       独自にコード追加。三角形が2枚合わさるように指定し、四角を描く
         *             LoadEvent の方で、 ViewPort と 視体積、視界の設定を先にしている。
         */
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {

            // Buffer(領域？)クリア
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // 四角を描く(三角形が2枚合わさるように指定)
            //  描画領域に関しては何の設定もしていないと 
            //  画面がどんな形でも領域は - 1 ≤ x ≤ 1, -1 ≤ y ≤ 1 となる。
            //  たとえば 領域のサイズが (300, 300) のとき，現在の正方形の1辺は -0.5～0.5 の 長さ 1 なので，150 pixel になる。
            //           領域のサイズが(300, 150) のとき，正方形の縦の長さは 75 pixel になる。
            //  以下の記述だとそうなる
            GL.Begin(BeginMode.TriangleStrip);  // Begin と Endで１つ。
            {
                // 4点を指定
                GL.Vertex2(0.5, 0.5);
                GL.Vertex2(-0.5, 0.5);
                GL.Vertex2(0.5, -0.5);
                GL.Vertex2(-0.5, -0.5);
            }
            GL.End();
            //
            // y を基準とすれば，x = y * (コントロールの幅) / (コントロールの高さ) となる。
            // この 幅/高さを Aspect比とよび
            // 領域を考慮して Aspect比で割って指定すると正方形になる
            //
            //float r = glControl1.AspectRatio;
            //
            //GL.Begin(BeginMode.TriangleStrip);
            //{
            //    GL.Vertex2(0.5 / r, 0.5);
            //    GL.Vertex2(-0.5 / r, 0.5);
            //    GL.Vertex2(0.5 / r, -0.5);
            //    GL.Vertex2(-0.5 / r, -0.5);
            //}
            //GL.End();
            // 上記をやめ、もとの rを使用しない方にし、glControl1_Loaded に
            // (1)sから(1)eまでを追加して、正方形を表示


            //バックグラウンドで描画した画面を現在の画面と入れ替。（Controlに対して行うことに注意とのこと）
            glControl1.SwapBuffers();
        }
    }
}
