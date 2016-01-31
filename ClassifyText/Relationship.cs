using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassifyText
{
    class Relationship
    {
        public String target;
        public String source;
        public String type;
        public int value;


        public int getID()
        {
            return value;
        }
        public void setID(int newID)
        {
            this.value = newID;
        }
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

        public String getSent()
        {
            return type;
        }
        public void setSent(String newSent)
        {
            type = newSent;

        }
    }
    
}
