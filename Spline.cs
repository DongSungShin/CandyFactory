using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;


namespace CandyFactoryMapEditor
{
    /// <summary>
    /// 라인을 따라 생성되는 포인트
    /// </summary>
    public class Curve
    {
        public readonly float DIV_FACTOR = 4.0f; //adjust this factor to adjust the curve smoothness    

        float Ax, Ay;
        float Bx, By;
        float Cx, Cy;
        int Ndiv;

        public Curve(float ax, float ay, float bx, float by, float cx, float cy, int ndiv)
        {
            Ax = ax;
            Ay = ay;
            Bx = bx;
            By = by;
            Cx = cx;
            Cy = cy;
            Ndiv = ndiv;
        }

        public Curve(float ax, float ay, float bx, float by, float cx, float cy)
        {
            Ax = ax;
            Ay = ay;
            Bx = bx;
            By = by;
            Cx = cx;
            Cy = cy;
            Ndiv = (int)(Math.Max(Math.Abs((int)Ax), Math.Abs((int)Ay)) / DIV_FACTOR);
        }

        public Curve() {}

        public void PutCurve(float ax, float ay, float bx, float by, float cx, float cy)
        {
            Ax = ax;
            Ay = ay;
            Bx = bx;
            By = by;
            Cx = cx;
            Cy = cy;
            Ndiv = (int)(Math.Max(Math.Abs((int)Ax), Math.Abs((int)Ay)) / DIV_FACTOR);
        }

       	public int GetCount()
	    {
		    if (Ndiv==0)
			    Ndiv=1;
		    int PointCount = 1;

		    for(int i=1; i<=Ndiv ; i++)
			    PointCount++;

		    return PointCount;
	    }

	    public void GetCurve(float x,float y, ref List<Point> points, ref int PointCount)
	    {
		    int X,Y;
		    float  t,f,g,h;
		    if (Ndiv==0)
			    Ndiv=1;

		    X = (int)x; 
		    Y= (int)y;
		    //points[PointCount].X = X;
		    //points[PointCount].Y = Y;
            Point pt = new Point(X, Y);
            points.Add(pt);
		    ++PointCount;

		    for(int i=1; i<=Ndiv ; i++)
		    {
			    t = 1.0f / (float)Ndiv * (float)i;
			    f = t*t*(3.0f-2.0f*t);
			    g = t*(t-1.0f)*(t-1.0f);
			    h = t*t*(t-1.0f);
			    X = (int)Math.Round(x + Ax*f + Bx*g + Cx*h);
			    Y = (int)Math.Round(y + Ay*f + By*g + Cy*h);
			    //points[PointCount].X = X;
			    //points[PointCount].Y = Y;

                pt = new Point(X, Y);
                points.Add(pt);
			    ++PointCount;
		    }
	    }
    }

    /// <summary>
    /// Curve 데이터로 라인 생성
    /// </summary>
    public class Spline
    {
        float[] Px;
	    float[] Py;
	    float[] Ax;
	    float[] Ay;
	    float[] Bx;
	    float[] By;
	    float[] Cx;
	    float[] Cy;
	    float[]  k;
	    float[,]  Mat = new float[3,3];

	    int  NP;

        public Spline(ref List<Point> pt, int np)
        {
            NP = np;
            Px = new float[NP];
            Py = new float[NP];
            Ax = new float[NP];
            Ay = new float[NP];
            Bx = new float[NP];
            By = new float[NP];
            Cx = new float[NP];
            Cy = new float[NP];
            k = new float[NP];
            Mat = new float[3, np];

            for(int i=0;i<NP ;i++) 
            {
	            Px[i] = (float)pt[i].X;  
	            Py[i] = (float)pt[i].Y;
            }
        }

