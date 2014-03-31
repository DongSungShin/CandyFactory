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

    // *.sav파일을 읽고 쓴다.
    // 데이터를 넘겨준다.
    public class FileManager
    {
        public List<Point> m_LPoint = new List<Point>();
        public string m_szMapFileName = null;
        public int m_nLineCount = 1;

        static FileManager m_Instance = new FileManager();

        public FileManager(){ }

        public FileManager Instance { get { return m_Instance; } }

        public void WirteFile(string szFileName)
        {
            FileStream fp = new FileStream(szFileName, FileMode.Create);
            BinaryWriter br = new BinaryWriter(fp);

            if (m_szMapFileName != null)
                br.Write(m_szMapFileName);
            else
                br.Write("null");

            br.Write(m_LPoint.Count);
            foreach (Point pt in m_LPoint)
            {
                br.Write(pt.X);
                br.Write(pt.Y);
            }
            br.Write(m_nLineCount);

            fp.Close();
            br.Close();
        }

        public void LoadFile(string szFileName)
        {
            FileStream fp = new FileStream(szFileName, FileMode.Open);
            BinaryReader br = new BinaryReader(fp);

            //int count = br.ReadInt32();
            //char[] mapfile = br.ReadChars(count);
            string mapfile = br.ReadString();
            if (mapfile.Equals("null"))
                m_szMapFileName = null;
            else
                m_szMapFileName = mapfile;

            int Count = br.ReadInt32();
            m_LPoint = null;
            m_LPoint = new List<Point>();

            for (int i = 0; i < Count; ++i)
            {
                int x = br.ReadInt32();
                int y = br.ReadInt32();

                m_LPoint.Add(new Point(x, y));
            }

            m_nLineCount = br.ReadInt32();

            fp.Close();
            br.Close();
        }
    }
}