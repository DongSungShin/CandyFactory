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

        List<Point> m_ControlPoints = new List<Point>();
        List<Point> m_CurvePoints = new List<Point>();

        Bitmap m_kMapImage = null;
        Bitmap m_backBuffer;        // 더블 버퍼링

        Graphics _graphics;
        Graphics m_Paintgraphics;

        BackBufferPanel m_Panel1 = new BackBufferPanel();

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
            // 최초 위치 설정
            Point pt = new Point();
            pt.X = 50;
            pt.Y = 50;
            m_ControlPoints.Add(pt);
            pt.X = 50 + 125;
            pt.Y = 50 + 125;
            m_ControlPoints.Add(pt);
            pt.X = 300;
            pt.Y = 300;
            m_ControlPoints.Add(pt);
            RecalcSpline();
        }

        //--------------------------------------------
        // 스플라인 곡선 재 계산
        //--------------------------------------------
        void RecalcSpline()
        {
            if(m_ControlPoints.Count() > 1)
	        {
		        // 스플라인 생성
		        Spline spline = new Spline(ref m_ControlPoints, m_ControlPoints.Count);
		        // 커브 생성
		        spline.Generate();
		        // 커브 수를 가져옴
                //m_CurvePoints.Capacity = spline.GetCurveCount();
                m_CurvePoints.Clear();
		        
		        int PointCount = 0;
                // 커브 데이터를 생성
		        spline.GetCurve(ref m_CurvePoints, ref PointCount);

		        //for(int i = 0; i < m_CurvePoints.Count(); ++i)
			    //    Point Pt = m_CurvePoints[i];

	        }

	        Invalidate(false);
        }

        //--------------------------------------------
        // 라인안에 있는지 체크
        //--------------------------------------------
        int IsLinePoint(Point point)
        {
	        if(m_CurvePoints.Count > 1)
	        {
		        //LPPOINT lpPoint = m_CurvePoints.GetData();
		        //LPPOINT lpPoint2 = m_ControlPoints.GetData();
                if (m_CurvePoints.Count > 0 && m_ControlPoints.Count > 0)
		        {
			        double dblDistance;
                    int nSize = m_CurvePoints.Count;
                    int nSize2 = m_ControlPoints.Count;
			        int nIndex = -1;
			        for(int i = 0; i < (nSize-1); ++i)
			        {
                        Point pt1 = m_CurvePoints[i];
                        Point pt2 = m_CurvePoints[i + 1];

				        for(int j = 0; j < (nSize2); ++j)
				        {
                            Point pt3 = m_ControlPoints[j];
					        if((pt1.X == pt3.X) && (pt1.Y == pt3.Y))
					        {
						        nIndex=j;
						        break;
					        }
				        }
				
				        dblDistance = CalcDistanceLine2Point(pt1,pt2,point);
				        if(dblDistance<=1.0f)
				        {
					        return nIndex;
				        }
			        }
		        }
	        }

	        return -1;
        }

        double CalcDistanceLine2Point(Point ptLineStart, Point ptLineEnd, Point point)
        {
	        double dLineStartX = ptLineStart.X;
	        double dLineStartY = ptLineStart.Y;
	
	        double dLineEndX = ptLineEnd.X;
	        double dLineEndY = ptLineEnd.Y;
	        double dPointX = point.X;
	        double dPointY = point.Y;
	        double dDistanceStart2Point	= Math.Abs( Math.Sqrt( Math.Pow((dLineStartX-dPointX),2) + Math.Pow((dLineStartY-dPointY),2) ) );
            double dDistanceEnd2Point = Math.Abs(Math.Sqrt(Math.Pow((dLineEndX - dPointX), 2) + Math.Pow((dLineEndY - dPointY), 2)));
            double dDistanceStart2End = Math.Abs(Math.Sqrt(Math.Pow((dLineEndX - dLineStartX), 2) + Math.Pow((dLineEndY - dLineStartY), 2)));
            double dAngleStart = Math.Acos((Math.Pow((dDistanceStart2Point), 2) + Math.Pow((dDistanceStart2End), 2) - Math.Pow((dDistanceEnd2Point), 2)) 
							        / (2*dDistanceStart2Point*dDistanceStart2End) );
            double dDistanceStart2PointCross = (dDistanceStart2Point * Math.Cos(dAngleStart));
            double dDistance = dDistanceStart2PointCross * Math.Tan(dAngleStart);
	        return dDistance;
        }

        //--------------------------------------------
        // 마우스가 포인트안에 있는지 체크
        //--------------------------------------------
        private int IsInsideControlPoint(Point point)
        {
            int count = m_ControlPoints.Count;
	        if(count > 0)	
	        {
		        for(int i = 0; i < count; ++i)
		        {
			        if(Distance(m_ControlPoints[i], point) < 5)
				        return i;
		        }
	        }

	        return -1;
        }

        //--------------------------------------------
        // 마우스가 포인트안에 있는지 거리 체크
        //--------------------------------------------
        private double Distance(Point p1, Point p2)
        {
	        int dx = Math.Abs(p1.X - p2.X);
	        int dy = Math.Abs(p1.Y - p2.Y);

	        return Math.Sqrt( (double)(dx*dx + dy*dy));
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            m_MoveIndex = -1;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            // 포인트 인덱스 가져오기
            int index = IsInsideControlPoint(e.Location);
            if (index >= 0)
            {
                this.Cursor = Cursors.Cross;
                if (m_MoveIndex >= 0)
                {
                    m_ControlPoints[m_MoveIndex] = e.Location;
                    RecalcSpline();
                }
            }
            else
            {
                int nLine = IsLinePoint(e.Location);
                if (nLine >= 0)
                    this.Cursor = Cursors.Hand;
                else
                    this.Cursor = Cursors.Default;
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            int index = IsInsideControlPoint(e.Location);

            if (index >= 0)
                m_MoveIndex = index;
            else //we are adding control points
            {
                int nLine = IsLinePoint(e.Location);
                if (nLine >= 0)
                    //m_ControlPoints.Add(e.Location);
                m_ControlPoints.Insert(m_ControlPoints.Count - 1, e.Location);

                RecalcSpline();
            }

            //Invalidate(false);
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

            // 커브선 그리기
            //_graphics.DrawBeziers(_pen, m_CurvePoints.ToArray());
            _graphics.DrawCurve(_pen, m_CurvePoints.ToArray());

            // 포인트 그리기
            _pen.Color = Color.Blue;
            for (int i = 1; i < m_ControlPoints.Count - 1; ++i)
                _graphics.DrawEllipse(_pen, new Rectangle(m_ControlPoints[i], new Size(5, 5)));

            // 시작과 끝은 따로
            _pen.Color = Color.Purple;
            _graphics.DrawRectangle(_pen, new Rectangle(m_ControlPoints[0], new Size(5, 5)));
            _graphics.DrawString("Start", new Font("Arial", 15), new SolidBrush(Color.Black), m_ControlPoints[0]);
            _pen.Color = Color.Green;
            _graphics.DrawRectangle(_pen, new Rectangle(m_ControlPoints[m_ControlPoints.Count - 1], new Size(5, 5)));
            _graphics.DrawString("End", new Font("Arial", 15), new SolidBrush(Color.Black), m_ControlPoints[m_ControlPoints.Count - 1]);

            _pen.Dispose();
            _graphics.Dispose();

            m_Panel1.Invalidate();
        }

        // 곡선 라인 저장
        private void SaveFile_Click(object sender, EventArgs e)
        {

        }

        // 불러오기
        private void LoadFile_Click(object sender, EventArgs e)
        {

        }

        // 맵파일로 출력
        private void ExportMapFile_Click(object sender, EventArgs e)
        {

        }
    }
}