        public Spline(float[] fx, float[] fy, int np)
        {
            NP = np;
            Px = new float[NP];
            Py = new float[NP];
            Ax = new float[NP];
            Ay = new float[NP];
            Bx = new float[NP];
            By = new float[NP];
            Cx = new float[NP];
            Cy = new float[NP];
            k = new float[NP];

            for (int i = 0; i < NP; i++)
            {
                Px[i] = fx[i];
                Py[i] = fy[i];
            }
        }

        public void Generate() 
        {
            float AMag, AMagOld;
            int i = 0;
            // vector A
            for (i = 0; i <= NP - 2; i++)
            {
                Ax[i] = Px[i + 1] - Px[i];
                Ay[i] = Py[i + 1] - Py[i];
            }
            // k
            AMagOld = (float)Math.Sqrt(Ax[0] * Ax[0] + Ay[0] * Ay[0]);
            for (i = 0; i <= NP - 3; i++)
            {
                AMag = (float)Math.Sqrt(Ax[i + 1] * Ax[i + 1] + Ay[i + 1] * Ay[i + 1]);
                k[i] = AMagOld / AMag;
                AMagOld = AMag;
            }
            k[NP - 2] = 1.0f;

            // Matrix
            for (i = 1; i <= NP - 2; i++)
            {
                Mat[0,i] = 1.0f;
                Mat[1,i] = 2.0f * k[i - 1] * (1.0f + k[i - 1]);
                Mat[2,i] = k[i - 1] * k[i - 1] * k[i];
            }

            Mat[1,0] = 2.0f;
            Mat[2,0] = k[0];
            Mat[0,NP - 1] = 1.0f;
            Mat[1,NP - 1] = 2.0f * k[NP - 2];

            // 
            for (i = 1; i <= NP - 2; i++)
            {
                Bx[i] = 3.0f * (Ax[i - 1] + k[i - 1] * k[i - 1] * Ax[i]);
                By[i] = 3.0f * (Ay[i - 1] + k[i - 1] * k[i - 1] * Ay[i]);
            }
            Bx[0] = 3.0f * Ax[0];
            By[0] = 3.0f * Ay[0];
            Bx[NP - 1] = 3.0f * Ax[NP - 2];
            By[NP - 1] = 3.0f * Ay[NP - 2];

            //
            MatrixSolve(Bx);
            MatrixSolve(By);

            for (i = 0; i <= NP - 2; i++)
            {
                Cx[i] = k[i] * Bx[i + 1];
                Cy[i] = k[i] * By[i + 1];
            }
        }

        void MatrixSolve(float[] B) 
        {
	        float[] Work = new float[NP];
	        float[] WorkB = new float[NP];
	        for(int i=0;i<=NP-1;i++) 
	        {
		        Work[i] = B[i] / Mat[1,i];
		        WorkB[i] = Work[i];
	        }

	        for(int j=0 ; j<10 ; j++) 
	        { ///  need convergence judge
		        Work[0] = (B[0] - Mat[2,0]*WorkB[1])/Mat[1,0];
		        for(int i=1; i<NP-1 ; i++ ) 
		        {
			        Work[i] = (B[i]-Mat[0,i]*WorkB[i-1]-Mat[2,i]*WorkB[i+1])
						        /Mat[1,i];
		        }

		        Work[NP-1] = (B[NP-1] - Mat[0,NP-1]*WorkB[NP-2])/Mat[1,NP-1];

		        for(int i=0 ; i<=NP-1 ; i++ ) 
			        WorkB[i] = Work[i];
	        }

	        for(int i=0 ; i<=NP-1 ; i++ ) 
		        B[i] = Work[i];
        }

        public int GetCurveCount()
	    {
		    Curve c = new Curve();
		    int count = 0;
		    for(int i=0; i<NP-1 ; i++) 
		    {
			    c.PutCurve(Ax[i],Ay[i],Bx[i],By[i],Cx[i],Cy[i]);
			    count += c.GetCount();
		    }
		    return count;
	    }

