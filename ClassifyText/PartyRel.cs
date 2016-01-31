using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassifyText
{
    class PartyRel
    {

        public String name;
        public String party;

        public String getName()
        {
            return name;
        }

        public void setName(String newName)
        {
            this.name = newName;
        }

        public String getParty()
        {
            return party;
        }

        public void setParty(String newPar)
        {
            this.party = newPar;
        }
    }
}
