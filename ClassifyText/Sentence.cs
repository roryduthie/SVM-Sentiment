using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassifyText
{
    class Sentence
    {
        public int target;
        public int source;
        public String text;
        public String sent;

        public int getAID()
        {
            return target;
        }
        public void setAID(int newID)
        {
            this.target = newID;
        }
        public int getSID()
        {
            return source;
        }
        public void setSID(int newID)
        {
            this.source = newID;
        }

        public String getText()
        {
            return text;
        }
        public void setText(String newText)
        {
            text = newText;
            text = text.Replace(",", "");
        }

        public String getSent()
        {
            return sent;
        }
        public void setSent(String newSent)
        {
            sent = newSent;

        }
    }


}
