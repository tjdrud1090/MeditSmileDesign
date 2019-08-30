using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Process_Page_Change.Util
{
    class PatientInfo
    {
        public static PatientInfo Patient_Info = new PatientInfo();


        public string Name;
        public string Num;
        public string Date;
        public string Memo;
        public BitmapImage frontfile;
        public BitmapImage teeth_opener_file;
        public BitmapImage downfacefile;
        public BitmapImage upfacefile;
        public BitmapImage Lfacefile;
        public BitmapImage Rfacefile;
        public string frontfilename;
        public string teeth_opener_filename;
        public string downfacefilename;
        public string upfacefilename;
        public string Lfacefilename;
        public string Rfacefilename;



        public PatientInfo()
        {

        }
    }
}
