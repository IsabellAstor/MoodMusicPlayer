using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    class Discography
    {
        #region Field
        private string _title;
        private string _artist;
        private string _album;
        private int _year;

        











        #endregion

        #region Properties
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public string Artist
        {
            get { return _artist; }
            set { _artist = value; }
        }
        public string Album
        {
            get { return _album; }
            set { _album = value; }
        }
        public int Year
        {
            get { return _year; }
            set { _year = value; }
        }
        #endregion

        #region Methods

        #endregion

        #region Constructors

         public Discography ()
        {

        }

        #endregion
    }
}
