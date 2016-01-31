using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassifyText
{
    class Person
    {
        public String fname;
        
        public int pos;
        public int neg;
        public int id;
        public String party;

        public String getName()
        {
            return fname;
        }

        public void setName(String newName)
        {
            this.fname = newName;
        }

        public String getParty()
        {
            return party;
        }

        public void setParty(String newPar)
        {
            this.party = newPar;
        }


        public int getID()
        {
            return id;
        }
        public void setID(int newID)
        {
            this.id = newID;
        }

        public int getPos()
        {
            return pos;
        }
        public void setPos(int newPos)
        {
            this.pos = newPos;
        }

        public int getNeg()
        {
            return neg;
        }
        public void setNeg(int newNeg)
        {
            this.neg = newNeg;
        }
    }
}