	    public void GetCurve(ref List<Point> points, ref int PointCount)
	    {
		    Curve c = new Curve();
		    for(int i=0; i<NP-1 ; i++) 
		    {
			    c.PutCurve(Ax[i],Ay[i],Bx[i],By[i],Cx[i],Cy[i]);
			    c.GetCurve(Px[i],Py[i], ref points, ref PointCount);
		    }
	    }
    }

    /// <summary>
    /// Spline을 화면에 그린다.
    /// </summary>
    public class DrawLine
    {
        public List<Point> m_ControlPoints = new List<Point>();
        public List<Point> m_CurvePoints = new List<Point>();

        public DrawLine()
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
        public void RecalcSpline()
        {
            if (m_ControlPoints.Count() > 1)
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
            }
        }

        //--------------------------------------------
        // 라인안에 있는지 체크
        //--------------------------------------------
        public int IsLinePoint(Point point)
        {
            if (m_CurvePoints.Count > 1)
            {
                //LPPOINT lpPoint = m_CurvePoints.GetData();
                //LPPOINT lpPoint2 = m_ControlPoints.GetData();
                if (m_CurvePoints.Count > 0 && m_ControlPoints.Count > 0)
                {
                    double dblDistance;
                    int nSize = m_CurvePoints.Count;
                    int nSize2 = m_ControlPoints.Count;
                    int nIndex = -1;
                    for (int i = 0; i < (nSize - 1); ++i)
                    {
                        Point pt1 = m_CurvePoints[i];
                        Point pt2 = m_CurvePoints[i + 1];

                        for (int j = 0; j < (nSize2); ++j)
                        {
                            Point pt3 = m_ControlPoints[j];
                            if ((pt1.X == pt3.X) && (pt1.Y == pt3.Y))
                            {
                                nIndex = j;
                                break;
                            }
                        }

                        dblDistance = CalcDistanceLine2Point(pt1, pt2, point);
                        if (dblDistance <= 1.0f)
                        {
                            return nIndex;
                        }
                    }
                }
            }

            return -1;
        }

        public double CalcDistanceLine2Point(Point ptLineStart, Point ptLineEnd, Point point)
        {
            double dLineStartX = ptLineStart.X;
            double dLineStartY = ptLineStart.Y;

            double dLineEndX = ptLineEnd.X;
            double dLineEndY = ptLineEnd.Y;
            double dPointX = point.X;
            double dPointY = point.Y;
            double dDistanceStart2Point = Math.Abs(Math.Sqrt(Math.Pow((dLineStartX - dPointX), 2) + Math.Pow((dLineStartY - dPointY), 2)));
            double dDistanceEnd2Point = Math.Abs(Math.Sqrt(Math.Pow((dLineEndX - dPointX), 2) + Math.Pow((dLineEndY - dPointY), 2)));
            double dDistanceStart2End = Math.Abs(Math.Sqrt(Math.Pow((dLineEndX - dLineStartX), 2) + Math.Pow((dLineEndY - dLineStartY), 2)));
            double dAngleStart = Math.Acos((Math.Pow((dDistanceStart2Point), 2) + Math.Pow((dDistanceStart2End), 2) - Math.Pow((dDistanceEnd2Point), 2))
                                    / (2 * dDistanceStart2Point * dDistanceStart2End));
            double dDistanceStart2PointCross = (dDistanceStart2Point * Math.Cos(dAngleStart));
            double dDistance = dDistanceStart2PointCross * Math.Tan(dAngleStart);
            return dDistance;
        }

        //--------------------------------------------
        // 마우스가 포인트안에 있는지 체크
        //--------------------------------------------
        public int IsInsideControlPoint(Point point)
        {
            int count = m_ControlPoints.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; ++i)
                {
                    if (Distance(m_ControlPoints[i], point) < 5)
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

            return Math.Sqrt((double)(dx * dx + dy * dy));
        }
    }
}
