namespace MyPSG.API.Models
{
     public class QueryHelper{
        public string SetConditionAND(string OldCond ,string NewCond ) 
        {
            return OldCond != ""? OldCond + " AND " + " (" + NewCond + ") " : " (" + NewCond + ") ";
        }

        public string SetConditionOR(string OldCond ,string NewCond ) 
        {
            return OldCond != ""? OldCond + " OR " + " (" + NewCond + ") " : " (" + NewCond + ") ";
        }
    }


}