using System;
using System.Collections.Generic;
using System.Text;

namespace IBbasic
{
    public class IBbasicReview
    {
        public List<IBReview> reviews = new List<IBReview>();
        public List<IBEndorse> endorsements = new List<IBEndorse>();

        public IBbasicReview()
        {

        }
    }

    public class IBReview
    {
        public string userID = "none";
        public string userName = "anonymous";
        public string moduleName = "";
        public int moduleVersion = 0;
        public string review = "";
        public string reviewDate = "";

        public IBReview()
        {

        }
    }

    public class IBEndorse
    {
        public string userID = "none";
        public string userName = "anonymous";
        public string moduleName = "";
        public bool endorsed = false;

        public IBEndorse()
        {

        }
    }
}
