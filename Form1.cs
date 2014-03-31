using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CandyFactoryMapEditor
{
    public partial class Form1 : Form
    {
        int m_MoveIndex = -1;
        int m_nLineCount = 1;
        string m_szMapFilePath = null;

        Bitmap m_kMapImage = null;  // 맵 이미지
        Bitmap m_backBuffer;        // 더블 버퍼링

        Graphics _graphics;

        BackBufferPanel m_Panel1 = new BackBufferPanel();

        List<DrawLine> m_LDrawLine = new List<DrawLine>();

        public Form1()
        {
            InitializeComponent();

            this.splitContainer1.Panel1.Controls.Add(m_Panel1);


            m_backBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            _graphics = Graphics.FromImage(m_backBuffer);

            m_Panel1.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
            m_Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            m_Panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            m_Panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            m_Panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            m_Panel1.Resize += new EventHandler(this.OnResize);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DrawLine drawline = new DrawLine();
            m_LDrawLine.Add(drawline);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            m_MoveIndex = -1;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            foreach (DrawLine line in m_LDrawLine)
            {
                // 포인트 인덱스 가져오기
                int index = line.IsInsideControlPoint(e.Location);
                if (index >= 0)
                {
                    this.Cursor = Cursors.Cross;
                    if (m_MoveIndex >= 0)
                    {
                        line.m_ControlPoints[m_MoveIndex] = e.Location;
                        line.RecalcSpline();
                    }
                }
                else
                {
                    int nLine = line.IsLinePoint(e.Location);
                    if (nLine >= 0)
                        this.Cursor = Cursors.Hand;
                    else
                        this.Cursor = Cursors.Default;
                }
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            foreach (DrawLine line in m_LDrawLine)
            {
                int index = line.IsInsideControlPoint(e.Location);

                if (index >= 0)
                    m_MoveIndex = index;
                else //we are adding control points
                {
                    int nLine = line.IsLinePoint(e.Location);
                    if (nLine >= 0)
                        //m_ControlPoints.Add(e.Location);
                        line.m_ControlPoints.Insert(line.m_ControlPoints.Count - 1, e.Location);

                    line.RecalcSpline();
                }
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(m_backBuffer, 0, 0);
            //e.Graphics.DrawImage(m_backBuffer, 0, 0);
        }

        private void OnResize(object sender, EventArgs e)
        {
            if (m_backBuffer != null)
            {
                m_backBuffer.Dispose();
                m_backBuffer = null;
            }

            m_backBuffer = new Bitmap(this.splitContainer1.Panel1.Width, this.splitContainer1.Panel1.Height);
            m_Panel1.Size = new Size(this.splitContainer1.Panel1.Width, this.splitContainer1.Panel1.Height);
        }

        // 맵에 사용할 파일을 가져온다.
        private void MapOpen_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            this.openFileDialog1.DefaultExt = "MapFile";
            this.openFileDialog1.Filter = "All Files(*.*)|*.*|PNG Files(*.png)|*.png |JPG Files(*.jpg)|*.jpg";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                m_kMapImage = new Bitmap(this.openFileDialog1.FileName);
                //m_kClipView.OpenProject(openFileDialog1.FileName);
                m_szMapFilePath = this.openFileDialog1.FileName;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _graphics = Graphics.FromImage(m_backBuffer);
            _graphics.Clear(Color.Gray);

            // Map Draw
            if (m_kMapImage != null)
                _graphics.DrawImage(m_kMapImage, new Point(0, 0));

            Pen _pen = new Pen(Color.Red);
            _pen.Width = 3;

            foreach (DrawLine line in m_LDrawLine)
            {
                // 커브선 그리기
                //_graphics.DrawBeziers(_pen, m_CurvePoints.ToArray());
                _graphics.DrawCurve(_pen, line.m_CurvePoints.ToArray());

                // 포인트 그리기
                _pen.Color = Color.Blue;
                for (int i = 1; i < line.m_ControlPoints.Count - 1; ++i)
                    _graphics.DrawEllipse(_pen, new Rectangle(line.m_ControlPoints[i], new Size(5, 5)));

                // 시작과 끝은 따로
                _pen.Color = Color.Purple;
                _graphics.DrawRectangle(_pen, new Rectangle(line.m_ControlPoints[0], new Size(5, 5)));
                _graphics.DrawString("Start", new Font("Arial", 15), new SolidBrush(Color.Black), line.m_ControlPoints[0]);
                _pen.Color = Color.Green;
                _graphics.DrawRectangle(_pen, new Rectangle(line.m_ControlPoints[line.m_ControlPoints.Count - 1], new Size(5, 5)));
                _graphics.DrawString("End", new Font("Arial", 15), new SolidBrush(Color.Black), line.m_ControlPoints[line.m_ControlPoints.Count - 1]);

            }

            _pen.Dispose();
            _graphics.Dispose();

            m_Panel1.Invalidate();
        }

        // 곡선 라인 저장
        private void SaveFile_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            this.saveFileDialog1.DefaultExt = "SaveFile";
            this.saveFileDialog1.Filter = "Save Files(*.sav)|*.sav";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //FileManager filemanager = new FileManager();
                //filemanager.m_szMapFileName = m_szMapFilePath;
                //filemanager.m_LPoint = m_ControlPoints;
                //filemanager.m_nLineCount = m_nLineCount;

                //filemanager.WirteFile(saveFileDialog1.FileName);
            }
        }

        // 불러오기
        private void LoadFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            this.openFileDialog1.DefaultExt = "SaveFile";
            this.openFileDialog1.Filter = "Save Files(*.sav)|*.sav";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileManager filemanager = new FileManager();
                filemanager.LoadFile(this.openFileDialog1.FileName);

                // 맵 파일이 있다면 불러오기
                if (filemanager.m_szMapFileName != null)
                {
                    m_szMapFilePath = filemanager.m_szMapFileName;
                    if (m_kMapImage != null)
                        m_kMapImage.Dispose();

                    m_kMapImage = new Bitmap(m_szMapFilePath);
                }

                //m_ControlPoints = filemanager.m_LPoint;
                //m_nLineCount = filemanager.m_nLineCount;

                //// 곡선 재 계산
                //RecalcSpline();
            }
        }

        // 맵파일로 출력 버튼 (게임에서 사용할 데이터)
        private void ExportMapFile_Click(object sender, EventArgs e)
        {

        }

        // 라인 추가 버튼
        private void AddLine_Click(object sender, EventArgs e)
        {

        }
    }
}
