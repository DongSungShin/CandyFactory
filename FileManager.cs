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
    public struct DataFile
    {
        public List<Point> m_LPoint;
    }

    // *.sav파일을 읽고 쓴다.
    // 데이터를 넘겨준다.
    public class FileManager
    {
        public string m_szMapFileName;
        public List<DataFile> m_LDataFile = new List<DataFile>();

        static FileManager m_Instance = new FileManager();

        public FileManager(){ }

        public static FileManager Instance { get { return m_Instance; } }

        // 데이터 파일 추가
        public void AddData(List<Point> lPoint, string szMapFile)
        {
            DataFile data = new DataFile();

            data.m_LPoint = new List<Point>();
            data.m_LPoint = lPoint;

            m_szMapFileName = szMapFile;

            m_LDataFile.Add(data);
        }

        public void WirteFile(string szFileName)
        {
            FileStream fp = new FileStream(szFileName, FileMode.Create);
            BinaryWriter br = new BinaryWriter(fp);

            if (m_LDataFile.Count <= 0)
            {
                MessageBox.Show("데이터가 없다");
                return;
            }

            br.Write(m_LDataFile.Count);
            for (int i = 0; i < m_LDataFile.Count; ++i)
            {
                DataFile data = m_LDataFile[i];

                if (m_szMapFileName != null)
                    br.Write(m_szMapFileName);
                else
                    br.Write("null");

                br.Write(data.m_LPoint.Count);
                foreach (Point pt in data.m_LPoint)
                {
                    br.Write(pt.X);
                    br.Write(pt.Y);
                }
            }

            fp.Close();
            br.Close();
        }

        public void LoadFile(string szFileName)
        {
            FileStream fp = new FileStream(szFileName, FileMode.Open);
            BinaryReader br = new BinaryReader(fp);

            m_LDataFile.Clear();

            int count = br.ReadInt32();

            DataFile data = new DataFile();

            for (int i = 0; i < count; ++i)
            {
                string mapfile = br.ReadString();
                if (mapfile.Equals("null"))
                    m_szMapFileName = null;
                else
                    m_szMapFileName = mapfile;

                int Count = br.ReadInt32();
                data.m_LPoint = new List<Point>();

                for (int j = 0; j < Count; ++j)
                {
                    int x = br.ReadInt32();
                    int y = br.ReadInt32();

                    data.m_LPoint.Add(new Point(x, y));
                }

                m_LDataFile.Add(data);
            }

            fp.Close();
            br.Close();
        }
    }
}