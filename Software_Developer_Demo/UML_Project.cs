using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Software_Developer_Demo
{
    class UML_Project
    {
        public List<UserCaseDiagram> User_Cases = new List<UserCaseDiagram>();

        public string addUser_Cases()
        {
            User_Cases.Add(new UserCaseDiagram());
            return "UserCases" + User_Cases.Count.ToString();
        }
    }
}
