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
    // Panel1에 백버퍼를 사용하기 위해서 생성한 임의의 패널
    public class BackBufferPanel : Panel
    {
        // 이 함수를 상속해야 깜박임이 없다 (이것 때문에 클래스를 만듬)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
    }
}