using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassifyText
{
    class Sent
    {
        public String target;
        public String source;
        public String text;
        public String sent;

        public String getAID()
        {
            return target;
        }
        public void setAID(String newID)
        {
            this.target = newID;
        }
        public String getSID()
        {
            return source;
        }
        public void setSID(String newID)
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
